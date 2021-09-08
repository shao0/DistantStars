using DistantStars.Server.DBContext;
using DistantStars.Server.IService.Systems;

namespace DistantStars.Server.Service.Systems
{
    public class FileService : ServiceBase, IFileService
    {
        public FileService(EFCoreContext context) : base(context)
        {
        }
    }
}
