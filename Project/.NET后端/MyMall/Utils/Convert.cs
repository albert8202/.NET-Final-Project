using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Convert
    {
        public static string CategoryToString(List<string> categories)
        {
            StringBuilder res = new StringBuilder();
            foreach(string category in categories)
            {
                res.Append(category + " ");
            }
            return res.ToString();
        }
    }
}
