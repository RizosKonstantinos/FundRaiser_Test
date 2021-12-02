using FundRaiser.Common.Data;
using FundRaiser.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundRaiser.Common.Services
{
    public interface IMediaService
    {
        Task<Media> Create(Media _media);
        Task<Media> Update(int projectId, Media _media);
        Task<bool> Delete(int mediaId);
        Task<List<Media>> GetMedia(int projectId);
    }
    public class MediaService : IMediaService
    {
        private readonly FundRaiserDbContext _context;

        public MediaService(FundRaiserDbContext context)
        {
            _context = context;
        }

        public async Task<Media> Create(Media _media)
        {
            var media = new Media()
            {
                Description = _media.Description,
                Path = _media.Path,
                MediaType = _media.MediaType
            };

            await _context.Media.AddAsync(media);
            await _context.SaveChangesAsync();
            
            return media;
        }

        public async Task<bool> Delete(int mediaId)
        {
            var media = await _context.Media.FirstOrDefaultAsync(m => m.Id == mediaId);

            if (media == null) return false;

            _context.Media.Remove(media);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Media>> GetMedia(int mediaId)
        {
           return await _context.Media
                .Where(m => m.Id == mediaId)
                .ToListAsync();
        }

        public async Task<Media> Update(int mediaId, Media med)
        {
            var media = await _context.Media.FirstOrDefaultAsync(m => m.Id == mediaId);

            media.Description = med.Description ?? media.Description;
            media.Path = med.Path ?? media.Path;
            media.MediaType = med.MediaType;

            await _context.Media.AddAsync(media);
            await _context.SaveChangesAsync();

            return media;
        }
    }
}
