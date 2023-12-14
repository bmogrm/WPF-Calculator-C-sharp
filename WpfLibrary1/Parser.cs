using System;

namespace WpfLibrary1
{
    public class Calculator
    {
        public int i = 0;
        public string s = "";
        public bool isErrorPars;

        public double ProcE() //��������� 
        {
            s += '\0';
            double x = ProcT();
            if (s[i] == ',')
            {
                isErrorPars = true;
                return -1;
            }
            while (s[i] == '+' || s[i] == '-')
            {
                char p = s[i];
                i++;
                if (p == '+')
                {
                    x += ProcT();
                }
                else
                {
                    x -= ProcT();
                }
            }
            return Math.Round(x, 5);
        }

        public double ProcT()
        {
            double x = ProcM();
            while (s[i] == '*' || s[i] == '/')
            {
                char p = s[i];
                i++;
                if (p == '*')
                {
                    x *= ProcM();
                }
                else
                {
                    x /= ProcM();
                }
            }
            return x;
        }

        public double ProcM()
        {
            double x = 0;
            if (s[i] == '(')
            {
                i++;
                x = ProcE();
                if (s[i] != ')')
                {
                    isErrorPars = true;
                    return -1;
                }
                i++;
            }
            else
            {
                if (s[i] == '-')
                {
                    i++;
                    x -= ProcM();
                }
                else
                {
                    if (s[i] >= '0' && s[i] <= '9')
                    {
                        x = ProcC();
                    }
                    else
                    {
                        isErrorPars = true;
                        return -1;
                    }
                }
            }
            return x;

        }

        public double ProcC()
        {
            double x = 0;
            double y = 0, c = 0;
            while (s[i] >= '0' && s[i] <= '9')
            {
                x *= 10;
                x += s[i] - '0';
                i++;
            }
            if (s[i] == ',')
            {
                i++;
                while (s[i] >= '0' && s[i] <= '9')
                {
                    y *= 10;
                    y += s[i] - '0';
                    i++;
                    c++;
                }
                if (c != 0)
                {
                    x = x + y / Math.Pow(10, c);
                }
            }
            return x;
        }
    }
}
