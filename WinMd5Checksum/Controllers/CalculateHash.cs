using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Org.Vs.WinMd5.Data;
using Org.Vs.WinMd5.Interfaces;


namespace Org.Vs.WinMd5.Controllers
{
  /// <summary>
  /// CalculateHashsum
  /// </summary>
  public class CalculateHash : ICalculateHash
  {
    private static readonly ILog LOG = LogManager.GetLogger(typeof(CalculateHash));

    private ObservableCollection<WinMdChecksumData> _collection;

    /// <summary>
    /// Start hash calculation of data collection
    /// </summary>
    /// <param name="collection"><see cref="ObservableCollection{T}"/> of <see cref="WinMdChecksumData"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    public async Task StartCalculationAsync(ObservableCollection<WinMdChecksumData> collection, CancellationToken token)
    {
      _collection = collection;

      var tasks = new List<Task>(4)
      {
        new Task(async () => await CalculateMd5HashAsync(), token),
        new Task(async () => await CalculateSha1HashAsync(), token),
        new Task(async () => await CaclulateSha256HashAsnyc(), token),
        new Task(async () => await CalcaulteSha512HashAsync(), token)
      };

      Parallel.ForEach(tasks, task =>
      {
        task.Start();
      });

      await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    /// <summary>
    /// MD5
    /// </summary>
    /// <returns>Task</returns>
    private async Task CalculateMd5HashAsync()
    {
      Parallel.ForEach(_collection, data =>
      {

      });
    }

    /// <summary>
    /// SHA1
    /// </summary>
    /// <returns>Task</returns>
    private async Task CalculateSha1HashAsync()
    {
      Parallel.ForEach(_collection, data =>
      {

      });
    }

    /// <summary>
    /// SHA256
    /// </summary>
    /// <returns>Task</returns>
    private async Task CaclulateSha256HashAsnyc()
    {
      Parallel.ForEach(_collection, data =>
      {

      });
    }

    /// <summary>
    /// SHA512
    /// </summary>
    /// <returns>Task</returns>
    private async Task CalcaulteSha512HashAsync()
    {
      Parallel.ForEach(_collection, data =>
      {

      });
    }

    private string GetMd5HashOf(Stream input) => HashOf<MD5CryptoServiceProvider>(input);

    private string GetSha1HashOf(Stream input) => HashOf<SHA1CryptoServiceProvider>(input);

    private string GetSha256HashOf(Stream input) => HashOf<SHA256CryptoServiceProvider>(input);

    private string GetSha512HashOf(Stream input) => HashOf<SHA512CryptoServiceProvider>(input);

    private string HashOf<T>(Stream input) where T : HashAlgorithm, new()
    {
      const int bufferSize = 1024 * 1024 * 20;
      string hashString;
      System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();

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

        hashString = BitConverter.ToString(provider.Hash).Replace("-", string.Empty).ToLower();
        provider.Clear();
      }
      return hashString;
    }
  }
}
