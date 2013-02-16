using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace CSE445_Proj1_TeamAwesome
{
    

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        public delegate void priceCutEvent(Int32 pr); //define a delegate

        public class HotelSupplier
        {

            //This is code modified from the Chicken Farm tutorial. It can be used or changed by anyone who needs it.
            static Random rng = new Random(); // to generate randome numbers
            public static event priceCutEvent priceCut; // Link event to delegate
            private static Int32 roomPrice = 150;
            public Int32 getPrice() { return roomPrice; }

            public static void changePrice(Int32 price)
            {
                if (price < roomPrice)
                { //a price cut
                    if (priceCut != null) // there is at least one subscriber
                        priceCut(price); // emit event to subscribers
                }
                roomPrice = price;
            }

            public void supplierFunc()
            {
                for (Int32 i = 0; i < 50; i++)
                {
                    Thread.Sleep(500);
                    //Take the order from the queue of the orders;
                    //Decide the price based on the orders
                    Int32 p = rng.Next(75, 150);
                    //Console.WriteLine("New Price is {0}", p);
                    HotelSupplier.changePrice(p);
                }
            }

            //This is to cover number 4; to interact with the program we will need to make the call to
            //the method that purchases rooms, and other additions may be needed.
            //
            //Currently it is set to purchase rooms if the price is < $100 per room and the stock of available
            //rooms is less than 15, or if it is < 10 it will purchase rooms no matter the price.
            public class Agency
            {
                private int roomNum = 0;
                private DateTime sent;
                private DateTime received;
                private DateTime total;
                public void agencyFunc()
                {
                    HotelSupplier room = new HotelSupplier();
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Thread.Sleep(1000);
                        Int32 p = room.getPrice();
                        Console.WriteLine("Hotel {0} has everyday low price: ${1} per room.", Thread.CurrentThread.Name, p);
                    }
                }
                public void roomOnSale(Int32 p)
                { //Event handler
                    if (roomPrice < 100 && roomNum < 15)
                    {
                        roomNum = roomNum + 10;
                        Console.WriteLine("Agent {0} buys 10 rooms at ${1} per room. Currently at {2} rooms.", Thread.CurrentThread.Name, p, roomNum);
                        //send to order class
                        //after receiving encoded string from order class, record time sent and send to multicell buffer
                        sent = DateTime.Now;
                        //send to multicellbuffer; calculate time when order confirmation is received.
                        received = DateTime.Now;
                        total = received - sent; //something is wrong here--I think we need to cast it as a timespan.

                    }
                    if (roomNum < 10)
                    {
                        roomNum = roomNum + 5;
                        Console.WriteLine("Agent {0} buys 5 rooms at ${1} per room. Currently at {2} rooms.", Thread.CurrentThread.Name, p, roomNum);
                        //send to order class
                        //after receiving encoded string from order class, record time sent and send to multicell buffer
                        sent = DateTime.Now;
                        //send to multicellbuffer; calculate time when order confirmation is received.
                        received = DateTime.Now;
                        total = received - sent; //something is wrong here--I think we need to cast it as a timespan.
                    }
                    Console.WriteLine("Hotel {0} rooms are on sale: as low as ${1} per room.", Thread.CurrentThread.Name, p);
                }
            }



            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
                HotelSupplier room = new HotelSupplier();
                Thread supplier = new Thread(new ThreadStart(room.supplierFunc));
                supplier.Start(); //start one famer thread
                Agency agent = new Agency();
                HotelSupplier.priceCut += new priceCutEvent(agent.roomOnSale);
                Thread[] agents = new Thread[5];
                for (int i = 0; i < 3; i++)
                { //N=3 here/ start n retailer threads
                    agents[i] = new Thread(new ThreadStart(agent.agencyFunc));
                    agents[i].Name = (i + 1).ToString();
                    agents[i].Start();
                }

            }
        }
    }
}
