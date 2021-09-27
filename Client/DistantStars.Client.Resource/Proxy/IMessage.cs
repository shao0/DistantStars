namespace DistantStars.Client.Resource.Proxy
{
    /// <summary>
    /// 提示信息接口
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 提示信息
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// 关闭提示
        /// </summary>
        void Close();
    }
}
