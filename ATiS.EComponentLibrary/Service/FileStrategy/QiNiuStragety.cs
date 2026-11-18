using Microsoft.AspNetCore.Http;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;

namespace Service.FileStrategy
{
    public class QiNiuStragety : Strategy
    {
        public override Task<string> Upload(List<IFormFile> formFiles)
        {
            var res = Task.Run(() =>
             {
                 Mac mac = new Mac("xxxx", "xxxx");
                 List<string> result = new List<string>();
                 foreach (var file in formFiles)
                 {
                     if (file.Length > 0)
                     {
                         var filePath_temp = $"{AppContext.BaseDirectory}/Images_temp";
                         var fileName = $"/{DateTime.Now:yyyyMMddHHmmssffff}{file.FileName}";
                         if (!Directory.Exists(filePath_temp))
                         {
                             Directory.CreateDirectory(filePath_temp);
                         }

                         using (var stream = File.Create(Path.Combine(filePath_temp, fileName)))
                         {
                             file.CopyTo(stream);
                         }

                         //上传的文件名
                         string key = fileName;
                         //本地文件路径
                         string filePath = $"{filePath_temp}/{fileName}";
                         //存储空间名
                         string bucket = "pl-static";
                         //设置上传策略
                         PutPolicy putPolicy = new PutPolicy();
                         //设置要上传的目标空间
                         putPolicy.Scope = bucket;
                         //上传策略过期时间（单位：秒）
                         //文件上传完毕后，在多少天自动被删除
                         //生成上传的token
                         string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
                         Config config = new Config();
                         //设置上传区域
                         config.Zone = Zone.ZONE_CN_East;
                         //设置Https的上传
                         config.UseHttps = true;
                         config.UseCdnDomains = true;
                         config.ChunkSize = ChunkUnit.U512K;
                         //表单上传
                         FormUploader target = new FormUploader(config);
                         HttpResult httpResult = target.UploadFile(filePath, key, token, null);
                         result.Add(fileName);
                         //删除备份文件夹
                         Directory.Delete(filePath_temp, true);
                     }
                 }
                 return string.Join(',', result);
             });

            return res;
        }
    }
}
