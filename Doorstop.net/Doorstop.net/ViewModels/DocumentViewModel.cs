using Doorstop.net.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

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

    #region Commands
    public ICommand SaveDocumentCommand { get; set; }
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

    private void ExecuteSaveDocument(string Path = null)
    {
      System.Windows.MessageBox.Show("Not Implemented yet!!");

    }


    public DocumentViewModel()
    {
      DocumentItems = new ObservableCollection<Models.Item>();
      SaveDocumentCommand = new DelegateCommand<string>(ExecuteSaveDocument, (z) => { return true; });
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

  public class StringNullOrEmptyToVisibilityConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return string.IsNullOrEmpty(value as string)
          ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
    }
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return null;
    }
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }
  }

  public class NeedsToBeSavedBoolToColorConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return ((value as bool?) == true)
          ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.LightGreen);
    }
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return null;
    }
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }

  }

}
