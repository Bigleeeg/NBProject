using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat.Response.Message
{
    [Serializable]
    public class TemplateMessage
    {
        public string ToUser { get; set; }
        public string TemplateId { get; set; }
        public string Url { get; set; }
        public string TopColor { get; set; }
        public TemplateMessageData Data { get; set; }
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            const string touserFormat = "\"touser\":\"{0}\",";
            sb.AppendFormat(touserFormat, ToUser);
            const string templateFormat = "\"template_id\":\"{0}\",";
            sb.AppendFormat(templateFormat, TemplateId);
            const string urlFormat = "\"url\":\"{0}\",";
            sb.AppendFormat(urlFormat, Url);
            const string topcolorFormat = "\"topcolor\":\"{0}\",";
            sb.AppendFormat(topcolorFormat, TopColor);
            const string dataFormat = "\"data\":{0}";
            sb.AppendFormat(dataFormat, Data.ToJson());
            sb.Append("}");
            return sb.ToString();
        }
    }

    [Serializable]
    public class TemplateMessageData
    {
        public Note First { get; set; }
        public Note[] KeyNotes { get; set; }
        public Note Remark { get; set; }

        [Serializable]
        public class Note
        {
            public string Color { get; set; }
            public string Value { get; set; }
            public string ToJson()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                const string valueFormat = "\"value\":\"{0}\",";
                sb.AppendFormat(valueFormat, Value);
                const string colorFormat = "\"color\":\"{0}\"";
                sb.AppendFormat(colorFormat, Color);
                sb.Append("}");
                return sb.ToString();
            }
        }
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            const string firstFormat = "\"first\":{0},";
            sb.AppendFormat(firstFormat, First.ToJson());

            if (KeyNotes != null)
            {
                for (int i = 0; i < KeyNotes.Length; i++)
                {
                    const string keynoteFormat = "\"keynote{1}\":{0},";
                    sb.AppendFormat(keynoteFormat, KeyNotes[i].ToJson(), i + 1);
                }
            }

            const string remarkFormat = "\"remark\":{0}";
            sb.AppendFormat(remarkFormat, Remark.ToJson());
            sb.Append("}");
            return sb.ToString();
        }
    }
}
