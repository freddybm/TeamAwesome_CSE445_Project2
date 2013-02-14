using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSE445_Proj1_TeamAwesome{

    class OrderClass{
        public OrderClass(int cardNo, int amount, string senderId){
            this.cardNo = cardNo;
            this.amount = amount;
            this.senderId = senderId;
        }
    
        public string senderId {get; private set;}
        public int cardNo {get; private set;}
        public int amount {get; private set;}

        //Mutators with lock monitors to prevent multiple entry
        private void setCardNo(){
            lock (typeof (OrderClass)){
                this.cardNo = cardNo;
            }
        }
        private void setID(string senderId){
            lock (typeof (OrderClass)){
                senderId = this.senderId;
            }
        }
        private void setAmt(){
            lock (typeof (OrderClass)){
                amount = this.amount;
            }
        }
        
        //Accessors with lock monitors to prevent multiple entry
        public int getCardNo(){
            lock (typeof (OrderClass)){
                return amount;
            }
        }
        public string getID(){
            lock (typeof (OrderClass)){
                return senderId;
            }
        }
        public int getAmt(){
            lock (typeof (OrderClass)){
                return amount;
            }
        }
    }
}
