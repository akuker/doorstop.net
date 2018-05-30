using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Doorstop.net.Models
{
  public abstract class DoorstopBaseObject : INotifyPropertyChanged
  {
    #region INotifyPropertyChanged Utilities
    public event PropertyChangedEventHandler PropertyChanged;

    // This method is called by the Set accessor of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        if (propertyName != "NeedsToBeSaved")
        {
          NeedsToBeSaved = true;
        }
      }
    }
    #endregion




    /// <summary>
    /// Saves this object to a YAML file. If the filename is specified, it will be saved to that location
    /// (can be used to make copies of doorstop objects). Otherwise, will use the original file name of
    /// this object.
    ///
    /// Most of the time, the fileName will not be specified as an argument.
    /// </summary>
    /// <param name="fileName">Optional file name to save this object to</param>
    public abstract void Save(string fileName = null);

    private Dictionary<String,DoorstopBaseObject> children;

    public Dictionary<String, DoorstopBaseObject> Children
    {
      get { return children; }
      set { children = value; }
    }

    public abstract void AddChild(DoorstopBaseObject newChild);
    public abstract void RemoveChilde(DoorstopBaseObject delChild);


    #region Properties

    private bool needsToBeSaved = true;
    [YamlDotNet.Serialization.YamlIgnore]
    public bool NeedsToBeSaved
    {
      get { return needsToBeSaved; }
      set { needsToBeSaved = value; NotifyPropertyChanged(); }
    }

    private bool isInvalid = true;
    [YamlDotNet.Serialization.YamlIgnore]
    public bool IsInvalid
    {
      get { return isInvalid; }
      set { isInvalid = value; NotifyPropertyChanged(); }
    }

    /// <summary>
    /// Full path of the file associated with this View Model
    /// </summary>
    [YamlDotNet.Serialization.YamlIgnore]
    public virtual string DirName
    {
      get
      {
        return System.IO.Path.GetDirectoryName(fullFilePath);
      }
    }

    /// <summary>
    /// File name with extension (without path) of the file associated with this View Model
    /// </summary>
    [YamlDotNet.Serialization.YamlIgnore]
    public virtual string FileName
    {
      get
      {
        return System.IO.Path.GetFileName(fullFilePath);
      }
    }

    private string fullFilePath = "";
    /// <summary>
    /// Full file name (with path) of the file associated with this View Model
    /// </summary>
    [YamlDotNet.Serialization.YamlIgnore]
    public virtual string FullFilePath
    {
      get { return fullFilePath; }
      set
      {
        if (fullFilePath != value)
        {
          fullFilePath = System.IO.Path.GetFullPath(value);
          // The Directory name and file name properties are generated from this
          // property, so we need to raise the property changed event for them as well.
          NotifyPropertyChanged();
          NotifyPropertyChanged("DirName");
          NotifyPropertyChanged("FileName");
        }
      }
    }
    #endregion

  }

}
