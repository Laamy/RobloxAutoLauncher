using System;

namespace RobloxAutoLauncher.SDK.Jobs
{
    public class Job
    {
        public Func<bool> isFinished { get; }
        public Action OnStart { get; }

        public Job (Func<bool> isFinished, Action onStart)
        {
            this.isFinished = isFinished;
            OnStart = onStart;
        }
    }
}
