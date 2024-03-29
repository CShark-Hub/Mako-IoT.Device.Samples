﻿using System;
using System.Threading;
using MakoIoT.Device.Displays.Led;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Samples.WBC.Device.Services
{
    public class DisplayController : IDisplayController
    {
        private readonly ILog _logger;
        private readonly RgbPixel _pixel;
        private Thread _displayThread;
        private CancellationTokenSource _displayTokenSource;
        private CancellationToken _cancellationToken;

        public DisplayController(IPixelDriver pixelDriver, ILog logger)
        {
            _pixel = new RgbPixel(pixelDriver);
            _logger = logger;
        }

        public void DisplayTodaysBins(Color[] bins)
        {
            if (bins == null || bins.Length == 0)
            {
                Blank();
                return;
            }

            SwitchDisplay(() =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    for (int i = 0; i < bins.Length && !_cancellationToken.IsCancellationRequested; i++)
                    {
                        _pixel.Transition(bins[i], _cancellationToken);
                        _cancellationToken.WaitHandle.WaitOne(1000, false);
                        _pixel.Transition(new Color(0, 0, 0), _cancellationToken);
                    }
                }
            });
        }

        public void DisplayTomorrowsBins(Color[] bins)
        {
            if (bins == null || bins.Length == 0)
            {
                Blank();
                return;
            }

            SwitchDisplay(() =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    for (int i = 0; i < bins.Length && !_cancellationToken.IsCancellationRequested; i++)
                    {
                        _pixel.Transition(bins[i], _cancellationToken);
                        _cancellationToken.WaitHandle.WaitOne(2000, false);
                    }
                }
            });
        }

        public void Blank()
        {
            Cancel();
            _pixel.SetColor(new Color(0, 0, 0));
        }

        public void DisplayError()
        {
            SwitchDisplay(() =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    _pixel.SetColor(new Color(255,0,0));
                    _cancellationToken.WaitHandle.WaitOne(500, false);
                    _pixel.SetColor(new Color(0, 0, 0));
                    _cancellationToken.WaitHandle.WaitOne(3000, false);
                }
            });
        }

        public void DisplayUpdating()
        {
            SwitchDisplay(() =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    _pixel.SetColor(new Color(255, 0, 255));
                    _cancellationToken.WaitHandle.WaitOne(2000, false);
                    _pixel.SetColor(new Color(0, 0, 0));
                    _cancellationToken.WaitHandle.WaitOne(500, false);
                }
            });
        }

        public void DisplayUpdatingError()
        {
            Cancel();
            for (int i = 0; i < 3; i++)
            {
                _pixel.SetColor(new Color(255, 0, 255));
                Thread.Sleep(500);
                _pixel.SetColor(new Color(0, 0, 0));
                Thread.Sleep(500);
            }

        }

        public void DisplayConfigMode()
        {
            SwitchDisplay(() =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    _pixel.SetColor(new Color(255, 255, 0));
                    _cancellationToken.WaitHandle.WaitOne(1000, false);
                    _pixel.SetColor(new Color(0, 0, 0));
                    _cancellationToken.WaitHandle.WaitOne(1000, false);
                }
            });
        }

        private void SwitchDisplay(Action displayMethod)
        {
            Cancel();

            _displayTokenSource = new CancellationTokenSource();
            _cancellationToken = _displayTokenSource.Token;
            _displayThread = new Thread(() => displayMethod());
            _displayThread.Start();
            _logger.Trace("New display method started");
        }

        private void Cancel()
        {
            if (_displayThread != null)
            {
                _logger.Trace("Cancelling current display method");
                _displayTokenSource.Cancel();
                // _displayThread.Join(); //TODO: exception on Thread.Join()
                Thread.Sleep(100); //once join is fixed, this can be removed
                _displayThread = null;
            }
        }
    }
}
