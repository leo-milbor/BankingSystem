using BankingSystem;

namespace BankingSystemTests
{
    internal class ReaderMock : IReader
    {
        private Queue<string> _values;

        public ReaderMock(params string[] values) => _values = new Queue<string>(values);

        public string Read() => _values.TryDequeue(out var v) ? v : "";
    }
    internal class StringWriter : IWriter
    {
        private readonly List<string> _outputs = new();

        public void Write(string value)
        {
            _outputs.Add(value);
        }

        public string GetOutputNumber(int outputNumber) => _outputs[outputNumber - 1];
    }

    internal class ReadWriterMock : IReadWriter
    {
        private readonly IReader _reader;
        private readonly IWriter _writer;

        public ReadWriterMock(IReader reader, IWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public string Read() => _reader.Read();

        public void Write(string value) => _writer.Write(value);
    }
}
