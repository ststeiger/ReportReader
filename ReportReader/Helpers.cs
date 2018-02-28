
namespace ReportReader
{


    class Helpers
    {


        public static string RemoveSqlComments(string sql)
        {
            sql = RemoveCstyleComments(sql);
            return RemoveSingleLineSqlComments(sql);
        }

        private static string RemoveSingleLineSqlComments(string sql)
        {
            string pattern = @"(?<=^ ([^'""] |['][^']*['] |[""][^""]*[""])*) (--.*$|/\*(.|\n)*?\*/)";
            return System.Text.RegularExpressions.Regex.Replace(sql, pattern, "",
                System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                System.Text.RegularExpressions.RegexOptions.Multiline);
        }

        private static string RemoveSingleLineSqlComments_old(string sql)
        {
             // sql = "--this is a test\r\nselect stuff where substaff like '--this comment should stay' --this should be removed\r\n";
            char[] quotes = { '\'', '"' };
            int newCommentLiteral, lastCommentLiteral = 0;
            while ((newCommentLiteral = sql.IndexOf("--", lastCommentLiteral)) != -1)
            {
                int countQuotes = sql.Substring(lastCommentLiteral, newCommentLiteral - lastCommentLiteral).Split(quotes).Length - 1;
                if (countQuotes % 2 == 0) //this is a comment, since there's an even number of quotes preceding
                {
                    int eol = sql.IndexOf("\r\n") + 2;
                    if (eol == -1)
                        eol = sql.Length; //no more newline, meaning end of the string
                    sql = sql.Remove(newCommentLiteral, eol - newCommentLiteral);
                    lastCommentLiteral = newCommentLiteral;
                }
                else //this is within a string, find string ending and moving to it
                {
                    int singleQuote = sql.IndexOf("'", newCommentLiteral);
                    if (singleQuote == -1)
                        singleQuote = sql.Length;
                    int doubleQuote = sql.IndexOf('"', newCommentLiteral);
                    if (doubleQuote == -1)
                        doubleQuote = sql.Length;

                    lastCommentLiteral = System.Math.Min(singleQuote, doubleQuote) + 1;

                    //instead of finding the end of the string you could simply do += 2 but the program will become slightly slower
                }
            }

            return sql;
        }


        // https://stackoverflow.com/questions/8323445/c-sharp-regex-to-remove-c-style-comments-and-extract-text-between-brackets
        private static string RemoveCstyleComments(string strInput)
        {
            string strPattern = @"/[*][\w\d\s]+[*]/";
            //strPattern = @"/\*.*?\*/"; // Doesn't work
            //strPattern = "/\\*.*?\\*/"; // Doesn't work
            //strPattern = @"/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/ "; // Doesn't work
            //strPattern = @"/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/ "; // Doesn't work

            // http://stackoverflow.com/questions/462843/improving-fixing-a-regex-for-c-style-block-comments
            strPattern = @"/\*(?>(?:(?>[^*]+)|\*(?!/))*)\*/";  // Works !

            string strOutput = System.Text.RegularExpressions.Regex.Replace(strInput, strPattern, string.Empty, System.Text.RegularExpressions.RegexOptions.Multiline);
            return strOutput;
        } // End Function RemoveCstyleComments

    }
}
