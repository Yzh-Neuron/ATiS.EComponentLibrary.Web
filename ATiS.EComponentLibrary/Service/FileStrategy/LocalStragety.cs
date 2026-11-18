using Microsoft.AspNetCore.Http;

namespace Service.FileStrategy
{
    public class LocalStragety : Strategy
    {
        public override async Task<string> Upload(List<IFormFile> formFiles)
        {
            var res = await Task.Run(() =>
            {
                List<string> result = new List<string>();
                foreach (IFormFile file in formFiles)
                {
                    if (file.Length > 0)
                    {
                        var filePath = $"{AppContext.BaseDirectory}/wwwroot";
                        var fileName = $"/{DateTime.Now:yyyyMMddHHmmssffff}{file.FileName}";
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        using (var stream = File.Create(Path.Combine(filePath, fileName)))
                        {
                            file.CopyTo(stream);
                        }

                        result.Add(fileName);
                    }
                }

                return string.Join(',', result);
            });

            return res;
        }
    }
}
