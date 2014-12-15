using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace JIAOFENG.Practices.Database
{
    /// <summary>
    /// Returns detailed error information about an exception associated with a data access operation.
    /// </summary>
    [Serializable()]
    public class DatabaseExecutionException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DatabaseExecutionException class.
        /// </summary>
        internal DatabaseExecutionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DatabaseExecutionException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        internal DatabaseExecutionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DatabaseExecutionException class with a generic error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        internal DatabaseExecutionException(Exception innerException)
            : base("See inner exception for details.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DatabaseExecutionException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        internal DatabaseExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DatabaseExecutionException class with a reference to the SqlCommand and inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        /// <param name="command">The SqlCommand that caused the exception.</param>
        internal DatabaseExecutionException(string message, Exception innerException, DbCommand command)
            : base(message, innerException)
        {
            this.Data.Add("CommandState", GetCommandState(command));
        }

        /// <summary>
        /// Initializes a new instance of the DatabaseExecutionException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected DatabaseExecutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Builds a string that includes that captures the state of a given SqlCommand.
        /// </summary>
        /// <param name="command">SqlCommand that has its state captured.</param>
        /// <returns>The string including the state of a given SqlCommand.</returns>
        private static string GetCommandState(DbCommand command)
        {
            StringBuilder commandState = new StringBuilder(256);

            using (StringWriter stringWriter = new StringWriter(commandState))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlWriter.WriteStartElement("DbCommand");
                    AddElement(xmlWriter, "ConnectionString", command.Connection.ConnectionString);
                    AddElement(xmlWriter, "ConnectionTimeOut", command.Connection.ConnectionTimeout);
                    AddElement(xmlWriter, "CommandText", command.CommandText);
                    AddElement(xmlWriter, "CommandType", command.CommandType);
                    AddElement(xmlWriter, "CommandTimeOut", command.CommandTimeout);

                    if (command.Parameters.Count > 0)
                    {
                        xmlWriter.WriteStartElement("Parameters");

                        foreach (DbParameter parameter in command.Parameters)
                        {
                            xmlWriter.WriteStartElement("Parameter");
                            AddElement(xmlWriter, "Name", parameter.ParameterName);
                            AddElement(xmlWriter, "Type", parameter.DbType);
                            //Tianjin CP1 - not logging value until security concerns are addressed
                            //AddElement(xmlWriter, "Value", parameter.SqlValue);
                            AddElement(xmlWriter, "Direction", parameter.Direction);
                            xmlWriter.WriteEndElement();
                        }
                    }

                    xmlWriter.WriteEndElement();
                }
            }

            return commandState.ToString();
        }

        private static void AddElement(XmlWriter writer, string elementName, object elementValue)
        {
            writer.WriteStartElement(elementName);
            writer.WriteValue(elementValue.ToString());
            writer.WriteEndElement();
        }

        #endregion
    }
}
