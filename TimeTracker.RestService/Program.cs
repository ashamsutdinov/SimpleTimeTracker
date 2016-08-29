﻿using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceProcess;

namespace TimeTracker.RestService
{
    public class Program : 
        ServiceBase
    {
        private static ServiceHost _serviceHost;

        public Program()
        {
            ServiceName = "SimpleTimeTrackingService";
        }

        private static void Main()
        {
            if (!Environment.UserInteractive)
            {
                Run(new Program());
            }
            else
            {
                OpenServiceHost();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            OpenServiceHost();
        }

        protected override void OnStop()
        {
            base.OnStop();
            CloseServiceHost();
        }

        private static void OpenServiceHost()
        {
            CloseServiceHost();
            _serviceHost = new WebServiceHost(typeof (TimeTrackingService));
            _serviceHost.Open();
        }

        private static void CloseServiceHost()
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
            }
        }
    }
}
