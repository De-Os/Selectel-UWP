using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Selectel.Libs.APi.Responses
{
    public class BillingResponse
    {
        public class BillingInfo
        {
            [JsonProperty("user_id")]
            public int UserId;

            [JsonProperty("transaction_type")]
            public string TransactionType;

            [JsonProperty("transaction_group")]
            public string TransactionGroup;

            [JsonProperty("description")]
            public Dictionary<string, string> Description;

            [JsonProperty("balance")]
            public string Balance;

            [JsonProperty("dir")]
            public string Direction;

            [JsonProperty("state")]
            public string State;

            [JsonProperty("created")]
            public DateTime Created;

            [JsonProperty("price")]
            public int Price;
        }

        public class Balance
        {
            [JsonProperty("currency")]
            public string Currency;

            [JsonProperty("is_postpay")]
            public bool IsPostPay;

            [JsonProperty("discount")]
            public int Discount;

            [JsonProperty("primary")]
            public PrimaryBalance Primary;

            [JsonProperty("storage")]
            public OtherBalance Storage;

            [JsonProperty("vpc")]
            public OtherBalance VPC;

            [JsonProperty("vmware")]
            public OtherBalance VMWare;

            public class PrimaryBalance : Balances
            {
                [JsonProperty("ref")]
                public int Ref;

                [JsonProperty("hold")]
                public Balances Hold;
            }

            public class OtherBalance : Balances
            {
                [JsonProperty("debt")]
                public int Debt;

                [JsonProperty("sum")]
                public int Sum;
            }

            public class Balances
            {
                [JsonProperty("main")]
                public int Main;

                [JsonProperty("bonus")]
                public int Bonus;

                [JsonProperty("vk_rub")]
                public int VkRub;
            }
        }
    }
}