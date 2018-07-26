using System.Collections.ObjectModel;
using System.Linq;


namespace Org.Vs.WinMd5.UI.UserControls.DataModels.Data
{
  /// <summary>
  /// Virtual Studios <see cref="VsDataGrid"/> hierarchial data
  /// </summary>
  public class VsDataGridHierarchialData : ObservableCollection<VsDataGridHierarchialDataModel>
  {
    /// <summary>
    /// <see cref="ObservableCollection{T}"/> of <see cref="VsDataGridHierarchialDataModel"/>
    /// </summary>
    public ObservableCollection<VsDataGridHierarchialDataModel> RowData
    {
      get;
      set;
    }

    /// <summary>
    /// Standard constructor
    /// </summary>
    public VsDataGridHierarchialData() => RowData = new ObservableCollection<VsDataGridHierarchialDataModel>();

    /// <summary>
    /// Initialize
    /// </summary>
    public void Initialize()
    {
      Clear();

      foreach ( VsDataGridHierarchialDataModel item in RowData.Where(p => p.IsVisible).SelectMany(p => new[] { p }.Concat(p.VisibleDescendants)) )
      {
        Add(item);
      }
    }

    /// <summary>
    /// Add children
    /// </summary>
    /// <param name="item"><see cref="VsDataGridHierarchialDataModel"/></param>
    public void AddChildren(VsDataGridHierarchialDataModel item)
    {
      if ( !Contains(item) )
        return;

      int parentIndex = IndexOf(item);

      foreach ( VsDataGridHierarchialDataModel child in item.Children )
      {
        parentIndex++;
        Insert(parentIndex, child);
      }
    }

    /// <summary>
    /// Remove children
    /// </summary>
    /// <param name="item"><see cref="VsDataGridHierarchialDataModel"/></param>
    public void RemoveChildren(VsDataGridHierarchialDataModel item)
    {
      foreach ( VsDataGridHierarchialDataModel child in item.Children )
      {
        if ( Contains(child) )
          Remove(child);
      }
    }
  }
}
