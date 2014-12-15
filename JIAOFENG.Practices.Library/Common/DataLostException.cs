using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.Common
{
    [Serializable]
    public class DataLostException : CommonException
    {
        public string DBTable { get; set; }
        public string DataCode { get; set; }

        private const string MessageFormat = "Data:\"{0}\" is lost in table {1}";
        public DataLostException(string dataCode, string dbTable)
            : base(string.Format(MessageFormat, dataCode, dbTable), Constant.DataLostErrorCode)
        {
            this.DBTable = dbTable;
            this.DataCode = dataCode;
        }
    }
}
