using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSE445_Proj1_TeamAwesome
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private String Encoder(object o)
        {
            String order = o.ToString(); // Converts OrderObject into a string
            String eMessage;
            EncryptionService.ServiceClient Encoder = new EncryptionService.ServiceClient();
            eMessage = Encoder.Encrypt(order); // Encodes the string
            return eMessage;  
        }

        private Object Decoder(string e)
        {
            Object o;
            EncryptionService.ServiceClient Decoder = new EncryptionService.ServiceClient();
            Decoder.Decrypt(e);  // Decodes the string
            o = e;  // Converts Decrypted string back into OrderObject
            return o;
        }
    }
}
