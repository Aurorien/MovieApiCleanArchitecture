using Movies.Core.Requests;

namespace Movies.Services.Contracts
{
    public interface IBaseService<TDto, TCreateDto, TUpdateDto>
    {
        Task<bool> AnyAsync(Guid id);
        Task<(IEnumerable<TDto>, PaginationMetadata)> GetAllAsync(BaseRequestParams requestParams);
        Task<TDto> CreateAsync(TCreateDto entity);
        Task<bool> UpdateAsync(Guid id, TUpdateDto entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
