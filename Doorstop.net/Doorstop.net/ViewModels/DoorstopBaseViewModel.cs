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

    #region Properties
    /// <summary>
    /// Full path of the file associated with this View Model
    /// </summary>
    public virtual string DirName
    {
      get
      {
        return System.IO.Path.GetDirectoryName(fullFilePath);
      }
    }

    /// <summary>
    /// File name with extension (without path) of the file associated with this View Model
    /// </summary>
    public virtual string FileName
    {
      get
      {
        return System.IO.Path.GetFileName(fullFilePath);
      }
    }
    
    private string fullFilePath;
    /// <summary>
    /// Full file name (with path) of the file associated with this View Model
    /// </summary>
    public virtual string FullFilePath
    {
      get { return fullFilePath; }
      set
      {
        if (fullFilePath != value)
        {
          fullFilePath = System.IO.Path.GetFullPath(value);
          // The Directory name and file name properties are generated from this
          // property, so we need to raise the property changed event for them as well.
          NotifyPropertyChanged();
          NotifyPropertyChanged("DirName");
          NotifyPropertyChanged("FileName");
        }
      }
    }
    #endregion

    public DoorstopBaseViewModel()
    {
      SaveDocumentCommand = new DelegateCommand<string>(ExecuteSaveDocument, (z) => { return true; });
    }

    protected virtual void ExecuteSaveDocument(string Path = null) { }
  }
}
