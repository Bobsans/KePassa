using System.IO;
using MessagePack;
using SecretStore.Data;

namespace SecretStore.Core;

public class RecordManager {
    private const string FILE_NAME = "storage.sse";

    public List<RecordGroup> Load(string password) {
        var data = File.ReadAllBytes(FILE_NAME);
        var decrypted = Encryptor.Decrypt(data, password);
        return MessagePackSerializer.Deserialize<List<RecordGroup>>(decrypted);
    }

    public void Save(IEnumerable<RecordGroup> groups, string password) {
        var buffer = MessagePackSerializer.Serialize(groups);
        var encrypted = Encryptor.Encrypt(buffer, password);
        File.WriteAllBytes(FILE_NAME, encrypted);
    }
}
