using System.Collections.ObjectModel;
using MauiAppLogin.Models;

namespace MauiAppLogin.ViewModels;

public class ForkliftChecklistViewModel
{
    public ObservableCollection<ChecklistSection> Sections { get; set; } = new();

    public ForkliftChecklistViewModel()
    {
        Sections = new ObservableCollection<ChecklistSection>
        {
            new ChecklistSection
            {
                Title = "Fugas de aceite",
                Questions =
                {
                    new ChecklistQuestion
                    {
                        Label = "Fugas de aceite",
                        Options = new ObservableCollection<string> { "Sí", "No" }
                    }
                }
            },

            new ChecklistSection
            {
                Title = "Frenos",
                Questions =
                {
                    new ChecklistQuestion
                    {
                        Label = "Funcionan",
                        Options = new ObservableCollection<string> { "Sí", "No" }
                    }
                }
            },

            new ChecklistSection
            {
                Title = "Equipo de seguridad",
                Questions =
                {
                    new ChecklistQuestion
                    {
                        Label = "Extintor",
                        Options = new ObservableCollection<string> { "Sí", "No" }
                    },
                    new ChecklistQuestion
                    {
                        Label = "Claxon",
                        Options = new ObservableCollection<string> { "Sí", "No" }
                    },
                    new ChecklistQuestion
                    {
                        Label = "Correa antiestática",
                        Options = new ObservableCollection<string> { "Sí", "No" }
                    }
                }
            },

            new ChecklistSection
            {
                Title = "Baterías",
                Questions =
                {
                    new ChecklistQuestion
                    {
                        Label = "Conector de batería",
                        Options = new ObservableCollection<string>
                        {
                            "Funcionando",
                            "Dañado",
                            "Roto"
                        }
                    }
                }
            }
        };
    }
}