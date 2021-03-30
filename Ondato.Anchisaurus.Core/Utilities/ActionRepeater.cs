using System;
using System.Threading;

namespace Ondato.Anchisaurus.Core.Utilities
{
    public class ActionRepeater : IDisposable
    {
        private readonly Timer timer;
        private readonly Action action;

        public ActionRepeater(int intervalInSeconds, Action action)
        {
            if (intervalInSeconds < 0)
                throw new ArgumentException("Interval cannot be less than zero");

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            this.action = action;

            timer = new Timer(
                c => DoWork(), null, TimeSpan.Zero, TimeSpan.FromSeconds(intervalInSeconds)
            );
        }

        private void DoWork()
        {
            action?.Invoke();
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}