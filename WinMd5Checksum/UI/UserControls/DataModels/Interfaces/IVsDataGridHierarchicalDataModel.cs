using System.Collections.Generic;
using System.Collections.ObjectModel;
using Org.Vs.WinMd5.UI.UserControls.DataModels.Data;


namespace Org.Vs.WinMd5.UI.UserControls.DataModels.Interfaces
{
  /// <summary>
  /// Virtual Studios <see cref="VsDataGrid"/> hierarchial data model interface
  /// </summary>
  public interface IVsDataGridHierarchialDataModel
  {
    /// <summary>
    /// Parent of <see cref="VsDataGridHierarchialDataModel"/>
    /// </summary>
    VsDataGridHierarchialDataModel Parent
    {
      get;
      set;
    }

    /// <summary>
    /// DataManager of <see cref="VsDataGridHierarchialData"/>
    /// </summary>
    VsDataGridHierarchialData DataManager
    {
      get;
      set;
    }

    /// <summary>
    /// The data as <see cref="object"/>
    /// </summary>
    object Data
    {
      get;
      set;
    }

    /// <summary>
    /// Has children
    /// </summary>
    bool HasChildren
    {
      get;
    }

    /// <summary>
    /// <see cref="List{T}"/> of <see cref="VsDataGridHierarchialDataModel"/>
    /// </summary>
    ObservableCollection<VsDataGridHierarchialDataModel> Children
    {
      get;
      set;
    }

    /// <summary>
    /// Visible descendants
    /// </summary>
    IEnumerable<VsDataGridHierarchialDataModel> VisibleDescendants
    {
      get;
    }

    /// <summary>
    /// Is visible
    /// </summary>
    bool IsVisible
    {
      get;
      set;
    }

    /// <summary>
    /// Is expanded
    /// </summary>
    bool IsExpanded
    {
      get;
      set;
    }

    /// <summary>
    /// Level
    /// </summary>
    int Level
    {
      get;
    }

    /// <summary>
    /// Add child
    /// </summary>
    /// <param name="child"><see cref="VsDataGridHierarchialDataModel"/></param>
    void AddChild(VsDataGridHierarchialDataModel child);
  }
}
