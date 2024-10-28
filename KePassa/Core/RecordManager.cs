using System.IO;
using DimTim.Logging;
using MessagePack;
using SecretStore.Data;

namespace SecretStore.Core;

public class RecordManager(ILogger logger) {
    private const string FILE_NAME = "storage.sse";

    public List<Record> Load(string password) {
        var data = File.ReadAllBytes(FILE_NAME);
        var decrypted = Encryptor.Decrypt(data, password);
        var result = MessagePackSerializer.Deserialize<List<Record>>(decrypted);
        logger.Info($"Loaded {result.Count} records");
        return result;
    }

    public void Save(IEnumerable<Record> groups, string password) {
        var buffer = MessagePackSerializer.Serialize(groups);
        var encrypted = Encryptor.Encrypt(buffer, password);
        File.WriteAllBytes(FILE_NAME, encrypted);
        logger.Info("Records have been saved.");
    }
}
