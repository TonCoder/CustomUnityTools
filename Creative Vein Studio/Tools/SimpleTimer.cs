using UnityEngine;

namespace MAIN_PROJECT._Scripts.Tools
{
    /// <summary>
    /// Simple Timer class allows you to leverage a custom stopwatch function that can be reset, updated, started, and stopped.
    /// Settings up the timer's constructor helps set specific start time.
    /// </summary>
    public class SimpleTimer
    {
        private float _timer;
        private float _waitTime;

        private bool startTimer = false;

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void StartTimer() => startTimer = true;


        /// <summary>
        /// Stops the timer
        /// </summary>
        public void StopTimer() => startTimer = false;

        /// <summary>
        /// Checks if the timer was met / completed
        /// </summary>
        public bool TimeMet { get; private set; }

        public SimpleTimer(float waitTime = 1, bool autoStart = false)
        {
            this._waitTime = waitTime;
            this.startTimer = autoStart;
        }

        /// <summary>
        /// Allows the time to be set at a different point other than the constructor
        /// </summary>
        /// <param name="wait"></param>
        public void SetWaitTime(float wait) => _waitTime = wait;

        /// <summary>
        /// Restarts the timer from 0
        /// </summary>
        public void Restart()
        {
            _timer = 0;
        }

        /// <summary>
        /// Called within the Update function to allow the time to pass within the timer
        /// </summary>
        public void TickOneValidation()
        {
            if (!startTimer && !TimeMet) return;

            _timer += Time.deltaTime;
            if (_timer > _waitTime)
            {
                TimeMet = true;
            }
        }

        /// <summary>
        /// Called within the Update function to allow the time to pass within the timer
        /// </summary>
        public void Tick()
        {
            if (!startTimer) return;

            _timer += Time.deltaTime;
            if (_timer > _waitTime)
            {
                _timer = 0;
                TimeMet = true;
                return;
            }

            TimeMet = false;
        }

        /// <summary>
        /// Called within the Update function to allow the time to pass but requires the deltatime to be provided.
        /// Useful in case another timing option is being uses, say : coroutine or network deltatime
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Tick(in float deltaTime)
        {
            if (!startTimer) return;

            _timer += deltaTime;
            if (_timer > _waitTime)
            {
                _timer = 0;
                TimeMet = true;
                return;
            }

            TimeMet = false;
        }
    }
}