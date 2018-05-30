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

    public RoutedUICommand JumpToItemCommand { get; set; }
    private Models.Document document;

    public Models.Document Document
    {
      get { return document; }
      set { document = value; NotifyPropertyChanged(); }
    }


    protected override void ExecuteSaveDocument(string Path = null)
    {
      foreach(var itemView in Document.DocumentItems)
      {
        if (itemView.NeedsToBeSaved)
          itemView.Save();
      }
    }

    public ICommand OpenItemEditorCommand { get; set; }

    public DocumentViewModel() : base()
    {
      OpenItemEditorCommand = new DelegateCommand<string>(ExecuteOpenItemEditor, (z) => { return true; });
      JumpToItemCommand = new RoutedUICommand("JumpTo", "JumpTo", typeof(DocumentViewModel));

    }

    public void ExecuteOpenItemEditor(string str=null)
    {
      System.Windows.MessageBox.Show("hi");

    }


    public void ExecuteJumpToItem(object obj = null)
    {
      if (obj == null)
        obj = new object();
      System.Windows.MessageBox.Show("Jumped to something: " + obj.GetType().ToString());
    }

    public void ReadDocument(string fileName)
    {
      string FullFilePath = fileName;
      Document myNewDoc = Document.Load(FullFilePath);
      myNewDoc.LoadChildren();
      this.Document = myNewDoc;
    }

    private void JumpToItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = this.Document.DocumentItems.Count > 1;

    }

    private void JumpToItem_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      System.Windows.MessageBox.Show("Trying to jump to something");
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
