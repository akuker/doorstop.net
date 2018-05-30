using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace Doorstop.net.Models
{
  /// <summary>
  /// Represents a document directory containing an outline of items
  /// </summary>
  public class Document : DoorstopBaseObject
  {
    #region Global Constants
    public static class DocumentConstants
    {
      public const string ConfigFileName = ".doorstop.yml";
      public const string SkipFileName = ".doorstop.skip";
      public const string AssetsDirName = "assets";
      public const string Index = "index.yml";
    }
    #endregion


    #region Properties
    public ObservableCollection<Item> DocumentItems { get; set; }

    /// <summary>
    /// Get the path to the document's file.
    /// </summary>
    public String ConfigFile { get { return FullFilePath; } }
    /// <summary>
    /// Get the path to the document's assets if they exist else empty string .
    /// </summary>
    public String AssetsDir
    {
      get
      {
        if (System.IO.File.Exists(DirName))
          return DirName;
        else
          return "";
      }
    }
    public String ProjectRootPath { get; private set; } = "";
    public Types.Prefix Prefix { get; private set; } = new Types.Prefix { Value = "REQ" };
    public int Digits { get; private set; } = 3;
    public String Separator { get; private set; } = "";
    public String Parent { get; set; } = "";
    /// <summary>
    /// Indicate the document should be skipped
    /// </summary>
    public bool ShouldSkip
    {
      get
      {
        var path = System.IO.Path.Combine(this.DirName, DocumentConstants.SkipFileName);
        return System.IO.File.Exists(path);
      }
    }
    #endregion

    #region Data Dictionary
    /// <summary>
    /// Data dictionary for storing information about this document
    /// </summary>
    private Dictionary<String, String> data;

    #endregion


    public Document(String path, String root, Types.Prefix prefix)
    {
      data = new Dictionary<string, string>();
      Children = new Dictionary<string, DoorstopBaseObject>();
      DocumentItems = new ObservableCollection<Item>();
      FullFilePath = path;
      ProjectRootPath = root;
      Prefix = prefix;

      // Check for an existing document
      if (!System.IO.File.Exists(ConfigFile))
      {
        Logger.Warning("Could not find file " + path);
        throw new Exception("Could not find file " + path);

      }
    }

    /// <summary>
    /// Load the document's properties from its file.
    /// </summary>
    /// <param name="path">File system path to the directory with the .doorstop.yml file</param>
    /// <returns>Doorstop Document object if the .doorstop.yml file is valid. Else, return null</returns>
    public static Document Load(String path)
    {
      Logger.Debug("Loading Document file: " + path);
      Document retValue = null;
      // Load the YML file
      try {
        var yamlFilePath = System.IO.Path.GetFullPath(path);
        // If the path is a file name, we need to get its enclosing directory.
        if (System.IO.File.Exists(yamlFilePath))
          yamlFilePath = System.IO.Path.GetDirectoryName(yamlFilePath);
        var yamlFileName = System.IO.Path.Combine(yamlFilePath, DocumentConstants.ConfigFileName);
        using (var fileStream = new System.IO.StreamReader(yamlFileName))
        {
          var yamlStream = new YamlStream();
          yamlStream.Load(fileStream);


          var mapping = (YamlMappingNode)yamlStream.Documents[0].RootNode;
          foreach (var entry in mapping.Children)
          {
              Console.WriteLine("Child: " + ((YamlScalarNode)entry.Key).Value);
          }

          Types.Prefix newPrexi = new Types.Prefix();
          String newSeparator = "";
          String newParent = "";
          int newDigits = -1;

          var settings = (YamlMappingNode)mapping.Children[new YamlScalarNode("settings")];
          foreach (var setting in settings.Children)
          {
            Console.WriteLine(setting);
            switch(setting.Key.ToString())
            {
              case "prefix":
                newPrexi.Value = setting.Value.ToString();
                break;
              case "sep":
                newSeparator = setting.Value.ToString().Trim();
                break;
              case "parent":
                newParent = setting.Value.ToString().Trim();
                break;
              case "digits":
                int tempInt = -1;
                if (int.TryParse(setting.Value.ToString(), out tempInt))
                {
                  newDigits = tempInt;
                }
                else
                  Logger.Warning("Found invalid digits setting \"" + setting.Key + "\" in " + yamlFileName);
                break;
              default:
                Logger.Warning("Found unhandled setting: " + setting.Key + " in " + yamlFileName);
                break;
            }
          }

          retValue = new Document(yamlFileName, yamlFilePath, newPrexi);
          if (newSeparator.Length > 0)
            retValue.Separator = newSeparator;
          if (newParent.Length > 0)
            retValue.Parent = newParent;
          if (newDigits > 0)
            retValue.Digits = newDigits;
        }
      }
      catch(Exception ex)
      {
        Logger.Warning(ex);
      }
      return retValue;



    }

    public void LoadChildren()
    {
      string searchFilter = Prefix.Value + "*.yml";
      string[] itemsToLoad = System.IO.Directory.GetFiles(DirName, searchFilter);
      foreach (string itemToLoad in itemsToLoad)
      {
        Models.Item newItem = Models.Item.Load(itemToLoad, this);
        if (newItem != null)
          DocumentItems.Add(newItem);
        else
          Logger.Warning("Error while reading itemToLoad");
      }
    }

    /// <summary>
    /// Saves this object to a YAML file. If the filename is specified, it will be saved to that location
    /// (can be used to make copies of doorstop objects). Otherwise, will use the original file name of
    /// this object.
    ///
    /// Most of the time, the fileName will not be specified as an argument.
    /// </summary>
    /// <param name="fileName">Optional file name to save this object to</param>
    public override void Save(string fileName = null)
    {
      throw new NotImplementedException();
    }

    public override void AddChild(DoorstopBaseObject newChild)
    {
      throw new NotImplementedException();
    }

    public override void RemoveChilde(DoorstopBaseObject delChild)
    {
      throw new NotImplementedException();
    }
  }
}
