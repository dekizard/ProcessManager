using ProcessManager.Application.Models;
using ProcessManager.Infrastructure.Interfaces;

namespace ProcessManager.Infrastructure.Services;

public class CardService : ICardService
{
    public CardService()
    {
    }

    public async Task<Result> RequireCreditOffer()
    {
        return Result.CreateSuccess();
    }

    public async Task<Result> AcceptCreditOffer()
    {       
        return Result.CreateSuccess();
    }

    public async Task<Result> DeclineCreditOffer()
    {
        return Result.CreateSuccess();
    }
}
