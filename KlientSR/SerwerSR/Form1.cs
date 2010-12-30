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
      
        public Form1()
        {
            InitializeComponent();
        }

      


        Wiadomosci mm = null;
        HttpChannel tempChannel = null;
        
      

        Thread abcd;
        Thread _odbieranie;
        Thread _aktywni;


        private void odbieranie()
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
            try
            {
                mm.rozlaczServer();
            }
            catch { }

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
                   
                    int dlPotokuServera2 = (int)Math.Ceiling(Math.Log10(Double.Parse(textBox4.Text)+1));
                    int dl__ = (dlPotokuServera > dlPotokuServera2 ? dlPotokuServera : dlPotokuServera2);
                    int dlPotoku = (dlPotokuMax < dl__  ? dlPotokuMax : dl__);
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
                    for (int i = 0; i < bloki.Length; i++)
                    {
                        if (bloki[i] != null)
                            for (int j = 0; j < bloki[i].elementy.Count; j++)
                            {
                                dane2 += (string)bloki[i].elementy[j] + " ";
                            }
                    }

                    //wyslac i usunac 
                    mm.nadajWiadomosc("posortowano", "Server", (string)dokogo[doposortowania.Count - 1], dane2, 0, 0);
                    dokogo.RemoveAt(doposortowania.Count - 1);
                    doposortowania.RemoveAt(doposortowania.Count - 1);
                }
            }
        }

        private void lista_aktywnych()
        {
            while (true)
            {

                ArrayList uzytkownicy = mm.AktywniUrzytk();
                string dane = "------------ Aktywni ------------\n";
                for (int i = 0; i < uzytkownicy.Count; i++)
                    dane += (String)uzytkownicy[i] + "\n";

                string dane2 = uzytkownicy.Count.ToString();
                dane += "\n---------- Niezajeci -------------\n";
                uzytkownicy = mm.NieZajeciUrzytk();
                for (int i = 0; i < uzytkownicy.Count; i++)
                    dane += (String)uzytkownicy[i] + "\n";
                wyswietlUzytk(dane);
                wyswietlUzytk2(dane2);

            }
        }

        public void wyswietlUzytk(String tekst)
        {

            if (richTextBox3.InvokeRequired)
                richTextBox3.Invoke(new Action<string>(wyswietlUzytk), tekst);
            else
            {
                
                richTextBox3.Text = tekst;
            }
        }
        public void wyswietlUzytk2(String tekst)
        {

            if (textBox4.InvokeRequired)
                textBox4.Invoke(new Action<string>(wyswietlUzytk2), tekst);
            else
            {

                textBox4.Text = tekst;
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


        private void button6_Click_1(object sender, EventArgs e)
        {

            tempChannel = new HttpChannel(textBox3.Text.Length == 0 ? 3000 : int.Parse(textBox3.Text));
            ChannelServices.RegisterChannel(tempChannel);



            Type ServerType = typeof(SimpleConection.Wiadomosci);
            RemotingConfiguration.RegisterWellKnownServiceType(ServerType,
                                "Polaczenie",
                                WellKnownObjectMode.Singleton);
            

            mm = (Wiadomosci)Activator.GetObject(typeof(SimpleConection.Wiadomosci),
                                                "http://localhost:" + (textBox3.Text.Length == 0 ? "3000" : textBox3.Text) + "/Polaczenie");
           
            abcd = new Thread(new ThreadStart(poszukuj));
            abcd.Start();
            _aktywni = new Thread(new ThreadStart(lista_aktywnych));
            _aktywni.Start();
            _odbieranie = new Thread(new ThreadStart(odbieranie));
            _odbieranie.Start();
            button7.Enabled = true;
            button6.Enabled = false;
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            textBox4.Clear();
            richTextBox3.Clear();
            
            abcd.Abort();
            _odbieranie.Abort();
            _aktywni.Abort();
            mm.rozlaczServer();
            ChannelServices.UnregisterChannel(tempChannel);
            button6.Enabled = true;
            button7.Enabled = false;
        }
    }
}
