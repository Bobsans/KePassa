namespace SecretStore.Model;

public class MasterPasswordWindowModel : BaseModel {
    public string Password { get; set; } = string.Empty;
    public string Confirm { get; set; } = string.Empty;

    public bool CanConfirm => Password.Length > 8 && Password == Confirm;
}
