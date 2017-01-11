using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Org.Vs.WinMd5Checksum.Data
{
  public static class Md5Files
  {
    private readonly static List<Md5Structure> filesContainer = new List<Md5Structure>();
    private static Md5Structure item = new Md5Structure();


    /// <summary>
    /// Get files container
    /// </summary>
    public static List<Md5Structure> GetFileContainer
    {
      get
      {
        return (filesContainer);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmdArg"></param>
    public static void AddFileToContainer (string cmdArg)
    {
      RegexFunction(cmdArg);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void FinishOperation ()
    {
      if ((item.key != null && item.compare != null) || (item.key != null && item.compare == null) || (item.key == null && item.compare != null))
        AddFileContainer();
    }

    /// <summary>
    /// 
    /// </summary>
    public static void ClearAll ()
    {
      filesContainer.Clear();
      item = new Md5Structure();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmdArg"></param>
    private static void RegexFunction (string cmdArg)
    {
      // Files pattern
      string pattern = @"^\\|\w(.*)(?=\.\w)(.*)";
      Match match = Regex.Match(cmdArg, pattern);

      if (match.Success)
      {
        if (item.compare == null && item.key != null)
          AddFileContainer();

        item.key = cmdArg;

        if (item.key != null && item.compare != null)
          AddFileContainer();

        return;
      }

      // Md5 Hash pattern
      pattern = @"[a-fA-F\d]{32}";
      match = Regex.Match(cmdArg, pattern);

      if (!match.Success)
        return;

      item.compare = cmdArg;

      if (item.key != null && item.compare != null)
        AddFileContainer();

      return;
    }

    /// <summary>
    /// 
    /// </summary>
    private static void AddFileContainer ()
    {
      filesContainer.Add(item);
      item = new Md5Structure();
    }
  }
}
