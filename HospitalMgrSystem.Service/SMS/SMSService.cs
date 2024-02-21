using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HospitalMgrSystem.Model;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using System.Drawing;

namespace HospitalMgrSystem.Service.SMS
{
    public class SMSService : ISMSService
    {
        private readonly HttpClient _httpClient;

        public SMSService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<SMSAPILogin> GetAccessToken()
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.SMSAPILogin _smsApiLogin = new Model.SMSAPILogin();
                // Check for existing valid token
                var existingToken = dbContext.SMSAPILogin.FirstOrDefault(t => t.Token != null && t.ExpirationTime > DateTime.Now);

            if (existingToken != null)
            {
                    _smsApiLogin.Id = existingToken.Id; // Use existing an Id property
                    return existingToken; // Use existing valid token
            }
            else
            {
                    _smsApiLogin.Id = 0; 
             }

            // Token expired or absent, proceed with acquiring a new one
            var request = new
            {
                username = "kumuduhos",
                password = "Kumuduhos@1234567"
            };

            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://e-sms.dialog.lk/api/v1/login", content);

            if (response.IsSuccessStatusCode)
            {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<TokenResponse>(responseContent);
                    if (responseObject != null)
                    {
                        string token = responseObject.Token;
                        string status = responseObject.Status;
                        int expiration = responseObject.Expiration;

                        // Now you can use token, status, and expiration as needed
            
                    if (status == "success")
                    {
                       // Get expiration time(handle potential null / negative values)
                        var expirationSeconds = expiration;
                        if (expirationSeconds > 0)
                        {
                            // Calculate expiration date/time using UTC for consistency
                            var currentDateTime = DateTime.Now;
                            var expirationTimeSpan = TimeSpan.FromSeconds(expirationSeconds);
                            var expirationDateTime = currentDateTime.Add(expirationTimeSpan);

                            _smsApiLogin.Comment = "";
                            _smsApiLogin.Expiration = expiration;
                            _smsApiLogin.ExpirationTime = expirationDateTime;
                            _smsApiLogin.RemainingCount =0;
                            _smsApiLogin.Token = token;
                            _smsApiLogin.CreateDate = DateTime.Now;
                            _smsApiLogin.CreateUser = 1;

                            if (_smsApiLogin.Id != 0)
                            {
                                HospitalMgrSystem.Model.SMSAPILogin result = (from p in dbContext.SMSAPILogin where p.Id == _smsApiLogin.Id select p).SingleOrDefault();
                                result.Token = token;
                                result.Expiration = expiration;
                                result.RemainingCount = 0;
                                result.Comment = ""; 
                                result.ExpirationTime = expirationDateTime;
                                result.CreateDate = _smsApiLogin.CreateDate;
                                result.CreateUser = _smsApiLogin.CreateUser;
                                }
                            else
                            {
                                dbContext.SMSAPILogin.Add(_smsApiLogin);
      
                            }
                            await dbContext.SaveChangesAsync();

                            return _smsApiLogin;
                        }
                        else
                        {
                            Console.WriteLine("Invalid or non-positive expiration received.");
                            return null;
                        }


                    }
                    else
                    {
                        Console.WriteLine($"Login failed with status: {token}");
                        return null;
                    }

                    }
                    else
                    {
                        return null;
                    }
                }
            else
            {
                Console.WriteLine($"HTTP error: {response.StatusCode}");
                return null;
            }

            }
        }





        public async Task<string> SendSMSToken(ChannelingSMS channelingSMS)
        {

            Model.SMSAPILogin _smsApiLogin = new Model.SMSAPILogin();
            Model.SMSCampaign _SMSCampaign = new Model.SMSCampaign();

            using (var httpClient = new HttpClient())
            {
                _smsApiLogin =await GetAccessToken();
                if (_smsApiLogin != null && _smsApiLogin != null)
                {
                    Model.SMSCampaign smsCampaign = new Model.SMSCampaign();
                    _SMSCampaign.CampaignID =0;
                    _SMSCampaign.CampaignCost = 0;
                    _SMSCampaign.duplicateNo = 0;
                    _SMSCampaign.invaliedNo = 0;
                    _SMSCampaign.maskBlockedUser = 0;
                    _SMSCampaign.CreateDate =DateTime.Now;
                    _SMSCampaign.CreateUser = 0;

                    smsCampaign = createSMSCampaign(_SMSCampaign);

           

                    var request = new SMSTokenRequest
                    {
                        msisdn = new List<MobileNumber>
                        {
                            new MobileNumber { mobile = "0702869830" }
                        },
                        sourceAddress = "Kumudu Hospital",
                        message = "This is a test message",
                        transaction_id = smsCampaign.Id,
                        payment_method = 4,
                        push_notification_url = ""
                    };

                    var jsonRequest = JsonSerializer.Serialize(request);
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    // Add Authorization header
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _smsApiLogin.Token);

                    var response = await httpClient.PostAsync("https://e-sms.dialog.lk/api/v2/sms", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonSerializer.Deserialize<ApiResponse>(responseContent);

                        if (responseObject.status == "success")
                        {
                            var campaignId = responseObject.data.campaignId;
                            var campaignCost = responseObject.data.campaignCost;

                            smsCampaign.CampaignID = campaignId;
                            smsCampaign.CampaignCost = campaignCost;
                            smsCampaign.duplicateNo = responseObject.data.duplicatesRemoved;
                            smsCampaign.invaliedNo = responseObject.data.invalidNumbers;
                            smsCampaign.maskBlockedUser = responseObject.data.mask_blocked_numbers;
        
                            // Handle success case
                            return $"Campaign ID: {campaignId}, Campaign Cost: {campaignCost}";
                        }
                        else
                        {
                            var errorCode = responseObject.errCode;
                            var reason = responseObject.comment;
                            // Handle failure case
                            throw new Exception($"API call failed. Reason: {reason}, Error Code: {errorCode}");
                        }
                    }
                    else
                    {
                        // Handle HTTP error response
                        var statusCode = response.StatusCode;
                        throw new Exception($"HTTP request failed with status code {statusCode}");
                    }
                }
                return null;
            }
        }

        public async Task<string> SendSMSToken()
        {

            Model.SMSAPILogin _smsApiLogin = new Model.SMSAPILogin();
            Model.SMSCampaign _SMSCampaign = new Model.SMSCampaign();

            using (var httpClient = new HttpClient())
            {
                _smsApiLogin = await GetAccessToken();
                if (_smsApiLogin != null)
                {
                    Model.SMSCampaign smsCampaign = new Model.SMSCampaign();
                    _SMSCampaign.CampaignID = 0;
                    _SMSCampaign.CampaignCost = 0;
                    _SMSCampaign.duplicateNo = 0;
                    _SMSCampaign.invaliedNo = 0;
                    _SMSCampaign.maskBlockedUser = 0;
                    _SMSCampaign.CreateDate = DateTime.Now;
                    _SMSCampaign.CreateUser = 0;
                    _SMSCampaign.sceduleID = 64;
                    smsCampaign = createSMSCampaign(_SMSCampaign);



                    var request = new SMSTokenRequest
                    {
                        msisdn = new List<MobileNumber>
                        {
                            new MobileNumber { mobile = "0710101773" },
                            new MobileNumber { mobile = "0702869830" }
                        },
                        sourceAddress = "Kumudu hos ",
                        message = "රෝගීන් පර්ක්ෂා කිරීම අවසන් විය ",
                        transaction_id = smsCampaign.Id,
                        payment_method = 0,
                        push_notification_url = ""
                    };

                    var jsonRequest = JsonSerializer.Serialize(request);
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    // Add Authorization header
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _smsApiLogin.Token);

                    var response = await httpClient.PostAsync("https://e-sms.dialog.lk/api/v2/sms", content);

                    if (response.IsSuccessStatusCode)
                    {
                        try {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            var responseObject = JsonSerializer.Deserialize<ApiResponse>(responseContent);

                            if (responseObject.status == "success")
                            {
                                var campaignId = responseObject.data.campaignId;
                                var campaignCost = responseObject.data.campaignCost;

                                smsCampaign.CampaignID = campaignId;
                                smsCampaign.CampaignCost = campaignCost;
                                smsCampaign.duplicateNo = responseObject.data.duplicatesRemoved;
                                smsCampaign.invaliedNo = responseObject.data.invalidNumbers;
                                smsCampaign.maskBlockedUser = responseObject.data.mask_blocked_numbers;
                                smsCampaign = createSMSCampaign(smsCampaign);

                                // Handle success case
                                return $"Campaign ID: {campaignId}, Campaign Cost: {campaignCost}";
                            }
                            else
                            {
                                var errorCode = responseObject.errCode;
                                var reason = responseObject.comment;
                                // Handle failure case
                                throw new Exception($"API call failed. Reason: {reason}, Error Code: {errorCode}");
                            }


                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }

                    }
                    else
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonSerializer.Deserialize<ApiResponse>(responseContent);
                        // Handle HTTP error response
                        var statusCode = response.StatusCode;
                        throw new Exception($"HTTP request failed with status code {statusCode}");
                    }
                }
                return null;
            }
        }

        public HospitalMgrSystem.Model.SMSCampaign createSMSCampaign(HospitalMgrSystem.Model.SMSCampaign sMSCampaign)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (sMSCampaign.Id == 0)
                    {
                        dbContext.SMSCampaign.Add(sMSCampaign);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        HospitalMgrSystem.Model.SMSCampaign result = (from p in dbContext.SMSCampaign where p.Id == sMSCampaign.Id select p).SingleOrDefault();
                        result.CampaignID = sMSCampaign.CampaignID;
                        result.CampaignCost = sMSCampaign.CampaignCost;
                        result.duplicateNo = sMSCampaign.duplicateNo;
                        result.invaliedNo = sMSCampaign.invaliedNo;
                        result.maskBlockedUser = sMSCampaign.maskBlockedUser;
                        result.sceduleID = sMSCampaign.sceduleID;
                        dbContext.SaveChanges();
                    }
                    return dbContext.SMSCampaign.Find(sMSCampaign.Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
        public class ApiResponse
        {
            [JsonPropertyName("status")]
            public string status { get; set; }

            [JsonPropertyName("comment")]
            public string comment { get; set; }

            [JsonPropertyName("data")]
            public Data? data { get; set; }

            [JsonPropertyName("errCode")]
            public string errCode { get; set; }
        }

        public class Data
        {
            [JsonPropertyName("campaignId")]
            public int campaignId { get; set; }

            [JsonPropertyName("campaignCost")]
            public decimal campaignCost { get; set; }

            [JsonPropertyName("walletBalance")]
            public string? walletBalance { get; set; }

            [JsonPropertyName("userMobile")]
            public int userMobile { get; set; }

            [JsonPropertyName("userId")]
            public int userId { get; set; }

            [JsonPropertyName("duplicatesRemoved")]
            public int duplicatesRemoved { get; set; }


            [JsonPropertyName("invalidNumbers")]
            public int invalidNumbers { get; set; }

            [JsonPropertyName("mask_blocked_numbers")]
            public int mask_blocked_numbers { get; set; }
        }

    }
}
