using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Equipment;
using MauiAppLogin.Models;
using System.Collections.ObjectModel;

namespace MauiAppLogin.ViewModels;

public partial class ForkliftChecklistViewModel : ObservableObject
{
    private readonly EquipmentQuestionService _equipmentQuestionService;

    [ObservableProperty]
    private ObservableCollection<ChecklistSection> sections = new();

    [ObservableProperty]
    private EquipmentDto? equipment;

    [ObservableProperty]
    private bool isLoading;

    public ForkliftChecklistViewModel(EquipmentQuestionService equipmentQuestionService)
    {
        _equipmentQuestionService = equipmentQuestionService;
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
                var question = new ChecklistQuestion { Label = q.QuestionText };
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
}
