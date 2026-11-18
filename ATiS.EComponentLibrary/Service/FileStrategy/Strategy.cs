using Microsoft.AspNetCore.Http;

namespace Service.FileStrategy
{
    /// <summary>
    /// 文件操作抽象类
    /// </summary>
    public abstract class Strategy
    {
        public abstract Task<string> Upload(List<IFormFile> formFiles);
    }
}
