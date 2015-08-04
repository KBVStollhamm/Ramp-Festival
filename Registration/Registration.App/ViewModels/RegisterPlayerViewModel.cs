using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Registration.Infrastructure;
using Registration.Models;

namespace Registration.ViewModels
{
	public class RegisterPlayerViewModel : BindableBase, IRegisterPlayerViewModel
	{
		public RegisterPlayerViewModel()
		{
			_submitCommand = DelegateCommand.FromAsyncHandler(Submit, CanSubmit);
			this.CancelCommand = new DelegateCommand(Cancel);

			_playerName = string.Empty;

			this.SetInitialValidState();
		}

		private string _playerName;
		[Required(ErrorMessage = "Name ist erforderlich.", AllowEmptyStrings = false)]
		public string PlayerName
		{
			get
			{
				return _playerName;
			}
			set 
			{
				this.ValidatePlayerName(value, true);

				SetProperty(ref _playerName, value);
			}
		}

		private DelegateCommand _submitCommand;
		public ICommand SubmitCommand
		{
			get
			{
				return _submitCommand;
			}
		}
		public ICommand CancelCommand { get; private set; }

		public event EventHandler CloseViewRequested = delegate { };

		public async Task Submit()
		{
			await Task.Delay(1000);

			this.CloseViewRequested(this, EventArgs.Empty);
		}
		public bool CanSubmit()
		{
			return !this.HasErrors;
		}

		public void Cancel()
		{
			this.CloseViewRequested(this, EventArgs.Empty);
		}

		private void SetInitialValidState()
		{
			this.ValidatePlayerName(this.PlayerName, false);
		}

		private void ValidatePlayerName(string newValue, bool throwException)
		{
			if (string.IsNullOrWhiteSpace(newValue))
			{
				this.AddError("PlayerName");
				if (throwException)
				{
					throw new InputValidationException("Der Name des Spielers ist zwingend erforderlich.");
				}
			}
			else
			{
				this.RemoveError("PlayerName");
			}
		}

		private readonly List<string> _errors = new List<string>();
		public bool HasErrors
		{
			get
			{
				return _errors.Count > 0;
			}
		}


		private void AddError(string ruleName)
		{
			if (!_errors.Contains(ruleName))
			{
				_errors.Add(ruleName);
				_submitCommand.RaiseCanExecuteChanged();
			}
		}

		private void RemoveError(string ruleName)
		{
			if (_errors.Contains(ruleName))
			{
				_errors.Remove(ruleName);
				if (_errors.Count == 0)
				{
					_submitCommand.RaiseCanExecuteChanged();
				}
			}
		}
	}
}
