using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        delegate int DelegateType(object args);
        static DelegateType delegateName;

        public static int silniaRek(object args)
        {
            int n = (int)args;
            if (n == 0) return 1;
            else return n * silniaRek(n - 1);
        } 

        public static int silniaIte(object args)
        {
            int n = (int)args;
            int wynik = 1;
            for (int i = 1; i <= n; i++) wynik *= i;
            return wynik;
        }

        public static int fibRek(object args)
        {
            int n = (int)args;
            if (n < 3) return 1;
            else return fibRek(n - 2) + fibRek(n - 1);
        }

        public static int fibIte(object args)
        {
            int n = (int)args;
            int fib = 1;
            int a = 0;
            for (int i = 0; i < n; i++)
            {
                int temp = a;
                a = fib;
                fib = temp + fib;
            }
            return a;
        }

        private static void myAsyncCallback(IAsyncResult iResult)
        {
            
            byte[] buffer = (byte[])((object[])iResult.AsyncState)[1];
            FileStream fs = (FileStream)((object[])iResult.AsyncState)[0];
            AutoResetEvent are = ((object[])iResult.AsyncState)[2] as AutoResetEvent;
            int len = fs.EndRead(iResult);
            fs.Close();
            Console.WriteLine(Encoding.ASCII.GetString(buffer,0,len));
            are.Set();
        }
        static void Main(string[] args)
        {
            FileStream fs = File.Open("C:/dane.txt", FileMode.OpenOrCreate);
            byte[] buffer = new byte[1024];
            //zad6
            //AutoResetEvent are = new AutoResetEvent(false);
            //fs.BeginRead(buffer, 0, 1024, myAsyncCallback, new object[] { fs, buffer, are });
            //are.WaitOne();

            //zad7
            //IAsyncResult ar = fs.BeginRead(buffer, 0, 1024, null, null);
            //int len = fs.EndRead(ar);
            //Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, len));
            //fs.Close();

            delegateName = new DelegateType(silniaRek);
            IAsyncResult ar = delegateName.BeginInvoke((object)10, null, null);
            int res = delegateName.EndInvoke(ar);
            Console.WriteLine("Silnia rekurencyjna: " + res);

            delegateName = new DelegateType(silniaIte);
            ar = delegateName.BeginInvoke((object)10, null, null);
            res = delegateName.EndInvoke(ar);
            Console.WriteLine("Silnia iteracyjna: " + res);

            delegateName = new DelegateType(fibRek);
            ar = delegateName.BeginInvoke((object)10, null, null);
            res = delegateName.EndInvoke(ar);
            Console.WriteLine("Fibonacci rekurencyjny: " + res);

            delegateName = new DelegateType(fibIte);
            ar = delegateName.BeginInvoke((object)10, null, null);
            res = delegateName.EndInvoke(ar);
            Console.WriteLine("Fibonacci iteracyjny: " + res);
            
            Console.ReadKey();
        }
    }
}