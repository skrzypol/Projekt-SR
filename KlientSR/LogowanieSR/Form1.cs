using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LogowanieSR
{
    public partial class Form1 : Form
    {
        bool autoryzacja = true;
        bool rejestracja = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(autoryzacja == true)
            {
                System.Diagnostics.Process.Start(@"E:\STUDIA\SEMSETR VII\SR - projekt\GitHub\Projekt-SR\KlientSR\KlientSR\bin\Debug\KlientSR.exe");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(rejestracja == true)
            {
                MessageBox.Show("Rejestracja przebiegła poprawnie!");
                tabLogowanie.Show();
            }
        }
    }
}
