using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Payment
    {
        [Key]
        public long Payment_Id { get; set; }
        public string Payment_Type { get; set; }
        public string OrderId { get; set; }
        public string OrderDetailId { get; set; }
        public string User_Id { get; set; }
        public string User_Type { get; set; }
        public DateTime Payment_Date { get; set; }
        public string PaymentBy { get; set; }
        
        //Cheque
        public DateTime Cheque_Date { get; set; }
        public string Cheque_Bankname { get; set; }
        public string Cheque_Number { get; set; }
        
        //Debit/Credit
        public string creditORdebitcard_date { get; set; }
        public string Card_Holder_Name { get; set; }
        public string Card_Last4digits { get; set; }
        
        //Bank Transfer
        public string Bank_Transfer_date { get; set; }
        public string Bank_Transfer_Name { get; set; }
        public string Bank_Transaction_ID { get; set; }
        public string Bank_Transfer_IFSCcode { get; set; }
        public string Bank_Transfer_Branchname { get; set; }

        //Wallet
        public string Wallet_Date { get; set; }
        public string Wallet_Number { get; set; }

        //Cash
        public string Cash_Date { get; set; }
        public string Cash_Card_Holdername { get; set; }

        public string Invoiced_Amount { get; set; }
        public string Received_Amount { get; set; }
        public string Opening_Balance { get; set; }
        public string Current_Balance { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
