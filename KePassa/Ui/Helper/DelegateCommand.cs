using System.Windows.Input;

namespace SecretStore.Ui.Helper;

public class DelegateCommand(Action<object?> execute, Predicate<object?>? canExecute = null) : ICommand {
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) {
        return canExecute is not null || canExecute!(parameter);
    }

    public void Execute(object? parameter) {
        execute(parameter);
    }

    public void RaiseCanExecuteChanged() {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
