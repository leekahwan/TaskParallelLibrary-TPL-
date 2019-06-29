using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskParallelLibrary_TPL_
{
    class Program
    {
        //https://www.cnblogs.com/tianqing/p/6970331.html
        static void Main(string[] args)
        {
            //CreateAndRunTask();

            //TaskCancel();

            CreateTasks();
        }

        //创建和运行任务
        public static void CreateAndRunTask()
        {
            //创建任务
            var task1 = new Task(() =>
            {
                Console.WriteLine("Create and start task!");
            });
            //启动任务
            task1.Start();

            //创建并启动任务
            var task2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task factory startnew task!");
            });

            Console.ReadKey();
        }

        static int TaskAction(string name,int seconds,CancellationToken token)
        {
            Console.WriteLine(string.Format("Task:{0} is runing on Thread:{1}",name,Thread.CurrentThread.ManagedThreadId));
            for (int i = 0; i < seconds; i++)
            {
                Thread.Sleep(1000);
                if (token.IsCancellationRequested)
                {
                    return -1;
                }
            }
            return 42 * seconds;
        }

        //取消任务
        public static void TaskCancel()
        {
            var cts = new CancellationTokenSource();
            var longTask = new Task<int>(() =>
            {
                return TaskAction("Task 1",10,cts.Token);
            });
            Console.WriteLine(longTask.Status);
            cts.Cancel();
            Console.WriteLine(longTask.Status);
            Console.WriteLine("Task 1 已经被取消，未启动执行！");

            cts = new CancellationTokenSource();
            longTask = new Task<int>(() =>
            {
                return TaskAction("Task 2", 10, cts.Token);
            });
            longTask.Start();
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                Console.WriteLine(longTask.Status);
            }
            cts.Cancel();
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                Console.WriteLine(longTask.Status);
            }
            Console.WriteLine("Task 2 已经被取消，结果:" + longTask.Result);
            Console.ReadKey();
        }

        //创建任务集合并输出结果
        public static void CreateTasks()
        {
            var tasks = new List<Task<string>>()
            {
                Task.Factory.StartNew<string>(() =>
                {
                    return "Task 1";
                }),
                Task.Factory.StartNew<string>(() =>
                {
                    return "Task 2";
                }),
                Task.Factory.StartNew<string>(() =>
                {
                    return "Task 3";
                })
            };

            foreach (var task in tasks)
            {
                Console.WriteLine(task.Result);
            }
            Console.ReadKey();
        }
    }
}
