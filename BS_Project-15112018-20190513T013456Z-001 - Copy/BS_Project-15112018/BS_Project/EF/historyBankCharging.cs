namespace BS_Project.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class historyBankCharging
    {
        public int Id { get; set; }

        public string fullname { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string transaction_info { get; set; }

        public string order_code { get; set; }

        public int? price { get; set; }

        public string payment_id { get; set; }

        public string payment_type { get; set; }

        public string error_text { get; set; }

        public string secure_code { get; set; }

        public DateTime date_trans { get; set; }
    }
}
