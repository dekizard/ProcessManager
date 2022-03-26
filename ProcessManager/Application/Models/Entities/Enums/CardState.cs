namespace ProcessManager.Application.Models.Entities.Enums;

public enum CardState
{
    Init,
    RequireCreditOfferSuccess,
    RequireCreditOfferFailed,
    AcceptCreditOfferSuccess,
    AcceptCreditOfferFailed,
    DeclineCreditOfferSuccess,
    DeclineCreditOfferFailed
}
