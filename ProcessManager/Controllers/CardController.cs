using Microsoft.AspNetCore.Mvc;
using ProcessManager.Application.Models;
using ProcessManager.Application.Models.Dtos;
using ProcessManager.Application.ProcessManager;
using System.Net;

namespace ProcessManager.Controllers;

[ApiController]
[Route("[controller]")]
public class CardController : ControllerBase
{
    private readonly CardProcessManager _cardProcessManager;
    private readonly ILogger<CardController> _logger;

    public CardController(ILogger<CardController> logger, CardProcessManager cardProcessManager)
    {
        _logger = logger;
        _cardProcessManager = cardProcessManager;
    }

    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [HttpPost(nameof(RequireCreditOffer))]
    public async Task<Result> RequireCreditOffer([FromBody] RequireCreditOfferRequest request)
    {
        try
        {
            var result = await _cardProcessManager.RequireCreditOffer(request);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, "RequireCreditOffer - error - {@request}", request);
            return Result.CreateFailed("An error occurred while requiring credit offer.");
        }        
    }

    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [HttpPost(nameof(AcceptCreditOffer))]
    public async Task<Result> AcceptCreditOffer([FromBody] AcceptCreditOfferRequest request)
    {
        try
        {
            var result = await _cardProcessManager.AcceptCreditOffer(request);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, "AcceptCreditOffer - error - {@request}", request);
            return Result.CreateFailed("An error occurred while accepting credit offer.");
        }
    }

    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [HttpPost(nameof(DeclineCreditOffer))]
    public async Task<Result> DeclineCreditOffer([FromBody] DeclineCreditOfferRequest request)
    {
        try
        {
            var result = await _cardProcessManager.DeclineCreditOffer(request);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, "DeclineCreditOffer - error - {@request}", request);
            return Result.CreateFailed("An error occurred while declining credit offer.");
        }
    }
}
