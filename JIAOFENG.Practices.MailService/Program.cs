using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace DashingMailService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase coreServices = new CoreService();
            ServiceBase.Run(coreServices);

            //CoreService coreServices = new CoreService();
            //coreServices.Start(null);
        }
    }
}
