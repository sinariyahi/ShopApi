using Application.Interfaces.PaymentGateway;
using Infrastructure.Models.PaymentGateway;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ShopApi.Controllers.PaymentGateway
{
    [Route("[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly PaymentConfigs paymentConfigs;
        private readonly IMellatPaymentService mellatPaymentService;

        public CheckoutController(ILogger<CheckoutController> logger, IMellatPaymentService mellatPaymentService,
            IOptions<PaymentConfigs> options)
        {
            this.logger = logger;
            this.mellatPaymentService = mellatPaymentService;
            this.paymentConfigs = options.Value;
        }

        /// <summary>
        ///    دریافت داده های ارسالی از درگاه پرداخت، پس از پرداخت توسط کاربر
        /// </summary>
        /// <returns></returns>
        [HttpPost("MellatConfirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> MellatPaymentGatewayConfirm([FromForm] GatewayResponseDto model)
        {
            logger.LogInformation("Call Action PaymentGatewayConfirm with model:{0}", model);
            var confirmPaymentResult = await mellatPaymentService.SalePaymentConfirm(model);

            logger.LogInformation("Result Action PaymentGatewayConfirm, Result:{0}", confirmPaymentResult);

            //عملیات موفق بوده است
            if (confirmPaymentResult.IsSuccess)
            {
                return Redirect($"{paymentConfigs.AppGatewayResult}{model.SaleReferenceId}");
            }

            return Redirect($"{paymentConfigs.AppGatewayResult}{model.SaleReferenceId}");
        }
    }
}
