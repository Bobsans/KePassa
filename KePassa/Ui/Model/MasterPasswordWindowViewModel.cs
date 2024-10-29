namespace SecretStore.Ui.Model;

public class MasterPasswordWindowViewModel : BaseViewModel {
    public string Password { get; set; } = string.Empty;
    public string Confirm { get; set; } = string.Empty;

    public bool CanConfirm => Password.Length > 8 && Password == Confirm;
}
