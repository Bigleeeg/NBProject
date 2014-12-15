using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace JIAOFENG.Practices.Logic.Log
{
    public sealed class AdapterElementCollection : ConfigurationElementCollection
    {
        public AdapterElement this[int index]
        {
            get { return (AdapterElement)base.BaseGet(index); }

            set
            {
                if (base.BaseGet(index) != null)
                    base.BaseRemoveAt(index);

                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AdapterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AdapterElement)element).Name;
        }
    }
}
