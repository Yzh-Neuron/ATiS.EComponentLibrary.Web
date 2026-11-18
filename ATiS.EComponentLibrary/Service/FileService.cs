using Interface;
using Microsoft.AspNetCore.Http;
using Model.Enum;
using Service.FileStrategy;

namespace Service
{
    public class FileService : IFileService
    {
        public async Task<string> Upload(List<IFormFile> files, UploadMode mode)
        {
            FileContext fileContext = new FileContext(FileFactory.CreateStrategy(mode), files);
            return await fileContext.ContextInterface();
        }
    }
}
