namespace Movies.Core.Requests
{
    public class BaseRequestParams
    {
        private int pageSize = 10;
        const int maxPageSize = 100;
        public int Page { get; set; } = 1;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > maxPageSize ? maxPageSize : value;
        }
    }
}
