using System.Text.Json.Serialization;

namespace HospitalMgrSystem.Model
{
    public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("expiration")]
        public int Expiration { get; set; }
    }
}
