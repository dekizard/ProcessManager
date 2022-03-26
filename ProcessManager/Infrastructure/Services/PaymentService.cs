using ProcessManager.Application.Models;
using ProcessManager.Application.Models.Dtos;
using ProcessManager.Infrastructure.Interfaces;

namespace ProcessManager.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    public PaymentService()
    {
    }

    public async Task<Result> Pay(PayRequest reqeust)
    {
        return Result.CreateSuccess();
    }
}
