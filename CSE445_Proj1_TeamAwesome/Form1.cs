using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

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

    public delegate void priceCutEvent(Int32 pr); //define a delegate

    class HotelSupplier
    {
        public Int32 p = 0; // counts price cuts
        public static event priceCutEvent priceCut;  // Link event to delegate
        private static Int32 roomPrice = 150;
        public Int32 getPrice() { return roomPrice; }

        public static void changePrice(Int32 price)  // uses the pricing model to change price if necessary
        {
            if (price < roomPrice)
            { //a price cut
                if (priceCut != null) // there is at least one subscriber
                    priceCut(price); // emit event to subscribers
            }
            roomPrice = price;
        }

        public void receiveOrder(){

            List<String> encryptedOrderList = new List<String>();
            List<String> orderList = new List<String>();

            encryptedOrderList.Add(MultiCellBuffer());  // Place Orders into list

            for(int i = 0; i < encryptedOrderList.Count; i++) // Loop through List with for
	        {
                orderList.Add(Decoder(encryptedOrderList[i]));
	        }
        }

        public void orderProcess()
        {
            OrderProcessing op = new OrderProcessing();
            Thread[] order = new Thread[5];
                
            for (int i = 0; i < 3; i++)
            { //N=3 here/ start n retailer threads
                order[i] = new Thread(new ThreadStart(op.currentPriceFunc));
                order[i].Start();
            }
        }

        public void timeStamp()
        {
            DateTime CurrentDate;
            CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
        }

    }
}
