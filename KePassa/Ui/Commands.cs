using System.Windows.Input;

namespace SecretStore.Ui;

public static class Commands {
    public static readonly RoutedUICommand Settings = new("Settings", "Settings", typeof(Commands), new InputGestureCollection {
        new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Alt)
    });

    public static readonly RoutedUICommand AddCategory = new("Add category", "AddCategory", typeof(Commands));
    public static readonly RoutedUICommand AddRecord = new("Add record", "AddRecord", typeof(Commands));
    public static readonly RoutedUICommand EditRecord = new("Edit record", "EditRecord", typeof(Commands));
    public static readonly RoutedUICommand DeleteRecord = new("Delete record", "DeleteRecord", typeof(Commands));
}
