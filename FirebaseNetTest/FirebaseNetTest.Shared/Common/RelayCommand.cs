using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace FirebaseNetTest.Shared.Common
{
	public class RelayCommand : ICommand
	{
		private readonly Action<object> _action;
		public event EventHandler CanExecuteChanged;

		public RelayCommand(Action action)
		{
			this._action = (obj) => action();
		}

		public RelayCommand(Action<object> action)
		{
			this._action = action;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			this._action(parameter);
		}
	}
}
