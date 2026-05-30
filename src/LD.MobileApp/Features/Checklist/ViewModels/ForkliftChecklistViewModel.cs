using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.Checklist;
using LD.Contracts.Equipment;
using LD.Contracts.Responses;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.Views.Controls;
using MvvmHelpers.Commands;
using Plugin.Maui.OCR;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class ForkliftChecklistViewModel : ObservableObject
{
    private readonly EquipmentQuestionService _equipmentQuestionService;
    private readonly ChecklistService _checklistService;
    private readonly IDialogService _dialogService;
    public IAsyncRelayCommand EscanearHorometroCommand { get; }

    [ObservableProperty]
    private ObservableCollection<ChecklistSection> sections = new();

    [ObservableProperty]
    private EquipmentDto? equipment;

    [ObservableProperty]
    private bool isSaving;

    [ObservableProperty]
    private string statusSubida = string.Empty;

    [ObservableProperty]
    private string horometro = string.Empty;

    [ObservableProperty]
    private string operador = string.Empty;

    [ObservableProperty]
    private string equipo = string.Empty;

    public ObservableCollection<ChecklistPhotoItem> Photos { get; } = new();
    public ICommand RemovePhotoCommand { get; }
    public ICommand ViewPhotoCommand { get; }

    public ForkliftChecklistViewModel(
        EquipmentQuestionService equipmentQuestionService,
        ChecklistService checklistService,
        IDialogService dialogService)
    {
        _equipmentQuestionService = equipmentQuestionService;
        _checklistService         = checklistService;
        _dialogService            = dialogService;
        EscanearHorometroCommand  = new AsyncRelayCommand(EscanearHorometroAsync);

        RemovePhotoCommand = new MvvmHelpers.Commands.Command<ChecklistPhotoItem>(item =>
        {
            if (item is null) return;
            Photos.Remove(item);
            for (int i = 0; i < Photos.Count; i++)
                Photos[i].Order = i;
        });

        ViewPhotoCommand = new MvvmHelpers.Commands.Command<ChecklistPhotoItem>(item =>
        {
            if (item?.Source is null) return;
            (Shell.Current.CurrentPage ?? Application.Current?.MainPage)
                ?.ShowPopup(new ImagePreviewPopup(item.Source));
        });
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
            _dialogService.ShowBlocking("Cargando", "Obteniendo preguntas del checklist...");
            var response = await _equipmentQuestionService.GetByEquipmentType(Equipment.EquipmentTypeId);
            if (!response.IsSuccess || response.Data is null)
            {
                await _dialogService.ShowErrorAsync("Error",
                    response.ErrorMessage ?? "No se pudieron cargar las preguntas.");
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
            _dialogService.HideBlocking();
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
                await _dialogService.ShowErrorAsync("OCR", "No se pudo leer texto en la imagen.");
                return;
            }

            var match = Regex.Match(result.AllText, @"\d+[\.,]?\d*");
            if (match.Success)
                Horometro = match.Value;
            else
                await _dialogService.ShowErrorAsync("OCR", "No se encontró un número en la imagen.");
        }
        catch (PermissionException)
        {
            await _dialogService.ShowErrorAsync("Permiso requerido", "Se necesita acceso a la cámara para leer el horómetro.");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", $"No se pudo procesar la imagen: {ex.Message}");
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

public class ChecklistPhotoItem
{
    public byte[] Bytes { get; set; } = Array.Empty<byte>();
    public string Side { get; set; } = "custom";
    public int Order { get; set; }
    public ImageSource? Source { get; set; }
}
