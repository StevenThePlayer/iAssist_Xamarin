using System;
using System.Collections.Generic;
using System.Text;

namespace iAssist_Xamarin.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public string tasktitle { get; set; }
        public string BidAmount { get; set; }
        public string TotalAmount { get; set; }
        public string Commission { get; set; }
        public string Payer { get; set; }
        public string Reciever { get; set; }
        public DateTime Created_At { get; set; }
    }
}
