using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IPrice
    {
        Task<Price> Post(PriceDto price);

    }
}
