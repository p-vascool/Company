namespace Company.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Company.Common;
    using Company.Data;
    using Company.Data.Models;
    using Company.Services;
    using Company.Services.Data.Contracts;
    using Company.Services.Mapping;
    using Company.Web.ViewModels.Car.InputModels;
    using Microsoft.EntityFrameworkCore;

    public class CarsService : ICarsService
    {
        private IUnitOfWork _unitOfWork;
        private ICloudinaryService _cloudinaryService;

        public CarsService(
                        IUnitOfWork unitOfWork,
                        ICloudinaryService cloudinaryService)
        {
            this._unitOfWork = unitOfWork;
            this._cloudinaryService = cloudinaryService;
        }

        public async Task CreateAsync(string userId, CarCreateInputModel input)
        {
            var car = new Car
            {
                UserId = userId,
                ImageUrl = GlobalConstants.NoCarPictureLocation,
                Brand = input.Brand,
                Model = input.Model,
                Manifactured = input.YearOfManufacture,
                Color = input.Color,
                Seats = input.Seats,
                IsLuggageAvaliable = input.IsLuggageAvaliable,
                IsSmokingAllowed = input.IsSmokingAllowed,
                IsAirConditioningAvailable = input.IsAirConditiningAvailable,
                IsAllowedForPets = input.IsAllowedForPets,
            };

            if (input.CarPicture != null)
            {
                var carImageUrl = await this._cloudinaryService.UploadImageAsync(
                    file: input.CarPicture,
                    fileName: string.Format(GlobalConstants.CloudinaryCarPictureName, userId));
                car.ImageUrl = carImageUrl;
            }

            await this._unitOfWork.Cars.AddAsync(car);
            await this._unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(string userId, int id)
        {
            var car = await this._unitOfWork
                                .Cars
                                .All()
                                .Where(x => x.UserId == userId)
                                .FirstOrDefaultAsync(x => x.Id == id);
            if (car == null)
            {
                throw new NullReferenceException($"Car with id {id} cannot be found.");
            }

            car.IsDeleted = true;
            this._unitOfWork.Cars.Update(car);
            await this._unitOfWork.CompleteAsync();
        }

        public async Task EditAsync(string userId, CarEditInputModel model)
        {
            {
                var car = this._unitOfWork.Cars.All().Where(x => x.Id == model.Id && x.UserId == userId).FirstOrDefault();

                car.Brand = model.Brand;
                car.Model = model.Model;
                car.Manifactured = model.YearOfManufacture;
                car.Color = model.Color;
                car.Seats = model.Seats;
                car.IsLuggageAvaliable = model.IsLuggageAvaliable;
                car.IsSmokingAllowed = model.IsSmokingAllowed;
                car.IsAirConditioningAvailable = model.IsAirConditiningAvailable;
                car.IsAllowedForPets = model.IsAllowedForPets;

                if (model.CarImageUrl != null)
                {
                    car.ImageUrl = model.CarImageUrl;
                }

                this._unitOfWork.Cars.Update(car);
                await this._unitOfWork.CompleteAsync();
            }
        }

        public async Task<CarEditInputModel> ExtractCar(string userId, int id)
        {
            var car = await this._unitOfWork.Cars
               .All()
               .Where(x => x.Id == id && x.UserId == userId)
               .FirstOrDefaultAsync();

            return new CarEditInputModel
            {
                Id = car.Id,
                UserId = userId,
                CarImageUrl = car.ImageUrl,
                Brand = car.Brand,
                Model = car.Model,
                YearOfManufacture = car.Manifactured,
                Color = car.Color,
                Seats = car.Seats,
                IsLuggageAvaliable = car.IsLuggageAvaliable,
                IsSmokingAllowed = car.IsSmokingAllowed,
                IsAirConditiningAvailable = car.IsAirConditioningAvailable,
                IsAllowedForPets = car.IsAllowedForPets,
            };
        }

        public IEnumerable<T> GetAllUserCarsByUserId<T>(string userId)
        {
            var cars = this._unitOfWork.Cars
                                       .All()
                                       .Where(x => x.UserId == userId)
                                       .To<T>()
                                       .ToList();

            return cars;
        }

        public IEnumerable<T> GetAllUserCarsByUserName<T>(string userName)
        {
            IQueryable<Car> cars = this._unitOfWork.Cars
                                                   .All()
                                                   .Where(x => x.User.UserName == userName);

            return cars.To<T>().ToList();
        }

        public T GetCarById<T>(int id)
        {
            var car = this._unitOfWork.Cars
                                      .All()
                                      .Where(x => x.Id == id)
                                      .To<T>()
                                      .FirstOrDefault();

            if (car == null)
            {
                throw new NullReferenceException($"Car with id {id} not found.");
            }

            return car;
        }

        public bool IsCarExists(string userId, int id)
        {
            var car = this._unitOfWork.Cars
               .All()
               .Include(x => x)
               .Where(x => x.Id == id && x.UserId == userId)
               .FirstOrDefault();

            if (car == null)
            {
                return false;
            }

            return true;
        }
    }
}
