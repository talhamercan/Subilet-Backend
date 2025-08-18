namespace SubiletServer.WebAPI.Models
{
    /// <summary>
    /// Angular frontend için standart API response formatı
    /// </summary>
    /// <typeparam name="T">Response data tipi</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// İşlem başarılı mı?
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Kullanıcıya gösterilecek mesaj
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Response data
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Hata mesajları listesi
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Toplam kayıt sayısı (pagination için)
        /// </summary>
        public int? TotalCount { get; set; }

        /// <summary>
        /// Sayfa numarası (pagination için)
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// Sayfa boyutu (pagination için)
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Başarılı response oluştur
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "İşlem başarılı")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Hata response oluştur
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

        /// <summary>
        /// Pagination ile başarılı response oluştur
        /// </summary>
        public static ApiResponse<T> SuccessResponseWithPagination(T data, int totalCount, int pageNumber, int pageSize, string message = "İşlem başarılı")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }

    /// <summary>
    /// Pagination için model
    /// </summary>
    public class PaginationModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool IsAscending { get; set; } = true;
    }

    /// <summary>
    /// Pagination result
    /// </summary>
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
} 