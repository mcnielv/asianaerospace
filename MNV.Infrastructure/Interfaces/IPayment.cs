using MNV.Infrastructure.Domain.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Interfaces
{
    public interface IPayment
    {
        PaymentResponse SubmitPayment(string url, string login, string key, string testRequest, string duplicateWindow, decimal amount, string creditCardNumber, int expireMonth, int expireYear, string cardCode, string firstName, string lastName, string address, string city, string state, string zip, string phone, string email, string description);
    }
}
