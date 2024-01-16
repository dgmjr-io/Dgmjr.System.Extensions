using System;

namespace System.IO;

public class MultiWriter : System.IO.TextWriter
{
    private readonly System.IO.TextWriter[] _writers;

    public override System.Text.Encoding Encoding => _writers[0].Encoding;

    public MultiWriter(params System.IO.TextWriter[] writers)
    {
        if (writers == null)
        {
            throw new ArgumentNullException(nameof(writers));
        }

        if (writers.Length == 0)
        {
            throw new ArgumentException("Must have at least one writer", nameof(writers));
        }

        _writers = writers;
    }

    public override void Write(char[] buffer, int index, int count)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        if (index < 0 || index >= buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (count < 0 || count > buffer.Length - index)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        foreach (var writer in _writers)
        {
            writer.Write(buffer, index, count);
        }
    }
}
