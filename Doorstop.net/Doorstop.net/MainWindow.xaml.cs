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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Doorstop.net
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    ViewModels.MainWindowViewModel viewModel;

    public MainWindow()
    {
      InitializeComponent();
      viewModel = (ViewModels.MainWindowViewModel)this.DataContext;
      this.Closing += new CancelEventHandler(OnWindowClosing);
    }

    void OnWindowClosing(object sender, CancelEventArgs e)
    {
      viewModel.CleanupCommand.Execute("");
    }

    private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      TreeView tree = sender as TreeView;

      if (tree != null)
      {
        Models.RequirementsDocument document = tree.SelectedValue as Models.RequirementsDocument;
        if (document is Models.RequirementsFolder)
          return;

        if (document != null)
        {
          try
          {
            viewModel.OpenDocumentCommand.Execute(document.FullPath);
          }
          catch (Exception ex)
          {
            MessageBox.Show(ex.Message, "Error opening document file", MessageBoxButton.OK, MessageBoxImage.Error);
            Logger.Warning(ex.Message);
          }
        }

      }

    }

    private void OnExitMenuButton_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
