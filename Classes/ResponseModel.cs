

using Newtonsoft.Json;

namespace Pt_For_Me.Classes
{
    public class ResponseModel<T>
    {
        public ResponseModel()
        {
            IsSuccess = true;
            Message = "";
        }

        [JsonProperty("IsSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
