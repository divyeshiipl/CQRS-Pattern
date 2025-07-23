namespace Web.Endpoints
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/vehicleinsurance")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class VehicleInsuranceInquiryController : ControllerBase
    {
        private readonly ISender _sender;
        public VehicleInsuranceInquiryController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("inquiry")]
        public async Task<Ok<ReturnResult<VehicleInsuranceInquiryResponseDto>>> VehicleInsuranceInquiry([FromQuery] VehicleInsuranceInquiryQuery query)
        {
            var result = await _sender.Send(query);
            return TypedResults.Ok(result);
        }
    }
}
