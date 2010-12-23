using System;

using System.Collections;
using System.Collections.Generic;

using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

using SimpleConection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

namespace SerwerSR
{
    public partial class Form1 : Form
    {
        string plik = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button4.Enabled = true;
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

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            button7.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button6.Enabled = true;
            button7.Enabled = false;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            try
            {
                int pozycja = richTextBox1.SelectionStart;
                string input = richTextBox1.Text;
                Match match = Regex.Match(input, @"^([0-9\n]*)$");
                if (!match.Success)
                {
                    richTextBox1.Text = input.Remove(pozycja - 1, 1);
                    richTextBox1.SelectionStart = pozycja - 1;
                }
            }
            catch { }
        }

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
            if (dialog.FileName != "")
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
            if (richTextBox2.Text != "")
            {
                DialogResult odp = czyZapisac();
                if (odp == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            button5.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string liczby = "";
            richTextBox1.Clear();
            Random random = new Random();
            int tmp2 = int.Parse(random.Next(100/*int.MaxValue*/ ).ToString());
            for (int i = 0; i < tmp2; i++)
            {
                for (int j = 0; j < int.Parse(textBox1.Text); j++)
                {
                    liczby += random.Next(9).ToString();
                }
                liczby += "\n";
            }
            richTextBox1.Text = liczby;

        }
        Wiadomosci mm = null;
        HttpChannel tempChannel = null;
        private void button8_Click(object sender, EventArgs e)
        {

            //HttpChannel c = new HttpChannel(3000);
            // rozlaczenie polaczenia ubieglego
            if (tempChannel != null)
            {
                mm.rozlaczServer();
                backgroundWorker1.CancelAsync();
                backgroundWorker3.CancelAsync();
                ChannelServices.UnregisterChannel(tempChannel);
            }
            //RemotingConfiguration.
            //HttpChannel c = new HttpChannel(textBox3.Text.Length <= 0 ? 3000 : int.Parse(textBox3.Text));
            tempChannel = new HttpChannel(3000);
            ChannelServices.RegisterChannel(tempChannel);


           
            Type ServerType = typeof(SimpleConection.Wiadomosci);
            RemotingConfiguration.RegisterWellKnownServiceType(ServerType,
                                "Polaczenie",
                                WellKnownObjectMode.Singleton);
            //RemotingConfiguration.RegisterWellKnownClientType(ServerType, "http://localhost:3000/Polaczenie");

            //---------------------------------------------------------------------------------
            string myID = "Server";

            mm = (Wiadomosci)Activator.GetObject(typeof(SimpleConection.Wiadomosci),
                                                "http://localhost:3000/Polaczenie");
            //HttpChannel d = new HttpChannel();
            //ChannelServices.RegisterChannel(d);
            // (2) Rejestruje typ — obiekt aktywowany przez serwer przez port 3200
            
            // (3) Tworzy instancję zdalnego obiektu
           // mm = new Wiadomosci();
            
            // Pozwala użytkownikom wysyłać i odbierać wiadomości

            
            backgroundWorker1.RunWorkerAsync();
            abcd = new Thread(new ThreadStart(poszukuj));
            abcd.Start();
            //backgroundWorker2.RunWorkerAsync();
            backgroundWorker3.RunWorkerAsync();
        }

        Thread abcd;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {   
            Bufor s = null;
            String dane = "";
            ArrayList Posortuj;
            while (true)
            {
                
                if (mm != null)
                {
                    s = mm.OdbierzWiadomoscSerwer();
                    
                    if (s != null)
                    {
                        Posortuj = getLista(s.Posortuj);
                        dane = "OD: " + s.Nadawca + "\tDO: " + s.Odbiorca + "\n" + s.Wiadomosc + "\n"+ s.Posortuj+"\n\n";
                        wyswietlLog(dane);
                        if (s.Wiadomosc == "posortuj" && Posortuj != null)
                        {
                            doposortowania.Add(Posortuj);
                            dokogo.Add(s.Nadawca);
                            mm.nadajWiadomosc("przyjołem", "Server", s.Nadawca, null, 0,0);
                        }
                        else if (s.Wiadomosc == "posortowano" && Posortuj != null)
                        {
                            bloki[s.blok].elementy = Posortuj;
                            bloki[s.blok].koniec = true;
                        }
                    }

                }
            }
        }

        public void wyswietlLog(String tekst)
        {

            if (richTextBox4.InvokeRequired)
                richTextBox4.Invoke(new Action<string>(wyswietlLog), tekst);
            else
            {
                richTextBox4.Focus();
                richTextBox4.AppendText(tekst);
                richTextBox4.ScrollToCaret();
            }
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            mm.rozlaczServer();
        }

        class Blok{
            public ArrayList elementy = new ArrayList();
            public string kto = "";
            public bool koniec = false;
        }

        ArrayList doposortowania = new ArrayList();
        Blok[] bloki;
        ArrayList dokogo = new ArrayList();
        bool zajety = false;

        

        private void poszukuj()
        {
            while (true)
            {
                if (doposortowania.Count > 0)
                {
                    zajety = true;
                    ArrayList dane = (ArrayList)doposortowania[doposortowania.Count - 1];
                    int dlDanych = dane.Count;
                    int dlPotokuMax = ((string)dane[0]).Length;
                    int dlPotokuServera = int.Parse(textBox5.Text);
                    int dlPotoku = (dlPotokuMax < dlPotokuServera ? dlPotokuMax : dlPotokuServera);
                    int liczbaBlokow = (int)Math.Pow(10, dlPotoku);
                    bloki = new Blok[liczbaBlokow];
                    for (int i = 0; i < liczbaBlokow; i++)
                    {
                        bloki[i] = new Blok();
                    }
                    for (int i = 0; i < dlDanych; i++)
                    {
                        string dana = (string)dane[i];
                        int pol = int.Parse(dana.Substring(0, dlPotoku));
                        bloki[pol].elementy.Add(dana);
                    }

                    /// miejsce na funkcje przydzielania pracy
                    /// 
                    bool koniec = false;
                    ArrayList uz;
                    do
                    {
                        
                        int licz = 0;
                        for (int i = 0; i<bloki.Length;i++ )
                        {
                            if (bloki[i].koniec) licz++;
                            else if (bloki[i].kto == "" || !mm.aktywny(bloki[i].kto)){
                                uz = mm.NieZajeciUrzytk();
                                if (uz.Count > 0)
                                {
                                    mm.nadajWiadomosc("sortuj", "Server", (string)uz[0], getString(bloki[i].elementy), 0, i);
                                    bloki[i].kto = (string)uz[0];
                                }
                            }
                            
                        }
                        if (licz == bloki.Length) koniec = true;
                    }while(!koniec);
                    string dane2 = "";
                    for(int i=0;i<bloki.Length;i++){
                        for(int j=0;j<bloki[i].elementy.Count;j++){
                            dane2 += (string)bloki[i].elementy[j]+" ";
                        }
                    }

                    //wyslac i usunac 
                    mm.nadajWiadomosc("posortowano", "Server", (string)dokogo[doposortowania.Count - 1], dane2, 0, 0);
                    dokogo.RemoveAt(doposortowania.Count - 1);
                    doposortowania.RemoveAt(doposortowania.Count - 1);
                }
            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                ArrayList uzytkownicy = mm.AktywniUrzytk();
                string dane = "------------ Aktywni ------------\n";
                for (int i = 0; i < uzytkownicy.Count; i++)
                    dane += (String)uzytkownicy[i] + "\n";
                

                dane += "\n---------- Niezajeci -------------\n";
                uzytkownicy = mm.NieZajeciUrzytk();
                for (int i = 0; i < uzytkownicy.Count; i++)
                    dane += (String)uzytkownicy[i] + "\n";
                wyswietlUzytk(dane);
            }
        }

        public void wyswietlUzytk(String tekst)
        {

            if (richTextBox3.InvokeRequired)
                richTextBox3.Invoke(new Action<string>(wyswietlUzytk), tekst);
            else
            {
                richTextBox3.Clear();
                richTextBox3.Focus();
                richTextBox3.AppendText(tekst);
                richTextBox3.ScrollToCaret();
            }
        }

        ArrayList getLista(string dane)
        {
            ArrayList lista = new ArrayList();
            if(dane!=null)
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
            if (dane != null)
           if(dane.Count > 0)
           {
                for (int i = 0; i < dane.Count; i++)
                {
                    lista += (string)dane[i] + " ";
                }
                return lista;
            }
            return "";
        }
    }
}
