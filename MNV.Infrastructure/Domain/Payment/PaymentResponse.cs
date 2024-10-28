using MNV.Infrastructure.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Domain.Payment
{
    public class PaymentResponse : Response
    {
        public string PaymentResponseString { get; set; }
        public long TransactionId { get; set; }
        public int ResponseCode { get; set; }
    }
}
