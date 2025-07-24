namespace Movies.Contracts
{
    public interface IBaseService<TDto, TCreateDto, TUpdateDto>
    {
        Task<bool> AnyAsync(Guid id);
        Task<IEnumerable<TDto>> GetAllAsync(bool trackChanges = false);
        Task<TDto?> GetAsync(Guid id, bool trackChanges = false);
        Task<TDto> CreateAsync(TCreateDto entity);
        Task<bool> UpdateAsync(Guid id, TUpdateDto entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
