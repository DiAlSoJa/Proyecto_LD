using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class PickingPage : ContentPage
{


    public PickingPage()
    {
        InitializeComponent();


    }
    private async void OnPickedClicked(object sender, EventArgs e)
    {
        var parameters = new Dictionary<string, object>
       {
           { "TextInformation", "202010120203 - G21A" }
       };
        await Shell.Current.GoToAsync("ChangeLocationPage", parameters);
      

    }
    private void OnFiltroChanged(object sender, TextChangedEventArgs e)
    {
        var text = (e.NewTextValue ?? "").Trim().ToLowerInvariant();

      
    }


}
  