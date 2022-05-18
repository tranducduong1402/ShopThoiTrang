using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ShopThoiTrang.Library
{
    public class XString
    {
        public static string str_slug(String s)
        {
            String[][] symbols =
            {
                new String [] {"[áàảãạăắặằẵẳâấầậẫả]", "a"},
                new String [] {"[đ]", "d"},
                new String [] {"[éèẻẽẹêềếểệễ]", "e"},
                new String [] {"[íịìỉĩ]", "i"},
                new String [] {"[òóỏõọôồốổỗộơờớởỡợ]", "o"},
                new String [] {"[ủùúũụưừứửữự]", "u"},
                new String [] {"[ýỳỷỹỵ]", "y"},
                new String [] {"[\\s'\";,]", "-"}
            };
            s = s.ToLower();
            foreach (var ss in symbols)
            {
                s = Regex.Replace(s, ss[0], ss[1]);
            }
            return s;
        }
    }
}