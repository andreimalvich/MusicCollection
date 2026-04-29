using Avalonia.Controls;
using System.Threading.Tasks;

namespace MusicCollection.AvaloniaUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public async Task<bool> ConfirmDelete(string message)
    {
        var dialog = new Window
        {
            Title = "Подтверждение",
            Width = 350,
            Height = 160,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            SizeToContent = SizeToContent.Height,
            Padding = new Avalonia.Thickness(20),
            Content = new StackPanel
            {
                Spacing = 20,
                Children =
            {
                new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap },
                new StackPanel
                {
                    Orientation = Avalonia.Layout.Orientation.Horizontal,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                    Spacing = 10
                }
            }
            }
        };

        var btnStack = (StackPanel)((StackPanel)dialog.Content).Children[1];

        var btnYes = new Button { Content = "Да", Width = 60 };
        // Правильная подписка на событие в коде
        btnYes.Click += (s, e) => dialog.Close(true);

        var btnNo = new Button { Content = "Нет", Width = 60 };
        btnNo.Click += (s, e) => dialog.Close(false);

        btnStack.Children.Add(btnYes);
        btnStack.Children.Add(btnNo);

        return await dialog.ShowDialog<bool>(this);
    }

    private void MinBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void MaxBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized
               ? WindowState.Normal
               : WindowState.Maximized;
    }

    private void CloseBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => Close();

    private void TitleBar_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }
}