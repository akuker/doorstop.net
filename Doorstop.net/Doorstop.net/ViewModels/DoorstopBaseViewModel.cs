using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Doorstop.net.ViewModels
{
  public abstract class DoorstopBaseViewModel : INotifyPropertyChanged
  {

    #region INotifyPropertyChanged Utilities
    public event PropertyChangedEventHandler PropertyChanged;

    // This method is called by the Set accessor of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
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



    private void JumpToItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;

    }

    private void JumpToItem_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      System.Windows.MessageBox.Show("Trying to jump to something");
    }

    public DoorstopBaseViewModel()
    {
      SaveDocumentCommand = new DelegateCommand<string>(ExecuteSaveDocument, (z) => { return true; });
    }

    protected virtual void ExecuteSaveDocument(string Path = null) { }
  }
}
