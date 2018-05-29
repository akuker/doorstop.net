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

  public class LevelYamlTypeConverter : IYamlTypeConverter
  {
    public bool Accepts(Type type)
    {
      return type == typeof(Types.Level);
    }

    public object ReadYaml(IParser parser, Type type)
    {
      if (type == typeof(Types.Level))
      {
        Scalar myLevel;
        try
        {
          myLevel = parser.Expect<Scalar>();
        }
        catch(YamlException ex)
        {
          Logger.Warning("An invalid level string was provided");
          throw ex;
        }
        return new Types.Level(myLevel.Value);
      }
      else
      {
        throw new NotImplementedException("Got type " + type.ToString() + " to deserialize");
      }
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      Types.Level item = value as Types.Level;
      if (item != null)
      {
        emitter.Emit(new Scalar(item.ToString()));
      }
    }
  }



  public class UidYamlTypeConverter : IYamlTypeConverter
  {
    public bool Accepts(Type type)
    {
      return type == typeof(Types.UID);
    }

    public object ReadYaml(IParser parser, Type type)
    {
      throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      Types.UID item = value as Types.UID;
      if (item != null)
      {
        string blah = item.GetHashCode().ToString();
        emitter.Emit(new YamlDotNet.Core.Events.Scalar(item.Value, item.GetHashCode().ToString()));
      }
    }
  }


  public class LinkYamlTypeConverter : IYamlTypeConverter
  {
    public bool Accepts(Type type)
    {
      return type == typeof(Types.Link);
    }

    public object ReadYaml(IParser parser, Type type)
    {
      throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      Types.Link item = value as Types.Link;
      if (item != null)
      {
        try
        {
          //emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, ));kkc
          //emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, item.UID.ToString(), item.Stamp.ToString()));
          emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, "tonykey", item.Stamp.ToString()));
        }
        catch(Exception ex)
        {
          Logger.Warning("Unable to save Link: " + ex.Message);
        }
      }
    }
  }



  public class IItemAttributeYamlTypeConverter<T> : IYamlTypeConverter
  {
    public bool Accepts(Type type)
    {
      return type == typeof(IItemAttribute<T>);
    }

    public object ReadYaml(IParser parser, Type type)
    {
      throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      IItemAttribute<T> item = value as IItemAttribute<T>;
      if (item != null)
      {
        //emitter.Emit(new YamlDotNet.Core.Events.Scalar(item.Key, item.Value.ToString()));

        try
        {
          emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, item.Key.ToString()));
          emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, item.Value.ToString()));

          //emitter.Emit(new YamlDotNet.Core.Events.Scalar("anchor","tonytag", "tonyvalue"));
          //emitter.Emit(new YamlDotNet.Core.Events.Scalar("many", item.Value.ToString()));

        }
        catch (Exception ex)
        {
          Logger.Warning(ex.Message);
        }
        //try
        //{
        //  emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, item.Value.ToString()));
        //}
        //catch (Exception ex)
        //{
        //  Logger.Warning(ex.Message);
        //}
      }
    }
  }



  public class LinkCollectionYamlTypeConverter : IYamlTypeConverter
  {
    public bool Accepts(Type type)
    {
      return type == typeof(ObservableCollection<Types.Link>);
    }

    public object ReadYaml(IParser parser, Type type)
    {
      throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      ObservableCollection<Types.Link> items = value as ObservableCollection<Types.Link>;
      if (items != null)
      {
        emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));
        //emitter.Emit(new MappingStyle());


        var newMap = new YamlMappingNode();

        foreach (Types.Link item in items)
        {
          newMap.Children.Add(new KeyValuePair<YamlNode,YamlNode>(
            new YamlScalarNode(item.UID.ToString()),
            new YamlScalarNode(item.Stamp.ToString())));
          //emitter.Emit(new YamlDotNet.Core.Events.NodeEvent(  Item);

          //try
          //{
          //  emitter.Emit(new YamlDotNet.Core.Events.Scalar(item.Key, item.Value.ToString()));
          //}catch(Exception ex)
          //{
          //  Logger.Warning(ex.Message);
          //}

          //try
          //{
          //  emitter.Emit(new YamlDotNet.Core.Events.Scalar("tonykey--" + item.Key.ToString(), null));
          //}
          //catch (Exception ex)
          //{
          //  Logger.Warning(ex.Message);
          //}

          //try
          //{
          //  emitter.Emit(new YamlMappingNode())
          //  emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, null, item.UID.ToString()));
          //  emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, item.Stamp.ToString()));
          //}
          //catch (Exception ex)
          //{
          //  Logger.Warning(ex.Message);
          //}


          //emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, "tonyvalue--" + item.Value.ToString()));
        }
        //emitter.Emit(new YamlDotNet.Core.Events.Scalar(newMap));
        emitter.Emit(new MappingEnd());
      }
    }
  }


  public class IItemAttributeCollectionYamlTypeConverter<T> : IYamlTypeConverter
  {
    public bool Accepts(Type type)
    {
      return type == typeof(ObservableCollection<IItemAttribute<T>>);
    }

    public object ReadYaml(IParser parser, Type type)
    {
      throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      ObservableCollection<IItemAttribute<T>> items = value as ObservableCollection<IItemAttribute<T>>;
      if (items != null)
      {
        //emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

        foreach (IItemAttribute<T> item in items)
        {
          //emitter.Emit(new YamlDotNet.Core.Events.NodeEvent(  Item);

          //try
          //{
          //  emitter.Emit(new YamlDotNet.Core.Events.Scalar(item.Key, item.Value.ToString()));
          //}catch(Exception ex)
          //{
          //  Logger.Warning(ex.Message);
          //}

          //try
          //{
          //  emitter.Emit(new YamlDotNet.Core.Events.Scalar("tonykey--" + item.Key.ToString(), null));
          //}
          //catch (Exception ex)
          //{
          //  Logger.Warning(ex.Message);
          //}

          try
          {
            emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, item.Key));
            emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, item.Value.ToString()));
          }
          catch (Exception ex)
          {
            Logger.Warning(ex.Message);
          }


          //emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, "tonyvalue--" + item.Value.ToString()));
        }
        //emitter.Emit(new MappingEnd());
      }
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

    private int number;
    [YamlDotNet.Serialization.YamlIgnore]
    public int Number
    {
      get { return number; }
      set { number = value; NotifyPropertyChanged(); }
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
    public List<Dictionary<string,string>> ParentLinksDictionary
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
    public ObservableCollection<Types.Link> ChildLinks { get; set; } = new ObservableCollection<Types.Link>();
    [YamlDotNet.Serialization.YamlIgnore]
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
    [YamlDotNet.Serialization.YamlIgnore]
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

    #region Constructor/destructor
    public Item()
    {
      Level = ItemConstants.DefaultLevel;
    }
    #endregion

    #region Load and Save
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
          return retValue;
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


    public class NonSerializable
    {
      public string WillThrow { get { throw new Exception(); } }

      public string Text { get; set; }
    }

    public class NonSerializableTypeConverter : IYamlTypeConverter
    {
      public bool Accepts(Type type)
      {
        return typeof(NonSerializable).IsAssignableFrom(type);
      }

      public object ReadYaml(IParser parser, Type type)
      {
        var scalar = parser.Expect<Scalar>();
        return new NonSerializable { Text = scalar.Value };
      }

      public void WriteYaml(IEmitter emitter, object value, Type type)
      {
        emitter.Emit(new Scalar(((NonSerializable)value).Text));
      }
    }

    public void Save()
    {
      try
      {
        this.NeedsToBeSaved = false;
        System.IO.TextWriter textWriter = new System.IO.StreamWriter(FileName,false);

        //    var serializer = new SerializerBuilder()
        //.WithTypeConverter(new NonSerializableTypeConverter())
        //.Build();

        var yamlSerializer = new SerializerBuilder()
          //.WithTypeConverter(new IItemAttributeYamlTypeConverter<string>())
          //.WithTypeConverter(new IItemAttributeCollectionYamlTypeConverter<string>())
          //.WithTypeConverter(new UidYamlTypeConverter())
          //////.WithTypeConverter(new LinkCollectionYamlTypeConverter())
          .WithTypeConverter(new LevelYamlTypeConverter())
          .Build();

        Console.WriteLine("------------------------------------");
        yamlSerializer.Serialize(Console.Out, this);
        yamlSerializer.Serialize(textWriter, this);

  //      Console.WriteLine("--Attributes");

  //      foreach (var myAttr in Attributes)
  //      {
  ////        var yamlSerializer2 = new SerializerBuilder()
  ////.WithTypeConverter(new IItemAttributeYamlTypeConverter<string>())
  ////.WithTypeConverter(new IItemAttributeCollectionYamlTypeConverter<string>())
  ////.WithTypeConverter(new UidYamlTypeConverter())
  ////.Build();
  //        yamlSerializer.Serialize(Console.Out, myAttr);
  //        yamlSerializer.Serialize(textWriter, myAttr);
  //      }
  //      Console.WriteLine("--Done" + Environment.NewLine);







        //emitter.Emit(new Scalar(null, "Topics"));
        //emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));

        //foreach (string child in node.Topics)
        //{
        //  emitter.Emit(new Scalar(null, child));
        //}

        //emitter.Emit(new SequenceEnd());

        //foreach (IItemAttribute<string> attr in this.Attributes)
        //{
        //  yamlSerializer.e
        //  yamlSerializer.Serialize(textWriter, attr.Value);

        //}
        textWriter.Close();
      }catch(Exception ex)
      {
        Logger.Warning("Unable to save " + FileName + ": " + ex.Message);
      }

    }
    #endregion

    public override string ToString()
    {
      string retString = "";
      if (NeedsToBeSaved)
        retString += "* ";
      if ((this.header != null) && (this.Header.Length > 1))
      {
        retString += this.Header;
      }
      else if ((this.Text != null) && (this.Text.Length > 1))
      {
        retString += this.Text;
      }
      else
      {
        retString += this.UID.ToString() + " <empty>";
      }
      return retString;
    }

  }
}
