using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Org.Vs.WinMd5.Controllers.Interfaces;
using Org.Vs.WinMd5.Data;
using Org.Vs.WinMd5.UI.UserControls.DataModels;


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
    /// <param name="collection"><see cref="List{T}"/> of <see cref="VsDataGridHierarchialDataModel"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>If success <c>True</c> otherwise <c>False</c></returns>
    public async Task<bool> SaveHashAsync(List<VsDataGridHierarchialDataModel> collection, CancellationToken token)
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
          Parallel.ForEach(collection, hash =>
          {
            if ( hash.Children.FirstOrDefault(p => (p.Data as WinMdChecksumData)?.FileName == HashNames.Md5)?.Data is
                   WinMdChecksumData md5 && !string.IsNullOrWhiteSpace(md5.Hash) )
            {
              SaveContentIntoFile(GetHashName((hash.Data as WinMdChecksumData)?.FileName, "md5"), md5.Hash);
            }

            if ( hash.Children.FirstOrDefault(p => (p.Data as WinMdChecksumData)?.FileName == HashNames.Sha1)?.Data is
                   WinMdChecksumData sha1 && !string.IsNullOrWhiteSpace(sha1.Hash) )
            {
              SaveContentIntoFile(GetHashName((hash.Data as WinMdChecksumData)?.FileName, "sha1"), sha1.Hash);
            }

            if ( hash.Children.FirstOrDefault(p => (p.Data as WinMdChecksumData)?.FileName == HashNames.Sha256)?.Data is
                   WinMdChecksumData sha256 && !string.IsNullOrWhiteSpace(sha256.Hash) )
            {
              SaveContentIntoFile(GetHashName((hash.Data as WinMdChecksumData)?.FileName, "sha256"), sha256.Hash);
            }

            if ( hash.Children.FirstOrDefault(p => (p.Data as WinMdChecksumData)?.FileName == HashNames.Sha384)?.Data is
                   WinMdChecksumData sha384 && !string.IsNullOrWhiteSpace(sha384.Hash) )
            {
              SaveContentIntoFile(GetHashName((hash.Data as WinMdChecksumData)?.FileName, "sha384"), sha384.Hash);
            }

            if ( hash.Children.FirstOrDefault(p => (p.Data as WinMdChecksumData)?.FileName == HashNames.Sha512)?.Data is
                   WinMdChecksumData sha512 && !string.IsNullOrWhiteSpace(sha512.Hash) )
            {
              SaveContentIntoFile(GetHashName((hash.Data as WinMdChecksumData)?.FileName, "sha512"), sha512.Hash);
            }
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
