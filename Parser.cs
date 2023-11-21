using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace wpfCalculated
{
    public class Calculator
    {
        public int i = 0;
        public string s = "";
        public bool isErrorPars;

        public double ProcE() //результат 
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


/*namespace wpfCalculated
{
    public class Calculator
    {
        public int i = 0;
        public string s = "";

        public double ProcE() //результат 
        {
            s += '\0';
            double x = ProcT();
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
            i = 0;
            return Math.Round(x,5);
        }
        public double ProcT()
        {
            double x = ProcM();
            while (s[i] == '*' || s[i] == '/')
            {
                char p = s[i]; i++;
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
                x = ProcE(); if (s[i] != ')')
                {
                    Console.WriteLine("missing ')'");
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
                        Console.WriteLine("Syntex error.");
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
                while(s[i] >= '0' && s[i] <= '9')
                {
                    y *= 10;
                    y += s[i] - '0';
                    i++;
                    c++;
                }
                if(c != 0)
                {
                    x = x + y / Math.Pow(10, c);
                }
            }
            return x;
        }
    }
}*/

