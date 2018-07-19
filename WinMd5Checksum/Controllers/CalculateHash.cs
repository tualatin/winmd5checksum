using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using Org.Vs.WinMd5.Controllers.Interfaces;
using Org.Vs.WinMd5.Data;


namespace Org.Vs.WinMd5.Controllers
{
  /// <summary>
  /// CalculateHashsum
  /// </summary>
  public class CalculateHash : ICalculateHash
  {
    private static readonly ILog LOG = LogManager.GetLogger(typeof(CalculateHash));

    /// <summary>
    /// Start hash calculation of data collection
    /// </summary>
    /// <param name="collection"><see cref="ObservableCollection{T}"/> of <see cref="WinMdChecksumData"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    public async Task<ObservableCollection<WinMdChecksumData>> StartCalculationAsync(ObservableCollection<WinMdChecksumData> collection, CancellationToken token)
    {
      LOG.Trace("Start calculation");

      foreach ( WinMdChecksumData hash in collection )
      {
        if ( !File.Exists(hash.FileName) )
          continue;

        using ( var fs = new FileStream(hash.FileName, FileMode.Open) )
        {
          await CalculateMd5HashAsync(hash, fs, token).ConfigureAwait(false);
          await CalculateSha1HashAsync(hash, fs, token).ConfigureAwait(false);
          await CalculateSha256HashAsnyc(hash, fs, token).ConfigureAwait(false);
          await CalcaulteSha512HashAsync(hash, fs, token).ConfigureAwait(false);

          fs.Close();
        }
      }
      return collection;
    }

    private string CompareHash(string result, string expected)
    {
      string ok = Application.Current.TryFindResource("HashCompareOk").ToString();
      string failed = Application.Current.TryFindResource("HashCompareFailed").ToString();

      return string.Compare(result, expected, StringComparison.OrdinalIgnoreCase) == 0 ? ok : failed;
    }

    /// <summary>
    /// MD5
    /// </summary>
    /// <param name="data"><see cref="WinMdChecksumData"/></param>
    /// <param name="input"><see cref="Stream"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    private async Task CalculateMd5HashAsync(WinMdChecksumData data, Stream input, CancellationToken token)
    {
      await Task.Run(() =>
      {
        if ( !data.Md5IsEnabled || input == null )
        {
          data.Md5Hash = string.Empty;
          return;
        }

        if ( string.IsNullOrWhiteSpace(data.Md5Hash) )
          data.Md5Hash = HashOf<MD5CryptoServiceProvider>(input);

        if ( !string.IsNullOrWhiteSpace(data.Md5ToCompareHash) )
          data.Md5Result = CompareHash(data.Md5Hash, data.Md5ToCompareHash);

      }, token).ConfigureAwait(false);
    }

    /// <summary>
    /// SHA1
    /// </summary>
    /// <param name="data"><see cref="WinMdChecksumData"/></param>
    /// <param name="input"><see cref="Stream"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    private async Task CalculateSha1HashAsync(WinMdChecksumData data, Stream input, CancellationToken token)
    {
      await Task.Run(() =>
      {
        if ( !data.Sha1IsEnabled || input == null )
        {
          data.Sha1Hash = string.Empty;
          return;
        }

        if ( string.IsNullOrWhiteSpace(data.Sha1Hash) )
          data.Sha1Hash = HashOf<SHA1CryptoServiceProvider>(input);

        if ( !string.IsNullOrWhiteSpace(data.Sha1ToCompare) )
          data.Sha1Result = CompareHash(data.Sha1Hash, data.Sha1ToCompare);

      }, token).ConfigureAwait(false);
    }

    /// <summary>
    /// SHA256
    /// </summary>
    /// <param name="data"><see cref="WinMdChecksumData"/></param>
    /// <param name="input"><see cref="Stream"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    private async Task CalculateSha256HashAsnyc(WinMdChecksumData data, Stream input, CancellationToken token)
    {
      await Task.Run(() =>
      {
        if ( !data.Sha256IsEnabled || input == null )
        {
          data.Sha256Hash = string.Empty;
          return;
        }

        if ( string.IsNullOrWhiteSpace(data.Sha256Hash) )
          data.Sha256Hash = HashOf<SHA256CryptoServiceProvider>(input);

        if ( !string.IsNullOrWhiteSpace(data.Sha256ToCompare) )
          data.Sha256Result = CompareHash(data.Sha256Hash, data.Sha256ToCompare);

      }, token).ConfigureAwait(false);
    }

    /// <summary>
    /// SHA512
    /// </summary>
    /// <param name="data"><see cref="WinMdChecksumData"/></param>
    /// <param name="input"><see cref="Stream"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    private async Task CalcaulteSha512HashAsync(WinMdChecksumData data, Stream input, CancellationToken token)
    {
      await Task.Run(() =>
      {
        if ( !data.Sha512IsEnabled || input == null )
        {
          data.Sha512Hash = string.Empty;
          return;
        }

        if ( string.IsNullOrWhiteSpace(data.Sha512Hash) )
          data.Sha512Hash = HashOf<SHA512CryptoServiceProvider>(input);

        if ( !string.IsNullOrWhiteSpace(data.Sha512ToCompare) )
          data.Sha512Result = CompareHash(data.Sha512Hash, data.Sha512ToCompare);

      }, token).ConfigureAwait(false);
    }

    private string HashOf<T>(Stream input) where T : HashAlgorithm, new()
    {
      const int bufferSize = 1024 * 1024 * 20;
      System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
      input.Position = 0;

      using ( var provider = new T() )
      {
        int readCount;
        long bytesTransfered = 0;
        var buffer = new byte[bufferSize];

        while ( (readCount = input.Read(buffer, 0, buffer.Length)) != 0 )
        {
          if ( bytesTransfered + readCount == input.Length )
            provider.TransformFinalBlock(buffer, 0, readCount);
          else
            provider.TransformBlock(buffer, 0, bufferSize, buffer, 0);

          bytesTransfered += readCount;
          LOG.Debug(@"HashOf<{3}> {0}MB/{1}MB. Memory Used: {2}MB", bytesTransfered / 1000000, input.Length / 1000000, process.PrivateMemorySize64 / 1000000, provider.ToString());
        }

        string hashString = BitConverter.ToString(provider.Hash).Replace("-", string.Empty).ToLower();
        provider.Clear();

        return hashString;
      }
    }
  }
}
