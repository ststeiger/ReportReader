
namespace AnySqlWebAdmin 
{
    
    
    [System.Flags]
    public enum RenderType_t : int
    {
        Default = 0,
        Indented = 1,
        DataTable = 2,
        Array = 4,
        Data_Only = 8,
        Columns_Associative = 16,
        Columns_ObjectArray = 32,
        WithDetail = 64,
        ShortName = 128,
        LongName = 256,
        AssemblyQualifiedName = 512
    }



    public abstract class SqlService
    {
        protected virtual string GetConnectionString()
        {
            var csb = new System.Data.SqlClient.SqlConnectionStringBuilder();

            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                csb.DataSource = System.Environment.MachineName;
            else
                csb.DataSource = System.Environment.MachineName + @"\SQLEXPRESS";

            csb.InitialCatalog = "COR_Basic_Demo_V4";
            csb.IntegratedSecurity = System.Environment.OSVersion.Platform != System.PlatformID.Unix;
            if (!csb.IntegratedSecurity)
            {
                csb.UserID = "Test";
                csb.Password = "123";
            } // End if (!csb.IntegratedSecurity) 

            csb.PacketSize = 4096;
            csb.PersistSecurityInfo = false;
            csb.ApplicationName = "BlueMine";
            csb.ConnectTimeout = 15;
            csb.Pooling = true;
            csb.MinPoolSize = 1;
            csb.MaxPoolSize = 100;
            csb.MultipleActiveResultSets = false;
            csb.WorkstationID = System.Environment.MachineName;

            string cs = csb.ConnectionString;
            csb = null;

            return cs;
        }


        public virtual System.Data.Common.DbConnection Connection
        {
            get
            {
                return new System.Data.SqlClient.SqlConnection(GetConnectionString());
            }
        }


