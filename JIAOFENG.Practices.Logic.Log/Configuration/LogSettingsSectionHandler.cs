using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;
namespace JIAOFENG.Practices.Logic.Log
{
    public class LogSettingsSectionHandler : ConfigurationSection
    {
        //[ConfigurationProperty("adapters", IsRequired = true)]
        //public List<AdapterElement> Adapters
        //{
        //    get
        //    {
        //        List<AdapterElement> ac = new List<AdapterElement>();
        //        foreach (AdapterElement e in (AdapterElementCollection)this["adapters"])
        //        {
        //            switch (e.MediaType)
        //            {
        //                case MediaType.Diagnostics:
        //                    ac.Add((DiagnosticsAdapterElement)e);
        //                    break;
        //                case MediaType.File:
        //                    ac.Add((FileAdapterElement)e);
        //                    break;
        //                case MediaType.Database:
        //                    ac.Add((DatabaseAdapterElement)e);
        //                    break;
        //                case MediaType.EventLog:
        //                    ac.Add((EventLogAdapterElement)e);
        //                    break;
        //                default:
        //                    throw new ArgumentOutOfRangeException("name", e.Name, "Unrecognized MediaType member.");
        //            }
        //        }
        //        return ac;
        //    }
        //}

        [ConfigurationProperty("adapters", IsRequired = true)]
        public AdapterElementCollection Adapters
        {
            get 
            {
                return (AdapterElementCollection)this["adapters"]; 
            }
        }
    }
}
