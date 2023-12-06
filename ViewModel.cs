using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Amazon.Runtime.Internal;
using MongoDB.Driver;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Ocsp;

namespace wpfCalculated
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Calculator _model;
        private string _result;
        string connStr = "server=92.246.214.15;port=3306;user=ais_skorba1854_calculatedwpf;database=ais_skorba1854_calculatedwpf;password=r9vqj25IGEYMhgV3ICHhYkkI;";

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
            LoadFromBD();
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
            string number = parameter as string;
            Result += number;
        }

        private void ExecuteCalculate(object parameter)
        {
            if (!string.IsNullOrEmpty(Result))
            {
                _model.s = Result;
                if(isOddBrackets())
                {
                    Result = "Ошибка";
                    AddToHistory(_model.s + "  Ошибка");
                    _model.isErrorPars = false;
                    _model.i = 0;
                    return;
                }
                Result = _model.ProcE().ToString();
                if(_model.isErrorPars)
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
            Result = Result.Remove(Result.Length - 1);
        }

        public bool isOddBrackets()
        {
            int openBrackets = 0, closeBrackets = 0;
            for(int i = 0; i < _model.s.Length; i++)
            {
                char ch = _model.s[i];
                if (ch == '(') openBrackets++;
                if (ch == ')') closeBrackets++;
            }
            if (openBrackets != closeBrackets)
            {
                return true;
            }
            return false;
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
            SaveInBD();
            SaveInFile();

            OnPropertyChanged(nameof(CalculationHistory));
        }

        public void SaveInBD()
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string requestInServer = "INSERT INTO `Calculator`(`input`, `result`) VALUES(@_model.s, @Result)";
            MySqlCommand comand = new MySqlCommand(requestInServer, conn);
            comand.Parameters.AddWithValue("@_model.s", _model.s);
            comand.Parameters.AddWithValue("@Result", Result);
            comand.ExecuteScalar();
            conn.Close();
            
        }

        public void SaveInFile() 
        {
            string fileName = "C:\\Rodion\\Учеба\\3 курс\\ТРПО\\wpfCalculated\\wpfCalculated\\example.txt"; // Имя файла
            string content = _model.s + '=' + Result + '\n'; // Содержимое, которое нужно сохранить
            File.Exists(fileName);
            File.AppendAllText(fileName, content); // Записываем содержимое в файл
        }

        public void LoadFromBD()
        {
            string request = "SELECT `input` FROM `Calculator` ORDER BY id DESC LIMIT 4,1;";
            string request2 = "SELECT `result` FROM `Calculator` ORDER BY id DESC LIMIT 4,1;";
            ParseFromDB(request, request2);
            request = "SELECT `input` FROM `Calculator` ORDER BY id DESC LIMIT 3,1;";
            request2 = "SELECT `result` FROM `Calculator` ORDER BY id DESC LIMIT 3,1;";
            ParseFromDB(request, request2);
            request = "SELECT `input` FROM `Calculator` ORDER BY id DESC LIMIT 2,1;";
            request2 = "SELECT `result` FROM `Calculator` ORDER BY id DESC LIMIT 2,1;";
            ParseFromDB(request, request2);
            request = "SELECT `input` FROM `Calculator` ORDER BY id DESC LIMIT 1,1;";
            request2 = "SELECT `result` FROM `Calculator` ORDER BY id DESC LIMIT 1,1;";
            ParseFromDB(request, request2);
            request = "SELECT `input` FROM `Calculator` ORDER BY id DESC LIMIT 1;";
            request2 = "SELECT `result` FROM `Calculator` ORDER BY id DESC LIMIT 1;";
            ParseFromDB(request, request2);
        }

        public void ParseFromDB(string request, string request2)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlCommand comand = new MySqlCommand(request, conn);
            MySqlCommand comand2 = new MySqlCommand(request2, conn);
            object strRequest = comand.ExecuteScalar();
            object strRequest2 = comand2.ExecuteScalar();
            try
            {
                CalculationHistory.Add(strRequest.ToString() + '=' + strRequest2.ToString());
            }
            catch
            {

            }
            conn.Close();
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