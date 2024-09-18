using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace VdbAPI.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class LinePayController : ControllerBase
    //{
    //}

    [Route("api/[controller]")]
    [ApiController]
    public class LinePayController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string channelId = "2006296514";  // 從 LINE 開發者平台取得
        private readonly string channelSecretKey = "2194de8d031e19c70ac9a44eb6d0e34b";  // 從 LINE 開發者平台取得
        private readonly string linePayBaseApiUrl = "https://sandbox-api-pay.line.me";  // Sandbox API URL
        private static readonly Dictionary<string, decimal> OrderPayments = new Dictionary<string, decimal>();//儲存訂單編號對應的金額

        public LinePayController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> RequestPayment([FromBody] PaymentRequestDto dto)
        {
            var requestUrl = "/v3/payments/request";
            var nonce = Guid.NewGuid().ToString();
            var requestPayload = new
            {
                amount = dto.amount, //訂單總額
                currency = "TWD",  //幣值
                orderId = dto.orderId,  //訂單編號
                packages = new[]
                {
                    new {
                        id = dto.orderId,  //一個訂單就付款一次的話這裡就是訂單編號
                        amount = dto.amount, //相當於訂單總額
                        name = "商品名稱", //訂單描述
                        products = new[]  //訂單明細
                        {
                            new { name = "商品名稱", quantity = 1, price = dto.amount }
                        }
                    }
                },
                redirectUrls = new
                {
                    confirmUrl = "https://c70d-1-160-11-201.ngrok-free.app/api/linepay/confirm",  // 支付完成後的回調 URL
                    cancelUrl = "http://localhost:4200/shoppingCart/finish"  // 用戶取消支付的回調 URL
                }
            };

            // 存儲 orderId 與 amount
            OrderPayments[dto.orderId] = dto.amount;

            var jsonPayload = JsonConvert.SerializeObject(requestPayload);
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + jsonPayload + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-LINE-ChannelId", channelId);
            request.Headers.Add("X-LINE-Authorization-Nonce", nonce);
            request.Headers.Add("X-LINE-Authorization", signature);

            var response = await _httpClient.SendAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var linePayResponse = JsonConvert.DeserializeObject<PaymentResponseDto>(responseData);

                Console.WriteLine($"Response: {responseData}");  // 檢查 LINE Pay 回應內容
                return Ok(new { paymentUrl = linePayResponse.info.paymentUrl.web });
            }
            else
            {
                // 打印出錯誤訊息以幫助調試
                //Console.WriteLine($"Error: {responseData}");

                return BadRequest("Payment request failed");
            }
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId)
        {
            var nonce = Guid.NewGuid().ToString();
            var apiUrl = $"/v3/payments/{transactionId}/confirm";

            // 根據 orderId 獲取原始金額
            if (!OrderPayments.TryGetValue(orderId, out var amount))
            {
                return BadRequest("Invalid orderId or no payment found.");
            }

            var confirmRequest = new
            {
                amount = amount,  // 金額應與支付請求中的一致
                currency = "TWD"
            };

            var jsonPayload = JsonConvert.SerializeObject(confirmRequest);
            var signatureString = channelSecretKey + apiUrl + jsonPayload + nonce;
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, signatureString);
            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + apiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-LINE-ChannelId", channelId);
            request.Headers.Add("X-LINE-Authorization-Nonce", nonce);
            request.Headers.Add("X-LINE-Authorization", signature); // 這裡的 Authorization Header 可能需要調整

            var response = await _httpClient.SendAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                /*Console.WriteLine($"Response: {responseData}");*/  // 檢查 LINE Pay 回應內容
                return Redirect("http://localhost:4200/shoppingCart/finish");
            }
            else
            {
                //Console.WriteLine($"Error: {responseData}");
                return BadRequest($"Payment confirmation failed: {responseData}");
            }
        }
    }

    public static class SignatureProvider
    {
        public static string HMACSHA256(string key, string message)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            //取的 key byte 值
            byte[] keyByte = encoding.GetBytes(key);

            // 取得 key 對應的 hmacsha256
            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            // 取的 message byte 值
            byte[] messageBytes = encoding.GetBytes(message);

            // 將 message 使用 key 值對應的 hamcsha256 作 hash 簽章
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

            return Convert.ToBase64String(hashmessage);
        }
    }

    public class PaymentRequestDto
    {
        public int amount { get; set; }
        public string orderId { get; set; }
    }

    public class PaymentResponseDto
    {
        public PaymentResponseInfo info { get; set; }
    }

    public class PaymentResponseInfo
    {
        public PaymentUrl paymentUrl { get; set; }
    }

    public class PaymentUrl
    {
        public string web { get; set; }
    }
    
}
