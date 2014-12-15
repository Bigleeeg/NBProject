using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.OracleClient;
using System.Configuration;

namespace EasyCoder
{
    public class OracleCreator : ICreator
    {
        const int sqlServerMaxParameterCount = 2099;
        private string connectionString = "";
        private string dBName = "";
        private string namespacename = "";
        private string ownerName = "";
        public void CreateAllCode(string connectionString,string dbName, string tablename, string namespacename,string ownerName)
        {
            this.namespacename = namespacename;
            this.connectionString = connectionString;
            this.dBName = dbName;
            this.ownerName = ownerName;

            List<string> tablenames = new List<string>();
            tablenames = tablename.Split(',').ToList();

            DataSet ds = GetDataInfo(tablenames, namespacename);
            CreateModel(ds, namespacename);
            CreateDal(ds, namespacename);
            //CreateLogic(ds, namespacename);


            DataSet ds_Enum = GetEnumData(tablenames);
            CreateEnum(ds_Enum, namespacename);
        }

        //获取数据库选择表基本信息
        public DataSet GetDataInfo(List<string> tablenames, string namespacename)
        {
            DataSet ds = new DataSet();
            OracleAccess ora = new OracleAccess();
            for (int i = 0; i < tablenames.Count; i++)
            {
                DataTable dt = new DataTable();
                string sqlText = @" select a.column_name as NAME,a.column_id as COLID,a.DATA_TYPE as TYPE,a.DATA_LENGTH as BYTE,a.DATA_PRECISION as LENGTH,
                                    case when exists(select 1 from all_constraints where constraint_type='P'and constraint_name in 
                                    ( select constraint_name from all_cons_columns where table_name=a.table_name
                                     and column_name=a.column_name)) then '1' else '0' end PK,case when a.nullable='Y' then '1' else '0' end ISNULLABLE,
                                     a.table_name ,a.owner
                                    from all_tab_columns a  where a.table_name='" + tablenames[i] + "' AND a.table_name not like '%_Enum' and a.owner='" + this.ownerName + "'  order by a.column_id";
                dt = ora.ExecuteDataTableSql(sqlText, this.connectionString);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = tablenames[i];
                    ds.Tables.Add(dt.Copy());
                }
            }
            return ds;
        }

