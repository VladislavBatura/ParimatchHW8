using Common;
namespace Hw4.Exercise2;

public class CryptoApplication
{
    private readonly IFileSystemProvider _fileSystemProvider;

    public CryptoApplication(IFileSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
    }

    /// <summary>
    /// Runs crypto application.
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>
    /// Returns <see cref="ReturnCode.Success"/> in case of successful encrypt/decrypt process.
    /// Returns <see cref="ReturnCode.InvalidArgs"/> in case of invalid <paramref name="args"/>.
    /// </returns>
    public ReturnCode Run(string[] args)
    {
        throw new NotImplementedException("Should be implemented by executor");
    }
}
