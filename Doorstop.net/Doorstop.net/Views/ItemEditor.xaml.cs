using Doorstop.net.Models;
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
  /// Interaction logic for ItemEditor.xaml
  /// </summary>
  public partial class ItemEditor : Window
  {
    public ItemEditor()
    {
      InitializeComponent();
      this.Closing += new CancelEventHandler(OnWindowClosing);

    }




    public ItemEditor(Item thisItem)
  : this()
    {
      ViewModels.ItemEditorViewModel viewModel = new ViewModels.ItemEditorViewModel();
      viewModel.Item = thisItem;
      this.DataContext = thisItem;
    }

    void OnWindowClosing(object sender, CancelEventArgs e)
    {
      this.DataContext = null;
    }


  }
}
