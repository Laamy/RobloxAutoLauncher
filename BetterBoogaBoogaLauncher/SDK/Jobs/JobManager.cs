using System;
using System.Collections.Generic;

namespace RobloxAutoLauncher.SDK.Jobs
{
    public class JobManager
    {
        private readonly Queue<Job> jobs = new Queue<Job>();
        private readonly Action<int> updateProgress = null;
        private int totalJobs;
        private Job currentJob = null;

        public JobManager(Action<int> updateProgress)
        {
            this.updateProgress = updateProgress;
        }

        public void AddJob(Job job) => jobs.Enqueue(job);

        public void End() => totalJobs = jobs.Count;

        public void TickJobs()
        {
            if (currentJob != null && currentJob.isFinished())
                currentJob = null;

            if (currentJob == null && jobs.Count > 0)
            {
                currentJob = jobs.Dequeue();
                currentJob.OnStart?.Invoke();
                updateProgress((totalJobs - jobs.Count - 1) * 100 / (totalJobs - 1));
            }
        }
}
    }
