using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Checklist;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Checklist.Commands;

public class SubmitChecklistCommand : SubmitChecklistRequest, IRequest<Result<SubmitChecklistResponse>>
{
    // Poblado por el controller desde CurrentUserId (claim JWT)
    public string UserId { get; set; } = string.Empty;
}

public class SubmitChecklistCommandHandler
    : IRequestHandler<SubmitChecklistCommand, Result<SubmitChecklistResponse>>
{
    private readonly IRepository<LD.Domain.Entities.Equipment> _equipmentRepository;
    private readonly IChecklistRepository _checklistRepository;

    public SubmitChecklistCommandHandler(
        IRepository<LD.Domain.Entities.Equipment> equipmentRepository,
        IChecklistRepository checklistRepository)
    {
        _equipmentRepository = equipmentRepository;
        _checklistRepository = checklistRepository;
    }

    public async Task<Result<SubmitChecklistResponse>> Handle(
        SubmitChecklistCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId);

            if (equipment is null)
                return Result<SubmitChecklistResponse>.Failure("Equipo no encontrado.", new(), 404);

            var checklist = new Domain.Entities.Checklist
            {
                EquipmentId    = request.EquipmentId,
                EquipmentTypeId = equipment.EquipmentTypeId,
                UserId         = request.UserId,
                UserName       = request.UserName,
                Turno          = request.Turno,
                Observaciones  = request.Observaciones
            };

            foreach (var a in request.Answers)
            {
                checklist.Answers.Add(new ChecklistAnswer
                {
                    QuestionId           = a.QuestionId,
                    QuestionTextSnapshot = a.QuestionText,
                    AnswerText           = a.AnswerText,
                    IsOk                 = a.IsOk
                });
            }

            foreach (var m in request.DefectMarks)
            {
                checklist.DefectMarks.Add(new ChecklistDefectMark
                {
                    Side     = m.Side,
                    XPercent = m.XPercent,
                    YPercent = m.YPercent,
                    Note     = m.Note
                });
            }

            int order = 1;
            foreach (var p in request.Photos)
            {
                checklist.Photos.Add(new ChecklistPhoto
                {
                    RelativePath = p.RelativePath,
                    Side         = p.Side,
                    Order        = order++
                });
            }

            var id = await _checklistRepository.CreateChecklistAsync(checklist);

            return Result<SubmitChecklistResponse>.Success(
                new SubmitChecklistResponse { ChecklistId = id, CreatedAt = checklist.CreatedAt },
                "Checklist guardado correctamente.");
        }
        catch (Exception ex)
        {
            return Result<SubmitChecklistResponse>.Failure(
                "Error al guardar el checklist.", new List<string> { ex.Message });
        }
    }
}
