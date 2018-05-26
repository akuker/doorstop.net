using Doorstop.net.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Doorstop.net.ViewModels
{
  public class DocumentViewModel : INotifyPropertyChanged
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

    public ObservableCollection<Models.Item> DocumentItems { get; set; }

    private string filePath;

    public string FilePath
    {
      get { return filePath; }
      set
      {
        if (filePath != value)
        {
          filePath = System.IO.Path.GetDirectoryName(value);
          NotifyPropertyChanged();
          ReadDocument();
        }
      }
    }


    public DocumentViewModel()
    {
      DocumentItems = new ObservableCollection<Models.Item>();

    }

    public void ReadDocument()
    {
      Document myNewDoc = Document.Load(FilePath);
      string searchFilter = myNewDoc.Prefix.Value + "*.yml";
      string[] itemsToLoad = System.IO.Directory.GetFiles(FilePath, searchFilter);
      foreach (string itemToLoad in itemsToLoad)
      {
        Item newItem = Item.Load(itemToLoad,myNewDoc);
        if (newItem != null)
          DocumentItems.Add(newItem);
        else
          Logger.Warning("Error while reading itemToLoad");
      }
    }

  }
}
