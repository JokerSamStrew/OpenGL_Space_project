using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kursach_Cosmos2._0.Classes;


namespace Kursach_Cosmos2._0
{
    static class Program
    {
 
        static void Main()
        {
            ProjectWindow projectWindow = new ProjectWindow();
            projectWindow.Run(60);
        }
    }
}
