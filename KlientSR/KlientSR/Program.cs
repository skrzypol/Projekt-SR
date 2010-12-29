using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KlientSR
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            Logowanie logowania = new Logowanie();
            
            if (logowania.ShowDialog() == DialogResult.OK)
            {
                
                Application.Run(new Form1(logowania.login,logowania.mm));
            }
        }
    }
}
