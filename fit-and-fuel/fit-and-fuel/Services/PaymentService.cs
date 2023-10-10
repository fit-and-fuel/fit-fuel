using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;

using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;

namespace fit_and_fuel.Services
{
    public class PaymentService : IPayment
    {
        private readonly AppDbContext _context;
       
        private INotification _notificationService;

        public PaymentService(AppDbContext context,  INotification notificationService)
        {
            _context = context;

           
            _notificationService = notificationService;
        }

        /// <summary>
        /// Processes a payment and records it in the database.
        /// </summary>
        /// <param name="paymentDto">DTO containing payment information.</param>
        /// <param name="UserId">The ID of the user making the payment.</param>
        /// <returns>True if the payment is successfully processed and recorded, otherwise false.</returns>

        public async Task<bool> Post(PaymentDto paymentDto, int UserId)
       {

            try
            {
                // Retrieve the patient associated with the user ID

                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == UserId);

                if (patient == null)
                {
                    // Patient not found for the provided user ID
                    return false;
                }

                // Create a new payment record
                var payment = new Payment()
                {
                    PaymentId = paymentDto.PaymentId,
                    Amount = paymentDto.Amount,
                    PatientId = patient.Id,
                    NutritionistId = (int)patient.NutritionistId,
                    PaymentDate = DateTime.Now,
                };

                
                _context.PaymentRecords.Add(payment);
                await _context.SaveChangesAsync();

                // Send a notification about the new payment
                var content = new NotificationDto()
                {
                    Content = $"A new payment needs to be verified with Id : {payment.Id} & PaymentId {paymentDto.PaymentId}"
                };

                await _notificationService.SendNotification("1", content);

                // Payment successfully processed and recorded
                return true;
            }
            catch (Exception)
            {
                // An exception occurred during payment processing
                return false;
            }
        }
    }
}
