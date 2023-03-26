using System;

namespace Utilities
{
    public class TimeManager : IDisposable
    {
        private Action _eachSecondElapsed;
        
        public void RegisterForEachSecondElapsed(Action listener)
        {
            _eachSecondElapsed += listener;
        }

        public void UnregisterForEachSecondElapsed(Action listener)
        {
            _eachSecondElapsed -= listener;
        }

        
        public void InvokeSecondsElapsed()
        {
            _eachSecondElapsed?.Invoke();
        }

        public void Dispose()
        {
        }
    }
}