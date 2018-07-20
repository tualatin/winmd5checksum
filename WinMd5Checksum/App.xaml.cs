﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using log4net;
using Org.Vs.WinMd5.BaseView;
using Org.Vs.WinMd5.Core.Data.Base;
using Org.Vs.WinMd5.Core.Utils;
using Org.Vs.WinMd5.Data;


namespace Org.Vs.WinMd5
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App
  {
    private static readonly ILog LOG = LogManager.GetLogger(typeof(App));

    private ObservableCollection<WinMdChecksumData> _collection;

    private void ApplicationStartup(object sender, StartupEventArgs e)
    {
      // If it is not the minimum .NET version installed
      if ( EnvironmentContainer.NetFrameworkKey <= 393295 )
      {
        LOG.Error("Wrong .NET version! Please install .NET 4.6 or newer.");
        Shutdown();
        return;
      }

      if ( e.Args.Length > 0 )
      {
        _collection = new ObservableCollection<WinMdChecksumData>();

        foreach ( var arg in e.Args )
        {

        }

        NotifyTaskCompletion ntc = NotifyTaskCompletion.Create(EnvironmentContainer.Instance.StartCalculationAsync(_collection, new CancellationTokenSource().Token));
        ntc.PropertyChanged += OnTaskCompletionPropertyChanged;

        Current.Shutdown(0);
        return;
      }

      MainWindow wnd = new MainWindow();
      AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
      wnd.Show();
    }

    private void OnTaskCompletionPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if ( !e.PropertyName.Equals("IsSuccessfullyCompleted") )
        return;
    }

    private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e) =>
      LOG.Error("{0} caused a(n) {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.ExceptionObject.GetType().Name, e.ExceptionObject);
  }
}