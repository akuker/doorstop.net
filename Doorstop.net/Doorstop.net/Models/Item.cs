using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace Doorstop.net.Models
{



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
        if (propertyName != "NeedsToBeSaved")
        {
          NeedsToBeSaved = true;
        }
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
    [YamlDotNet.Serialization.YamlIgnore]
    public Types.UID UID
    {
      get { return uid; }
      set { uid = value; NotifyPropertyChanged(); }

  }

    private Types.Prefix prefix;
    [YamlDotNet.Serialization.YamlIgnore]
    public Types.Prefix Prefix
    {
      get { return prefix; }
      set { prefix = value; NotifyPropertyChanged(); }
    }

    private Types.Level level;
    [YamlDotNet.Serialization.YamlMember(SerializeAs = typeof(Types.Level),Alias= "level", Order = 4)]
    public Types.Level Level
    {
      get { return level; }
      set { level = value; NotifyPropertyChanged(); }
    }

    private string header = "";
    [YamlDotNet.Serialization.YamlMember(SerializeAs = typeof(String), Alias = "header", Order = 3, ScalarStyle = ScalarStyle.Literal)]
    public string Header
    {
      get { return header; }
      set { header = value; NotifyPropertyChanged(); }
    }

    private string text = "";
    [YamlDotNet.Serialization.YamlMember(SerializeAs = typeof(String), Alias = "text", Order = 9, ScalarStyle = ScalarStyle.Literal)]
    public string Text
    {
      get { return text; }
      set { text = value; NotifyPropertyChanged(); }
    }

    private string reviewed;
    [YamlDotNet.Serialization.YamlMember(SerializeAs = typeof(String), Alias = "reviewed", Order = 8)]
    public string Reviewed
    {
      get { return reviewed; }
      set { reviewed = value; NotifyPropertyChanged(); }
    }

    private string normative;
    [YamlDotNet.Serialization.YamlMember(SerializeAs = typeof(String), Alias = "normative", Order = 6)]
    public string Normative
    {
      get { return normative; }
      set { normative = value; NotifyPropertyChanged(); }
    }

    private string reference;
    [YamlDotNet.Serialization.YamlMember(SerializeAs = typeof(String), Alias = "ref", Order = 7)]
    public string Reference
    {
      get { return reference; }
      set { reference = value; NotifyPropertyChanged(); }
    }

    private string derived;
    [YamlDotNet.Serialization.YamlMember(SerializeAs = typeof(String), Alias = "derived", Order = 2)]
    public string Derived
    {
      get { return derived; }
      set { derived = value; NotifyPropertyChanged(); }
    }

    private string active;
    [YamlDotNet.Serialization.YamlMember(SerializeAs = typeof(String), Alias = "active", Order = 1)]
    public string Active
    {
      get { return active; }
      set { active = value; NotifyPropertyChanged(); }
    }

    private string fileName;
    [YamlDotNet.Serialization.YamlIgnore]
    public string FileName
    {
      get { return fileName; }
      set { fileName = value; NotifyPropertyChanged(); UID = new Types.UID { Value = System.IO.Path.GetFileNameWithoutExtension(value) }; }
    }

    private bool needsToBeSaved;
    [YamlDotNet.Serialization.YamlIgnore]
    public bool NeedsToBeSaved
    {
      get { return needsToBeSaved; }
      set { needsToBeSaved = value; NotifyPropertyChanged(); }
    }

    private bool isInvalid;
    [YamlDotNet.Serialization.YamlIgnore]
    public bool IsInvalid
    {
      get { return isInvalid; }
      set { isInvalid = value; NotifyPropertyChanged(); }
    }

    #endregion

    #region Links
    [YamlDotNet.Serialization.YamlMember(Alias = "links", Order = 5, ScalarStyle = ScalarStyle.Literal )]
    ///<remarks>
    ///The python version of Doorstop saves the links in a sequence of mapping nodes. In order to
    ///emulate that in C#, we need to present the links to YamlDotNet as a list of dictionaryies
    ///
    /// Behind the scenes, the links are stored as just a plain ObservableCollection, so they're
    /// easier to work with from WPF/C#. The 'ParentLinksYaml' attribute is only used when
    /// serializing or deserializing links.
    ///</remarks>
    public List<Dictionary<string,string>> ParentLinksYaml
    {
      get
      {
        var retList = new List<Dictionary<string, string>>();
        foreach (Types.Link thisLink in ParentLinks)
        {
          var retDict = new Dictionary<string, string>();
          retList.Add(retDict);
          retDict[thisLink.UID.ToString()] = thisLink.Stamp;
        }
        return retList;
      }
      set
      {
        List<Dictionary<string, string>> listDictionary = value as List<Dictionary<string, string>>;
        if (value != null)
        {
          try
          {
            foreach (var dictionary in listDictionary)
            {
              foreach (string uidString in dictionary.Keys)
              {
                Types.Link newLink = new Types.Link
                {
                  UID = new Types.UID { Value = uidString },
                  Stamp = dictionary[uidString]
                };
                this.ParentLinks.Add(newLink);
              }
            }
          }
          catch (Exception ex)
          {
            Logger.Warning("Error while processing the links for " + this.FileName + ": " + ex.Message);
            throw ex;
          }
        }
      }
    }
    [YamlDotNet.Serialization.YamlIgnore]
    public ObservableCollection<Types.Link> ParentLinks { get; set; } = new ObservableCollection<Types.Link>();
    [YamlDotNet.Serialization.YamlIgnore]
    // ChildLinks are not stored here yet..... this will be future functionality when we go collect all the incoming
    // links.
    public ObservableCollection<Types.Link> ChildLinks { get; set; } = new ObservableCollection<Types.Link>();
    [YamlDotNet.Serialization.YamlIgnore]
    // ParentLinksString is used to provide an easy way of dumping the Links to a string for debugging purposes
    // or for listing them in a GUI.
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
    #endregion

    #region Constructor, Load and Save
    public Item()
    {
      Level = ItemConstants.DefaultLevel;
    }

    public static Item Load(string path, Document doc)
    {
      // Load the YML file
      Logger.Debug("Loading Item file: " + path);
      Item retValue;

      var yamlFileName = System.IO.Path.GetFullPath(path);
      using (var fileStream = new System.IO.StreamReader(yamlFileName))
      {
        // Load with deserializer
        try
        {
          var deserializer = new DeserializerBuilder()
            .WithTypeConverter(new LevelYamlTypeConverter())
            .Build();
          retValue = deserializer.Deserialize<Item>(fileStream);
          retValue.FileName = yamlFileName;
          retValue.NeedsToBeSaved = false;
          // YamlDotNet can put an extra carriage return after some fields. This
          // code is to work around that. For more information see:
          //     https://github.com/aaubry/YamlDotNet/issues/246
          retValue.Header = retValue.Header.TrimEnd(new char[]{'\n','\r'});
          retValue.Text = retValue.Text.TrimEnd(new char[] { '\n', '\r' });
        }
        catch (Exception ex)
        {
          Logger.Warning(ex.Message);
          retValue = new Item
          {
            FileName = yamlFileName,
            Text = "_**Error Loading " + yamlFileName + "**_" + Environment.NewLine + Environment.NewLine + ex.Message,
            IsInvalid = true
          };
        }
        finally
        {
          fileStream.Close();
        }
        return retValue;
      }
    }

    public void Save()
    {
      System.IO.TextWriter textWriter = new System.IO.StreamWriter(FileName, false);

      try
      {
        this.NeedsToBeSaved = false;

        var yamlSerializer = new SerializerBuilder()
          .WithTypeConverter(new LevelYamlTypeConverter())
          .Build();
        yamlSerializer.Serialize(textWriter, this);
      }
      catch (Exception ex)
      {
        Logger.Warning("Unable to save " + FileName + ": " + ex.Message);
      }
      finally
      {
        textWriter.Close();
      }
    }
    #endregion

    /// <summary>
    /// Used for debugging purposes (mostly). Will return:
    ///   - the header if it is defined
    ///   - the first N characters of the item text if it is defined
    ///   - Item ID (ex REQ0001)
    /// </summary>
    /// <returns>String representation of the Item object</returns>
    public override string ToString()
    {
      string retString = "";
      if (NeedsToBeSaved)
        retString += "* ";
      if ((this.header != null) && (this.Header.Trim().Length > 1))
      {
        retString += this.Header;
      }
      else if ((this.Text != null) && (this.Text.Trim().Length > 1))
      {
        retString += this.Text.Substring(0, (Text.Length > 20) ? 20 : Text.Length);
      }
      else
      {
        retString += this.UID.ToString() + " <empty>";
      }
      return retString;
    }

  }
}
