using MNV.Infrastructure.Domain.Messages;
//using MNV.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MNV.Infrastructure.Domain.Payment
{
    public class AuthorizeNetPayment //: IPayment
    {
        private string _paymentUrl = string.Empty;

        public string GeneratePayment(string login, string key, string testRequest, string duplicateWindow, decimal amount, string creditCardNumber, int expireMonth, int expireYear, string cardCode, string firstName, string lastName, string address, string city, string state, string zip, string phone, string email, string description)
        {

            string lastFourDigits = string.IsNullOrEmpty(creditCardNumber) ? "" : creditCardNumber
                .Trim()
                .Substring(creditCardNumber.Length - 4);
            string name = string.Format("{0} {1}", firstName, lastName);

            StringBuilder post = new StringBuilder();
            post.AppendFormat("x_login={0}", login);
            post.AppendFormat("&x_tran_key={0}", key);
            post.Append("&x_type=AUTH_CAPTURE");
            post.Append("&x_method=CC");
            post.AppendFormat("&x_amount={0}", amount);
            post.Append("&x_delim_data=TRUE");
            post.Append("&x_delim_char=|");
            post.Append("&x_relay_response=FALSE&");
            post.AppendFormat("&x_card_num={0}", creditCardNumber);
            post.AppendFormat("&x_exp_date={0}", expireMonth.ToString() + expireYear.ToString());
            post.AppendFormat("&x_card_code={0}", cardCode);
            post.AppendFormat("&x_test_request={0}", testRequest);
            post.AppendFormat("&x_duplicate_window={0}", duplicateWindow);
            post.Append("&x_version=3.1");
            post.AppendFormat("&x_first_name={0}", HttpUtility.UrlEncode(firstName));
            post.AppendFormat("&x_last_name={0}", HttpUtility.HtmlEncode(lastName));
            post.AppendFormat("&x_address={0}", HttpUtility.HtmlEncode(address));
            post.AppendFormat("&x_state={0}", HttpUtility.HtmlEncode(state));
            post.AppendFormat("&x_zip={0}", HttpUtility.HtmlEncode(zip));
            post.AppendFormat("&x_phone={0}", HttpUtility.HtmlEncode(phone));
            if(!string.IsNullOrEmpty(email))
                post.AppendFormat("&x_email={0}", HttpUtility.HtmlEncode(email));
            if(!string.IsNullOrEmpty(description))
                post.AppendFormat("&x_description={0}", HttpUtility.HtmlEncode(description));

            return post.ToString();
        }

        public PaymentResponse SubmitPayment(string url, string login, string key, string testRequest, string duplicateWindow, decimal amount, string creditCardNumber, int expireMonth, int expireYear, string cardCode, string firstName, string lastName, string address, string city, string state, string zip, string phone, string email, string description)
        {
            PaymentResponse response = new PaymentResponse();

            string payment = GeneratePayment(login, 
                key, 
                testRequest, 
                duplicateWindow, 
                amount, 
                creditCardNumber, 
                expireMonth, 
                expireYear, 
                cardCode, 
                firstName, 
                lastName, 
                address, 
                city, 
                state, 
                zip, 
                phone, 
                email, 
                description);

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.Timeout = 60000;
                webRequest.ContentLength = payment.Length;
                webRequest.ContentType = "application/x-www-form-urlencoded";

                StreamWriter writer = null;
                writer = new StreamWriter(webRequest.GetRequestStream());
                writer.Write(payment);
                writer.Close();

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader responseStream = new StreamReader(webResponse.GetResponseStream());

                response.PaymentResponseString = responseStream.ReadToEnd();
                responseStream.Close();

                string[] responseArray = response.PaymentResponseString.Split('|');
                response.TransactionId = long.Parse(responseArray[6] ?? "0");
                response.ResponseCode = int.Parse(responseArray[0]);
                response.Message = responseArray[3];
                response.Success = true;
            }
            catch (Exception)
            {
                response.Success = false;
                response.ResponseCode = 3;
            }

            return response;
        }
    }

    public enum AuthorizeNetResponseCode
    {
        Approved = 1,
        Declined = 2,
        Error = 3,
        HeldForReview = 4
    }
}
