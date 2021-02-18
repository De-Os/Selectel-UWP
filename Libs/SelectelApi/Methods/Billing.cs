using RestSharp;
using Selectel.Libs.APi.Responses;
using System;
using System.Collections.Generic;

namespace Selectel.Libs.Api.Methods
{
    public class Billing
    {
        private readonly SelectelApi _api;

        internal Billing(SelectelApi api) => this._api = api;

        public TransactionsMethod Transactions => new TransactionsMethod(_api);
        public BalanceMethod Balance => new BalanceMethod(_api);

        public class TransactionsMethod
        {
            private readonly SelectelApi _api;

            internal TransactionsMethod(SelectelApi api) => this._api = api;

            public List<BillingResponse.BillingInfo> Get(int limit = Int16.MaxValue, int offset = 0, string balances = "main,vk_rub,bonus")
            {
                var request = new RestRequest("v2/billing/transactions");
                request.AddOrUpdateParameter("balances", balances);
                request.AddOrUpdateParameter("created_from", new DateTime(1970, 01, 01, 00, 00, 00).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
                request.AddOrUpdateParameter("created_to", DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
                request.AddOrUpdateParameter("limit", limit);
                request.AddOrUpdateParameter("offset", offset);
                return this._api.Call<List<BillingResponse.BillingInfo>>(request);
            }
        }

        public class BalanceMethod
        {
            private readonly SelectelApi _api;

            internal BalanceMethod(SelectelApi api) => this._api = api;

            public BillingResponse.Balance Get() => this._api.Call<BillingResponse.Balance>(new RestRequest("v3/billing/balance"));
        }
    }
}