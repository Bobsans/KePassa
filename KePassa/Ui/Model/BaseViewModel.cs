using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace SecretStore.Ui.Model;

[AddINotifyPropertyChangedInterface]
public abstract class BaseViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        Console.WriteLine($"SF {propertyName}");
        if (EqualityComparer<T>.Default.Equals(field, value)) {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected Reactive<T> ReactiveProperty<T>(T? value, [CallerMemberName] string? propertyName = null) {
        Console.WriteLine($"RP {propertyName}");
        return new Reactive<T>(this, value, propertyName);
    }

    public class Reactive<T>(BaseViewModel vm, T value, [CallerMemberName] string? propertyName = null) {
        private T _value = value;

        public T Value {
            get => _value;
            set => vm.SetField(ref _value, value, propertyName);
        }
    }
}
