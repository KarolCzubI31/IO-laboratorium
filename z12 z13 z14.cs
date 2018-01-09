using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace lab3
{
    class Program
    {
        public struct TResultDataStructure //zad12
        {
            public int a;
            public int A 
            {
                get { return a; }
                set { a = value; }
            }
            public int b;
            public int B {
                get { return b; }
                set { b = value; }
            }
        }

        public Task<bool> Zadanie2() //zad13
        {
            return Task.Run(() =>
                    {
                        bool Z2 = true;
                        return Z2;
                    });
        }

        public static async Task<XmlDocument> Zadanie3(string address) //zad14
        {
            WebClient webClient = new WebClient();
            string xmlContent = await webClient.DownloadStringTaskAsync(new Uri(address));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            return doc;
        }


        static void Main(string[] args)
        {
            Console.WriteLine(Zadanie3("http://www.feedforall.com/sample.xml").Result.InnerText); //wywołanie zad14

            Console.ReadKey();
        }
    }
}
