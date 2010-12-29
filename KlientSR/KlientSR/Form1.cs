using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

using SimpleConection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

namespace KlientSR
{
    public partial class Form1 : Form
    {
        string plik = "";
        Wiadomosci mm;
        public Form1(String Login,Wiadomosci polaczenie)
        {
            InitializeComponent();
            mm = polaczenie;
            login = Login;
            backgroundWorker1.RunWorkerAsync();
            
        }
        //---mechanizm sprawdzania liczbe/litera - usuwa litery
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                DialogResult odp = czyZapisac();
                if (odp == DialogResult.Cancel)
                    return;
                plik = "";
                richTextBox1.Clear();
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Plik tekstowy (*.txt)|*.txt";
            dialog.Multiselect = false;
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                plik = dialog.FileName;
                StreamReader f = new StreamReader(plik);
                richTextBox1.Text = f.ReadToEnd();
                f.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Plik tekstowy (*.txt)|*.txt";
            dialog.ShowDialog();
            if(dialog.FileName != "")
            {
                plik = dialog.FileName;
                StreamWriter f = new StreamWriter(plik);
                f.Write(richTextBox1.Text);
                f.Close();
            }
        }

        private DialogResult czyZapisac()
        {
            DialogResult odp = MessageBox.Show("Chcesz zapisać zmiany?", "Notatnik", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (odp == DialogResult.Yes)
                button1_Click(null, null);
            return odp;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                mm.rozlaczClient(login);
            }
            catch { }
            if (richTextBox2.Text != "")
            {
                DialogResult odp = czyZapisac();
                if (odp == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }
        string liczbyO = "";
        ArrayList posortuj = null;
        private void button4_Click(object sender, EventArgs e)
        {
            
            liczbyO = "";
            Random random = new Random();
            int imax = int.Parse(textBox4.Text);
            int jmax = int.Parse(textBox1.Text);
            //int tmp2 = int.Parse(random.Next(100/*int.MaxValue*/ ).ToString());
            for (int i = 0; i < imax; i++)
            {
                for (int j = 0; j < jmax; j++)
                {
                    liczbyO+= random.Next(10).ToString();
                }
                
    
                    liczbyO += " ";
            }
            richTextBox1.Text = liczbyO;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Plik tekstowy (*.txt)|*.txt";
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                plik = dialog.FileName;
                StreamWriter f = new StreamWriter(plik);
                f.Write(richTextBox1.Text);
                f.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button4.Enabled = true;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            button5.Enabled = true;
        }
        string login = "dawid";
        private void button3_Click(object sender, EventArgs e)
        {
                string odbiorca = "Server";
                string liczby = richTextBox1.Text;
                wyswietlLog("OD: " + login + "\tDO: " + odbiorca + "\nposortuj:\n" + liczby+"\n\n");
                mm.nadajWiadomosc("posortuj", login, odbiorca, liczby, 0,0);
        }
         
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bufor s;
            string dane;
            ArrayList Posortuj;
            while (true)
            {

                if (mm != null)
                {
                    s = mm.OdbierzWiadomosc(login);
                    if (s != null)
                    {
                        Posortuj = getLista(s.Posortuj);
                        dane = "OD: " + s.Nadawca + "\tDO: " + s.Odbiorca + "\n" + s.Wiadomosc + "\n"+s.Posortuj+"\n\n";
                        
                        wyswietlLog(dane );
                        if (s.Wiadomosc == "sortuj" && s.Posortuj != null)
                        {
                            ArrayList a = Posortuj;
                            a.Sort();
                            //s.Posortuj.Sort();
                            
                            mm.nadajWiadomosc("posortowano", s.Odbiorca, "Server", getString(a), 0, s.blok);
                        }
                        if (s.Wiadomosc == "posortowano" && s.Posortuj != null)
                        {
                            mm.nadajWiadomosc("odebrano", s.Odbiorca, "Server", "", 0, 0);
                        }
                    }
                }
            }
        }


        public void wyswietlLog(String tekst)
        {

            if (richTextBox3.InvokeRequired)
                richTextBox3.Invoke(new Action<string>(wyswietlLog), tekst);
            else
            {
                richTextBox3.Focus();
                richTextBox3.AppendText(tekst);
                richTextBox3.ScrollToCaret();
            }
        }


        ArrayList getLista(string dane)
        {
            ArrayList lista = new ArrayList();
            if (dane != null)
                if (dane.Length > 0)
                {
                    MatchCollection matches = Regex.Matches(dane, @"([0-9]+)");

                    foreach (Match match in matches)
                    {

                        lista.Add((string)match.Value);
                    }
                    return lista;

                }
            return null;
        }
        string getString(ArrayList dane)
        {
            string lista = "";
            if(dane!=null)
            if (dane.Count > 0)
            {
                for (int i = 0; i < dane.Count; i++)
                {
                    lista += (string)dane[i] + " ";
                }
                return lista;
            }
            return "";
        }

        private void button6_Click(object sender, EventArgs e)
        {

            
        }


    }
}
