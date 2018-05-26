using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorstop.net
{
  public static class Logger
  {
    public static void Warning(string message)
    {
      Console.WriteLine("!!Warning: " + message);
    }

    public static void Warning(Exception ex)
    {
      Warning(ex.Message);
    }

    public static void Debug(string message)
    {
      Console.WriteLine(" Debug: " + message);
    }
  }
}
