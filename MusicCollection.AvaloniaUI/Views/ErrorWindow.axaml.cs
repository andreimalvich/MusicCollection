using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MusicCollection.AvaloniaUI;

public partial class ErrorWindow : Window
{
    public ErrorWindow()
    {
        InitializeComponent();
    }

    public void SetMessage(string message) => MessageBlock.Text = message;

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
