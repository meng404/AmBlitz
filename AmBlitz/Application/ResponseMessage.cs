namespace AmBlitz.Application
{
    public class ResponseMessage<T>
    {
        /// <summary>
        /// 0 表示成功,
        /// </summary>
        public int Code { get; set; }


        public string Message { set; get; }
 

        public T Data { set; get; }

        /// <summary>
        /// 生成成功响应
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static ResponseMessage<T> MakeSucc(T data)
        {
            return new ResponseMessage<T>
            {
                Code = 0,
                Data = data
            };
        }

        public static ResponseMessage<T> CreateFailed(string defaultMessage = "失败!", int code = -1)
        {
            var response = new ResponseMessage<T>
            {
                Message = defaultMessage,
                Code = code
            };
            return response;
        }
    }
}
