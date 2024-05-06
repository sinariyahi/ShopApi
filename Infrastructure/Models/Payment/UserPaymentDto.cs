using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Payment
{
    public class UserPaymentDto
    {

        public Guid Id { get; set; }


    }

    public class PersonnelInfoModel
    {
        public string HomePhone { get; set; }
        public string NationalCode { get; set; }

        public string Phone { get; set; }
        public string FullName { get; set; }
        public string PostalCode { get; set; }
        public string PostAddress { get; set; }

        public string BirthDate { get; set; }
        public string IssueNumber { get; set; }

    }

    public class BankResultDto
    {
        public int Status { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public string OrderNumber { get; set; }
        public long Amount { get; set; }
    }

    public class UserConfirmPaymentResultDto
    {
        public int Status { get; set; }
        public long Token { get; set; }
        public long OrderId { get; set; }
        public int TerminalNumber { get; set; }
        public long RRN { get; set; }
        public string Amount { get; set; }
        public string DiscountAmount { get; set; }
        public string CardNumberHashed { get; set; }
        public string PaymentPageUrl { get; set; }
    }

    public class UserPaymentResultDto
    {
        public string PaymentGatewayRedirectUrl { get; set; }
        public string OrderId { get; set; }
        public string RefId { get; set; }

    }


    public class UserPaymenstDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string NationalCode { get; set; }

        public long Amount { get; set; }
        public string Title { get; set; }
        public string OrderDate { get; set; }
        public string Status { get; set; }
        public string RefId { get; set; }
        public string Token { get; set; }
        public string MobileNo { get; set; }
        public string PaymentResult { get; set; }
        public string BankConfirmResult { get; set; }
    }
    public class UserPaymenstModel
    {
        public string OrderNumber { get; set; }

        public string Token { get; set; }
        public string OrderDate { get; set; }
        public long Amount { get; set; }
        public string Status { get; set; }

    }


    public class UserPaymentFilterDto : GridQueryModel
    {
        public Guid Id { get; set; }
        public string NezamCode { get; set; }

        public long Amount { get; set; }
        public string Title { get; set; }
        public string OrderDate { get; set; }
        public string RequestStatus { get; set; }

        public bool? IsSuccess { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Token { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class UserPaymentsFilterModel : GridQueryModel
    {
        public Guid Id { get; set; }
        public string Token { get; set; }

        public Guid UserId { get; set; }


        public bool? IsSuccess { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

}
