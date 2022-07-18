namespace Company.Web.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using Company.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SelectListGenerator
    {
        public static IEnumerable<SelectListItem> GetAllDestinations(IDestinationsService destinationsService)
        {
            var destinations = destinationsService.GetAllDestinationsAsync();
            var groups = new List<SelectListGroup>();
            foreach (var destinationViewModel in destinations)
            {
                if (groups.All(g => g.Name != destinationViewModel.Name))
                {
                    groups.Add(new SelectListGroup { Name = destinationViewModel.Name });
                }
            }

            return destinations.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            });
        }
    }
}
