using System.Diagnostics;


namespace WinMd5Checksum.Utils
{
  public static class TraceHelper
  {
    public static string GetFunctionName (string objectName)
    {
      StackTrace st = new StackTrace ( );

      return (string.Format ("{0}: {1}", objectName, st.GetFrame (1).GetMethod ().Name));
    }
  }
}
