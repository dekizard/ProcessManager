using ProcessManager.Application.Models.Entities.Enums;
using Stateless;

namespace ProcessManager.Application.ProcessManager;

public static class CardStateMachineExtension
{
    public static void Configure(this StateMachine<CardState, CardTrigger> stateMachine)
    {
        stateMachine.Configure(CardState.Init)
            .PermitReentry(CardTrigger.RequireCreditOffer)
            .Permit(CardTrigger.RequireCreditOfferSuccess, CardState.RequireCreditOfferSuccess)
            .Permit(CardTrigger.RequireCreditOfferFailed, CardState.RequireCreditOfferFailed);

        stateMachine.Configure(CardState.RequireCreditOfferSuccess)
            .PermitReentry(CardTrigger.AcceptCreditOffer)
            .Permit(CardTrigger.AcceptCreditOfferSuccess, CardState.AcceptCreditOfferSuccess)
            .Permit(CardTrigger.AcceptCreditOfferFailed, CardState.AcceptCreditOfferFailed);

        stateMachine.Configure(CardState.RequireCreditOfferSuccess)
          .PermitReentry(CardTrigger.DeclineCreditOffer)
          .Permit(CardTrigger.DeclineCreditOfferSuccess, CardState.DeclineCreditOfferSuccess)
          .Permit(CardTrigger.DeclineCreditOfferFailed, CardState.DeclineCreditOfferFailed);

        stateMachine.Configure(CardState.AcceptCreditOfferSuccess)
            .PermitReentry(CardTrigger.Pay);

        stateMachine.Configure(CardState.RequireCreditOfferFailed)
            .SubstateOf(CardState.Init);
        stateMachine.Configure(CardState.AcceptCreditOfferFailed)
            .SubstateOf(CardState.Init);
        stateMachine.Configure(CardState.DeclineCreditOfferFailed)
            .SubstateOf(CardState.Init);
    }
}
