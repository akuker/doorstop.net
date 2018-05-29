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
  public class ItemEditorViewModel : DoorstopBaseViewModel
  {
    #region Properties
    private Models.Item item;

    public Models.Item Item
    {
      get { return item; }
      set { item = value; NotifyPropertyChanged(); }
    }
    #endregion

    public ItemEditorViewModel(): base()
    {
      SaveDocumentCommand = new DelegateCommand<string>(ExecuteSaveDocument, (z) => { return true; });
    }

    protected override void ExecuteSaveDocument(string Path = null)
    {
      if(Item.NeedsToBeSaved)
          Item.Save();
    }
  }
}
