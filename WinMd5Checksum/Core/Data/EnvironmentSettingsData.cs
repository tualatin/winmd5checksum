using Org.Vs.WinMd5.Core.Data.Base;


namespace Org.Vs.WinMd5.Core.Data
{
  /// <summary>
  /// Environment settings data
  /// </summary>
  public class EnvironmentSettingsData : NotifyMaster
  {
    private bool _alwaysOnTop;

    /// <summary>
    /// Alway on top
    /// </summary>
    public bool AlwaysOnTop
    {
      get => _alwaysOnTop;
      set
      {
        if ( value == _alwaysOnTop )
          return;

        _alwaysOnTop = value;
        OnPropertyChanged();
      }
    }

    private bool _upperCaseHash;

    /// <summary>
    /// Hashes in upper case
    /// </summary>
    public bool UpperCaseHash
    {
      get => _upperCaseHash;
      set
      {
        if (value == _upperCaseHash)
          return;

        _upperCaseHash = value;
        OnPropertyChanged();
      }
    }

    private bool _md5IsEnabled;

    /// <summary>
    /// MD5 IsEnabled
    /// </summary>
    public bool Md5IsEnabled
    {
      get => _md5IsEnabled;
      set
      {
        if ( value == _md5IsEnabled )
          return;

        _md5IsEnabled = value;
        OnPropertyChanged();
      }
    }


    private bool _sha1IsEnabled;

    /// <summary>
    /// SHA1 IsEnabled
    /// </summary>
    public bool Sha1IsEnabled
    {
      get => _sha1IsEnabled;
      set
      {
        if ( value == _sha1IsEnabled )
          return;

        _sha1IsEnabled = value;
        OnPropertyChanged();
      }
    }

    private bool _sha256IsEnabled;

    /// <summary>
    /// SHA256 IsEnabled
    /// </summary>
    public bool Sha256IsEnabled
    {
      get => _sha256IsEnabled;
      set
      {
        if ( value == _sha256IsEnabled )
          return;

        _sha256IsEnabled = value;
        OnPropertyChanged();
      }
    }

    private bool _sha512IsEnabled;

    /// <summary>
    /// SHA512 IsEnabled
    /// </summary>
    public bool Sha512IsEnabled
    {
      get => _sha512IsEnabled;
      set
      {
        if ( value == _sha512IsEnabled )
          return;

        _sha512IsEnabled = value;
        OnPropertyChanged();
      }
    }
  }
}
