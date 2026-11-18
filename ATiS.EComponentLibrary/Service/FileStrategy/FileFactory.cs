using Model.Enum;

namespace Service.FileStrategy
{
    /// <summary>
    /// 工厂类负责对象的实例化
    /// </summary>
    public class FileFactory
    {
        public static Strategy CreateStrategy(UploadMode mode)
        {
            switch (mode)
            {
                case UploadMode.Local:
                    return new LocalStragety();
                default:
                    return new QiNiuStragety();
            }
        }
    }
}
