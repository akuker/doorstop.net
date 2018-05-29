using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

      public override string ToString()
      {
        return this.Value;
      }
    }

    /// <summary>
    /// Unique item ID built from document prefix and number.
    /// </summary>
    public class UID
    {
      ///TODO: copy the logic from the python code. Doesn't do any checking for the valid prefixes here. Maybe relyin on Python for that????
      public String Value { get; set; }

      public override string ToString()
      {
        return this.Value;
      }
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
      public override string ToString()
      {
        string retString = "";
        retString = this.levelComponents[0].ToString();
        for (int i=1; i<this.levelComponents.Count; i++)
        {
          retString += ("." + this.levelComponents[i].ToString());
        }
        return retString;
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

    public class Link : INotifyPropertyChanged
    {
      #region INotifyPropertyChanged Utilities
      public event PropertyChangedEventHandler PropertyChanged;

      // This method is called by the Set accessor of each property.
      // The CallerMemberName attribute that is applied to the optional propertyName
      // parameter causes the property name of the caller to be substituted as an argument.
      private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
      {
        if (PropertyChanged != null)
        {
          PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
      }
      #endregion

      public String Stamp { get; set; }
      public Types.UID UID { get; set; }
      public override string ToString()
      {
        return UID.ToString();
      }

    }

  }

  /// <summary>
  /// Template class for holding Item attributes. THIS IS NOT USED curently. But,
  /// someday it probably should be when the code is refactored to treat attributes
  /// generically, instead of using hard-coded attributes.
  ///
  /// The attribute key will always be a string.
  /// The attribute value can changed, based upon the class passed to this template
  /// </summary>
  /// <typeparam name="T">Class of the attribute value.</typeparam>
  public class IItemAttribute<T> : INotifyPropertyChanged
  {
    #region INotifyPropertyChanged Utilities
    public event PropertyChangedEventHandler PropertyChanged;

    // This method is called by the Set accessor of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    #endregion

    private string localKey;
    public string Key
    {
      get { return localKey; }
      set { localKey = value; NotifyPropertyChanged(); }
    }

    private T localValue;
    public T Value
    {
      get { return localValue; }
      set { localValue = value; NotifyPropertyChanged(); }
    }

    public override string ToString()
    {
      return "[" + Key + "] " + Value.ToString();
    }

  }

}
