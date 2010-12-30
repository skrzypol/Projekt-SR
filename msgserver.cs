using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.IO;
using System.Text;
using System.Windows.Forms;



namespace SimpleConection
{
    public class Wiadomosci : MarshalByRefObject
    {
        ArrayList Messages = new ArrayList();
        ArrayList Urzytkownicy = new ArrayList();
        public Wiadomosci()
        {

            twListyUz();
        }


        void twListyUz()
        {
            try
            {
                string login = "";
                string haslo = "";
                XmlTextReader textReader = new XmlTextReader("uzytkownicy.xml");
                string sekwencja = "";

                while (textReader.Read())
                {

                    XmlNodeType nType = textReader.NodeType;
                    if (nType == XmlNodeType.Text)
                    {
                        if (sekwencja.ToLower() == "login")
                            login = textReader.Value.ToString();
                        if (sekwencja.ToLower() == "haslo")
                        {
                            haslo = textReader.Value.ToString();
                            Urzytkownicy.Add(new Uzytkownik(login, haslo));
                        }
                    }

                    // if node type is an element

                    if (nType == XmlNodeType.Element)
                    {
                        sekwencja = textReader.Name.ToString();
                    }



                }
            }
            catch { }
           
        }

        //uwerzytelnienie
        public bool logowanie(String login,String haslo)
        {
            Uzytkownik a;
            for (int i = 0;i<Urzytkownicy.Count ; i++)
            {
                a = (Uzytkownik)Urzytkownicy[i];
                if (a.Login == login)
                {
                    if (a.Haslo == haslo || !a.Aktywny)
                    {
                        a.Aktywny = true;
                        nadajWiadomosc("zalogowano", login, "Server", null, 0, 0);
                        return true;
                    }
                    else
                        return false;
                }
            }

            return false;
        }

        public bool rejestracja(String login, String haslo)
        {
            Uzytkownik a;
            for (int i = 0; i < Urzytkownicy.Count; i++)
            {
                a = (Uzytkownik)Urzytkownicy[i];
                if (a.Login == login)
                {
                    return false;
                }
            }
            
            Urzytkownicy.Add(new Uzytkownik(login, haslo));
            
            return true;
        }

        // Metoda ³¹czy wszystkie wiadomoœci i zwraca klientowi zawieraj¹cy je ci¹g znaków
        public bool aktywny(String odbiorca)
        {
            Uzytkownik a;
            for (int i = 0; i < Urzytkownicy.Count; i++)
            {
                a = (Uzytkownik)Urzytkownicy[i];
                if (a.Login == odbiorca)
                {
                    if (a.Aktywny)
                    {
                        
                        return true;
                    }
                    else
                        return false;
                }
            }
            return false;
        }


        public bool zajety(String odbiorca)
        {
            Uzytkownik a;
            for (int i = 0; i < Urzytkownicy.Count; i++)
            {
                a = (Uzytkownik)Urzytkownicy[i];
                if (a.Login == odbiorca)
                {
                    if (a.Zajety)
                    {

                        return true;
                    }
                    else
                        return false;
                }
            }
            return false;
        }

        void aktywuj(String odbiorca)
        {
            Uzytkownik a;
            for (int i = 0; i < Urzytkownicy.Count; i++)
            {
                a = (Uzytkownik)Urzytkownicy[i];
                if (a.Login == odbiorca)
                {
                    ((Uzytkownik)Urzytkownicy[i]).Zajety = true;
                    return;
                }
            }
        }

        void deaktywuj(String odbiorca)
        {
            Uzytkownik a;
            for (int i = 0; i < Urzytkownicy.Count; i++)
            {
                a = (Uzytkownik)Urzytkownicy[i];
                if (a.Login == odbiorca)
                {
                    ((Uzytkownik)Urzytkownicy[i]).Zajety = false;
                    return;
                }
            }
        }

        public ArrayList NieZajeciUrzytk()
        {
            ArrayList lista = new ArrayList();
            Uzytkownik a;
            for (int i = 0; i < Urzytkownicy.Count; i++)
            {
                a = (Uzytkownik)Urzytkownicy[i];
                if (a.Aktywny && !a.Zajety)
                    lista.Add(a.Login);
            }
            return lista;
        }

