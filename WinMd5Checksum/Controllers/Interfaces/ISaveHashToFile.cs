using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Org.Vs.WinMd5.UI.UserControls.DataModels;


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
    /// <param name="collection"><see cref="List{T}"/> of <see cref="VsDataGridHierarchialDataModel"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>If success <c>True</c> otherwise <c>False</c></returns>
    Task<bool> SaveHashAsync(List<VsDataGridHierarchialDataModel> collection, CancellationToken token);
  }
}
