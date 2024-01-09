using System;
using System.Threading;

namespace MakoIoT.Device.Services.WiFi
{
    public class DelayCancellationTokenSource : CancellationTokenSource
    {
        private bool _isDisposed;
        private Thread _delayThread;

        /// <summary>
        /// Constructs a <see cref="T:System.Threading.CancellationTokenSource" /> that will be canceled after a specified time span.
        /// </summary>
        /// <param name="millisecondsDelay">The time span to wait before canceling this <see cref="T:System.Threading.CancellationTokenSource" /></param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// The exception that is thrown when <paramref name="millisecondsDelay" /> is less than -1.
        /// </exception>
        /// <remarks>
        /// <para>
        /// The countdown for the millisecondsDelay starts during the call to the constructor.  When the millisecondsDelay expires,
        /// the constructed <see cref="T:System.Threading.CancellationTokenSource" /> is canceled (if it has
        /// not been canceled already).
        /// </para>
        /// <para>
        /// Subsequent calls to CancelAfter will reset the millisecondsDelay for the constructed
        /// <see cref="T:System.Threading.CancellationTokenSource" />, if it has not been
        /// canceled already.
        /// </para>
        /// </remarks>
        public DelayCancellationTokenSource(int millisecondsDelay)
        {
            if (millisecondsDelay < -1)
                throw new ArgumentOutOfRangeException();
            StartDelay(millisecondsDelay);
        }

        /// <summary>
        /// Constructs a <see cref="T:System.Threading.CancellationTokenSource" /> that will be canceled after a specified time span.
        /// </summary>
        /// <param name="delay">The time span to wait before canceling this <see cref="T:System.Threading.CancellationTokenSource" /></param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// The exception that is thrown when <paramref name="delay" /> is less than -1 or greater than Int32.MaxValue.
        /// </exception>
        /// <remarks>
        /// <para>
        /// The countdown for the delay starts during the call to the constructor.  When the delay expires,
        /// the constructed <see cref="T:System.Threading.CancellationTokenSource" /> is canceled, if it has
        /// not been canceled already.
        /// </para>
        /// <para>
        /// Subsequent calls to CancelAfter will reset the delay for the constructed
        /// <see cref="T:System.Threading.CancellationTokenSource" />, if it has not been
        /// canceled already.
        /// </para>
        /// </remarks>
        public DelayCancellationTokenSource(TimeSpan delay)
        {
            long totalMilliseconds = (long)delay.TotalMilliseconds;
            if (totalMilliseconds < -1L || totalMilliseconds > (long)int.MaxValue)
                throw new ArgumentOutOfRangeException();
            StartDelay((int)totalMilliseconds);
        }


        private void StartDelay(int delay)
        {
            _delayThread = new Thread(() =>
            {
                Thread.Sleep(delay);
                TimerCallbackLogic();
            });
            _delayThread.Start();
        }

        private void TimerCallbackLogic()
        {
            if (_isDisposed)
                return;
            try
            {
                Cancel();
            }
            catch (ObjectDisposedException ex)
            {
                if (_isDisposed)
                    return;
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_delayThread != null)
                _delayThread.Abort();

            base.Dispose(disposing);
            _isDisposed = true;
        }
    }
}