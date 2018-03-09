namespace AmBlitzCore.ToolKit
{
    public class ResponseMessage<T>
    {

        public int Code { get; set; }


        public string Message { set; get; }


        public T Data { set; get; }


        /// <summary>
        /// 生成成功响应
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <param name="message">消息</param>
        /// <param name="code">错误码</param>
        /// <returns></returns>
        public static ResponseMessage<T> MakeSucc(T data, string message = "success", int code = 0)
        {
            return new ResponseMessage<T>
            {
                Code = code,
                Data = data,
                Message = message
            };
        }
        /// <summary>
        /// 生成错误消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="code">错误码</param>
        /// <returns></returns>
        public static ResponseMessage<T> MakeFailed(string message = "failed", int code = -1)
        {
            var response = new ResponseMessage<T>
            {
                Message = message,
                Code = code
            };
            return response;
        }
    }
}
