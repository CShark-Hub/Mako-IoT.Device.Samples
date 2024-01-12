using System;
using System.Reflection;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Samples.WBC.Device.Tests.Mocks
{
    public class MockLog : ILog
    {
        public void Trace(Exception exception, string message, MethodInfo format)
        {
            
        }

        public void Trace(Exception exception, string message)
        {
            
        }

        public void Trace(string message)
        {
            
        }

        public void Trace(Exception exception)
        {
            
        }

        public void Information(Exception exception, string message, MethodInfo format)
        {
            
        }

        public void Information(Exception exception, string message)
        {
            
        }

        public void Information(string message)
        {
            
        }

        public void Information(Exception exception)
        {
            
        }

        public void Warning(Exception exception, string message, MethodInfo format)
        {
            
        }

        public void Warning(Exception exception, string message)
        {
            
        }

        public void Warning(string message)
        {
            
        }

        public void Warning(Exception exception)
        {
            
        }

        public void Error(Exception exception, string message, MethodInfo format)
        {
            
        }

        public void Error(string message, Exception exception)
        {
            
        }

        public void Error(string message)
        {
            
        }

        public void Error(Exception exception)
        {
            
        }

        public void Critical(Exception exception, string message, MethodInfo format)
        {
            
        }

        public void Critical(Exception exception, string message)
        {
            
        }

        public void Critical(string message)
        {
            
        }

        public void Critical(Exception exception)
        {
            
        }
    }
}
