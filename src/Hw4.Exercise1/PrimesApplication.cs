using Common;
namespace Hw4.Exercise1;

public sealed class PrimesApplication
{
    private readonly IFileSystemProvider _fileSystemProvider;

    public PrimesApplication(IFileSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
    }

    /// <summary>
    /// Runs application.
    /// </summary>
    public ReturnCode Run()
    {
        throw new NotImplementedException("Should be implemented by executor");
    }
}
