using Newtonsoft.Json;
using RestSharp;
using Selectel.Libs.Api.Methods;
using Selectel.Libs.Api.Responses;
using System.Collections.Generic;

namespace Selectel.Libs.Api
{
    public class SelectelApi
    {
        public enum RequestType
        {
            GET,
            POST,
            PUT
        }

        private readonly RestClient _http = new RestClient("https://api.selectel.ru/");
        private readonly string _token;

        public SelectelApi(string token)
        {
            this._token = token;
        }

        public ResponseType Call<ResponseType>(RestRequest request, RequestType type = RequestType.GET)
        {
            dynamic result = this.Call(request, type);
            System.Diagnostics.Debug.WriteLine($"{request.Resource}: \n\n${result}");
            result = JsonConvert.DeserializeObject<BasicResponse<ResponseType>>(result);
            if (result.Status != "SUCCESS" && result.Status != "success")
            {
                throw new System.Exception(result.Status);
            }
            return result.Data ?? result.Result;
        }

        private string Call(RestRequest request, RequestType type)
        {
            request.AddHeaders(new Dictionary<string, string> {
                {"X-token", this._token }
            });
            switch (type)
            {
                case RequestType.POST:
                    return this._http.Post(request).Content;

                case RequestType.PUT:
                    return this._http.Put(request).Content;

                default:
                    return this._http.Get(request).Content;
            }
        }

        public Servers Servers => new Servers(this);
        public Billing Billing => new Billing(this);
    }
}