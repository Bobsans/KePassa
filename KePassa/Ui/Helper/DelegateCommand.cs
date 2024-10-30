using System.Windows.Input;

namespace SecretStore.Ui.Helper;

public class DelegateCommand<T>(Action<T> execute, Predicate<T>? canExecute = null) : ICommand {
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => canExecute is null || canExecute((T)parameter!);
    public void Execute(object? parameter) => execute((T)parameter!);
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public class DelegateCommand(Action<object?> execute, Predicate<object?>? canExecute = null) : DelegateCommand<object?>(execute, canExecute);
