using ProcessManager.Application.Models;
using ProcessManager.Application.Models.Dtos;

namespace ProcessManager.Infrastructure.Interfaces;

public interface IPaymentService
{
    Task<Result> Pay(PayRequest reqeust);
}
