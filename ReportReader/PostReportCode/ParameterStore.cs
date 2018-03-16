using System;
using System.Collections.Generic;
using System.Text;

namespace ReportReader.PostReportCode
{
    // PostReportCode.ParameterStore
    using Microsoft.VisualBasic.CompilerServices;
    using PostReportCode;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;


    public class ParameterStore
    {
        private Dictionary<string, ReportParameter> dict;

        public ReportParameter this[string index]
        {
            get
            {
                return this.dict[index];
            }
            set
            {
                this.dict[index] = value;
            }
        }

        public ParameterStore(params ReportParameter[] u) 
            : this()
        {
            checked
            {
                int num = u.Length - 1;
                for (int i = 0; i <= num; i++)
                {
                    this.dict.Add(u[i].Key, u[i]);
                }
            }
        }

        public ParameterStore(Dictionary<string, ReportParameter> parameters)
        {
            this.dict = parameters;
        }

        public ParameterStore()
        {
            this.dict = new Dictionary<string, ReportParameter>(StringComparer.OrdinalIgnoreCase);
        }

        public void Add(ReportParameter para)
        {
            this.dict.Add(para.Key, para);
        }

        public static object IIF(bool expression, object truePart, object falsePart)
        {
            return (!expression) ? falsePart : truePart;
        }

        public static string ExecuteParameter()
        {
            ParameterStore Parameters = new ParameterStore();
            Parameters.Add(new ReportParameter("Vermessungsbezirk", "Alle", "00000000-0000-0000-0000-000000000000"));
            Parameters.Add(new ReportParameter("Kreis", "Alle", "00000000-0000-0000-0000-000000000000"));
            Parameters.Add(new ReportParameter("Gemeinde", "Alle", "00000000-0000-0000-0000-000000000000"));
            string str = ParameterStore.ExecuteParameter(Parameters);
            Console.WriteLine(str);
            return str;
        }


        public static string ExecuteParameter(ParameterStore Parameters)
        {
            return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("SELECT 2 AS ID, 2 AS Sort, 'Alle' AS Name UNION SELECT DISTINCT PZ_IsAltlasten AS ID, PZ_IsAltlasten AS Sort, 'Ja' AS Name FROM V_AP_BERICHT_SEL_PZ_Altlasten  WHERE PZ_IsAltlasten = 1  ", ParameterStore.IIF(EmbeddedOperators.CompareString(Parameters["Vermessungsbezirk"].Value, "00000000-0000-0000-0000-000000000000", false) == 0, RuntimeHelpers.GetObjectValue(ParameterStore.IIF(EmbeddedOperators.CompareString(Parameters["Kreis"].Value, "00000000-0000-0000-0000-000000000000", false) == 0, RuntimeHelpers.GetObjectValue(ParameterStore.IIF(EmbeddedOperators.CompareString(Parameters["Gemeinde"].Value, "00000000-0000-0000-0000-000000000000", false) == 0, "", "AND GM_ApertureID = '" + Parameters["Gemeinde"].Value + "' ")), "AND KS_ApertureID = '" + Parameters["Kreis"].Value + "' ")), "AND VB_ApertureID = '" + Parameters["Vermessungsbezirk"].Value + "' ")), "UNION "), "SELECT DISTINCT PZ_IsAltlasten AS ID, PZ_IsAltlasten AS Sort, 'Nein' AS Name "), "FROM V_AP_BERICHT_SEL_PZ_Altlasten "), "WHERE PZ_IsAltlasten = 0 "), ParameterStore.IIF(EmbeddedOperators.CompareString(Parameters["Vermessungsbezirk"].Value, "00000000-0000-0000-0000-000000000000", false) == 0, RuntimeHelpers.GetObjectValue(ParameterStore.IIF(EmbeddedOperators.CompareString(Parameters["Kreis"].Value, "00000000-0000-0000-0000-000000000000", false) == 0, RuntimeHelpers.GetObjectValue(ParameterStore.IIF(EmbeddedOperators.CompareString(Parameters["Gemeinde"].Value, "00000000-0000-0000-0000-000000000000", false) == 0, "", "AND GM_ApertureID = '" + Parameters["Gemeinde"].Value + "' ")), "AND KS_ApertureID = '" + Parameters["Kreis"].Value + "' ")), "AND VB_ApertureID = '" + Parameters["Vermessungsbezirk"].Value + "' ")), "ORDER BY Sort DESC "));
        }


    }


}
