﻿using System;
using System.Windows;
using WinMd5Checksum.Utils;
using System.Threading;
using System.ComponentModel;
using Microsoft.Win32;
using WinMd5Checksum.Data;
using System.Windows.Controls;
using System.IO;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Input;
using System.Reflection;
using System.Drawing;


namespace WinMd5Checksum
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow: Window, IDisposable
  {
    private readonly BackgroundWorker bw = new BackgroundWorker ( );
    private DataGridCell currentDataGridCell;
    private WinMdTrayIcon trayIcon;
    

    public void Dispose ()
    {
      if (bw == null)
        return;
        
      bw.Dispose ( );

      if (trayIcon == null)
        return;

      trayIcon.Dispose ( );
      trayIcon = null;
    }

    public MainWindow ()
    {
      InitializeComponent ( );

      Title = LogFile.ApplicationCaption ( );

      bw.WorkerReportsProgress = true;
      bw.WorkerSupportsCancellation = true;

      bw.DoWork += bw_WaitingThread;
      bw.ProgressChanged += bw_ProgressChanged;
      bw.RunWorkerCompleted += bw_RunWorkerCompleted;

      // TrayIcon stuff
      Stream stream = Application.GetResourceStream (new Uri ("pack://application:,,,/WinMd5Checksum;component/Res/Key.ico")).Stream;
      Icon icon = new Icon (stream);
      trayIcon = new WinMdTrayIcon (icon, string.Format ("{0} ready ...", LogFile.ApplicationCaption ( )));

      trayIcon.NotifyIcon.DoubleClick += DoubleClickNotifyIcon;

      // Close application when Escape is pressed
      PreviewKeyDown += HandleEsc;
    }

    private void Window_Loaded (object sender, RoutedEventArgs e)
    {
      if (Md5Files.GetFileContainer ( ).Count > 0)
        StartCalculation ( );

      btnFile.Focus ( );
    }
    
    private void Hyperlink_RequestNavigate (object sender, RequestNavigateEventArgs e)
    {
      Process.Start (new ProcessStartInfo (e.Uri.AbsoluteUri));
      e.Handled = true;
    }

    private void dataGridFiles_Drop (object sender, DragEventArgs e)
    {
      object text = e.Data.GetData (DataFormats.FileDrop);
      DataGrid dg = sender as DataGrid;

      if (dg != null)
      {
        try
        {
          string fileName = string.Format ("{0}", ((string[]) text)[0]);

          if ((Path.GetExtension (fileName).CompareTo (".md5")) == 0)
          {
            fileName = CalcMd5Checksum.GetValueFromHashFile (fileName);
            Md5Structure file = Md5Files.GetFileContainer ( )[Md5Files.GetFileContainer ( ).Count - 1];

            file.compare = fileName;

            RefreshDataSource ( );
            return;
          }

          if ((Path.GetExtension (fileName).CompareTo (".sha256")) == 0)
          {
            fileName = CalcMd5Checksum.GetValueFromHashFile (fileName);
            Md5Structure file = Md5Files.GetFileContainer ( )[Md5Files.GetFileContainer ( ).Count - 1];

            file.compare256hash = fileName;
            RefreshDataSource ( );
            return;
          }

          Md5Files.AddFileToContainer (fileName);
          Md5Files.FinishOperation ( );

          RefreshDataSource ( );
        }
        catch
        {

        }
      }
    }

    private void dataGridFiles_GotFocus (object sender, RoutedEventArgs e)
    {
      if (e.OriginalSource is DataGridCell)
      {
        if ((e.OriginalSource as DataGridCell).Column.DisplayIndex == 1 || (e.OriginalSource as DataGridCell).Column.DisplayIndex == 4)
          currentDataGridCell = e.OriginalSource as DataGridCell;
        else
          currentDataGridCell = null;
      }
    }

    private void dataGridFiles_PreviewKeyDown (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
      {
        if (currentDataGridCell != null)
        {
          if (Clipboard.ContainsText ( ))
            Clipboard.Clear ( );

          Clipboard.SetText ((currentDataGridCell.Content as TextBlock).Text);
        }
      }
    }

    #region ClickEvents

    private void DoubleClickNotifyIcon (object sender, EventArgs e)
    {
      if (WindowState == WindowState.Minimized)
        WindowState = WindowState.Normal;

      Activate ( );
      Focus ( );
    }

    private void btnFile_Click (object sender, RoutedEventArgs e)
    {
      OpenFileDialog openDialog = new OpenFileDialog ( )
      {
        Filter = "All files (*.*)|*.*",
        RestoreDirectory = true,
        Title = "Please select a file to calculate md5/sha256 hashsum"
      };

      Nullable<bool> result = openDialog.ShowDialog ( );

      if (result == true)
      {
        Md5Files.AddFileToContainer (openDialog.FileName);
        Md5Files.FinishOperation ( );

        RefreshDataSource ( );
      }
    }

    private void btnStart_Click (object sender, RoutedEventArgs e)
    {
      PreStartCalculation ( );
    }

    private void btnClear_Click (object sender, RoutedEventArgs e)
    {
      Md5Files.ClearAll ( );
      RefreshDataSource ( );
    }

    private void btnSave_Click (object sender, RoutedEventArgs e)
    {
      Md5Files.GetFileContainer ( ).ForEach (file =>
      {
        if (!string.IsNullOrEmpty (file.calc))
        {
          SaveFileDialog saveDialog = new SaveFileDialog() { Filter = "md5 files (*.md5)|*.md5", RestoreDirectory = true, Title = string.Format ("Hashsum file for {0}", file.key) };
          Nullable<bool> result = saveDialog.ShowDialog ( );

          if (result == true)
          {
            string fileName = saveDialog.FileName;
            if (LogFile.WriteSaveFile (fileName, file.calc, file.sha256hash) == true)
              MessageBox.Show (string.Format ("File {0} successfully saved.", fileName), LogFile.ApplicationCaption ( ), MessageBoxButton.OK, MessageBoxImage.Information);
            else
              MessageBox.Show (string.Format ("File {0} not saved!", fileName), LogFile.ApplicationCaption ( ), MessageBoxButton.OK, MessageBoxImage.Error);
          }
        }
      });
    }

    private void btnCancel_Click (object sender, RoutedEventArgs e)
    {
      OnCancel ( );
    }

    private void btnHelp_Click (object sender, RoutedEventArgs e)
    {
      LogFile.ShowHelp ( );
    }

    private void btnAbout_Click (object sender, RoutedEventArgs e)
    {
      Assembly assembly = Assembly.GetExecutingAssembly ( );

      MessageBox.Show (string.Format ("{0}\nVersion: {1}\nBuild Date: {2}\nAuthor: M. Zoennchen, Copryright 2013\nAll rights reserved\n\nIts a opensource .NET project", LogFile.ApplicationCaption ( ), assembly.GetName ( ).Version, BuildDate.GetBuildDateTime (assembly)), LogFile.ApplicationCaption ( ), MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void btnWeb_Click (object sender, RoutedEventArgs e)
    {
      Process.Start (new ProcessStartInfo ("http://www.virtual-studios.org"));
    }

    private void checkBoxOnTop_Click (object sender, RoutedEventArgs e)
    {
      if (checkBoxOnTop.IsChecked == true)
        Topmost = true;
      else
        Topmost = false;
    }

    private void btnExit_Click (object sender, RoutedEventArgs e)
    {
      OnExit ( );
    }

    private void Window_Closing (object sender, CancelEventArgs e)
    {
      OnExit ( );
    }

    #endregion ClickEvents

    #region Thread

    private void bw_WaitingThread (object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;

      while (CalcMd5Checksum.GetThread ( ).ThreadState != System.Threading.ThreadState.Stopped)
      {
        if ((worker.CancellationPending == true))
        {
          e.Cancel = true;
          break;
        }
        else
        {
          Thread.Sleep (1);
        }
      }
    }

    private void bw_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
    {
      if ((e.Cancelled == true))
        waitingOperation.Visibility = System.Windows.Visibility.Hidden;
      else if (!(e.Error == null))
        waitingOperation.Visibility = System.Windows.Visibility.Hidden;
      else
      {
        RefreshDataSource ( );

        btnCancel.IsEnabled = false;
        waitingOperation.Visibility = System.Windows.Visibility.Hidden;
        trayIcon.NotifyIcon.Text = LogFile.ApplicationCaption ( ) + " ready ...";
        trayIcon.NotifyIcon.ShowBalloonTip (4000, LogFile.ApplicationCaption ( ), "Calculation finished ...", System.Windows.Forms.ToolTipIcon.Info);
      }
    }

    private void bw_ProgressChanged (object sender, ProgressChangedEventArgs e)
    {
      throw new NotImplementedException ("ProgressChange not implemented yet");
    }

    #endregion Thread

    #region Helperfunction

    private void PreStartCalculation ()
    {
      btnCancel.IsEnabled = true;

      StartCalculation ( );
    }

    private void StartCalculation ()
    {
      trayIcon.NotifyIcon.Text = LogFile.ApplicationCaption ( ) + " calculating ...";
      trayIcon.NotifyIcon.ShowBalloonTip (4000, LogFile.ApplicationCaption ( ), "Start calculation ...", System.Windows.Forms.ToolTipIcon.Info);

      CalcMd5Checksum.CalcMd5HashSum ( );

      if (CalcMd5Checksum.GetThread ( ).ThreadState == System.Threading.ThreadState.Stopped)
        return;

      waitingOperation.Visibility = System.Windows.Visibility.Visible;

      if (bw.IsBusy != true)
        bw.RunWorkerAsync ( );
    }

    private void RefreshDataSource ()
    {
      dataGridFiles.DataContext = Md5Files.GetFileContainer ( );
      dataGridFiles.Items.Refresh ( );

      if (Md5Files.GetFileContainer ( ).Count > 0)
        btnClear.IsEnabled = btnStart.IsEnabled = true;
      else
        btnSave.IsEnabled = btnClear.IsEnabled = btnStart.IsEnabled = false;

      Md5Files.GetFileContainer ( ).ForEach (item =>
      {
        if (!string.IsNullOrEmpty (item.calc))
          btnSave.IsEnabled = true;
      });
    }

    private void OnCancel ()
    {
      try
      {
        if (CalcMd5Checksum.GetThread ( ) != null && CalcMd5Checksum.GetThread ( ).ThreadState != System.Threading.ThreadState.Stopped)
          CalcMd5Checksum.OnExit ( );

        if (bw.IsBusy == true)
        {
          if (bw.WorkerSupportsCancellation == true)
            bw.CancelAsync ( );
        }
      }
      catch (Exception ex)
      {
        Console.Write (string.Format ("OnCancel exception {0}\n{1}", TraceHelper.GetFunctionName (GetType ( ).Name), ex.Message));
      }
      finally
      {
        btnCancel.IsEnabled = false;
      }
    }

    private void HandleEsc (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        OnExit ( );
    }

    private void OnExit ()
    {
      OnCancel ( );
      Dispose ( );
      Application.Current.Shutdown ( );
    }

    #endregion Helperfunction
  }
}