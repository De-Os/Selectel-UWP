using Newtonsoft.Json;

namespace Selectel.Libs.Api.Responses
{
    public class BasicResponse<Type>
    {
        [JsonProperty("execution_time")]
        public double ExecutionTime { get; set; }

        [JsonProperty("item_count")]
        public int ItemCount { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("task_id")]
        public string task_id { get; set; }

        [JsonProperty("result")]
        public Type Result { get; set; }

        [JsonProperty("data")]
        public Type Data { get; set; }
    }
}