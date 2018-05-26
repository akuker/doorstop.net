using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Doorstop.net.Models
{

  public class RequirementsDocument: INotifyPropertyChanged
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

    private string shortName;

    public string ShortName
    {
      get { return shortName; }
      set { shortName = value; NotifyPropertyChanged(); }
    }

    private string fullPath;

    public string FullPath
    {
      get { return fullPath; }
      set
      {
        fullPath = value;
        ShortName = System.IO.Path.GetFileName(fullPath);
        NotifyPropertyChanged();
      }
    }

    public override string ToString()
    {
      return FullPath;
    }

  }

  public class RequirementsFolder : RequirementsDocument
  {
    public ObservableCollection<RequirementsDocument> Children { get; set; }
    public RequirementsFolder()
    {
      Children = new ObservableCollection<RequirementsDocument>();
    }

    /// <summary>
    /// Checks to see if the directory contains a doorstop document at any level.
    /// </summary>
    /// <returns>True if this direcotry contains a doorstop document</returns>
    public bool ContainsDoorstopDocument()
    {
      foreach (var child in Children)
      {
        if (child is RequirementsDocument)
          return true;
        else if (child is RequirementsFolder)
          if (((RequirementsFolder)child).ContainsDoorstopDocument())
            return true;

      }
      return false;
    }

    public void LoadChildren()
    {
      List<string> files = new List<string>(System.IO.Directory.GetDirectories(this.FullPath));
      files.AddRange(new List<string>(System.IO.Directory.GetFiles(this.FullPath)));

      foreach (string curFile in files)
      {
        if (System.IO.File.Exists(curFile))
        {
          // We only care about Doorstop documents. Ignore everything else
          if(System.IO.Path.GetFileName(curFile) == Document.DocumentConstants.ConfigFileName)
            Children.Add(new RequirementsDocument { FullPath = curFile });
        }
        else if (System.IO.Directory.Exists(curFile))
        {
          var newDir = new RequirementsFolder { FullPath = curFile };
          newDir.LoadChildren();
          if(newDir.ContainsDoorstopDocument())
            Children.Add(newDir);
        }
        else
        {
          Logger.Warning("Could not find the file \"" + curFile + "\"");
        }
      }

    }
  }
}
