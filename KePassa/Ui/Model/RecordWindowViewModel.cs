namespace SecretStore.Ui.Model;

public class RecordWindowViewModel : BaseViewModel {
    private RecordViewModel _record = new();

    public RecordViewModel Record {
        get => _record;
        set => SetField(ref _record, value);
    }
}
