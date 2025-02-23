
namespace GrammyAwards.Models
{
    public class ServiceResponse
    {
        // Not to be confused with HTTP response!
        // The type of response to give on direct data manipulations (Create, Delete, Update)

        public enum ServiceStatus { NotFound, Created, Updated, Deleted, Error, Success }

        public ServiceStatus Status { get; set; }

        public int CreatedId { get; set; }

        // Allows for more information, such as logic/validation errors
        public List<string> Messages { get; set; } = new List<string>();

        public static implicit operator bool(ServiceResponse v)
        {
            throw new NotImplementedException();
        }
    }

    // Generic version to support data responses
    public class ServiceResponse<T> : ServiceResponse
    {
        public T? Data { get; set; } // Holds any type of data (e.g., List<int>)
    }
}