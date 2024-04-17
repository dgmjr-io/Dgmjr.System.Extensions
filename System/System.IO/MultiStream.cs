namespace System.IO;

public class MultiStream(params System.IO.Stream[] streams) : Stream
{
    private T ForEach<T>(Func<Stream, T> action)
    {
        var results = new List<T>(streams.Length);
        foreach (var stream in streams)
        {
            results.Add(action(stream));
        }
        return results[^1];
    }

    private void ForEach(Action<Stream> action)
    {
        ForEach(stream =>
        {
            action(stream);
            return 0;
        });
    }

    public override bool CanRead => TrueForAll(streams, s => s.CanRead);

    public override bool CanSeek => TrueForAll(streams, s => s.CanSeek);

    public override bool CanWrite => TrueForAll(streams, s => s.CanWrite);

    public override long Length => streams.FirstOrDefault()?.Length ?? 0;

    public override long Position
    {
        get => streams.FirstOrDefault()?.Position ?? 0;
        set => ForEach(s => s.Position = value);
    }

    public override void Flush() => ForEach(s => s.Flush());

    public override int Read(byte[] buffer, int offset, int count) =>
        ForEach(s => s.Read(new byte[buffer.Length], offset, count));

    public override long Seek(long offset, SeekOrigin origin) =>
        ForEach(s => s.Seek(offset, origin));

    public override void SetLength(long value) => ForEach(s => s.SetLength(value));

    public override void Write(byte[] buffer, int offset, int count) =>
        ForEach(s => s.Write(buffer, offset, count));
}
