
namespace GrammyAwards.Models
{
    public class ServiceResponse
    {

        public enum ServiceStatus { NotFound, Created, Updated, Deleted, Error, Success }

        public ServiceStatus Status { get; set; }

        public int CreatedId { get; set; }

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