namespace Movies.Contracts
{
    public interface IBaseService<TDto, TCreateDto, TUpdateDto>
    {
        Task<bool> AnyAsync(Guid id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetAsync(Guid id);
        Task<TDto> CreateAsync(TCreateDto entity);
        Task<bool> UpdateAsync(Guid id, TUpdateDto entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
