using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.PushService
{
    public abstract class PushService : IPushService
    {
        public PushService(int failRetryIntervalPow, int failMostNum, ClientConfig cc)
        {
            this.FailRetryIntervalPow = failRetryIntervalPow;
            this.FailMostNum = failMostNum;
            this.Client = cc;
        }
        public int FailRetryIntervalPow { get; private set; }
        public int FailMostNum { get; private set; }

        public ClientConfig Client { get; private set; }
        public virtual void Push()
        {
            throw new NotImplementedException();
        }
    }
}
