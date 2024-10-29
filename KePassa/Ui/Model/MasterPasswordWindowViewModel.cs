namespace SecretStore.Ui.Model;

public class MasterPasswordWindowViewModel : BaseViewModel {
    private string _password = string.Empty;
    private string _confirm = string.Empty;

    public string Password {
        get => _password;
        set {
            SetField(ref _password, value);
            OnPropertyChanged(nameof(CanConfirm));
        }
    }

    public string Confirm {
        get => _confirm;
        set {
            SetField(ref _confirm, value);
            OnPropertyChanged(nameof(CanConfirm));
        }
    }

    public bool CanConfirm => Password.Length > 8 && Password == Confirm;
}
