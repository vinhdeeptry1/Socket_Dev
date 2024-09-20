//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ThucThiVaGiaiThichTieuTrinh
//{
//    internal class MyThreadClass
//    {
//        private const int RANDOM_SLEEP_MAX = 1000;
//        private const int LOOP_COUNT = 10;

//        private string greeting;

//        public MyThreadClass(string greeting)
//        { this.greeting = greeting; }

//        public void runMyThread()
//        {
//            Random rand = new Random();
//            for (int x = 0; x < LOOP_COUNT; x++)
//            {
//                Console.WriteLine(greeting + "(Thread ID: " + Thread.CurrentThread.GetHashCode()+")");
//                try
//                {
//                    Thread.Sleep(rand.Next(0, RANDOM_SLEEP_MAX));
//                } catch (ThreadInterruptedException) { }
//            }
//        }
//    }
//}
