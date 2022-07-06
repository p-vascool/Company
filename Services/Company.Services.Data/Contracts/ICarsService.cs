namespace Company.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Company.Web.ViewModels.Car.InputModels;

    public interface ICarsService
    {
        Task CreateAsync(string userId, CarCreateInputModel carInput);

        Task EditAsync(string userId, CarEditInputModel carInput);

        Task DeleteAsync(string userId, int id);

        IEnumerable<T> GetAllUserCarsByUserId<T>(string userId);

        IEnumerable<T> GetAllUserCarsByUserName<T>(string userName);

        T GetCarById<T>(int id);

        Task<CarEditInputModel> ExtractCar(string userId, int id);

        bool IsCarExists(string userId, int id);
    }
}
