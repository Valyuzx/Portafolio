using BACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace BACKEND.Helpers
{
    public class BlackListService
    {
        private readonly ApplicationDbContext _context;

        public BlackListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToBlackListAsync( string tokenId, string token, Guid userId, DateTime expirationDate)
        {
            var exists = await _context.BlackList.AnyAsync(b => b.TokenId == tokenId);

            if (exists) return;

            var entry = new BlackList
            {
                Id = Guid.NewGuid(),
                TokenId = tokenId,
                Token = token,
                UserId = userId,
                ExpirationDate = expirationDate
            };

            _context.BlackList.Add(entry);
            await _context.SaveChangesAsync();
        }

    
        public async Task<bool> IsBlackListedAsync(string tokenId)
        {
            return await _context.BlackList.AnyAsync(b => b.TokenId == tokenId);
        }

        public async Task<int> CleanupExpiredEntriesAsync()
        {
            var expiredEntries = await _context.BlackList
                .Where(b => b.ExpirationDate < DateTime.UtcNow)
                .ToListAsync();

            _context.BlackList.RemoveRange(expiredEntries);
            return await _context.SaveChangesAsync();
        }
    }
}
