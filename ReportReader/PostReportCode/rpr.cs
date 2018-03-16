
namespace ReportReader
{
    
    public class ReportParameter
    {
        public string Key;

        public string Label;

        public string Value;

        public ReportParameter(string key, string lab, string val)
        {
            this.Key = key;
            this.Label = lab;
            this.Value = val;
        }

        public ReportParameter(string key, string val) : this(key, val, val)
        {
        }
    }

}
