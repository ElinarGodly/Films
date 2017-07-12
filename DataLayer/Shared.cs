using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDataLayer
{
    public class Shared
    {
        public Dictionary<string, int> headerDict(string[] headers)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int i = 0; i < headers.Length; i++)
                dict.Add(headers[i], i);
            return dict;
        }

        public Dictionary<string, int> headerDict(DataColumnCollection columns)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int i = 0; i < columns.Count; i++)
                dict.Add(columns[i].ColumnName, i);
            return dict;
        }
    }
}
