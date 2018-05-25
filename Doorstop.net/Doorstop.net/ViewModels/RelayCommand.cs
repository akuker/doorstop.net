using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This was taken from https://www.codeproject.com/Tips/813345/Basic-MVVM-and-ICommand-Usage-Example,
/// which is distributed under the Code Project Open License (CPOL) 1.02
/// https://www.codeproject.com/info/cpol10.aspx
/// </summary>
namespace Doorstop.net.ViewModels
{
  public class RelayCommand
  {
    private Action<object> execute;

    private Predicate<object> canExecute;

    private event EventHandler CanExecuteChangedInternal;

    public RelayCommand(Action<object> execute)
        : this(execute, DefaultCanExecute)
    {
    }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
      if (execute == null)
      {
        throw new ArgumentNullException("execute");
      }

      if (canExecute == null)
      {
        throw new ArgumentNullException("canExecute");
      }

      this.execute = execute;
      this.canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        //CommandManager.RequerySuggested += value;
        this.CanExecuteChangedInternal += value;
      }

      remove
      {
        //CommandManager.RequerySuggested -= value;
        this.CanExecuteChangedInternal -= value;
      }
    }

    public bool CanExecute(object parameter)
    {
      return this.canExecute != null && this.canExecute(parameter);
    }

    public void Execute(object parameter)
    {
      this.execute(parameter);
    }

    public void OnCanExecuteChanged()
    {
      EventHandler handler = this.CanExecuteChangedInternal;
      if (handler != null)
      {
        //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
        handler.Invoke(this, EventArgs.Empty);
      }
    }

    public void Destroy()
    {
      this.canExecute = _ => false;
      this.execute = _ => { return; };
    }

    private static bool DefaultCanExecute(object parameter)
    {
      return true;
    }
  }
}

