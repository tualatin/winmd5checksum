using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Org.Vs.WinMd5.Data;


namespace Org.Vs.WinMd5.Interfaces
{
  /// <summary>
  /// CalculateHashsum interface
  /// </summary>
  public interface ICalculateHash
  {
    /// <summary>
    /// Start hash calculation of data collection
    /// </summary>
    /// <param name="collection"><see cref="ObservableCollection{T}"/> of <see cref="WinMdChecksumData"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    Task<ObservableCollection<WinMdChecksumData>> StartCalculationAsync(ObservableCollection<WinMdChecksumData> collection, CancellationToken token);
  }
}
