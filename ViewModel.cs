using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace wpfCalculated
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Calculator _model;
        private string _result;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public ICommand calculateCommand { get; }
        public ICommand addToTextBoxCommand { get; }
        public ICommand clearCommand { get; }
        public ICommand clearOnceCommand { get; }

        public ViewModel()
        {
            _model = new Calculator();
            calculateCommand = new RelayCommand(ExecuteCalculate);
            addToTextBoxCommand = new RelayCommand(ExecuteAdd);
            clearCommand = new RelayCommand(ExecuteClear);
            clearOnceCommand = new RelayCommand(ExecuteClearOnce);
        }

        private void ExecuteAdd(object parameter)
        {
            if (!string.IsNullOrEmpty(Result))
            {
                if (Result.Contains('='))
                {
                    Result = string.Empty;
                }
            }
            string number = parameter as string;
            Result += number;
        }

        private void ExecuteCalculate(object parameter)
        {
            if (!string.IsNullOrEmpty(Result))
            {
                _model.s = Result;
                Result =  _model.ProcE().ToString();
                AddToHistory(_model.s + " = " + Result);
            }
        }

        private void ExecuteClear(object parameter)
        {
            Result = string.Empty;
        }

        private void ExecuteClearOnce(object parameter)
        {
            Result = Result.Remove(Result.Length - 1);
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<string> _calculationHistory = new ObservableCollection<string>();
        public ObservableCollection<string> CalculationHistory
        {
            get { return _calculationHistory; }
            set
            {
                _calculationHistory = value;
                OnPropertyChanged(nameof(CalculationHistory));
            }
        }

        public void AddToHistory(string calculation)
        {
            if (CalculationHistory.Count == 5)
            {
                CalculationHistory.RemoveAt(0);
            }
            CalculationHistory.Add(calculation);
            OnPropertyChanged(nameof(CalculationHistory));
        }
    }





    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}