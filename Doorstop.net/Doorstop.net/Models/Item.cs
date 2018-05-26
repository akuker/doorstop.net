using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace Doorstop.net.Models
{

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

  public class Item : INotifyPropertyChanged
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

    #region Global Constants
    public static class ItemConstants
    {
      public static Types.Level DefaultLevel = new Types.Level("1.0");
      public const string FileExtension = ".yml";
    }
    #endregion

    #region Properties
    private Types.UID uid;

    public Types.UID UID
    {
      get { return uid; }
      set { uid = value; NotifyPropertyChanged(); }

  }

    private Types.Prefix prefix;

    public Types.Prefix Prefix
    {
      get { return prefix; }
      set { prefix = value; NotifyPropertyChanged(); }
    }

    private int number;

    public int Number
    {
      get { return number; }
      set { number = value; NotifyPropertyChanged(); }
    }

    private Types.Level level;

    public Types.Level Level
    {
      get { return level; }
      set { level = value; NotifyPropertyChanged(); }
    }

    private string heading;

    public string Heading
    {
      get { return heading; }
      set { heading = value; NotifyPropertyChanged(); }
    }

    private string text;

    public string Text
    {
      get { return text; }
      set { text = value; NotifyPropertyChanged(); }
    }

    private string fileName;

    public string FileName
    {
      get { return fileName; }
      set { fileName = value; NotifyPropertyChanged(); UID = new Types.UID { Value = System.IO.Path.GetFileNameWithoutExtension(value) }; }
    }

    #endregion

    #region Links
    public ObservableCollection<Types.Link> ParentLinks { get; set; } = new ObservableCollection<Types.Link>();
  public ObservableCollection<Types.Link> ChildLinks { get; set; } = new ObservableCollection<Types.Link>();

    public string ParentLinksString
    {
      get
      {
        string retString = "";
        foreach (var attribute in ParentLinks)
        {
          retString += attribute.ToString() + Environment.NewLine;
        }
        return retString;
      }
    }
    public string ChildrenLinksString
    {
      get
      {
        string retString = "";
        foreach (var attribute in ChildLinks)
        {
          retString += attribute.ToString() + Environment.NewLine;
        }
        return retString;
      }
    }
    #endregion

    #region Attributes
    public ObservableCollection<IItemAttribute<string>> Attributes { get; set; }

    public string AttributesString
    {
      get
      {
        string retString = "";
        foreach (var attribute in Attributes)
        {
          retString += attribute.ToString() + Environment.NewLine;
        }
        return retString;
      }
    }
    #endregion

    #region Constructor/destructor
    public Item()
    {
      Attributes = new ObservableCollection<IItemAttribute<string>>();
      Level = ItemConstants.DefaultLevel;
    }
    #endregion

    #region Load and Save
    public static Item Load(string path, Document doc)
    {
      Logger.Debug("Loading Item file: " + path);
      Item retValue = new Item();
      retValue.FileName = path;

      // Load the YML file
      try
      {
        var yamlFileName = System.IO.Path.GetFullPath(path);
        using (var fileStream = new System.IO.StreamReader(yamlFileName))
        {
          var yamlStream = new YamlStream();
          yamlStream.Load(fileStream);


          var mapping = (YamlMappingNode)yamlStream.Documents[0].RootNode;
          foreach (var entry in mapping.Children)
          {
            Console.WriteLine("Child[" + entry.Key.ToString() + "]: " + ((YamlScalarNode)entry.Key).Value);
            string currentKey = entry.Key.ToString();
            var currentValue = entry.Value;
            switch (currentKey)
            {
              case "level":
                retValue.Level = new Types.Level(currentValue.ToString());
                break;
              case "text":
                retValue.Text = currentValue.ToString();
                break;
              case "header":
                retValue.Heading = currentValue.ToString();
                break;
              case "links":
                retValue.addLinks(currentValue);
                break;
              default:
                retValue.Attributes.Add(new IItemAttribute<string> { Key = currentKey, Value = currentValue.ToString() });
                Logger.Debug("Found un-handled Item Attribute: " + currentKey);
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Warning(ex);
      }
      return retValue;
    }

    private void addLinks(YamlNode yamlNode)
    {
      YamlSequenceNode listOfLinksNode = yamlNode as YamlSequenceNode;
      if(listOfLinksNode != null)
      {
        foreach (YamlNode child in listOfLinksNode.Children)
        {
          YamlMappingNode linkList = child as YamlMappingNode;
          if(linkList != null)
          {
            foreach(var link in linkList.Children)
            {

              YamlScalarNode linkUid = link.Key as YamlScalarNode;
              YamlScalarNode linkStamp = link.Value as YamlScalarNode;

              Types.Link newLink = new Types.Link();
              if((linkUid != null) && (linkUid.ToString().Length > 1))
              {
                newLink.UID = new Types.UID { Value = linkUid.ToString() };
                if ((linkStamp != null) && (linkStamp.ToString().Length > 1))
                  newLink.Stamp = linkStamp.ToString();
                this.ParentLinks.Add(newLink);
                Logger.Debug("[" + this.UID.Value.ToString() + "] links to: " + linkUid.ToString() + ":" + linkStamp.ToString());
              }
              else
              {
                Logger.Warning("Found invalid link UID in " + this.FileName);
              }
            }
          }
          else
          {
            Logger.Warning("Found invalid link item in " + this.FileName);
          }
        }
      }
      else
      {
        Logger.Warning("Found invalid links in " + this.FileName);
      }
      Console.WriteLine(yamlNode);

    }

    public void Save()
    {

    }
    #endregion


  }
}
