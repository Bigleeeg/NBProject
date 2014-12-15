using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Logic.Log
{
    internal static class LogTargetFactory
    {
        //internal static ILogTarget GetLogTarget(MediaType mediaType, params string[] parameters)
        //{
        //    ILogTarget ret = null;
        //    switch (mediaType)
        //    {
        //        case MediaType.Diagnostics:
        //            ret = new DiagnosticsAdapter();
        //            break;
        //        case MediaType.File:
        //            ret = new FileAdapter(parameter);
        //            break;
        //        case MediaType.Database:
        //            ret = new DatabaseAdapter(parameter);
        //            break;
        //        case MediaType.EventLog:
        //            string[] parameters = parameter.Split(',');
        //            ret = new EventLogAdapter(parameters[0], parameters[1]);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("target", mediaType, "Unrecognized MediaType member.");
        //    }
        //    return ret;
        //}
    }
}
