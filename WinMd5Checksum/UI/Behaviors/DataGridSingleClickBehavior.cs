﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Org.Vs.WinMd5.UI.Behaviors.Base;
using Org.Vs.WinMd5.UI.Extensions;
using Org.Vs.WinMd5.UI.UserControls;


namespace Org.Vs.WinMd5.UI.Behaviors
{
  /// <summary>
  /// <see cref="DataGrid"/> single click behavior
  /// </summary>
  public class DataGridSingleClickBehavior : BehaviorBase<DataGrid>
  {
    /// <summary>
    /// Setup <see cref="BehaviorBase{T}"/>
    /// </summary>
    protected override void OnSetup()
    {
      AssociatedObject.PreviewMouseLeftButtonDown += OnDataGridPreviewMouseLeftButtonDown;
      AssociatedObject.PreviewMouseLeftButtonUp += OnDataGridPreviewMouseLeftButtonUp;
    }

    /// <summary>
    /// Release all resource used by <see cref="BehaviorBase{T}"/>
    /// </summary>
    protected override void OnCleanup()
    {
      AssociatedObject.PreviewMouseLeftButtonDown -= OnDataGridPreviewMouseLeftButtonDown;
      AssociatedObject.PreviewMouseLeftButtonUp -= OnDataGridPreviewMouseLeftButtonUp;
    }

    private void OnDataGridPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if ( !(sender is VsDataGrid dataGrid) )
        return;

      FrameworkElement fe = GetFrameworkElement(e.OriginalSource);
      DataGridCell cell = fe?.Ancestors().OfType<DataGridCell>().FirstOrDefault();

      if ( cell == null || cell.IsEditing || cell.IsReadOnly || Keyboard.Modifiers != ModifierKeys.None )
        return;

      if ( !cell.IsFocused )
        cell.Focus();

      if ( dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow )
      {
        if ( !cell.IsSelected )
          cell.IsSelected = true;
      }
      else
      {
        DataGridRow row = cell.Ancestors().OfType<DataGridRow>().FirstOrDefault();

        if ( row == null || !row.IsSelected )
          return;

        row.IsSelected = true;
      }
    }

    private void OnDataGridPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if ( !(sender is VsDataGrid dataGrid) )
        return;

      if ( !(dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.CurrentCell.Item) is DataGridRow row) )
        return;

      DataGridCellsPresenter presenter = row.Descendents().OfType<DataGridCellsPresenter>().FirstOrDefault();

      if ( presenter == null )
        return;

      int index = dataGrid.Columns.IndexOf(dataGrid.CurrentColumn);

      if ( index < 0 )
        return;

      if ( !(presenter.ItemContainerGenerator.ContainerFromIndex(index) is DataGridCell cell) || cell.IsEditing )
        return;

      dataGrid.BeginEdit(e);
    }

    private FrameworkElement GetFrameworkElement(object element)
    {
      if ( element is FrameworkElement frameworkElement )
        return frameworkElement;

      if ( !(element is FrameworkContentElement parent) )
        return null;

      return parent.Parent as FrameworkElement;
    }
  }
}
