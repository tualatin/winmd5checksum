using Org.Vs.WinMd5.Core.Utils;

namespace Org.Vs.WinMd5.Core.Data
{
  public partial class EnvironmentSettingsData
  {
    /// <summary>
    /// Save data to memento
    /// </summary>
    /// <returns>Copy of <see cref="EnvironmentSettingsData"/></returns>
    public MementoEnvironmentSettings SaveToMemento() => new MementoEnvironmentSettings(this);

    /// <summary>
    /// Roll object back to state of the provided memento
    /// </summary>
    /// <param name="memento">The memento to roll back to</param>
    public void RestoreFromMemento(MementoEnvironmentSettings memento)
    {
      Arg.NotNull(memento, nameof(memento));

      AlwaysOnTop = memento.AlwaysOnTop;
      UpperCaseHash = memento.UpperCaseHash;
      MinimizeToTray = memento.MinimizeToTray;
      CloseToTray = memento.CloseToTray;
      SingleInstance = memento.SingleInstance;

      Md5IsEnabled = memento.Md5IsEnabled;
      Sha1IsEnabled = memento.Sha1IsEnabled;
      Sha256IsEnabled = memento.Sha256IsEnabled;
      Sha384IsEnabled = memento.Sha384IsEnabled;
      Sha512IsEnabled = memento.Sha512IsEnabled;
    }

    /// <summary>
    /// Memento design pattern
    /// </summary>
    public class MementoEnvironmentSettings
    {
      internal MementoEnvironmentSettings(EnvironmentSettingsData obj)
      {
        AlwaysOnTop = obj.AlwaysOnTop;
        UpperCaseHash = obj.UpperCaseHash;
        MinimizeToTray = obj.MinimizeToTray;
        CloseToTray = obj.CloseToTray;
        SingleInstance = obj.SingleInstance;

        Md5IsEnabled = obj.Md5IsEnabled;
        Sha1IsEnabled = obj.Sha1IsEnabled;
        Sha256IsEnabled = obj.Sha256IsEnabled;
        Sha384IsEnabled = obj.Sha384IsEnabled;
        Sha512IsEnabled = obj.Sha512IsEnabled;
      }

      /// <summary>
      /// Single instance
      /// </summary>
      public bool SingleInstance
      {
        get;
      }

      /// <summary>
      /// Alway on top
      /// </summary>
      public bool AlwaysOnTop
      {
        get;
      }

      /// <summary>
      /// Hashes in upper case
      /// </summary>
      public bool UpperCaseHash
      {
        get;
      }

      /// <summary>
      /// Minimize to tray
      /// </summary>
      public bool MinimizeToTray
      {
        get;
      }

      /// <summary>
      /// Close to tray
      /// </summary>
      public bool CloseToTray
      {
        get;
      }

      /// <summary>
      /// MD5 IsEnabled
      /// </summary>
      public bool Md5IsEnabled
      {
        get;
      }

      /// <summary>
      /// SHA1 IsEnabled
      /// </summary>
      public bool Sha1IsEnabled
      {
        get;
      }

      /// <summary>
      /// SHA256 IsEnabled
      /// </summary>
      public bool Sha256IsEnabled
      {
        get;
      }

      /// <summary>
      /// SHA384 IsEnabled
      /// </summary>
      public bool Sha384IsEnabled
      {
        get;
      }

      /// <summary>
      /// SHA512 IsEnabled
      /// </summary>
      public bool Sha512IsEnabled
      {
        get;
      }
    }
  }
}
