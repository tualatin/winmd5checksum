using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;


namespace WinMd5Checksum.Data
{
  public class Md5Structure: INotifyPropertyChanged
  {
    /// <summary>
    /// Declare the event
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;


    protected void OnPropertyChanged (string name)
    {
      PropertyChangedEventHandler handler = PropertyChanged;

      if (handler != null)
        handler (this, new PropertyChangedEventArgs (name));
    }

    /// <summary>
    /// Filename
    /// </summary>
    private string Key;
    public string key
    {
      get
      {
        return (Key);
      }

      set
      {
        Key = value;
        OnPropertyChanged ("key");
      }
    }

    /// <summary>
    /// Only file without path
    /// </summary>
    public string file
    {
      get
      {
        return (Path.GetFileName (Key));
      }
    }

    /// <summary>
    /// Calculation hash
    /// </summary>
    private string Calc;
    public string calc
    {
      get
      {
        return (Calc);
      }

      set
      {
        Calc = value;
        OnPropertyChanged ("calc");
      }
    }

    /// <summary>
    /// Result ig. OK or FAILED
    /// </summary>
    private string Result;
    public string result
    {
      get
      {
        return (Result);
      }

      set
      {
        Result = value;
        OnPropertyChanged ("result");
      }
    }

    /// <summary>
    /// To compare hash
    /// </summary>
    private string Compare;
    public string compare
    {
      get
      {
        return (Compare);
      }

      set
      {
        const string pattern = @"[a-fA-F\d]{32}";
        Match match = Regex.Match (value, pattern);

        if (match.Success)
          Compare = value;
        else
          Compare = string.Empty;

        OnPropertyChanged ("compare");
      }
    }

    /// <summary>
    /// SHA1 hash
    /// </summary>
    private string SHA256Hash;
    public string sha256hash
    {
      get
      {
        return (SHA256Hash);
      }

      set
      {
        SHA256Hash = value;
        OnPropertyChanged ("sha256hash");
      }
    }

    /// <summary>
    /// Compare 256 Hash
    /// </summary>
    private string Compare256Hash;
    public string compare256hash
    {
      get
      {
        return (Compare256Hash);
      }

      set
      {
        const string pattern = @"[A-Fa-f0-9]{64}";
        Match match = Regex.Match (value, pattern);

        if (match.Success)
          Compare256Hash = value;
        else
          Compare256Hash = string.Empty;

        OnPropertyChanged ("compare256hash");
      }
    }

    /// <summary>
    /// Result ig. OK or FAILED
    /// </summary>
    private string Result256Compare;
    public string result256compare
    {
      get
      {
        return (Result256Compare);
      }

      set
      {
        Result256Compare = value;
        OnPropertyChanged ("result256compare");
      }
    }
  }
}
