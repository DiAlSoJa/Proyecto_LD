using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Checklist;
using LD.Contracts.Equipment;
using LD.Contracts.Responses;
using MauiAppLogin.Models;
using System.Collections.ObjectModel;

namespace MauiAppLogin.ViewModels;

public partial class ForkliftChecklistViewModel : ObservableObject
{
    private readonly EquipmentQuestionService _equipmentQuestionService;
    private readonly ChecklistService _checklistService;

    [ObservableProperty]
    private ObservableCollection<ChecklistSection> sections = new();

    [ObservableProperty]
    private EquipmentDto? equipment;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isSaving;

    [ObservableProperty]
    private string statusSubida = string.Empty;

    public ForkliftChecklistViewModel(
        EquipmentQuestionService equipmentQuestionService,
        ChecklistService checklistService)
    {
        _equipmentQuestionService = equipmentQuestionService;
        _checklistService         = checklistService;
    }

    public async Task InicializarAsync(EquipmentDto equipmentData)
    {
        Equipment = equipmentData;
        await CargarPreguntasAsync();
    }

    private async Task CargarPreguntasAsync()
    {
        if (Equipment is null) return;
        try
        {
            IsLoading = true;
            var response = await _equipmentQuestionService.GetByEquipmentType(Equipment.EquipmentTypeId);
            if (!response.IsSuccess || response.Data is null) return;

            Sections.Clear();
            var section = new ChecklistSection { Title = Equipment.Tipo };
            foreach (var q in response.Data)
            {
                var question = new ChecklistQuestion { QuestionId = q.EquipmentQuestionDetId, Label = q.QuestionText };
                if (q.IsYesNo)
                {
                    question.Options.Add("Sí");
                    question.Options.Add("No");
                }
                else if (!string.IsNullOrWhiteSpace(q.OptionAnswerText))
                {
                    foreach (var opt in q.OptionAnswerText
                        .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x)))
                    {
                        question.Options.Add(opt);
                    }
                }
                section.Questions.Add(question);
            }
            Sections.Add(section);
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Sube foto desde bytes capturados por la cámara.
    public async Task<ApiResponseDto<EquipmentImageUploadDto>> UploadPhotoAsync(
        byte[] bytes, string fileName, string side)
    {
        return await _checklistService.UploadPhotoAsync(bytes, fileName, side);
    }

    // Envía el checklist completo al API.
    public async Task<(bool ok, string message)> SubmitAsync(SubmitChecklistRequest request)
    {
        var result = await _checklistService.SubmitAsync(request);
        return (result.IsSuccess,
                result.IsSuccess
                    ? "Checklist guardado correctamente."
                    : result.ErrorMessage ?? "Error al guardar el checklist.");
    }
}
