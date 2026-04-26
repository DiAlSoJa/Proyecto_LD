using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.EquipmentQuestion;
using LD.Contracts.EquipmentType;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.CheckList.ViewModels;

public partial class ConfiguracionPreguntasTabViewModel : ObservableObject
{
    private readonly EquipmentTypeService _equipmentTypeService;
    private readonly EquipmentQuestionService _equipmentQuestionService;

    [ObservableProperty]
    private EquipmentQuestionDto? selectedQuestion;

    [ObservableProperty]
    private bool canEditQuestion;

    [ObservableProperty]
    private bool canDeleteQuestion;

    public event Action<List<EquipmentTypeDto>>? OnTiposLoaded;
    public event Action<List<EquipmentQuestionDto>>? OnPreguntasLoaded;

    public ConfiguracionPreguntasTabViewModel(
        EquipmentTypeService equipmentTypeService,
        EquipmentQuestionService equipmentQuestionService)
    {
        _equipmentTypeService = equipmentTypeService;
        _equipmentQuestionService = equipmentQuestionService;
    }

    public async Task CargarTiposAsync()
    {
        try
        {
            var response = await _equipmentTypeService.GetEquipmentTypes();
            if (!response.IsSuccess || response.Data is null)
            {
                DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudieron cargar los tipos de equipo.");
                return;
            }
            var data = response.Data.OrderBy(x => x.EquipmentName).ToList();
            OnTiposLoaded?.Invoke(data);
            if (data.Count > 0)
                await CargarPreguntasAsync(data[0].EquipmentTypeId);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    public async Task CargarPreguntasAsync(int equipmentTypeId)
    {
        var response = await _equipmentQuestionService.GetByEquipmentType(equipmentTypeId);
        if (!response.IsSuccess || response.Data is null)
        {
            OnPreguntasLoaded?.Invoke(new List<EquipmentQuestionDto>());
            return;
        }
        var preguntas = response.Data.Select(x => new EquipmentQuestionDto
        {
            EquipmentQuestionId    = x.EquipmentQuestionId,
            EquipmentQuestionDetId = x.EquipmentQuestionDetId,
            EquipmentTypeId        = x.EquipmentTypeId,
            EquipmentTypeName      = x.EquipmentTypeName,
            QuestionText           = x.QuestionText,
            OptionAnswerText       = x.IsYesNo ? "Si / No" : (x.OptionAnswerText ?? string.Empty),
            IsYesNo                = x.IsYesNo
        }).ToList();
        OnPreguntasLoaded?.Invoke(preguntas);
    }

    public async Task<bool> GuardarPreguntaAsync(int equipmentTypeId, string questionText, bool isYesNo, string? options)
    {
        var request = new EquipmentQuestionRequest
        {
            EquipmentQuestionDetId = SelectedQuestion?.EquipmentQuestionDetId ?? 0,
            EquipmentTypeId        = equipmentTypeId,
            QuestionText           = questionText,
            IsYesNo                = isYesNo,
            OptionAnswerText       = options
        };
        var response = await _equipmentQuestionService.Save(request);
        if (!response.IsSuccess)
        {
            DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo guardar la pregunta.");
            return false;
        }
        DialogHelper.ShowSuccess(response.Message ?? "Pregunta guardada correctamente.");
        return true;
    }

    public async Task<bool> EliminarPreguntaAsync()
    {
        if (SelectedQuestion is null) return false;
        var response = await _equipmentQuestionService.Delete(SelectedQuestion.EquipmentQuestionDetId);
        if (!response.IsSuccess)
        {
            DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo eliminar la pregunta.");
            return false;
        }
        DialogHelper.ShowSuccess(response.Message ?? "Pregunta eliminada correctamente.");
        return true;
    }

    public void SetSelectedQuestion(EquipmentQuestionDto? question)
    {
        SelectedQuestion  = question;
        CanEditQuestion   = question is not null;
        CanDeleteQuestion = question is not null;
    }
}
