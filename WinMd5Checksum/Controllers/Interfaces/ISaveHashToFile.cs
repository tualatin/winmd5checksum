using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Org.Vs.WinMd5.Data;


namespace Org.Vs.WinMd5.Controllers.Interfaces
{
  /// <summary>
  /// Save hash to file interface
  /// </summary>
  public interface ISaveHashToFile
  {
    /// <summary>
    /// Save hash
    /// </summary>
    /// <param name="collection"><see cref="ObservableCollection{T}"/> of <see cref="WinMdChecksumData"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>If success <c>True</c> otherwise <c>False</c></returns>
    Task<bool> SaveHashAsync(ObservableCollection<WinMdChecksumData> collection, CancellationToken token);
  }
}
