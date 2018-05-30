using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Doorstop.net.Views
{
  /// <summary>
  /// Interaction logic for DocumentWindow.xaml
  /// </summary>
  public partial class DocumentView : Window
  {
    public static RoutedUICommand JumpToItemCommand { get; set; } =
      new RoutedUICommand("Jump To", "JumpTo", typeof(DocumentView));
    public static RoutedUICommand OpenItemCommand { get; set; } =
      new RoutedUICommand("Open Item", "OpenItem", typeof(DocumentView));

    private void OpenItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void OpenItem_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      Models.Item selectedItem = this.Requirements_DataGrid.SelectedItem as Models.Item;
      if (selectedItem != null)
      {
        Views.ItemEditor itemEditor = new Views.ItemEditor(selectedItem);
        itemEditor.ShowDialog();
      }
      else
      {
        MessageBox.Show("Invalid item selected" + this.Requirements_DataGrid.SelectedItem.GetType().ToString());
      }
    }
    private void JumpToItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void JumpToItem_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      Models.Item item = e.Parameter as Models.Item;
      if(item != null)
      {
        this.Requirements_DataGrid.SelectedItem = item;
        this.Requirements_DataGrid.UpdateLayout();
        this.Requirements_DataGrid.ScrollIntoView(this.Requirements_DataGrid.SelectedItem);
      }

    }

    public DocumentView()
    {
      InitializeComponent();
      this.Closing += new CancelEventHandler(OnWindowClosing);
    }
    public DocumentView(string path)
      : this()
    {
      ViewModels.DocumentViewModel viewModel = this.DataContext as ViewModels.DocumentViewModel;

      if (viewModel != null)
      {
        viewModel.ReadDocument(path);
      }

      Markdown.Xaml.Markdown myMarkdown = this.Resources["Markdown"] as Markdown.Xaml.Markdown;
      if (myMarkdown != null)
      {
        myMarkdown.AssetPathRoot = theViewModel.Document.DirName;
      }
    }

    void OnWindowClosing(object sender, CancelEventArgs e)
    {
      this.DataContext = null ;
    }

    private void FlowDocumentScrollViewer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      Requirements_DataGrid.BeginEdit();
      e.Handled = true;
    }

    /// <summary>
    /// When this event handler is tied to the 'loaded' event, this referened object will be
    /// given the Focus. If this is a text box, will move the cursor to the end of the text.
    /// </summary>
    /// <param name="sender">Sending control</param>
    /// <param name="e">not used</param>
    private void FocusHere_OnLoaded(object sender, RoutedEventArgs e)
    {
      Control senderControl = sender as Control;
      if(senderControl != null)
      {
        senderControl.Focus();
      }
      // If this is a textbox, move the cursor to the end
      TextBox senderTextBox = sender as TextBox;
      if(senderTextBox != null)
      {
        senderTextBox.CaretIndex = int.MaxValue;
      }
    }
  }
}
