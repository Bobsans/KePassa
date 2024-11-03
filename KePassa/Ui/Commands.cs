using System.Windows.Input;

namespace SecretStore.Ui;

public static class Commands {
    public static readonly RoutedUICommand Settings = new("Settings", "Settings", typeof(Commands), new InputGestureCollection {
        new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Alt)
    });
}
