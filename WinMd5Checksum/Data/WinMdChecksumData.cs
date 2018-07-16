﻿using System.IO;
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
        if (Equals(value, _fileName))
          return;

        _fileName = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// File without path
    /// </summary>
    public string File => string.IsNullOrWhiteSpace(FileName) ? string.Empty : Path.GetFileName(FileName);

    private string _md5Hash;

    /// <summary>
    /// MD5 hash sum
    /// </summary>
    public string Md5Hash
    {
      get => _md5Hash;
      set
      {
        if (Equals(value, _md5Hash))
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
        if (Equals(value, _md5ToCompareHash))
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
        if (Equals(value, _md5Result))
          return;

        _md5Result = value;
        OnPropertyChanged();
      }
    }
  }
}
