using System;
using System.Collections.Specialized;
using System.Threading;
using System.ComponentModel;

namespace lab_xtra1
{
    public delegate void MathMulCompletedEventHandler(object sender, CompletedEventArgs e);

    public class CompletedEventArgs : AsyncCompletedEventArgs
    {
        public int[][] tab1, tab2, tab3;

        public CompletedEventArgs(Exception ex, bool canceled, object userState, object[] tabs) : base(ex, canceled, userState)
        {
            tab1 = (int[][])tabs[0];
            tab2 = (int[][])tabs[1];
            tab3 = (int[][])tabs[2];
        }
    }


    public class MathMul
    {
        private delegate void WorkerEventHandler(int[][] tab1, int[][] tab2, AsyncOperation op);

        private HybridDictionary tasks = new HybridDictionary();
        private SendOrPostCallback compDel;
        public event MathMulCompletedEventHandler mmComp;

        public MathMul()
        {
            compDel = new SendOrPostCallback(CalculateCompleted);
        }

        private void CalculateCompleted(object operationState)
        {
            CompletedEventArgs e = operationState as CompletedEventArgs;
            if (mmComp != null)
            {
                mmComp(this, e);
            }
        }

        private bool TaskCancelled(object taskId)
        {
            return (tasks[taskId] == null);
        }

        public void CancelAsync(object taskId)
        {
            AsyncOperation op = tasks[taskId] as AsyncOperation;
            if (op != null)
            {
                lock (tasks.SyncRoot)
                {
                    tasks.Remove(taskId);
                }
            }
        }

        public void MathMulAsync(int[][] tab1, int[][] tab2, object userState)
        {
            AsyncOperation op = AsyncOperationManager.CreateOperation(userState);
            lock (tasks.SyncRoot)
            {
                if (tasks.Contains(userState))
                {
                    throw new ArgumentException("Jakis blad", "userState");
                }
                tasks[userState] = op;
            }
            WorkerEventHandler worker = new WorkerEventHandler(MathMulWorker);
            worker.BeginInvoke(tab1, tab2, op, null, null);
        }

        private void MathMulWorker(int[][] tab1, int[][] tab2, AsyncOperation op)
        {
            int[][] tab3 = Program.pomnoz(tab1, tab2);
            lock (tasks.SyncRoot)
            {
                tasks.Remove(op.UserSuppliedState);
            }
            object[] tabs = { tab1, tab2, tab3 };
            CompletedEventArgs e = new CompletedEventArgs(null, false, op.UserSuppliedState, tabs);
            op.PostOperationCompleted(compDel, e);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            int ilTask = 0;
            MathMul mm = new MathMul();
            mm.mmComp += onMathMulCompleted;
            for (int i = 0; i < 3; i++)
            {
                int[][] tab1 = nowaMac();
                int[][] tab2 = nowaMac();
                tab1 = wypelnij(tab1);
                Thread.Sleep(20);
                tab2 = wypelnij(tab2);
                mm.MathMulAsync(tab1, tab2, ilTask);
                ilTask++;
            }
            Thread.Sleep(20000);
        }

        private static void onMathMulCompleted(object sender, CompletedEventArgs e)
        {
            Console.WriteLine("Pierwsza macierz:");
            Program.wypisz(e.tab1);
            Console.WriteLine("Druga macierz:");
            Program.wypisz(e.tab2);
            Console.WriteLine("Macierz wynikowa:");
            Program.wypisz(e.tab3);
        }

        private static int[][] nowaMac()
        {
            int[][] tab = new int[20][];
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = new int[20];
            }
            return tab;
        }

        public static int[][] wypelnij(int[][] tab)
        {
            Random rand = new Random();
            for (int i = 0; i < tab.Length; i++)
            {
                for (int j = 0; j < tab.Length; j++)
                {
                    tab[i][j] = rand.Next(1, 10);
                }
            }
            return tab;
        }

        public static void wypisz(int[][] tab)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                for (int j = 0; j < tab.Length; j++)
                {
                    Console.Write(tab[i][j] + " ");
                }
                Console.WriteLine("");
            }
        }

        public static int[][] pomnoz(int[][] tab1, int[][] tab2)
        {
            int[][] tab3 = nowaMac();
            for (int i = 0; i < tab1.Length; i++)
            {
                for (int j = 0; j < tab1.Length; j++)
                {
                    int result = 0;
                    for (int k = 0; k < tab1.Length; k++)
                    {
                        result += tab1[i][k] * tab2[k][j];
                    }
                    tab3[i][j] = result;
                }
            }
            return tab3;
        }

    }
}
