using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Logic.Log
{
    public enum EventType
    {
        /// <summary>
        /// The event is for information.
        /// </summary>
        Information = 4,

        /// <summary>
        /// The event is a warning.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// The event is an error.
        /// </summary>
        Error = 1,

        /// <summary>
        /// Verified Successfully.
        /// </summary>
        SuccessAudit = 8,

        /// <summary>
        /// Failed to verify.
        /// </summary>
        FailureAudit = 16,

        /// <summary>
        /// The event is for debugging.
        /// </summary>
        Debug = 32
    }
}
