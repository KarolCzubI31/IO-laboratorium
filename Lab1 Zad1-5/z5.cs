﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace z5
{
    class Program
    {
        public static Object thisLock = new Object();
        public static int suma;
        public static AutoResetEvent[] sync;
        private static void ThreadProc(Object stateInfo)
        {
            int[] partTab = (int[])((object[])stateInfo)[0];
            for (int i=0; i<partTab.Length; i++)
            {
                lock (thisLock)
                {
                    suma += partTab[i];
                }
            }
            AutoResetEvent waitHandle = (AutoResetEvent)((object[])stateInfo)[1];
            waitHandle.Set();
        }

        static void Main(string[] args)
        {
            int n, part, ilWatkow, lastPart;
            Console.WriteLine("Podaj wielkosc tablicy");
            n = Convert.ToInt32(Console.ReadLine());
            int[] tab = new int[n];
            Console.WriteLine("Podaj wielkosc fragmentow, na jakie ma byc podzielona tablica");
            part = Convert.ToInt32(Console.ReadLine());
            ilWatkow = (int)Math.Ceiling((double)n / (double)part);
            sync = new AutoResetEvent[ilWatkow];
            for (int i = 0; i < ilWatkow; i++) sync[i] = new AutoResetEvent(false);
            lastPart = n % part;
            Random rand = new Random();
            suma = 0;
            for (int i = 0; i < n; i++) tab[i] = rand.Next(99);
            for (int i = 1; i <= ilWatkow; i++)
            {
                int startIndex = (i - 1) * part;
                if (i==ilWatkow && lastPart != 0)
                {
                    int[] subTab = new int[lastPart];
                    Array.Copy(tab, startIndex, subTab, 0, lastPart);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), new object[] { subTab, sync[i - 1] });
                }
                else
                {
                    int[] subTab = new int[part];
                    Array.Copy(tab, startIndex, subTab, 0, part);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), new object[] { subTab, sync[i - 1] });
                }
            }
            WaitHandle.WaitAll(sync);
            Console.WriteLine("Tablica:");
            for (int i = 0; i < tab.Length; i++) Console.WriteLine("tab[" + i + "] = " + tab[i]);
            Console.WriteLine("Suma = " + suma);

            Console.ReadKey();
        }
    }
}