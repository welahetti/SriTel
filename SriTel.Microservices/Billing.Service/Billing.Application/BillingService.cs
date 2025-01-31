﻿using Billing.Domain;
using SriTel.Billing.Application.Services.Interfaces;
using Billing.API.MessageBroker;
using Billing.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace SriTel.Billing.Application.Services
{
    public class BillingService : IBillingService
    {
        private readonly IBillingRepository _billRepository;
        private readonly RabbitMQPublisher _publisher;

        public BillingService(IBillingRepository billRepository, RabbitMQPublisher publisher)
        {
            _billRepository = billRepository;
            _publisher = publisher;
        }
      

        public async Task<Bill> GetBillByBillIdAsync(Guid billId)
        {
            var bill = await _billRepository.GetBillByBillIdAsync(billId);
            if (bill == null) throw new Exception("Bill not found");

            // Map to DTO
            return new Bill
            {
                BillID = bill.BillID,
                Amount = bill.Amount,
                DueDate = bill.DueDate,
                IsPaid = bill.IsPaid
            };
        }

        public async Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId)
        {
            var bills = await _billRepository.GetBillsByUserAsync(userId);

            return bills.Select(b => new Bill
            {
                BillID = b.BillID,
                Amount = b.Amount,
                DueDate = b.DueDate,
                IsPaid = b.IsPaid
            });
        }

        public async Task CreateBillAsync(Bill bill)
        {
            var billDTO = new Bill
            {
                UserID = bill.UserID,
                Amount = bill.Amount,
                DueDate = bill.DueDate,
                IsPaid = false
            };
            // Publish the event
            var billEvent = new
            {
                Event = "BillCreated",
                Data = bill
            };
            //_publisher.Publish(Convert.ToString(billEvent.Data.BillID),"bill created");

            await _billRepository.AddBillAsync(bill);
            // Create an event for RabbitMQ
            var billCreatedEvent = new
            {
                Event = "BillCreated",
                Data = new
                {
                    BillID = bill.BillID,
                    UserID = bill.UserID,
                    Amount = bill.Amount,
                    DueDate = bill.DueDate,
                    BillingDate = bill.BillingDate,
                    IsPaid=bill.IsPaid 
                }
            };

            // Publish the event to RabbitMQ
            var message = JsonSerializer.Serialize(billCreatedEvent);
            _publisher.Publish( message);

        }

      
    }
}
