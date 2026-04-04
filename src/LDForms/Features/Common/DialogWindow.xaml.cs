using System.Windows;

namespace LD.FormsX.Views.Common
{
    public partial class DialogWindow : Window
    {
        public DialogWindow(UIElement content)
        {
            InitializeComponent();
            ContentHost.Content = content;
        }
    }
}