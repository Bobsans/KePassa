using System.IO;
using MessagePack;

namespace SecretStore;

public class RecordManager {
    public IEnumerable<RecordGroup> Load() {
        var data = File.ReadAllBytes("storage.sse");
        var decrypted = Encryptor.Decrypt(data, "password");
        var result = MessagePackSerializer.Deserialize<IEnumerable<RecordGroup>>(decrypted);
        return result;
    }

    public void Save(IEnumerable<RecordGroup> records) {
        var buffer = MessagePackSerializer.Serialize(records);
        var encrypted = Encryptor.Encrypt(buffer, "password");
        File.WriteAllBytes("storage.sse", encrypted);
    }
}
