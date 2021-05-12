using System.Collections.Generic;
using System.Threading.Tasks;
using WebScraping.Core.Entities;

namespace WebScraping.Core.Interface.Repository
{
    public interface IFileInformationRepository
    {
        Task<FileInformation> Add(FileInformation entity);
        Task<IEnumerable<FileInformation>> Get();
    }
}