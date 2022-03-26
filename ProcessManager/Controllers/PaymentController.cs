using Microsoft.AspNetCore.Mvc;
using ProcessManager.Application.Models;
using ProcessManager.Application.Models.Dtos;
using ProcessManager.Application.ProcessManager;
using System.Net;

namespace ProcessManager.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly CardProcessManager _cardProcessManager;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(ILogger<PaymentController> logger, CardProcessManager cardProcessManager)
    {
        _cardProcessManager = cardProcessManager;
        _logger = logger;
    }

    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [HttpPost()]
    public async Task<Result> Pay([FromBody] PayRequest request)
    {
        try
        {
            var result = await _cardProcessManager.Pay(request);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Pay - error - {@request}", request);
            return Result.CreateFailed("An error occurred while paying balance.");
        }
    }
}
