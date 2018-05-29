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
  public class DocumentViewModel : DoorstopBaseViewModel
  {
    public class ItemViewModel
    {
      private Models.Item item;

      public Models.Item Item
      {
        get { return item; }
        set { item = value; }
      }

      public override string ToString()
      {
        return Item.ToString();
      }


      public ICommand LaunchItemEditorCommand { get; set; }

      public ItemViewModel(String path, Document doc)
      {
        Item = Models.Item.Load(path, doc);
        LaunchItemEditorCommand = new DelegateCommand<string>(ExecuteLaunchItemEditor, (z) => { return true; });
      }

      public void ExecuteLaunchItemEditor(string str = null)
      {
        Views.ItemEditor itemEditor = new Views.ItemEditor(this.Item);
        itemEditor.ShowDialog();
      }

    }

    public ObservableCollection<ItemViewModel> DocumentItems { get; set; }

    protected override void ExecuteSaveDocument(string Path = null)
    {
      foreach(var itemView in this.DocumentItems)
      {
        if (itemView.Item.NeedsToBeSaved)
          itemView.Item.Save();
      }
    }

    public ICommand OpenItemEditorCommand { get; set; }

    public DocumentViewModel() : base()
    {
      DocumentItems = new ObservableCollection<ItemViewModel>();

      OpenItemEditorCommand = new DelegateCommand<string>(ExecuteOpenItemEditor, (z) => { return true; });
    }

    public void ExecuteOpenItemEditor(string str=null)
    {
      System.Windows.MessageBox.Show("hi");

    }

    public void ReadDocument(string fileName)
    {
      FullFilePath = fileName;
      Document myNewDoc = Document.Load(FullFilePath);
      string searchFilter = myNewDoc.Prefix.Value + "*.yml";
      string[] itemsToLoad = System.IO.Directory.GetFiles(DirName, searchFilter);
      foreach (string itemToLoad in itemsToLoad)
      {
        ItemViewModel newItem = new ItemViewModel(itemToLoad,myNewDoc);
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

  public class NeedsToBeSavedBoolToBorderColorConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return ((value as bool?) == true)
          ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.LightGreen);
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

  public class NeedsToBeSavedBoolToBackgroundColorConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return ((value as bool?) == true)
          ? Brushes.Red : Brushes.LightGray;
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

  public class BoolToVisibilityConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return ((value as bool?) == true)
          ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
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

  public class BoolToBorderThicknessConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return ((value as bool?) == true)
          ? 1 : 0;
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