        public virtual void AddParameterList(System.Collections.Generic.Dictionary<string, object> pars, System.Data.Common.DbCommand cmd)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, object> kvp in pars)
            {
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(kvp.Key, kvp.Value));
            }
        } // End Sub AddParameterList 

    }


    public class SqlServiceJsonHelper 
    {
        
        
        private static string GetAssemblyQualifiedNoVersionName(string input)
        {
            int i = 0;
            bool isNotFirst = false;
            while (i < input.Length)
            {
                if (input[i] == ',')
                {
                    if (isNotFirst)
                        break;

                    isNotFirst = true;
                }
                i += 1;
            }
            
            return input.Substring(0, i);
        } // GetAssemblyQualifiedNoVersionName
        
        
        private static string GetAssemblyQualifiedNoVersionName(System.Type type)
        {
            if (type == null)
                return null;

            return GetAssemblyQualifiedNoVersionName(type.AssemblyQualifiedName);
        } // GetAssemblyQualifiedNoVersionName
        
        
        private static string GetTypeName(System.Type type, RenderType_t renderType)
        {
            if (type == null)
                return null;

            if (renderType.HasFlag(RenderType_t.ShortName))
                return type.Name;

            if (renderType.HasFlag(RenderType_t.LongName))
                return type.FullName;

            if (renderType.HasFlag(RenderType_t.AssemblyQualifiedName))
                return GetAssemblyQualifiedNoVersionName(type);

            return type.Name;
        } // GetAssemblyQualifiedNoVersionName
        
        
        
        private static void WriteAssociativeColumnsArray(Newtonsoft.Json.JsonTextWriter jsonWriter
            , System.Data.Common.DbDataReader dr, RenderType_t renderType)
        {
            //jsonWriter.WriteStartObject();
            jsonWriter.WriteStartObject();


            for (int i = 0; i <= dr.FieldCount - 1; i++)
            {
                jsonWriter.WritePropertyName(dr.GetName(i));
                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("index");
                jsonWriter.WriteValue(i);

                if (renderType.HasFlag(RenderType_t.WithDetail))
                {
                    jsonWriter.WritePropertyName("fieldType");
                    // jsonWriter.WriteValue(GetAssemblyQualifiedNoVersionName(dr.GetFieldType(i)));
                    jsonWriter.WriteValue(GetTypeName(dr.GetFieldType(i), renderType));
                }

                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
        } // WriteAssociativeArray
        
        
        private static void WriteComplexArray(Newtonsoft.Json.JsonTextWriter jsonWriter, System.Data.Common.DbDataReader dr, RenderType_t renderType)
        {
            //jsonWriter.WriteStartObject();
            jsonWriter.WriteStartArray();

            for (int i = 0; i <= dr.FieldCount - 1; i++)
            {
                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("name");
                jsonWriter.WriteValue(dr.GetName(i));

                jsonWriter.WritePropertyName("index");
                jsonWriter.WriteValue(i);

                if (renderType.HasFlag(RenderType_t.WithDetail))
                {
                    jsonWriter.WritePropertyName("fieldType");
                    //jsonWriter.WriteValue(GetAssemblyQualifiedNoVersionName(dr.GetFieldType(i)));
                    jsonWriter.WriteValue(GetTypeName(dr.GetFieldType(i), renderType));
                    
                }

                jsonWriter.WriteEndObject();
            }

            // jsonWriter.WriteEndObject();
            jsonWriter.WriteEndArray();
        } // WriteAssociativeArray
        
        
        private static void WriteArray(Newtonsoft.Json.JsonTextWriter jsonWriter, System.Data.Common.DbDataReader dr)
        {
            jsonWriter.WriteStartArray();
            for (int i = 0; i <= dr.FieldCount - 1; i++)
                jsonWriter.WriteValue(dr.GetName(i));

            jsonWriter.WriteEndArray();
        } // End Sub WriteArray 



        public static void AnyDataReaderToAnyJson(
              string sql
            , SqlService service
            , System.Collections.Generic.Dictionary<string, object> pars
            , System.Web.HttpContext context
            , RenderType_t format)
        {   
            using (System.Data.Common.DbConnection con = service.Connection)
            {
                
                using (System.Data.Common.DbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    service.AddParameterList(pars, cmd);

                    // cmd.ExecuteNonQuery
                    // cmd.ExecuteReader
                    // cmd.ExecuteScalar

                    using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader(
                        System.Data.CommandBehavior.SequentialAccess
                        | System.Data.CommandBehavior.CloseConnection))
                    {

                        using (System.IO.StreamWriter output = new System.IO.StreamWriter(context.Response.OutputStream))
                        {
                            using (Newtonsoft.Json.JsonTextWriter jsonWriter =
                                new Newtonsoft.Json.JsonTextWriter(output)) // context.Response.Output)
                            {
                                if (format.HasFlag(RenderType_t.Indented))
                                    jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;

                                context.Response.StatusCode = (int) System.Net.HttpStatusCode.OK;
                                context.Response.ContentType = "application/json";


                                jsonWriter.WriteStartObject();

                                jsonWriter.WritePropertyName("tables");
                                jsonWriter.WriteStartArray();

                                do
                                {

                                    if (!format.HasFlag(RenderType_t.Data_Only) &&
                                        !format.HasFlag(RenderType_t.DataTable))
                                    {
                                        jsonWriter.WriteStartObject();
                                        jsonWriter.WritePropertyName("columns");

                                        if (format.HasFlag(RenderType_t.Columns_Associative))
                                        {
                                            WriteAssociativeColumnsArray(jsonWriter, dr, format);
                                        }
                                        else if (format.HasFlag(RenderType_t.Columns_ObjectArray))
                                        {
                                            WriteComplexArray(jsonWriter, dr, format);
                                        }
                                        else // (format.HasFlag(RenderType_t.Array))
                                        {
                                            WriteArray(jsonWriter, dr);
                                        }
                                    } // End if (!format.HasFlag(RenderType_t.Data_Only)) 



                                    if (!format.HasFlag(RenderType_t.Data_Only) &&
                                        !format.HasFlag(RenderType_t.DataTable))
                                    {
                                        jsonWriter.WritePropertyName("rows");
                                    } // End if (!format.HasFlag(RenderType_t.Data_Only))

                                    jsonWriter.WriteStartArray();

                                    if (dr.HasRows)
                                    {
                                        string[] columns = null;
                                        if (format.HasFlag(RenderType_t.DataTable))
                                        {
                                            columns = new string[dr.FieldCount];
                                            for (int i = 0; i < dr.FieldCount; i++)
                                            {
                                                columns[i] = dr.GetName(i);
                                            } // Next i 
                                        } // End if (format.HasFlag(RenderType_t.DataTable)) 

                                        while (dr.Read())
                                        {
                                            if (format.HasFlag(RenderType_t.DataTable))
                                                jsonWriter.WriteStartObject();
                                            else
                                                jsonWriter.WriteStartArray();

                                            for (int i = 0; i <= dr.FieldCount - 1; i++)
                                            {
                                                object obj = dr.GetValue(i);
                                                if (obj == System.DBNull.Value)
                                                    obj = null;

                                                if (columns != null && format.HasFlag(RenderType_t.DataTable))
                                                {
                                                    jsonWriter.WritePropertyName(columns[i]);
                                                }

                                                jsonWriter.WriteValue(obj);
                                            } // Next i 

                                            if (format.HasFlag(RenderType_t.DataTable))
                                                jsonWriter.WriteEndObject();
                                            else
                                                jsonWriter.WriteEndArray();
                                        } // Whend 

                                    } // End if (dr.HasRows) 

                                    jsonWriter.WriteEndArray();

                                    if (!format.HasFlag(RenderType_t.Data_Only) &&
                                        !format.HasFlag(RenderType_t.DataTable))
                                    {
                                        jsonWriter.WriteEndObject();
                                    } // End if (!format.HasFlag(RenderType_t.Data_Only)) 

                                } while (dr.NextResult());
                                
                                jsonWriter.WriteEndArray();
                                jsonWriter.WriteEndObject();
                                
                                jsonWriter.Flush();
                                output.Flush();
                            } // jsonWriter 
                            
                        } // output 
                        
                    } // dr 
                    
                } // End Using cmd 
                
                if (con.State != System.Data.ConnectionState.Closed)
                    con.Close();
            } // con 
            
        } // End Sub WriteArray 
        
        
    } // End Class 
    
    
} // End Namespace 
