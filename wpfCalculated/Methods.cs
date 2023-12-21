using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfCalculated
{
    public interface IMemory
    {
        void SaveIt(string s, string result);
        string [] LoadIt();
    }
    class DataBased : IMemory
    {
        string connStr = "server=92.246.214.15;port=3306;user=ais_skorba1854_calculatedwpf;database=ais_skorba1854_calculatedwpf;password=r9vqj25IGEYMhgV3ICHhYkkI;";
        public void SaveIt(string s, string result)
        {
            MySqlConnection conn = new(connStr);
            conn.Open();
            string requestInServer = "INSERT INTO `Calculator`(`input`, `result`) VALUES(@s, @result)";
            MySqlCommand comand = new(requestInServer, conn);
            comand.Parameters.AddWithValue("@s", s);
            comand.Parameters.AddWithValue("@result", result);
            comand.ExecuteScalar();
            conn.Close();
        }

        public string[] LoadIt() 
        {
            string[] forHistory= new string [5];
            string request = "SELECT `input` FROM `Calculator` ORDER BY id DESC LIMIT 4,1;";
            string request2 = "SELECT `result` FROM `Calculator` ORDER BY id DESC LIMIT 4,1;";
            forHistory[0] = ParseFromDB(request, request2);
            request = "SELECT `input` FROM `Calculator` ORDER BY id DESC LIMIT 3,1;";
            request2 = "SELECT `result` FROM `Calculator` ORDER BY id DESC LIMIT 3,1;";
            forHistory[1] = ParseFromDB(request, request2);
            request = "SELECT `input` FROM `Calculator` ORDER BY id DESC LIMIT 2,1;";
            request2 = "SELECT `result` FROM `Calculator` ORDER BY id DESC LIMIT 2,1;";
            forHistory[2] = ParseFromDB(request, request2);
            request = "SELECT `input` FROM `Calculator` ORDER BY id DESC LIMIT 1,1;";
            request2 = "SELECT `result` FROM `Calculator` ORDER BY id DESC LIMIT 1,1;";
            forHistory[3] = ParseFromDB(request, request2);
            request = "SELECT `input` FROM `Calculator` ORDER BY id DESC LIMIT 1;";
            request2 = "SELECT `result` FROM `Calculator` ORDER BY id DESC LIMIT 1;";
            forHistory[4] = ParseFromDB(request, request2);
            return forHistory;
        }

        public string ParseFromDB(string request, string request2)
        {
            MySqlConnection conn = new(connStr);
            conn.Open();
            MySqlCommand comand = new(request, conn);
            MySqlCommand comand2 = new(request2, conn);
            object strRequest = comand.ExecuteScalar();
            object strRequest2 = comand2.ExecuteScalar();
            conn.Close();
            if(strRequest != null) return strRequest.ToString() + '=' + strRequest2.ToString();
            return "";
        }
    }

    internal class Memory : IMemory
    {
        public void SaveIt(string s, string result)
        {

        }

        public string[] LoadIt() 
        {
            string[] forHistory = new string[1];
            return forHistory;
        }
    }

    class Files : IMemory
    {
        public void SaveIt(string s, string result) 
        {
            string fileName = "C:\\Rodion\\Учеба\\3 курс\\ТРПО\\wpfCalculated\\wpfCalculated\\example.txt"; // Имя файла
            string content = s + '=' + result + '\n'; // Содержимое, которое нужно сохранить
            if (File.Exists(fileName))
            {
                File.AppendAllText(fileName, content);
            }
            else
            {
                File.WriteAllText(fileName, content);
            }
        }

        public string[] LoadIt()
        {
            int i = 4;
            string[] forHistory = new string[5];
            string fileName = "C:\\Rodion\\Учеба\\3 курс\\ТРПО\\wpfCalculated\\wpfCalculated\\example.txt";
            if (File.Exists(fileName))
            {
                
                foreach (string line in File.ReadLines(fileName).Reverse())
                {
                    if (i < 0) break;
                    forHistory[i] = line;
                    i--;
                }
            }
            return forHistory;
        }
    }
}
