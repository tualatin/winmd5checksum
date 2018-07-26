using Org.Vs.WinMd5.Core.Data.Base;


namespace Org.Vs.WinMd5.Data
{
  /// <summary>
  /// WinMdChecksum data
  /// </summary>
  public class WinMdChecksumData : NotifyMaster
  {
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

    private bool _hashIsEnabled;

    /// <summary>
    /// Hash IsEnabled
    /// </summary>
    public bool HashIsEnabled
    {
      get => _hashIsEnabled;
      set
      {
        if ( value == _hashIsEnabled )
          return;

        _hashIsEnabled = value;
        OnPropertyChanged();
      }
    }

    private string _hash;

    /// <summary>
    /// Hash sum
    /// </summary>
    public string Hash
    {
      get => _hash;
      set
      {
        if ( Equals(value, _hash) )
          return;

        _hash = value;
        OnPropertyChanged();
      }
    }

    private string _hashToCompare;

    /// <summary>
    /// Hash to compare
    /// </summary>
    public string HashToCompare
    {
      get => _hashToCompare;
      set
      {
        if ( Equals(value, _hashToCompare) )
          return;

        _hashToCompare = value;
        OnPropertyChanged();
      }
    }

    private string _result;

    /// <summary>
    /// Result (OK/FAILED)
    /// </summary>
    public string Result
    {
      get => _result;
      set
      {
        if ( Equals(value, _result) )
          return;

        _result = value;
        OnPropertyChanged();
      }
    }

    private bool _upperCase;

    /// <summary>
    /// Hash as upper case
    /// </summary>
    public bool UpperCase
    {
      get => _upperCase;
      set
      {
        if ( value == _upperCase )
          return;

        _upperCase = value;
        OnPropertyChanged();
      }
    }
  }
}
