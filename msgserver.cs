using System;
using System.Collections;
using System.Reflection;
using System.Threading;
namespace SimpleConection
{
    public class Wiadomosci : MarshalByRefObject
    {
        ArrayList Messages = new ArrayList();
        ArrayList Urzytkownicy = new ArrayList();
        public Wiadomosci()
        {

            Urzytkownicy.Add(new Uzytkownik("dawid","lala"));
            Urzytkownicy.Add(new Uzytkownik("konar", "lala"));
            Urzytkownicy.Add(new Uzytkownik("ripper", "lala"));
            Urzytkownicy.Add(new Uzytkownik("admin", "lala"));
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


        // Metoda ��czy wszystkie wiadomo�ci i zwraca klientowi zawieraj�cy je ci�g znak�w
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
                    
                    Messages.RemoveAt(i); // Usuwanie wiadomo�ci
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
                    return bufor;// Usuwanie wiadomo�ci
                }
            }
            return null;
        }

        public void rozlaczServer()
        {
            Uzytkownik a;
            for (int i = 0; i < Urzytkownicy.Count; i++)
            {
                a = (Uzytkownik)Urzytkownicy[i];
                if (a.Aktywny)
                {
                    nadajWiadomosc("wylaczono", "Server", a.Login, null, 0,0);
                }
            }
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

        // Metoda przyjmuje wiadomo�� od klienta i zapisuje j� w pami�ci
        public void nadajWiadomosc(string wiadomosc, string nadawca,
           string odbiorca,String posortuj,int id_alg,int blok)
        {
            // Zapisuje wiadomo�� otrzyman� od klienta jako obiekt w tablicy
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

            Messages.Add(bufor);     // Dodaje wiadomo�� do tablicy
        }
    }
    // Wiadomo�ci s� przechowywane w obiektach klasy Bufor
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