        public ArrayList AktywniUrzytk()
        {
            ArrayList lista = new ArrayList();
            Uzytkownik a; 
            for (int i = 0; i < Urzytkownicy.Count; i++)
            {
                a = (Uzytkownik)Urzytkownicy[i];
                if (a.Aktywny)
                    lista.Add(a.Login);
            }
            return lista;
        }

        

        public Bufor OdbierzWiadomosc(string odbiorca)
        {
            
            if (!aktywny(odbiorca)) return null;
            Bufor bufor; 
            for (int i = 0; i < Messages.Count; i++)
            {
                bufor = (Bufor)Messages[i];
                if (bufor.Odbiorca == odbiorca)
                {
                    
                    Messages.RemoveAt(i); // Usuwanie wiadomoœci
                    return bufor;
                }
            }
            return null;
        }

        public Bufor OdbierzWiadomoscSerwer()
        {
            Bufor bufor;
            for (int i =  0; i < Messages.Count; i++)
            {
                bufor = (Bufor)Messages[i];
                if (bufor.Odbiorca == "Server")
                {
                    
                    Messages.RemoveAt(i); 
                    return bufor;// Usuwanie wiadomoœci
                }
            }
            return null;
        }

        public void rozlaczServer()
        {
            FileStream fs;
            XmlWriter w;

            fs = new FileStream("uzytkownicy.xml", FileMode.Create);
            w = XmlWriter.Create(fs);
            w.WriteStartDocument();
            w.WriteStartElement("users");




            Uzytkownik a;
            for (int i = 0; i < Urzytkownicy.Count; i++)
            {
                a = (Uzytkownik)Urzytkownicy[i];
                if (a.Aktywny)
                {
                    nadajWiadomosc("wylaczono", "Server", a.Login, null, 0, 0);
                }
                {
                    w.WriteStartElement("user");
                    w.WriteElementString("Login", a.Login);
                    w.WriteElementString("Haslo", a.Haslo);
                    w.WriteEndElement();

                    

                }
            }
            w.WriteEndElement();
            w.WriteEndDocument();
            w.Flush();
            fs.Close();
            Thread.Sleep(1000);
            

        }

        public void rozlaczClient(string nadawca)
        {
            nadajWiadomosc("wylaczono", nadawca, "Server", null, 0,0);
            for (int i = 0; i < Urzytkownicy.Count; i++)
            {
                if (((Uzytkownik)Urzytkownicy[i]).Login == nadawca)
                    ((Uzytkownik)Urzytkownicy[i]).Aktywny = false;
                
            }
        }

        // Metoda przyjmuje wiadomoœæ od klienta i zapisuje j¹ w pamiêci
        public void nadajWiadomosc(string wiadomosc, string nadawca,
           string odbiorca,String posortuj,int id_alg,int blok)
        {
            // Zapisuje wiadomoœæ otrzyman¹ od klienta jako obiekt w tablicy
            Bufor bufor = new Bufor();
            bufor.Wiadomosc = wiadomosc;
            bufor.Nadawca = nadawca;
            bufor.Odbiorca = odbiorca;
            bufor.Posortuj = posortuj;
            bufor.Id_alg = id_alg;
            bufor.blok = blok;
            if (wiadomosc == "posortowano")
            {
                deaktywuj(nadawca);
                if(nadawca == "Server")
                    aktywuj(odbiorca);
            }
            else if (wiadomosc == "sortuj")
                aktywuj(odbiorca);
            else if (wiadomosc == "odebrano")
                deaktywuj(nadawca);

            Messages.Add(bufor);     // Dodaje wiadomoœæ do tablicy
        }
    }
    // Wiadomoœci s¹ przechowywane w obiektach klasy Bufor
    public class Bufor : MarshalByRefObject
    {
        public string Wiadomosc;
        public string Nadawca;
        public string Odbiorca;
        public string Posortuj;
        public int Id_alg = 0;
        public int blok = 0;
    }

    public class Uzytkownik
    {
        public Uzytkownik(String login, String haslo)
        {
            Login = login;
            Haslo = haslo;
        }
        public string Login;
        public string Haslo;
        public bool Aktywny = false;
        public bool Zajety = false;
        
    }
}