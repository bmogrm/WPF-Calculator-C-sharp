using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfLibrary1;


namespace wpfCalculated
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Calculator _model;
        private readonly IMemory _memory;
        private string _result;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CalculateCommand { get; }
        public ICommand AddToTextBoxCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand ClearOnceCommand { get; }

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public ViewModel(IMemory memory)
        {
            _memory = memory;
            _model = new Calculator();
            string[] forHistory = new string[5];
            forHistory = _memory.LoadIt();
            CalculationHistory = new(_memory.LoadIt());
            CalculateCommand = new RelayCommand(ExecuteCalculate);
            AddToTextBoxCommand = new RelayCommand(ExecuteAdd);
            ClearCommand = new RelayCommand(ExecuteClear);
            ClearOnceCommand = new RelayCommand(ExecuteClearOnce);
        }

        private void ExecuteAdd(object parameter)
        {
            if (!string.IsNullOrEmpty(Result))
            {
                if (Result.Contains('=') || Result == "Ошибка")
                {
                    Result = string.Empty;
                }
            }
            string? number = parameter as string;
            Result += number;
        }

        private void ExecuteCalculate(object parameter)
        {
            if (!string.IsNullOrEmpty(Result))
            {
                _model.s = Result;
                Result = _model.ProcE().ToString();
                if (_model.isErrorPars)
                {
                    Result = "Ошибка";
                    AddToHistory(_model.s + "  Ошибка");
                    _model.isErrorPars = false;
                    _model.i = 0;
                    return;
                }
                _model.isErrorPars = false;
                _model.i = 0;
                AddToHistory(_model.s + " = " + Result);
            }
        }

        private void ExecuteClear(object parameter)
        {
            Result = string.Empty;
        }

        private void ExecuteClearOnce(object parameter)
        {
            if(!string.IsNullOrEmpty(Result))
            {
                Result = Result.Remove(Result.Length - 1);
            }
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
            _memory.SaveIt(_model.s, Result);

            OnPropertyChanged(nameof(CalculationHistory));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object>? canExecute = null)
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