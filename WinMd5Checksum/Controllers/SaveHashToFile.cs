using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Org.Vs.WinMd5.Controllers.Interfaces;
using Org.Vs.WinMd5.Data;


namespace Org.Vs.WinMd5.Controllers
{
  /// <summary>
  /// Save hash to file
  /// </summary>
  public class SaveHashToFile : ISaveHashToFile
  {
    private static readonly ILog LOG = LogManager.GetLogger(typeof(SaveHashToFile));

    /// <summary>
    /// Save hash
    /// </summary>
    /// <param name="collection"><see cref="ObservableCollection{T}"/> of <see cref="WinMdChecksumData"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>If success <c>True</c> otherwise <c>False</c></returns>
    public async Task<bool> SaveHashAsync(ObservableCollection<WinMdChecksumData> collection, CancellationToken token)
    {
      var result = false;

      await Task.Run(() =>
      {
        if ( collection == null || collection.Count == 0 )
        {
          LOG.Error("File collection is null or empty!");
          return;
        }

        try
        {
          Parallel.ForEach(collection, item =>
          {
            if ( !string.IsNullOrWhiteSpace(item.Md5Hash) )
              SaveContentIntoFile(GetHashName(item.FileName, "md5"), item.Md5Hash);

            if ( !string.IsNullOrWhiteSpace(item.Sha1Hash) )
              SaveContentIntoFile(GetHashName(item.FileName, "sha1"), item.Sha1Hash);

            if ( !string.IsNullOrWhiteSpace(item.Sha256Hash) )
              SaveContentIntoFile(GetHashName(item.FileName, "sha256"), item.Sha256Hash);

            if ( !string.IsNullOrWhiteSpace(item.Sha512Hash) )
              SaveContentIntoFile(GetHashName(item.FileName, "sha512"), item.Sha512Hash);
          });

          result = true;
        }
        catch ( Exception ex )
        {
          LOG.Error(ex, "{0} caused a(n) {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.GetType().Name);
        }
      }, token).ConfigureAwait(false);

      return result;
    }

    private void SaveContentIntoFile(string path, string content)
    {
      if ( File.Exists(path) )
        File.Delete(path);

      File.AppendAllText(path, content);
    }

    private string GetHashName(string fileName, string extension) => $@"{Path.GetDirectoryName(fileName)}\{Path.GetFileNameWithoutExtension(fileName)}.{extension}";
  }
}
