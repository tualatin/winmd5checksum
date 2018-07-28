using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using Org.Vs.WinMd5.Controllers.Interfaces;
using Org.Vs.WinMd5.Core.Utils;
using Org.Vs.WinMd5.Data;
using Org.Vs.WinMd5.UI.UserControls.DataModels;


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
    /// <param name="collection"><see cref="List{T}"/> of <see cref="VsDataGridHierarchialDataModel"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    public async Task<List<VsDataGridHierarchialDataModel>> StartCalculationAsync(List<VsDataGridHierarchialDataModel> collection, CancellationToken token)
    {
      LOG.Trace("Start calculation");

      await Task.Run(() =>
      {
        Parallel.ForEach(collection, hash =>
        {
          if ( !File.Exists((hash.Data as WinMdChecksumData)?.FileName) )
            return;

          var tasks = new List<Task>
          {
            new Task(() => CalulcateHash<MD5CryptoServiceProvider>(hash.Children
              .FirstOrDefault(p => (p.Data as WinMdChecksumData)?.FileName == HashNames.Md5)?.Data as WinMdChecksumData, (hash.Data as WinMdChecksumData)?.FileName), token),
            new Task(() => CalulcateHash<SHA1CryptoServiceProvider>(hash.Children
              .FirstOrDefault(p => (p.Data as WinMdChecksumData)?.FileName == HashNames.Sha1)?.Data as WinMdChecksumData, (hash.Data as WinMdChecksumData)?.FileName), token),
            new Task(() => CalulcateHash<SHA256CryptoServiceProvider>(hash.Children
              .FirstOrDefault(p => (p.Data as WinMdChecksumData)?.FileName == HashNames.Sha256)?.Data as WinMdChecksumData, (hash.Data as WinMdChecksumData)?.FileName), token),
            new Task(() => CalulcateHash<SHA384CryptoServiceProvider>(hash.Children
              .FirstOrDefault(p => (p.Data as WinMdChecksumData)?.FileName == HashNames.Sha384)?.Data as WinMdChecksumData, (hash.Data as WinMdChecksumData)?.FileName), token),
            new Task(() => CalulcateHash<SHA512CryptoServiceProvider>(hash.Children
              .FirstOrDefault(p => (p.Data as WinMdChecksumData)?.FileName == HashNames.Sha512)?.Data as WinMdChecksumData, (hash.Data as WinMdChecksumData)?.FileName), token)
          };

          Parallel.ForEach(tasks, task =>
          {
            task.Start();
          });

          Task.WaitAll(tasks.ToArray(), token);
        });
      }, token).ConfigureAwait(false);

      return collection;
    }

    private string CompareHash(string result, string expected)
    {
      string ok = Application.Current.TryFindResource("HashCompareOk").ToString();
      string failed = Application.Current.TryFindResource("HashCompareFailed").ToString();

      return string.Compare(result, expected, StringComparison.OrdinalIgnoreCase) == 0 ? ok : failed;
    }

    private void CalulcateHash<T>(WinMdChecksumData data, string fileName) where T : HashAlgorithm, new()
    {
      if ( !data.HashIsEnabled )
      {
        data.Hash = string.Empty;
        return;
      }

      if ( string.IsNullOrWhiteSpace(data.Hash) )
        data.Hash = HashOf<T>(fileName);
      else if ( data.Hash.Any(char.IsLower) && EnvironmentContainer.Instance.CurrentSettings.UpperCaseHash )
        data.Hash = data.Hash.ToUpper();
      else if ( data.Hash.Any(char.IsUpper) && !EnvironmentContainer.Instance.CurrentSettings.UpperCaseHash )
        data.Hash = data.Hash.ToLower();

      data.Result = !string.IsNullOrWhiteSpace(data.HashToCompare) ? CompareHash(data.Hash, data.HashToCompare) : string.Empty;
    }

    private string HashOf<T>(string fileName) where T : HashAlgorithm, new()
    {
      using ( var input = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read) )
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
            LOG.Debug(@"HashOf<{3}> {0}MB/{1}MB. Memory Used: {2}MB", bytesTransfered / 1000000, input.Length / 1000000,
              process.PrivateMemorySize64 / 1000000, provider.ToString());
          }

          string hashString = EnvironmentContainer.Instance.CurrentSettings.UpperCaseHash ?
            BitConverter.ToString(provider.Hash).Replace("-", string.Empty).ToUpper() :
            BitConverter.ToString(provider.Hash).Replace("-", string.Empty).ToLower();

          provider.Clear();
          input.Close();

          return hashString;
        }
      }
    }
  }
}
