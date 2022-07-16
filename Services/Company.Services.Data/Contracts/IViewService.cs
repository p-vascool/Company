namespace Company.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IViewService
    {
        Task AddViewAsync(string tripId);
    }
}
