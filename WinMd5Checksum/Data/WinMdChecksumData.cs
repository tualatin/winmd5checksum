using System.IO;
using Org.Vs.WinMd5.Core.Data.Base;
using Org.Vs.WinMd5.Core.Utils;


namespace Org.Vs.WinMd5.Data
{
  /// <summary>
  /// WinMdChecksum data
  /// </summary>
  public class WinMdChecksumData : NotifyMaster
  {
    /// <summary>
    /// Standard constructor
    /// </summary>
    public WinMdChecksumData()
    {
      Md5IsEnabled = EnvironmentContainer.Instance.CurrentSettings.Md5IsEnabled;
      Sha1IsEnabled = EnvironmentContainer.Instance.CurrentSettings.Sha1IsEnabled;
      Sha256IsEnabled = EnvironmentContainer.Instance.CurrentSettings.Sha256IsEnabled;
      Sha512IsEnabled = EnvironmentContainer.Instance.CurrentSettings.Sha512IsEnabled;
    }

    private string _fileName;

    /// <summary>
    /// Filename
    /// </summary>
    public string FileName
    {
      get => _fileName;
      set
      {
        if ( Equals(value, _fileName) )
          return;

        _fileName = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// File without path
    /// </summary>
    public string File => string.IsNullOrWhiteSpace(FileName) ? string.Empty : Path.GetFileName(FileName);

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
        EnvironmentContainer.Instance.CurrentSettings.Md5IsEnabled = _md5IsEnabled;

        OnPropertyChanged();
      }
    }

    private string _md5Hash;

    /// <summary>
    /// MD5 hash sum
    /// </summary>
    public string Md5Hash
    {
      get => _md5Hash;
      set
      {
        if ( Equals(value, _md5Hash) )
          return;

        _md5Hash = value;
        OnPropertyChanged();
      }
    }

    private string _md5ToCompareHash;

    /// <summary>
    /// MD5 compare hash
    /// </summary>
    public string Md5ToCompareHash
    {
      get => _md5ToCompareHash;
      set
      {
        if ( Equals(value, _md5ToCompareHash) )
          return;

        _md5ToCompareHash = value;
        OnPropertyChanged();
      }
    }

    private string _md5Result;

    /// <summary>
    /// Result (OK/FAILED)
    /// </summary>
    public string Md5Result
    {
      get => _md5Result;
      set
      {
        if ( Equals(value, _md5Result) )
          return;

        _md5Result = value;
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
        EnvironmentContainer.Instance.CurrentSettings.Sha1IsEnabled = _sha1IsEnabled;

        OnPropertyChanged();
      }
    }

    private string _sha1Hash;

    /// <summary>
    /// SHA1 hash
    /// </summary>
    public string Sha1Hash
    {
      get => _sha1Hash;
      set
      {
        if ( Equals(value, _sha1Hash) )
          return;

        _sha1Hash = value;
        OnPropertyChanged();
      }
    }

    private string _sha1ToCompare;

    /// <summary>
    /// SHA1 compare hash
    /// </summary>
    public string Sha1ToCompare
    {
      get => _sha1ToCompare;
      set
      {
        if ( Equals(value, _sha1ToCompare) )
          return;

        _sha1ToCompare = value;
        OnPropertyChanged();
      }
    }

    private string _sha1Result;

    /// <summary>
    /// Result (OK/FAILED)
    /// </summary>
    public string Sha1Result
    {
      get => _sha1Result;
      set
      {
        if ( Equals(value, _sha1Result) )
          return;

        _sha1Result = value;
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
        EnvironmentContainer.Instance.CurrentSettings.Sha256IsEnabled = _sha256IsEnabled;

        OnPropertyChanged();
      }
    }

    private string _sha256Hash;

    /// <summary>
    /// SHA256 hash
    /// </summary>
    public string Sha256Hash
    {
      get => _sha256Hash;
      set
      {
        if ( Equals(value, _sha256Hash) )
          return;

        _sha256Hash = value;
        OnPropertyChanged();
      }
    }

    private string _sha256ToCompare;

    /// <summary>
    /// SHA256 compare hash
    /// </summary>
    public string Sha256ToCompare
    {
      get => _sha256ToCompare;
      set
      {
        if ( Equals(value, _sha256ToCompare) )
          return;

        _sha256ToCompare = value;
        OnPropertyChanged();
      }
    }

    private string _sha256Result;

    /// <summary>
    /// Result (OK/FAILED)
    /// </summary>
    public string Sha256Result
    {
      get => _sha256Result;
      set
      {
        if ( Equals(value, _sha256Result) )
          return;

        _sha256Result = value;
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
        EnvironmentContainer.Instance.CurrentSettings.Sha512IsEnabled = _sha512IsEnabled;

        OnPropertyChanged();
      }
    }

    private string _sha512Hash;

    /// <summary>
    /// SHA512 hash
    /// </summary>
    public string Sha512Hash
    {
      get => _sha512Hash;
      set
      {
        if ( Equals(value, _sha512Hash) )
          return;

        _sha512Hash = value;
        OnPropertyChanged();
      }
    }

    private string _sha512ToCompare;

    /// <summary>
    /// SHA512 compare hash
    /// </summary>
    public string Sha512ToCompare
    {
      get => _sha512ToCompare;
      set
      {
        if ( Equals(value, _sha512ToCompare) )
          return;

        _sha512ToCompare = value;
        OnPropertyChanged();
      }
    }

    private string _sha512Result;

    /// <summary>
    /// Result (OK/FAILED)
    /// </summary>
    public string Sha512Result
    {
      get => _sha512Result;
      set
      {
        if ( Equals(value, _sha512Result) )
          return;

        _sha512Result = value;
        OnPropertyChanged();
      }
    }
  }
}
