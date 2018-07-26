using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Org.Vs.WinMd5.UI.UserControls.DataModels;


namespace Org.Vs.WinMd5.Controllers.Interfaces
{
  /// <summary>
  /// CalculateHashsum interface
  /// </summary>
  public interface ICalculateHash
  {
    /// <summary>
    /// Start hash calculation of data collection
    /// </summary>
    /// <param name="collection"><see cref="List{T}"/> of <see cref="VsDataGridHierarchialDataModel"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    Task<List<VsDataGridHierarchialDataModel>> StartCalculationAsync(List<VsDataGridHierarchialDataModel> collection, CancellationToken token);
  }
}
