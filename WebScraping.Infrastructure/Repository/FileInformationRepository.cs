using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebScraping.Core.Entities;
using WebScraping.Core.Interface.Repository;
using WebScraping.Infrastructure.Data;

namespace WebScraping.Infrastructure.Repository
{
    public class FileInformationRepository : IFileInformationRepository
    {
        private readonly AppDbContext _dbContext;

        public FileInformationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FileInformation> Add(FileInformation entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<FileInformation>> Get()
        {
            var query = _dbContext.FileInformation
                .AsQueryable();

            return await query.ToListAsync();
        }

    }
}