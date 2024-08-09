using Application.Interfaces.PaymentGateway;
using Domain.Entities.Catalog;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.PaymentGateway;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.PaymentGateway
{
    public class MellatPaymentService : IMellatPaymentService
    {
        private readonly PaymentConfigs paymentConfigs;
        private readonly ILogger logger;
        private readonly BIContext context;

        public MellatPaymentService(IOptions<PaymentConfigs> options, ILogger<MellatPaymentService> logger, BIContext context)
        {
            this.paymentConfigs = options.Value;
            this.logger = logger;
            this.context = context;
        }

        public Task<ShopActionResult<GatewaySubmissionFormDto>> SalePayment(SalePaymentRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ShopActionResult<bool>> SalePaymentConfirm(GatewayResponseDto model)
        {
            throw new NotImplementedException();
        }

        //public async Task<ShopActionResult<GatewaySubmissionFormDto>> SalePayment(SalePaymentRequestModel model)
        //{
        //    logger.LogInformation("SalePayment with orderId:{0}", model.OrderId);

        //    var result = new ShopActionResult<GatewaySubmissionFormDto>();
        //    var client = new PaymentGateway.PaymentGatewayClient();
        //    DateTime currentDate = DateTime.Now;

        //    var localDate = string.Format("{0}{1}{2}", currentDate.Year, currentDate.Month, currentDate.Day);
        //    var localTime = string.Format("{0}{1}{2}", currentDate.Hour, currentDate.Minute, currentDate.Second);


        //    var paymentResult = await client.bpPayRequestAsync(paymentConfigs.TerminalId, paymentConfigs.UserName,
        //        paymentConfigs.UserPassword, model.OrderId, model.Amount
        //         , localDate, localTime, "",
        //        paymentConfigs.CallBackUrl, paymentConfigs.PayerId,
        //        model.CustomerMobileNumber, null, null, null, null);

        //    string[] resultArray = paymentResult.Body.@return.Split(',');

        //    logger.LogInformation("SalePayment Result with orderId:{0}, result:{1}", model.OrderId, resultArray);


        //    if (resultArray[0] == "0")
        //    {
        //        var paymentResultModel = new MellatGatewaySubmissionFormDto
        //        {
        //            RefId = resultArray[1]
        //        };
        //        result.Data = paymentResultModel;
        //        result.IsSuccess = true;
        //        return result;
        //    }

        //    result.IsSuccess = false;
        //    result.Message = string.Join(",", resultArray);
        //    return result;

        //}

        //public async Task<ShopActionResult<bool>> SalePaymentConfirm(GatewayResponseDto model)
        //{
        //    logger.LogInformation("SalePaymentConfirm with orderId:{0}, saleRefId:{1}", model.OrderId, model.SaleReferenceId);

        //    var result = new ShopActionResult<bool>();

        //    if (model.ResCode == "0")
        //    {
        //        var client = new MellatPaymentGateway.PaymentGatewayClient();

        //        var bpResult = await client.bpVerifyRequestAsync(paymentConfigs.TerminalId, paymentConfigs.UserName,
        //                        paymentConfigs.UserPassword, model.OrderId, model.SaleOrderId, model.SaleReferenceId);


        //        logger.LogInformation("SalePaymentConfirm Result with orderId:{0}, saleRefId:{1}, result:{2}",
        //            model.OrderId, model.SaleReferenceId, bpResult.Body.@return);


        //        if (bpResult.Body.@return == "0")
        //        {
        //            var orderInfo = await context.Orders.FirstOrDefaultAsync(f => f.OrderNumber == model.OrderId.ToString());
        //            if (orderInfo != null)
        //            {
        //                orderInfo.OrderStatus = OrderStatus.PaymentSuccess;

        //                var orderLog = new OrderLog
        //                {
        //                    Id = Guid.NewGuid(),
        //                    CreateDate = DateTime.Now,
        //                    OrderStatusRemark = OrderStatus.PaymentSuccess.GetNameAttribute(),
        //                    OrderStatus = OrderStatus.PaymentSuccess,
        //                    OrderId = orderInfo.Id

        //                };


        //                await context.OrderLogs.AddAsync(orderLog);
        //                await context.SaveChangesAsync();
        //                result.IsSuccess = true;
        //                return result;
        //            }

        //        }
        //    }
        //    var order = await context.Orders.FirstOrDefaultAsync(f => f.OrderNumber == model.OrderId.ToString());
        //    if (order != null)
        //    {
        //        order.OrderStatus = OrderStatus.PaymentFailed;

        //        var orderLog = new OrderLog
        //        {
        //            Id = Guid.NewGuid(),
        //            CreateDate = DateTime.Now,
        //            OrderStatusRemark = OrderStatus.PaymentFailed.GetNameAttribute(),
        //            OrderStatus = OrderStatus.PaymentFailed,
        //            OrderId = order.Id

        //        };


        //        await context.OrderLogs.AddAsync(orderLog);
        //        await context.SaveChangesAsync();
        //    }

        //    result.IsSuccess = false;
        //    result.Message = model.ResCode;
        //    return result;

        //}
    }
}
