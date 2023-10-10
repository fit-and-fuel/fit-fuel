using fit_and_fuel.DTOs;

namespace fit_and_fuel.Interfaces
{
    public interface IPayment
    {
        Task<bool> Post(PaymentDto paymentDto,int UserId);
    }
}
