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
  }
}
