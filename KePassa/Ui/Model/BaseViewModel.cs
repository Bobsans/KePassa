using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace SecretStore.Ui.Model;

[AddINotifyPropertyChangedInterface]
public abstract class BaseViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void OnPropertyChanged(string propertyName, object before, object after) {
        Console.WriteLine($"OPC {propertyName} {before} {after}");
        OnPropertyChanged(propertyName);
    }
}
