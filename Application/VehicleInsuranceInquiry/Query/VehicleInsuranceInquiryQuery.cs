namespace Application.VehicleInsuranceInquiry.Query;

public record VehicleInsuranceInquiryQuery : IRequest<ReturnResult<VehicleInsuranceInquiryResponseDto>>
{
    public string? VehicleSearchType { get; set; }
    public string? VehicleSearchValue { get; set; }
}

public class VehicleInsuranceInquiryHandler : IRequestHandler<VehicleInsuranceInquiryQuery, ReturnResult<VehicleInsuranceInquiryResponseDto>>
{
    private readonly IApiClientService _apiClientService;
    private readonly IOptions<LDAPIConfiguration> _ldAPIConfiguration;
    private readonly IOptions<CAMSAPIConfiguration> _camsAPIConfiguration;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly ISeriLogAppService _seriLogAppService;
    public VehicleInsuranceInquiryHandler
    (
        IApiClientService apiClientService,
        IOptions<LDAPIConfiguration> ldAPIConfiguration,
        IOptions<CAMSAPIConfiguration> camsAPIConfiguration,
        IConfiguration configuration,
        IMapper mapper,
        ISeriLogAppService seriLogAppService
    )
    {
        _apiClientService = apiClientService;
        _ldAPIConfiguration = ldAPIConfiguration;
        _camsAPIConfiguration = camsAPIConfiguration;
        _configuration = configuration;
        _mapper = mapper;
        _seriLogAppService = seriLogAppService;
    }

    public async Task<ReturnResult<VehicleInsuranceInquiryResponseDto>> Handle(VehicleInsuranceInquiryQuery request, CancellationToken cancellationToken)
    {
        var result = new ReturnResult<VehicleInsuranceInquiryResponseDto>
        {
            Value = new VehicleInsuranceInquiryResponseDto()
        };

        string accessToken = await _apiClientService.GetAccessToken(new IdentityTokenRequestDto()
        {
            ClientId = _configuration.GetValue<string>("APIGEESettings:ClientId") ?? string.Empty,
            ClientSecret = _configuration.GetValue<string>("APIGEESettings:ClientSecret") ?? string.Empty,
            GrantType = _configuration.GetValue<string>("APIGEESettings:GrantType") ?? string.Empty,
            TokenEndPoint = _configuration.GetValue<string>("APIGEESettings:TokenUrl") ?? string.Empty,
            TokenExpiryInSecond = 300
        });

        string url = _configuration.GetValue<string>("APIGEESettings:VehicleInsuranceInquiry") ?? string.Empty;
        var response = await _apiClientService.GetAsync<VehicleInsuranceInquiryQuery>(url, request, accessToken);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        var data = JsonSerializer.Deserialize<VehicleInsuranceInquiryAPIGEEResponseDto>(jsonResponse, options);

        if (data?.totalCount > 0 && data.policies != null)
        {
            result.Value = _mapper.Map<VehicleInsuranceInquiryResponseDto>(data);
        }

        if (!string.IsNullOrEmpty(accessToken))
        {
            #region LD
            LDResponseDto ldResponseDto = new LDResponseDto();


            #endregion

            #region CAMS
            CamsRequestDto camsRequestDto = new CamsRequestDto();
            camsRequestDto.policyNumbers = new List<string>
            {
                data.policies.FirstOrDefault()?.policyNumber
            };

            string? camsUrl = _configuration.GetValue<string>("CAMSSettings:CAMSUrl") ?? string.Empty;

            var camsResponse = await _apiClientService.PostAsync(camsUrl, camsRequestDto, accessToken);

            string jsonRequestString = JsonSerializer.Serialize(camsRequestDto);
            string jsonCamsResponse = await response.Content.ReadAsStringAsync();
            CamsResponseDto camsDetails = new CamsResponseDto();
            var camsOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            camsDetails = JsonSerializer.Deserialize<CamsResponseDto>(jsonResponse, options);

            await _seriLogAppService.LogExternalApiAuditLogs(camsUrl, "CamsRequest", jsonRequestString, response, "External API Call");
            #endregion
        }

        return result;
    }
}