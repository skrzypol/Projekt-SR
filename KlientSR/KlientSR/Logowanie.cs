using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SimpleConection;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
namespace KlientSR
{
    public partial class Logowanie : Form
    {
        public Logowanie()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }
        public Wiadomosci mm = null;
        public bool zatw = false; 
        public string login;
        private void button2_Click(object sender, EventArgs e)
        { HttpChannel d = new HttpChannel();
            try
            {
               
                ChannelServices.RegisterChannel(d);



                mm = (Wiadomosci)Activator.GetObject(typeof(SimpleConection.Wiadomosci),
                    "http://" + textBox3.Text + ":" + (textBox8.Text.Length == 0 ? "3000" : textBox8.Text) + "/Polaczenie");
                login = textBox1.Text;
                string haslo = maskedTextBox1.Text;
                if (mm.logowanie(login, haslo))
                {
                    zatw = true;
                    DialogResult = DialogResult.OK;
                }
                else MessageBox.Show("błedny login lub haslo");
                //backgroundWorker1.RunWorkerAsync();
            }
            catch {
                MessageBox.Show("błedny adres lub port serwera");
                ChannelServices.UnregisterChannel(d);
            }
        }

    }
}
