namespace Common;

public class FileSystemProvider : IFileSystemProvider
{
    public bool Exists(string filename)
    {
        return File.Exists(filename);
    }

    public Stream Read(string filename)
    {
        return new FileStream(filename, FileMode.Open);
    }

    public Task WriteAsync(string filename, Stream stream)
    {
        _ = stream.Seek(0, SeekOrigin.Begin);

        using var writeStream = File.OpenWrite(filename);

        return stream.CopyToAsync(writeStream);
    }
}
