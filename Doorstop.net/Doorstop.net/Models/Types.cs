using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorstop.net.Models
{
  public class Types
  {
    /// <summary>
    /// Unique document prefixes.
    /// </summary>
    public class Prefix
    {
      ///TODO: copy the logic from the python code. Doesn't do any checking for the valid prefixes here. Maybe relyin on Python for that????
      public String Value { get; set; }

      public String Short { get { return Value.ToLower(); } }
    }

    /// <summary>
    /// Unique item ID built from document prefix and number.
    /// </summary>
    public class UID
    {
      ///TODO: copy the logic from the python code. Doesn't do any checking for the valid prefixes here. Maybe relyin on Python for that????
      public String Value { get; set; }
    }
    /// <summary>
    ///    Variable-length numerical outline level values.
    ///
    ///    Level values cannot contain zeros. Zeros are reserved for
    ///    identifying "heading" levels when written to file.
    /// </summary>
    public class Level
    {
      private List<int> levelComponents;
      public bool IsHeading {get; set;} = false;

      /// <summary>
      /// Initialize an item level from a sequence of numbers.
      /// </summary>
      /// <param name="level">sequence of int, float, or period-delimited string</param>
      /// <param name="heading">force a heading value(or inferred from trailing zero)</param>
      public Level(String level = "", String heading="")
      {
        levelComponents = load_level(level);

      }
      public Level(Level level)
      {
        levelComponents = new List<int>(level.levelComponents);
        IsHeading = level.IsHeading;

      }

      /// <summary>
      /// Convert an iterable, number, or level string to a tuple.
      /// </summary>
      /// <example>
      ///    >>> Level.load_level("1.2.3")
      ///    [1, 2, 3]
      ///
      ///    Level.load_level(['4', '5'])
      ///    [4, 5]
      ///
      ///    >>> Level.load_level(4.2)
      ///    [4, 2]
      ///
      ///    >>> Level.load_level([7, 0, 0])
      ///    [7, 0]
      ///
      ///    >>> Level.load_level(1)
      ///     [1]
      /// </example>
      /// <param name="value"></param>
      /// <returns></returns>
      private static List<int> load_level(string value)
      {
        // Split strings by periods
        string[] components = value.Split('.');

        // Clean up multiple trailing zeros
        List<int> parts = new List<int>();
        foreach(string component in components)
        {
          int curInt = -1;
          if (!int.TryParse(component, out curInt))
            throw new FormatException("The level value " + value + " is invalid");
          parts.Add(curInt);
        }


        ///TODO: Need to clean up multiple trailing zeros.
        //Python code:
        //        # Clean up multiple trailing zeros
        //        parts = [int(n) for n in nums]
        //        if parts and parts[-1] == 0:
        //            while parts and parts[-1] == 0:
        //                del parts[-1]
        //            parts.append(0)

       if (parts.Count == 0)
          parts.Add(1);
        return parts;
      }

    }
  }
}