using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Registration.Infrastructure;
using Registration.Models;
using Registration.Services;

namespace Registration.ViewModels
{
	public class RegisterTeamViewModel : BindableBase, IRegisterTeamViewModel
    {
        private readonly IRegistrationService _registrationService;

        public RegisterTeamViewModel(IRegistrationService registrationService)
        {
            _registrationService = registrationService;

            _submitCommand = DelegateCommand.FromAsyncHandler(Submit, CanSubmit);
            this.CancelCommand = new DelegateCommand(Cancel);

            _teamName = string.Empty;
            _player1Name = string.Empty;
            _player2Name = string.Empty;
            _player3Name = string.Empty;
            _player4Name = string.Empty;
            _player5Name = string.Empty;

            this.SetInitialValidState();
        }

        public Guid ContestId { get; set; }

        private string _teamName;
        [Required(ErrorMessage = "Name ist erforderlich.", AllowEmptyStrings = false)]
        public string TeamName
        {
            get
            {
                return _teamName;
            }
            set
            {
                this.ValidateName(value, true, "TeamName");

                SetProperty(ref _teamName, value);
            }
        }

        private string _player1Name;
        [Required(ErrorMessage = "Name ist erforderlich.", AllowEmptyStrings = false)]
        public string Player1Name
        {
            get
            {
                return _player1Name;
            }
            set
            {
                this.ValidateName(value, true, "Player1Name");

                SetProperty(ref _player1Name, value);
            }
        }
        private string _player2Name;
        [Required(ErrorMessage = "Name ist erforderlich.", AllowEmptyStrings = false)]
        public string Player2Name
        {
            get
            {
                return _player2Name;
            }
            set
            {
                this.ValidateName(value, true, "Player2Name");

                SetProperty(ref _player2Name, value);
            }
        }
        private string _player3Name;
        [Required(ErrorMessage = "Name ist erforderlich.", AllowEmptyStrings = false)]
        public string Player3Name
        {
            get
            {
                return _player3Name;
            }
            set
            {
                this.ValidateName(value, true, "Player3Name");

                SetProperty(ref _player3Name, value);
            }
        }
        private string _player4Name;
        [Required(ErrorMessage = "Name ist erforderlich.", AllowEmptyStrings = false)]
        public string Player4Name
        {
            get
            {
                return _player4Name;
            }
            set
            {
                this.ValidateName(value, true, "Player4Name");

                SetProperty(ref _player4Name, value);
            }
        }
        private string _player5Name;
        [Required(ErrorMessage = "Name ist erforderlich.", AllowEmptyStrings = false)]
        public string Player5Name
        {
            get
            {
                return _player5Name;
            }
            set
            {
                this.ValidateName(value, true, "Player5Name");

                SetProperty(ref _player5Name, value);
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
            if (!this.CanSubmit())
            {
                throw new InvalidOperationException();
            }

            TeamContestRegistration registration = new TeamContestRegistration()
            {
                ContestId = this.ContestId,
                TeamName = this.TeamName,
                Player1Name = this.Player1Name,
                Player2Name = this.Player2Name,
                Player3Name = this.Player3Name,
                Player4Name = this.Player4Name,
                Player5Name = this.Player5Name,
            };

            await _registrationService.Submit(registration);

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
            this.ValidateName(this.TeamName, false, "TeamName");
            this.ValidateName(this.Player1Name, false, "Player1Name");
            this.ValidateName(this.Player2Name, false, "Player2Name");
            this.ValidateName(this.Player3Name, false, "Player3Name");
            this.ValidateName(this.Player4Name, false, "Player4Name");
            this.ValidateName(this.Player5Name, false, "Player5Name");
        }

        private void ValidateName(string newValue, bool throwException, string rule)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                this.AddError(rule);
                if (throwException)
                {
                    throw new InputValidationException("Eingabe zwingend erforderlich.");
                }
            }
            else
            {
                this.RemoveError(rule);
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
