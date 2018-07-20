using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Windows;
using log4net;
using Org.Vs.WinMd5.BaseView;
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
    private bool _md5;
    private bool _sha1;
    private bool _sha256;
    private bool _sha512;
    private bool _shortMode;

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
        ResetHashCalculation();

        _collection = new ObservableCollection<WinMdChecksumData>();

        foreach ( string arg in e.Args )
        {
          if ( string.Compare(arg, "/?", StringComparison.InvariantCulture) == 0 )
          {
            ShowHelp();
            break;
          }

          if ( string.Compare(arg, "/s", StringComparison.CurrentCultureIgnoreCase) == 0 )
          {
            _shortMode = true;
            continue;
          }

          if ( string.Compare(arg, "/!md5", StringComparison.CurrentCultureIgnoreCase) == 0 )
          {
            _md5 = false;
            continue;
          }

          if ( string.Compare(arg, "/sha1", StringComparison.InvariantCultureIgnoreCase) == 0 )
          {
            _sha1 = true;
            continue;
          }

          if ( string.Compare(arg, "/sha256", StringComparison.InvariantCultureIgnoreCase) == 0 )
          {
            _sha256 = true;
            continue;
          }

          if ( string.Compare(arg, "/sha512", StringComparison.CurrentCultureIgnoreCase) == 0 )
          {
            _sha512 = true;
            continue;
          }

          _collection.Add(new WinMdChecksumData
          {
            FileName = arg,
            Md5IsEnabled = _md5,
            Sha1IsEnabled = _sha1,
            Sha256IsEnabled = _sha256,
            Sha512IsEnabled = _sha512
          });

          ResetHashCalculation();
        }

        RunCalculation();
        Current.Shutdown(0);
        return;
      }

      var wnd = new MainWindow();
      AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
      wnd.Show();
    }

    private void ResetHashCalculation()
    {
      _md5 = true;
      _sha1 = false;
      _sha256 = false;
      _sha512 = false;
    }

    private void ShowHelp()
    {
      string shortMode = Current.TryFindResource("HintShortMode").ToString();
      string applicationHelp = Current.TryFindResource("HintApplicationHelp").ToString();
      string example1 = Current.TryFindResource("HintExample1").ToString();
      string example2 = Current.TryFindResource("HintExample2").ToString();
      string md5Disabled = Current.TryFindResource("HintMd5Disabled").ToString();
      string shaEnabled = Current.TryFindResource("HintShaEnabled").ToString();

      InteractionService.ShowInformationMessageBox($"{applicationHelp}\n\n/s\t{shortMode}\n/!md5\t{md5Disabled}\n/shaxxx\t{shaEnabled}\n\nExamples\n\n{example1}\n{example2}");
    }

    private void RunCalculation()
    {
      EnvironmentContainer.Instance.StartCalculationAsync(_collection, new CancellationTokenSource().Token).Wait();

      if ( _shortMode )
      {
        EnvironmentContainer.Instance.SaveHashAsync(_collection, new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token).Wait();
        return;
      }

      var message = new StringBuilder();

      foreach ( WinMdChecksumData item in _collection )
      {
        message.AppendLine($"{item.File}");

        if ( item.Md5IsEnabled )
          message.AppendLine($"MD5: {item.Md5Hash}");

        if ( item.Sha1IsEnabled )
          message.AppendLine($"SHA1: {item.Sha1Hash}");

        if ( item.Sha256IsEnabled )
          message.AppendLine($"SHA256: {item.Sha256Hash}");

        if ( item.Sha512IsEnabled )
          message.AppendLine($"SHA512: {item.Sha512Hash}");

        message.AppendLine();
      }

      InteractionService.ShowInformationMessageBox(message.ToString());
    }

    private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e) =>
      LOG.Error("{0} caused a(n) {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.ExceptionObject.GetType().Name, e.ExceptionObject);
  }
}
