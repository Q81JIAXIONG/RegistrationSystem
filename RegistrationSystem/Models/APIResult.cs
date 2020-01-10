
namespace RegistrationSystem.Models
{
    public struct APIResult
    {
        /// <summary>
        /// 此次呼叫 API 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 呼叫 API 失敗的錯誤訊息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 呼叫此API所得到的其他內容
        /// </summary>
        public object Payload { get; set; }
    }
}