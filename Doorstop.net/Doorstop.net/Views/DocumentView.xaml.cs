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
        viewModel.FilePath = path;
      }

      Markdown.Xaml.Markdown myMarkdown = this.Resources["Markdown"] as Markdown.Xaml.Markdown;
      if (myMarkdown != null)
      {
        myMarkdown.AssetPathRoot = theViewModel.FilePath;
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
  }
}
