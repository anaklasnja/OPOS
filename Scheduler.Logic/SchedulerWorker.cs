using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Logic
{
    public class SchedulerWorker
    {
        #region Attributes

        /// <summary>
        /// Thread that runs the queue processor
        /// </summary>
        private Thread workerThread;

        /// <summary>
        /// Flag to indicate if the current processor is currently running
        /// </summary>
        private bool running = false;

        /// <summary>
        /// Lock used for synchronizing access to the Running flag.
        /// </summary>
        private object tempLock = new object();

        #endregion  // Attributes

        public SchedulerWorker() 
        {
        }

        public Thread WorkerThread
        {
            get { return workerThread; }
            private set
            {
                workerThread = value;
            }
        }

        public string Status 
        { 
            get 
            {
                return Running ? "Running" : "Stopped";
            } 
        }


        public void Start()
        {
            if (WorkerThread == null) 
            {
                WorkerThread = new Thread(new ThreadStart(StartProcessing));
            }

            WorkerThread.IsBackground = true;
            WorkerThread.Name = "Scheduler Thread";
            WorkerThread.Start();
        }

        public void Stop()
        {
            Running = false;
            if (WorkerThread != null)
            {
                WorkerThread.Join(100); /* allow worker to complete */
            }

            // Wait for termination
            while(Running)
            {
                Thread.Sleep(50); /* 50 ms */
            }

            WorkerThread = null;
        }

        public void StartProcessing()
        {
            Running = true;

            /* Run until the "stop" condition (Running = false) is encountered */
            while (Running)
            {
                Thread.Sleep(50); /* 50 ms */
            }
        }
        public bool Running
        {
            get { lock (tempLock) { return running; } }
            set { lock (tempLock) { running = value; } }
        }
    }
}
