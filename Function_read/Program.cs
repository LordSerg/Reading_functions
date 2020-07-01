using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Function_read
{
    class Program
    {

        static void Main(string[] args)
        {
            string str;
            Console.WriteLine("Input the function:");
            str=Console.ReadLine();
            Console.Write("Input range: \nfrom: ");
            double a =Convert.ToDouble(Console.ReadLine());
            Console.Write("to: ");
            double b = Convert.ToDouble(Console.ReadLine());
            Console.Write("step: ");
            double c = Convert.ToDouble(Console.ReadLine());
            int []z=convert_to_function(str);//конвертируем функцию в набор действий

            //Function f = new Function(s);
            for (double i = a; i <= b; i += c)
            {
                Console.WriteLine("x={0}\ty={1}", i, f(i, z, 0, z.Length - 1));
            }
            Console.ReadKey();
        }
        static int[] convert_to_function(string s)
        {
            int[] a=new int [s.Length];
            int length=s.Length,k=0;
            char[] c = new char[s.Length];
            for (int i = 0; i < length; i++)
                c[i] = s[i];
            bool b = false;
            int breck = -100;
            for (int i=0;i<s.Length;i++)
            {
                if (c[i] == ' ')
                {
                    k++;
                    length--;
                }
                else if (c[i] == 'x')
                {
                    a[i - k] = -1;
                    b = false;
                }
                else if (c[i] == '+')
                {
                    a[i - k] = -2;
                    b = false;
                }
                else if (c[i] == '-')
                {
                    a[i - k] = -3;
                    b = false;
                }
                else if (c[i] == '*')
                {
                    a[i - k] = -4;
                    b = false;
                }
                else if (c[i] == '/')
                {
                    a[i - k] = -5;
                    b = false;
                }
                else if(c[i]=='^')
                {
                    a[i - k] = -6;
                    b = false;
                }
                else if(c[i]=='c'||c[i]=='C')//тригонометрические функции
                {//косинус или котангенс
                    length -= 2;
                    b = false;
                    if(c[i+1]=='o')
                    {//косинус cos
                        a[i - k] =-7;
                    }
                    else if(c[i+1]=='t')
                    {//котангенс ctg
                        a[i - k] = -10;
                    }
                    i += 2;
                }
                else if(c[i]=='s'||c[i]=='S')
                {//синус sin
                    b = false;
                    length -= 2;
                    a[i - k] = -8;
                    i += 2;
                }
                else if (c[i] == 't' || c[i] == 'T')
                {//тангенс
                    b = false;
                    if(c[i+1]=='g')//разновидности написания
                    {//tg
                        length--;
                        a[i - k]=-9;
                        i++;
                    }
                    else if(c[i+1]=='a')
                    {//tan
                        length-=2;
                        a[i - k] = -9;
                        i+=2;
                    }
                }
                else if(c[i]=='(')
                {
                    b = false;
                    a[i - k] = breck;
                    breck--;
                }
                else if(c[i]==')')
                {
                    b = false;
                    breck++;
                    a[i - k]=breck;
                }
                else if (c[i] >= '0' && c[i] <= '9')
                {
                    if (b == false)
                    {
                        a[i - k] = Convert.ToInt32(c[i] - '0');
                        b = true;
                    }
                    else
                    {
                        k++;
                        a[i - k] *= 10;
                        a[i - k] += Convert.ToInt32(c[i] - '0');
                        length--;
                    }
                }
            }
            int[] a1=new int[length];
            for (int i = 0; i < length; i++)
                a1[i] = a[i];
            return a1;
        }
        static double f(double x,int [] n,int i_start,int i_end)
        {
            double answer = 0;
            if (i_start < 0)
            {//если первым действием - отрицательное число, например: -5+4х, то
                //"-" в данном случае - действие: 0-5
                answer = 0;
            }
            else if (i_start == i_end)
            {
                if (n[i_start] == -1)
                    answer = x;
                else
                    answer = n[i_start];
            }
            else if (n[i_start] <= -100 && n[i_end] <= -100)
            {//если пришла функция в скобочках,
                answer = f(x, n, i_start + 1, i_end - 1);//то обкусываем их
            }
            else
            {
                int max = -50, imax = i_start;
                for (int i = i_start; i <= i_end; i++)
                {//поиск последнего действия
                    if (n[i] <= -100)
                    {//игнорируем скобочки
                        int j = i + 1;
                        while (n[i] != n[j])
                        {
                            j++;
                        }
                        i = j;
                    }
                    else if (n[i] < -1 && n[i] > max)
                    {
                        max = n[i];
                        imax = i;
                    }
                }
                if (max == -2)
                    answer = f(x, n, i_start, imax - 1) + f(x, n, imax + 1, i_end);
                else if (max == -3)
                    answer = f(x, n, i_start, imax - 1) - f(x, n, imax + 1, i_end);
                else if (max == -4)
                    answer = f(x, n, i_start, imax - 1) * f(x, n, imax + 1, i_end);
                else if (max == -5)
                    answer = f(x, n, i_start, imax - 1) / f(x, n, imax + 1, i_end);
                else if (max == -6)
                    answer = Math.Pow(f(x, n, i_start, imax - 1), f(x, n, imax + 1, i_end));
            }
            return answer;
        }
    }
}