        //创建实体类
        public void CreateModel(DataSet ds, string namespacename)
        {
            string entityDirectory = ConfigurationManager.AppSettings["EntityDirectory"];
            Directory.CreateDirectory(entityDirectory);

            string entityTemplateFile = Directory.GetCurrentDirectory() + "/Creator/Oracle/" + "EntityTemplate.txt";
            string entityTemplate = Utility.ReadFile(entityTemplateFile);

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                string emptyConstructorContent = GetEmptyConstructorContent(dt);
                string instanceName = "entity";
                string instanceConstructorContent = GetInstanceConstructorContent(dt, instanceName);
                string property = GetPropertyDefinition(dt);
                string newEntity = string.Format(entityTemplate, namespacename, ds.Tables[i].TableName, emptyConstructorContent, instanceName, instanceConstructorContent, property);

                string entityFilePath = entityDirectory + "/" + ds.Tables[i].TableName + "Entity.cs";
                FileStream fileStream = File.Create(entityFilePath);
                StreamWriter sw = new StreamWriter(fileStream);
                sw.Write(newEntity);
                sw.Close();
                fileStream.Close();
            }
        }
        private string GetEmptyConstructorContent(DataTable dt)
        {
            const string ContentFormat = "this.{0} = {1};\n";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["ISNULLABLE"].ToString() == "0")
                {
                    //to do
                }
            }
            return sb.ToString();
        }
        private string GetInstanceConstructorContent(DataTable dt, string instanceName)
        {
            const string ContentFormat = "          this.{0} = {1}.{0}; \n";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.AppendFormat(ContentFormat, dt.Rows[i]["NAME"].ToString(), instanceName);
            }
            return sb.ToString();
        }
        private string GetPropertyDefinition(DataTable dt)
        {
            const string PropertyFormat = "\n       public {0} {1} {{ get; set; }} \n";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["NAME"].ToString().ToLower() != "createtime" && dt.Rows[i]["NAME"].ToString().ToLower() != "createby" && dt.Rows[i]["NAME"].ToString().ToLower() != "updatetime" && dt.Rows[i]["NAME"].ToString().ToLower() != "updateby" )
                {
                    if (this.GetFieldType(dt.Rows[i]["TYPE"].ToString(), dt.Rows[i]["LENGTH"].ToString()) == "string" || this.GetFieldType(dt.Rows[i]["TYPE"].ToString(), dt.Rows[i]["LENGTH"].ToString()) == "byte[]")
                    {
                        sb.AppendFormat(PropertyFormat, this.GetFieldType(dt.Rows[i]["TYPE"].ToString(), dt.Rows[i]["LENGTH"].ToString()), dt.Rows[i]["NAME"].ToString());
                    }
                    else
                    {
                        if (dt.Rows[i]["ISNULLABLE"].ToString() == "0")
                        {
                            sb.AppendFormat(PropertyFormat, this.GetFieldType(dt.Rows[i]["TYPE"].ToString(), dt.Rows[i]["LENGTH"].ToString()), dt.Rows[i]["NAME"].ToString());
                        }
                        else
                        {
                            sb.AppendFormat(PropertyFormat, this.GetFieldType(dt.Rows[i]["TYPE"].ToString(), dt.Rows[i]["LENGTH"].ToString()) + "?", dt.Rows[i]["NAME"].ToString());
                        }
                    }
                }
            }
            return sb.ToString();
        }

        //创建增删改查方法Dal层
        public void CreateDal(DataSet ds, string namespacename)
        {
            string dalDirectory = ConfigurationManager.AppSettings["DalDirectory"];
            Directory.CreateDirectory(dalDirectory);

            string dalTemplateFile = Directory.GetCurrentDirectory() + "/Creator/Oracle/" + "DalTemplate.txt";
            string dalTemplate = Utility.ReadFile(dalTemplateFile); ;

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                string add = GetAddDefinition(dt);
                string delete = GetDeleteDefinition(dt);
                string update = GetUpdateDefinition(dt);
                string search = GetSearchDefinition(dt);
                string newEntity = string.Format(dalTemplate, namespacename, ds.Tables[i].TableName, add, delete, update, search);

                string dalFilePath = dalDirectory + "/" + ds.Tables[i].TableName + "Dal.cs";
                FileStream fileStream = File.Create(dalFilePath);
                StreamWriter sw = new StreamWriter(fileStream);
                sw.Write(newEntity);
                sw.Close();
                fileStream.Close();
            }
        }
        private string GetAddDefinition(DataTable dt)
        {
            string template = Utility.ReadFile(Directory.GetCurrentDirectory() + "/Creator/Oracle/" + "AddTemplate.txt");

            string columns = string.Empty;
            string values = string.Empty;
            string parameters = string.Empty;
            string setParametersFormat = "             currentParameter  = \":{0}\" + \"N\" + index.ToString(); oraParameterList.Add(new System.Data.OracleClient.OracleParameter(currentParameter, {1}));valuesParas = valuesParas + \",\" + currentParameter;\n";
            string setParameters = string.Empty;
            string pkIDName = string.Empty;
            int maxRecordCount = sqlServerMaxParameterCount / (dt.Rows.Count - 1);
            for (int i = 0; i < dt.Rows.Count; i++)
            { 
                columns += "[" + dt.Rows[i]["NAME"].ToString() + "],";
                if (dt.Rows[i]["PK"].ToString() == "0")
                {
                    values += ":" + dt.Rows[i]["NAME"].ToString() + ",";
                }
                else
                {
                    values += ":nextId,";
                }
                if (dt.Rows[i]["ISNULLABLE"].ToString() == "0")
                {
                    if (dt.Rows[i]["PK"].ToString() == "0")
                    {
                        parameters += "             oraParameterList.Add(new System.Data.OracleClient.OracleParameter(\":" + dt.Rows[i]["NAME"] + "\", entity." + dt.Rows[i]["NAME"] + "));\n";
                        setParameters = setParameters + string.Format(setParametersFormat, dt.Rows[i]["NAME"], "entity." + dt.Rows[i]["NAME"]);
                    }
                    else
                    {
                        parameters += "             oraParameterList.Add(new System.Data.OracleClient.OracleParameter(\":" + dt.Rows[i]["NAME"] + "\", nextId));\n";
                        setParameters = setParameters + string.Format(setParametersFormat, dt.Rows[i]["NAME"], "nextId");
                    }
                }
                else
                { 
                    parameters += "             oraParameterList.Add(new System.Data.OracleClient.OracleParameter(\":" + dt.Rows[i]["NAME"] + "\", entity." + dt.Rows[i]["NAME"] + " == null ? (object)DBNull.Value : entity." + dt.Rows[i]["NAME"] + "));\n";
                    setParameters = setParameters + string.Format(setParametersFormat, dt.Rows[i]["NAME"], "entity." + dt.Rows[i]["NAME"] + " == null ? (object)DBNull.Value : entity." + dt.Rows[i]["NAME"]);                     
                }  
                if (dt.Rows[i]["PK"].ToString() == "1")
                {
                    pkIDName = dt.Rows[i]["NAME"].ToString();
                }
            }
            columns = columns.TrimEnd(',');
            values = values.TrimEnd(',');
            parameters = parameters.TrimEnd('\n');

            return string.Format(template, dt.TableName, columns, values, pkIDName, parameters, setParameters, maxRecordCount);
        }
        private string GetDeleteDefinition(DataTable dt)
        {
            string template = Utility.ReadFile(Directory.GetCurrentDirectory() + "/Creator/Oracle/" + "DeleteTemplate.txt");
            string sqlPK = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["PK"].ToString() == "1")
                {
                    sqlPK = dt.Rows[i]["NAME"].ToString();
                    break;
                }
            }
            return string.Format(template, dt.TableName, sqlPK);
        }
        private string GetUpdateDefinition(DataTable dt)
        {
            string template = Utility.ReadFile(Directory.GetCurrentDirectory() + "/Creator/Oracle/" + "UpdateTemplate.txt");

            string sets = string.Empty;
            string wheres = string.Empty;
            string parameters = string.Empty;
            string setParametersFormat = "             currentParameter  = \":{0}\" + \"N\" + index.ToString(); oraParameterList.Add(new System.Data.OracleClient.OracleParameter(currentParameter, {1}));valuesParas = valuesParas + \",{0}=\" + currentParameter;\n";
            string setParameters = string.Empty;
            string setWhereFormat = "             currentParameter  = \":{0}\" + \"N\" + index.ToString(); oraParameterList.Add(new System.Data.OracleClient.OracleParameter(currentParameter, {1}));wherePara =  \"{0}=\" + currentParameter;\n";
            int maxRecordCount = sqlServerMaxParameterCount / dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["PK"].ToString() == "0")
                {
                    if (dt.Rows[i]["NAME"].ToString().ToLower() != "createtime" && dt.Rows[i]["NAME"].ToString().ToLower() != "createby")
                    {
                        sets += "[" + dt.Rows[i]["NAME"].ToString() + "] = :" + dt.Rows[i]["NAME"].ToString() + ",";
                        if (dt.Rows[i]["ISNULLABLE"].ToString() == "0")
                        {
                            parameters += "             oraParameterList.Add(new System.Data.OracleClient.OracleParameter(\":" + dt.Rows[i]["NAME"] + "\", entity." + dt.Rows[i]["NAME"] + "));\n";
                            setParameters = setParameters + string.Format(setParametersFormat, dt.Rows[i]["NAME"], "entity." + dt.Rows[i]["NAME"]);
                        }
                        else
                        {
                            parameters += "             oraParameterList.Add(new System.Data.OracleClient.OracleParameter(\":" + dt.Rows[i]["NAME"] + "\", entity." + dt.Rows[i]["NAME"] + " == null ? (object)DBNull.Value : entity." + dt.Rows[i]["NAME"] + "));\n";
                            setParameters = setParameters + string.Format(setParametersFormat, dt.Rows[i]["NAME"], "entity." + dt.Rows[i]["NAME"] + " == null ? (object)DBNull.Value : entity." + dt.Rows[i]["NAME"]);
                        }
                    }
                }
                else
                {
                    wheres += "[" + dt.Rows[i]["NAME"].ToString() + "] = :" + dt.Rows[i]["NAME"].ToString() + ",";
                    parameters += "oraParameterList.Add(new System.Data.OracleClient.OracleParameter(\":" + dt.Rows[i]["NAME"] + "\", entity." + dt.Rows[i]["NAME"] + " == null ? (object)DBNull.Value : entity." + dt.Rows[i]["NAME"] + "));\n";
                    setParameters = setParameters + string.Format(setWhereFormat, dt.Rows[i]["NAME"], "entity." + dt.Rows[i]["NAME"]);
                }
            }
            sets = sets.TrimEnd(',');
            wheres = wheres.TrimEnd(',');
            parameters = parameters.TrimEnd('\n');

            return string.Format(template, dt.TableName, sets, wheres, parameters, setParameters, maxRecordCount);
        }
        private string GetSearchDefinition(DataTable dt)
        {
            string template = Utility.ReadFile(Directory.GetCurrentDirectory() + "/Creator/Oracle/" + "SearchTemplate.txt");

            string format = "               if (dr[\"{0}\"] != DBNull.Value) {{entity.{0} = {1}(dr[\"{0}\"]);}} \n";
            string propertys = string.Empty;
            string sqlPK = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["PK"].ToString() == "1")
                {
                    sqlPK = dt.Rows[i]["NAME"].ToString();
                }
                string type = this.GetFieldType(dt.Rows[i]["TYPE"].ToString(), dt.Rows[i]["LENGTH"].ToString()).ToLower();
                string convert = string.Empty;
                switch (type)
                {
                    case "bool":
                        convert = "Convert.ToBoolean";
                        break;
                    case "byte":
                        convert = "Convert.ToInt32";
                        break;
                    case "byte[]":
                        convert = "(Byte[])";
                        break;
                    case "datetime":
                        convert = "Convert.ToDateTime";
                        break;
                    case "date":
                        convert = "Convert.ToDateTime";
                        break;
                    case "decimal":
                        convert = "Convert.ToDecimal";
                        break;
                    case "double":
                        convert = "Convert.ToDouble";
                        break;
                    case "guid":
                        convert = "Convert.ToString";
                        break;
                    case "image":
                        convert = "";
                        break;
                    case "int16":
                        convert = "Convert.ToInt16";
                        break;
                    case "uint16":
                        convert = "Convert.ToUInt16";
                        break;
                    case "int":
                        convert = "Convert.ToInt32";
                        break;
                    case "uint32":
                        convert = "Convert.ToUInt32";
                        break;
                    case "int64":
                        convert = "Convert.ToInt64";
                        break;
                    case "uint64":
                        convert = "Convert.ToUInt64";
                        break;
                    case "single":
                        convert = "Convert.ToSingle";
                        break;
                    case "string":
                        convert = "Convert.ToString";
                        break;

                    default:
                        convert = "Convert.ToString";
                        break;
                }
                propertys += string.Format(format, dt.Rows[i]["NAME"].ToString(), convert);
            }

            return string.Format(template, dt.TableName, propertys, sqlPK);
        }

        //创建BL层
        public void CreateLogic(DataSet ds, string namespacename)
        {
            //string logicDirectory = Directory.GetCurrentDirectory() + "/" + namespacename + "/Logic";
            string logicDirectory = ConfigurationManager.AppSettings["LogicDirectory"];
            Directory.CreateDirectory(logicDirectory);

            string logicTemplateFile = Directory.GetCurrentDirectory() + "/Creator/Oracle/" + "LogicTemplate.txt";
            string logicTemplate = Utility.ReadFile(logicTemplateFile); ;

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                string newLogic = string.Format(logicTemplate, namespacename, ds.Tables[i].TableName);

                string dalFilePath = logicDirectory + "/" + ds.Tables[i].TableName + "Logic.cs";
                FileStream fileStream = File.Create(dalFilePath);
                StreamWriter sw = new StreamWriter(fileStream);
                sw.Write(newLogic);
                sw.Close();
                fileStream.Close();
            }
        }

        private string GetFieldType(string fileType,string length)
        {
            string type = string.Empty;

            if (fileType.Length == 0)
                throw new ArgumentException(fileType);

            switch (fileType.ToLower())
            {
                case "NUMBER":
                    if (length == "1")
                    {
                        type = "bool";
                    }
                    else
                    {
                        type = "decimal";
                    }
                    break;
                case "BFILE":
                    type = "byte[]";
                    break;
                case "BLOB":
                    type = "byte[]";
                    break;
                case "LONG RAW":
                    type = "byte[]";
                    break;
                case "RAW":
                    type = "byte[]";
                    break;
                case "TIMESTAMP":
                    type = "DateTime";
                    break;
                case "DATE":
                    type = "DateTime";
                    break;
                case "INTEGER":
                    type = "decimal";
                    break;
                case "INTERVAL YEAR TO  MONTH":
                    type = "int";
                    break;
                case "CHAR":
                    type = "string";
                    break;
                case "CLOB":
                    type = "string";
                    break;
                case "LONG":
                    type = "string";
                    break;
                case "NCHAR":
                    type = "string";
                    break;
                case "NCLOB":
                    type = "string";
                    break;
                case "ROWID":
                    type = "string";
                    break;
                case "VARCHAR2":
                    type = "string";
                    break; 
                default:
                    type = "string";
                    break;
            }

            return type;
        }


        //获取数据库选择枚举表的数据
        public DataSet GetEnumData(List<string> tablenames)
        {
            DataSet ds = new DataSet();
            OracleAccess sql = new OracleAccess();
            sql.ConnectionString = this.connectionString;
            for (int i = 0; i < tablenames.Count; i++)
            {
                DataTable dt = new DataTable();
                string sqlText = @" SELECT * FROM " + tablenames[i] + " WHERE '" + tablenames[i] + "' like '%_Enum'";
                dt = sql.Query(sqlText);
                dt.TableName = tablenames[i];
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }

        //创建枚举类
        public void CreateEnum(DataSet ds, string namespacename)
        {
            string enumDirectory = ConfigurationManager.AppSettings["EnumDirectory"];
            Directory.CreateDirectory(enumDirectory);

            string enumTemplateFile = Directory.GetCurrentDirectory() + "/Creator/Oracle/" + "EnumTemplate.txt";
            string enumTemplate = Utility.ReadFile(enumTemplateFile);

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                string property = GetEnumPropertyDefinition(dt);
                if (string.IsNullOrWhiteSpace(property))
                {
                    continue;
                }
                string className = "E" + ds.Tables[i].TableName;
                string newEnum = string.Format(enumTemplate, namespacename, className, property);

                string enumFilePath = enumDirectory + "/" + className + ".cs";
                FileStream fileStream = File.Create(enumFilePath);
                StreamWriter sw = new StreamWriter(fileStream);
                sw.Write(newEnum);
                sw.Close();
                fileStream.Close();
            }
        }
        //生成枚举类中的属性
        private string GetEnumPropertyDefinition(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            string id = string.Empty;
            string name = string.Empty;
            string remark = string.Empty;
            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName.ToLower().EndsWith("id"))
                {
                    id = col.ColumnName;
                }
                if (col.ColumnName.ToLower().EndsWith("name"))
                {
                    name = col.ColumnName;
                }
                if (col.ColumnName.ToLower().EndsWith("remark"))
                {
                    remark = col.ColumnName;
                }
            }
            if (!(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(remark)))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i + 1 == dt.Rows.Count)
                    {
                        sb.AppendFormat("       {0} = {1} //{2} \n", dt.Rows[i][name].ToString(), dt.Rows[i][id].ToString(), dt.Rows[i][remark].ToString());
                    }
                    else
                    {
                        sb.AppendFormat("       {0} = {1}, //{2} \n", dt.Rows[i][name].ToString(), dt.Rows[i][id].ToString(), dt.Rows[i][remark].ToString());
                    }
                }
            }
            else if (!(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name)))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i + 1 == dt.Rows.Count)
                    {
                        sb.AppendFormat("       {0} = {1} \n", dt.Rows[i][name].ToString(), dt.Rows[i][id].ToString());
                    }
                    else
                    {
                        sb.AppendFormat("       {0} = {1}, \n", dt.Rows[i][name].ToString(), dt.Rows[i][id].ToString());
                    }
                }
            }

            return sb.ToString();
        }
    }

}