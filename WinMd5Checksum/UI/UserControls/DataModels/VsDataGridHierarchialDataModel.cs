using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Org.Vs.WinMd5.UI.UserControls.DataModels.Data;
using Org.Vs.WinMd5.UI.UserControls.DataModels.Interfaces;


namespace Org.Vs.WinMd5.UI.UserControls.DataModels
{
  /// <summary>
  /// Virtual Studios <see cref="VsDataGrid"/> hierarchial data model
  /// </summary>
  public class VsDataGridHierarchialDataModel : IVsDataGridHierarchialDataModel
  {
    private bool _isVisible;

    /// <summary>
    /// Is visible
    /// </summary>
    public bool IsVisible
    {
      get => _isVisible;
      set
      {
        if ( value == _isVisible )
          return;

        _isVisible = value;

        if ( _isVisible )
          ShowChildren();
        else
          HideChildren();
      }
    }

    private bool _isExpanded;

    /// <summary>
    /// Is expanded
    /// </summary>
    public bool IsExpanded
    {
      get => _isExpanded;
      set
      {
        if ( value == _isExpanded )
          return;

        _isExpanded = value;

        if ( _isExpanded )
          Expand();
        else
          Collapse();
      }
    }

    /// <summary>
    /// DataManager of <see cref="VsDataGridHierarchialData"/>
    /// </summary>
    public VsDataGridHierarchialData DataManager
    {
      get;
      set;
    }

    /// <summary>
    /// Parent of <see cref="VsDataGridHierarchialDataModel"/>
    /// </summary>
    public VsDataGridHierarchialDataModel Parent
    {
      get;
      set;
    }

    /// <summary>
    /// The data as <see cref="object"/>
    /// </summary>
    public object Data
    {
      get;
      set;
    }

    private int _level;

    /// <summary>
    /// Level
    /// </summary>
    public int Level
    {
      get
      {
        if ( _level == -1 )
          _level = Parent?.Level + 1 ?? 0;

        return _level;
      }
    }

    /// <summary>
    /// <see cref="ObservableCollection{T}"/> of <see cref="VsDataGridHierarchialDataModel"/>
    /// </summary>
    public ObservableCollection<VsDataGridHierarchialDataModel> Children
    {
      get;
      set;
    }

    /// <summary>
    /// Has children
    /// </summary>
    public bool HasChildren => Children.Count > 0;

    /// <summary>
    /// Visible descendants
    /// </summary>
    public IEnumerable<VsDataGridHierarchialDataModel> VisibleDescendants => Children.Where(p => p.IsVisible).SelectMany(p => new[] { p }.Concat(p.VisibleDescendants));

    /// <summary>
    /// Standard constructor
    /// </summary>
    public VsDataGridHierarchialDataModel()
    {
      _level = -1;
      Children = new ObservableCollection<VsDataGridHierarchialDataModel>();
    }

    /// <summary>
    /// Add children
    /// </summary>
    /// <param name="child"><see cref="VsDataGridHierarchialDataModel"/></param>
    public void AddChild(VsDataGridHierarchialDataModel child)
    {
      child.Parent = this;
      Children.Add(child);
    }

    private void Expand()
    {
      DataManager.AddChildren(this);

      foreach ( VsDataGridHierarchialDataModel child in Children )
      {
        child.IsVisible = true;
      }
    }

    private void Collapse()
    {
      DataManager.RemoveChildren(this);

      foreach ( VsDataGridHierarchialDataModel child in Children )
      {
        child.IsVisible = false;
      }
    }

    private void ShowChildren()
    {
      if ( !IsExpanded )
        return;

      DataManager.AddChildren(this);

      foreach ( VsDataGridHierarchialDataModel child in Children )
      {
        child.IsVisible = true;
      }
    }

    private void HideChildren()
    {
      if ( !IsExpanded )
        return;

      DataManager.RemoveChildren(this);

      foreach ( VsDataGridHierarchialDataModel child in Children )
      {
        child.IsVisible = false;
      }
    }
  }
}
