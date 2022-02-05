﻿namespace Common;

public class FileSystemProvider : IFileSystemProvider
{
    public bool Exists(string filename)
    {
        throw new NotImplementedException("Should be implemented by executor");
    }

    public Stream Read(string filename)
    {
        throw new NotImplementedException("Should be implemented by executor");
    }

    public Task WriteAsync(string filename, Stream stream)
    {
        throw new NotImplementedException("Should be implemented by executor");
    }
}
