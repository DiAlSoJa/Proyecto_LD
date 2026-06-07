using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.Checklist;
using LD.Contracts.Equipment;
using LD.Contracts.Responses;
using MauiAppLogin.Models;
using Plugin.Maui.OCR;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace MauiAppLogin.ViewModels;

public partial class ForkliftChecklistViewModel : ObservableObject
{
    private readonly EquipmentQuestionService _equipmentQuestionService;
    private readonly ChecklistService _checklistService;
    public IAsyncRelayCommand EscanearHorometroCommand { get; }

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

    [ObservableProperty]
    private string horometro = string.Empty;

    public ForkliftChecklistViewModel(
        EquipmentQuestionService equipmentQuestionService,
        ChecklistService checklistService)
    {
        _equipmentQuestionService = equipmentQuestionService;
        _checklistService         = checklistService;
        EscanearHorometroCommand  = new AsyncRelayCommand(EscanearHorometroAsync);
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
            if (!response.IsSuccess || response.Data is null)
            {
                await Shell.Current.DisplayAlertAsync("Error",
                    response.ErrorMessage ?? "No se pudieron cargar las preguntas.", "OK");
                return;
            }

            Sections.Clear();
            var section = new ChecklistSection { Title = Equipment.Tipo };
            foreach (var q in response.Data)
            {
                var question = new ChecklistQuestion { QuestionId = q.EquipmentQuestionDetId, Label = q.QuestionText };
                if (q.IsYesNo)
                {
                    question.AddOption("Sí");
                    question.AddOption("No");
                }
                else if (!string.IsNullOrWhiteSpace(q.OptionAnswerText))
                {
                    foreach (var opt in q.OptionAnswerText
                        .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x)))
                    {
                        question.AddOption(opt);
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

    private async Task EscanearHorometroAsync()
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo is null) return;

            using var stream = await photo.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var imageBytes = ms.ToArray();

            var result = await OcrPlugin.Default.RecognizeTextAsync(imageBytes);
            if (!result.Success || string.IsNullOrWhiteSpace(result.AllText))
            {
                await Shell.Current.DisplayAlertAsync("OCR", "No se pudo leer texto en la imagen.", "OK");
                return;
            }

            var match = Regex.Match(result.AllText, @"\d+[\.,]?\d*");
            if (match.Success)
                Horometro = match.Value;
            else
                await Shell.Current.DisplayAlertAsync("OCR", "No se encontró un número en la imagen.", "OK");
        }
        catch (PermissionException)
        {
            await Shell.Current.DisplayAlertAsync("Permiso requerido", "Se necesita acceso a la cámara para leer el horómetro.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo procesar la imagen: {ex.Message}", "OK");
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
