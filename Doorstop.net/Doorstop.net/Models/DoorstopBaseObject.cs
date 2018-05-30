using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Doorstop.net.Models
{
  public abstract class DoorstopBaseObject : INotifyPropertyChanged, IComparable<DoorstopBaseObject>
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


    #region Tree Support stuff
    private ObservableCollection<DoorstopBaseObject> children;

    public ObservableCollection<DoorstopBaseObject> Children
    {
      get { return children; }
      set { children = value; }
    }

    public abstract void AddChild(DoorstopBaseObject newChild);
    public abstract void RemoveChilde(DoorstopBaseObject delChild);

    protected void AddChildren(IEnumerable<DoorstopBaseObject> nodes)
    {
      var immediateChildrenNodes =
        from x in nodes
        where x.Level.ParentLevel == this.Level
        orderby x.Level ascending
        select x;

      this.Children = new ObservableCollection<DoorstopBaseObject>(immediateChildrenNodes);
      IEnumerable<DoorstopBaseObject> remainingNodes = nodes.Except(immediateChildrenNodes);

      foreach (DoorstopBaseObject child in this.Children)
      {
        var descendentNodes =
          from x in remainingNodes
          where x.Level.IsDescendentOf(child.Level)
          select x;

        child.AddChildren(descendentNodes);
        remainingNodes = remainingNodes.Except(descendentNodes);
      }

      if (remainingNodes.Count() > 0)
      {
        Logger.Warning("Unable to add some nodes to the tree. It might be incomplete!!!!");
      }
    }

    public int CompareTo(DoorstopBaseObject other)
    {
      return this.Level.CompareTo(other.Level);
    }


    public static IComparer<DoorstopBaseObject> SortAscendingLevel()
    {
      return (IComparer<DoorstopBaseObject>)new sortAscendingLevelHelper();
    }

    // Nested class to do descending sort on make property.
    private class sortAscendingLevelHelper : IComparer<DoorstopBaseObject>
    {
      public int Compare(DoorstopBaseObject x, DoorstopBaseObject y)
      {
        return x.Level.CompareTo(y.Level);
      }
    }

    #endregion

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

    private Types.Level myLevel = new Types.Level("");
    [YamlDotNet.Serialization.YamlIgnore]
    public Types.Level Level
    {
      get { return myLevel; }
      set { myLevel = value; NotifyPropertyChanged(); }
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
