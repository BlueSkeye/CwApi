
namespace CwApi
{
    public class PaginationRequest
    {
        public PaginationRequest()
        {
            PageNumber = 1;
            PageSize = 100;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
