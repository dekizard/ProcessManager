using ProcessManager.Application.Models;

namespace ProcessManager.Infrastructure.Interfaces;

public interface ICardService
{
    Task<Result> RequireCreditOffer();
    Task<Result> AcceptCreditOffer();
    Task<Result> DeclineCreditOffer();
}
