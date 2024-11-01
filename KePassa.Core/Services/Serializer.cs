using KePassa.Core.Abstraction;
using KePassa.Core.Data;
using MessagePack;
using MessagePack.Formatters;

namespace KePassa.Core.Services;

public class Serializer {
    private static readonly IFormatterResolver _resolver = MessagePack.Resolvers.CompositeResolver.Create(
        [new RecordFormatter()],
        [MessagePack.Resolvers.ContractlessStandardResolver.Instance]
    );

    private readonly MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard.WithResolver(_resolver);

    public byte[] Serialize<T>(T value) {
        return MessagePackSerializer.Serialize(value, options);
    }

    public T Deserialize<T>(byte[] bytes) {
        return MessagePackSerializer.Deserialize<T>(bytes, options);
    }

    private class RecordFormatter : IMessagePackFormatter<IRecord> {
        public void Serialize(ref MessagePackWriter writer, IRecord value, MessagePackSerializerOptions options) {
            switch (value) {
                case Record record:
                    writer.WriteUInt8(0);
                    MessagePackSerializer.Serialize(ref writer, record, options);
                    break;
                case RecordCategory category:
                    writer.WriteUInt8(1);
                    MessagePackSerializer.Serialize(ref writer, category, options);
                    break;
            }
        }

        public IRecord Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options) {
            var id = reader.ReadByte();
            return id switch {
                0 => MessagePackSerializer.Deserialize<Record>(ref reader, options),
                1 => MessagePackSerializer.Deserialize<RecordCategory>(ref reader, options),
                _ => throw new MessagePackSerializationException("Invalid record type")
            };
        }
    }
}
