using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Contracts;
using PMS.Application.DTOs;
using PMS.Application.Exceptions;
using PMS.Application.Models;
using PMS.Domain.Entities;
using PMS.Persistence.DatabaseContext;

namespace PMS.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _context;
        private readonly IMapper _mapper;
        public GenericRepository(PMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            var entity = _mapper.Map<T>(source);

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TResult>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity is null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            var query = _context.Set<T>().Where(x => x.IsDeleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                query = queryParameters.SortOrder == "desc" ? query.OrderByDescending(e => EF.Property<object>(e, queryParameters.SortBy)) : query.OrderBy(e => EF.Property<object>(e, queryParameters.SortBy));
            }
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync().ConfigureAwait(false);

            return new PagedResult<TResult>
            {
                Items = items,
                CurrentPage = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync()
                .ConfigureAwait(false);
        }


        public async Task<T> GetAsync(int? id)
        {
            var result = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (result is null)
            {
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");
            }

            return result;
        }

        public async Task<TResult> GetAsync<TResult>(int? id)
        {
            var result = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted).ConfigureAwait(false);
            if (result is null)
            {
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");
            }

            return _mapper.Map<TResult>(result);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync<TSource>(int id, TSource source) where TSource : BaseDto
        {
            if (id != source.Id)
            {
                throw new BadRequestException("Invalid Id used in request");
            }

            var entity = await GetAsync(id).ConfigureAwait(false);

            if (entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            _mapper.Map(source, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
