using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleExplorer
{
    public class BuiltinHtmlContents
    {
        public static string Start
        {
            get
            {
                string htmls = "<!DOCTYPE html>" + "\n";
                htmls += @"
<html>
    <head>
    
    </head>
    <body>
Hello
    </body>
</html>
";
                return htmls;
            }
        }
    }
}
