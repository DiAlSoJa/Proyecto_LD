using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Application.Common.Results;
using LD.Contracts.Constants;
using LD.Contracts.EquipmentQuestion;
using LD.Contracts.Requests;
using LD.Infrastructure.Persistence;
using LD.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class EquipmentQuestionController : ControllerBase
{
    private readonly LdProyectDbContext _context;

    public EquipmentQuestionController(LdProyectDbContext context)
    {
        _context = context;
    }

    [HttpGet("equipment-type/{equipmentTypeId}")]
    [Permission(PermissionKeys.EquipmentType_View)]
    public async Task<IActionResult> GetByEquipmentType(int equipmentTypeId)
    {
        var questions = await _context.EquipmentQuestionDets
            .AsNoTracking()
            .Include(x => x.EquipmentQuestion)!
            .ThenInclude(x => x!.EquipmentType)
            .Where(x => x.EquipmentQuestion != null && x.EquipmentQuestion.EquipmentTypeId == equipmentTypeId)
            .OrderBy(x => x.EquipmentQuestionDetId)
            .Select(x => new EquipmentQuestionDto
            {
                EquipmentQuestionId = x.EquipmentQuestionId,
                EquipmentQuestionDetId = x.EquipmentQuestionDetId,
                EquipmentTypeId = x.EquipmentQuestion!.EquipmentTypeId,
                EquipmentTypeName = x.EquipmentQuestion.EquipmentType != null ? x.EquipmentQuestion.EquipmentType.EquipmentName : string.Empty,
                QuestionText = x.QuestionText,
                OptionAnswerText = x.OptionAnswerText,
                IsYesNo = x.IsYesNo
            })
            .ToListAsync();

        return ResultExtensions.ToActionResult(Result<List<EquipmentQuestionDto>?>.Success(questions, "Preguntas obtenidas correctamente"));
    }

    [HttpPost]
    [Permission(PermissionKeys.EquipmentType_Create)]
    public async Task<IActionResult> Save([FromBody] EquipmentQuestionRequest request)
    {
        try
        {
            if (request.EquipmentTypeId <= 0)
                return ResultExtensions.ToActionResult(Result<string>.Failure("Debes seleccionar un tipo de equipo.", new()));

            if (string.IsNullOrWhiteSpace(request.QuestionText))
                return ResultExtensions.ToActionResult(Result<string>.Failure("La pregunta es obligatoria.", new()));

            var equipmentQuestion = await _context.EquipmentQuestions
                .FirstOrDefaultAsync(x => x.EquipmentTypeId == request.EquipmentTypeId);

            if (equipmentQuestion is null)
            {
                equipmentQuestion = new EquipmentQuestion
                {
                    EquipmentTypeId = request.EquipmentTypeId
                };

                await _context.EquipmentQuestions.AddAsync(equipmentQuestion);
                await _context.SaveChangesAsync();
            }

            EquipmentQuestionDet questionDet;

            if (request.EquipmentQuestionDetId > 0)
            {
                questionDet = await _context.EquipmentQuestionDets
                    .FirstOrDefaultAsync(x => x.EquipmentQuestionDetId == request.EquipmentQuestionDetId && x.EquipmentQuestionId == equipmentQuestion.EquipmentQuestionId)
                    ?? throw new InvalidOperationException("No se encontró la pregunta a actualizar.");

                questionDet.QuestionText = request.QuestionText.Trim();
                questionDet.IsYesNo = request.IsYesNo;
                questionDet.OptionAnswerText = request.IsYesNo
                    ? null
                    : NormalizeOptions(request.OptionAnswerText);
            }
            else
            {
                questionDet = new EquipmentQuestionDet
                {
                    EquipmentQuestionId = equipmentQuestion.EquipmentQuestionId,
                    QuestionText = request.QuestionText.Trim(),
                    IsYesNo = request.IsYesNo,
                    OptionAnswerText = request.IsYesNo
                        ? null
                        : NormalizeOptions(request.OptionAnswerText)
                };

                await _context.EquipmentQuestionDets.AddAsync(questionDet);
            }

            await _context.SaveChangesAsync();

            return ResultExtensions.ToActionResult(Result<string>.Success(string.Empty, "Pregunta guardada correctamente."));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(Result<string>.Failure("No se pudo guardar la pregunta.", new() { ex.Message }));
        }
    }

    [HttpDelete("{equipmentQuestionDetId}")]
    [Permission(PermissionKeys.EquipmentType_Update)]
    public async Task<IActionResult> Delete(int equipmentQuestionDetId)
    {
        try
        {
            var questionDet = await _context.EquipmentQuestionDets
                .FirstOrDefaultAsync(x => x.EquipmentQuestionDetId == equipmentQuestionDetId);

            if (questionDet is null)
                return ResultExtensions.ToActionResult(Result<string>.Failure("No se encontró la pregunta.", new(), 404));

            var headerId = questionDet.EquipmentQuestionId;

            _context.EquipmentQuestionDets.Remove(questionDet);
            await _context.SaveChangesAsync();

            var hasDetails = await _context.EquipmentQuestionDets.AnyAsync(x => x.EquipmentQuestionId == headerId);
            if (!hasDetails)
            {
                var header = await _context.EquipmentQuestions.FirstOrDefaultAsync(x => x.EquipmentQuestionId == headerId);
                if (header is not null)
                {
                    _context.EquipmentQuestions.Remove(header);
                    await _context.SaveChangesAsync();
                }
            }

            return ResultExtensions.ToActionResult(Result<string>.Success(string.Empty, "Pregunta eliminada correctamente."));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(Result<string>.Failure("No se pudo eliminar la pregunta.", new() { ex.Message }));
        }
    }

    private static string? NormalizeOptions(string? options)
    {
        if (string.IsNullOrWhiteSpace(options))
            return null;

        var normalized = string.Join(Environment.NewLine,
            options.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x)));

        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
