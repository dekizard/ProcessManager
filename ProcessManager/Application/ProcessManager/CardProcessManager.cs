using Microsoft.EntityFrameworkCore;
using ProcessManager.Application.Models;
using ProcessManager.Application.Models.Dtos;
using ProcessManager.Application.Models.Entities.Enums;
using ProcessManager.Infrastructure.Interfaces;
using ProcessManager.Infrastructure.Persistence;
using Stateless;

namespace ProcessManager.Application.ProcessManager;

public class CardProcessManager
{
    private readonly IPaymentService _paymentService;
    private readonly ICardService _cardService;
    private readonly StateMachine<CardState, CardTrigger> _stateMachine;
    private readonly CardDbContext _cardDbContext;

    public CardProcessManager(ICardService cardService, CardDbContext cardDbContext, IPaymentService payService)
    {
        _cardService = cardService;
        _paymentService = payService;
        _cardDbContext = cardDbContext;

        _stateMachine = new StateMachine<CardState, CardTrigger>(
            () => State, s => State = s);
        _stateMachine.Configure();
    }

    public CardState State { get; private set; } = CardState.Init;

    public async Task<Result> RequireCreditOffer(RequireCreditOfferRequest request)
    {
        var card = await _cardDbContext.Cards
          .Where(c => c.UserId == request.UserId && c.Active)
          .SingleOrDefaultAsync();

        if (card != null)
            InitStateMachineState(card.StateId);

        await _stateMachine.FireAsync(CardTrigger.RequireCreditOffer);

        await MakePreviousUserCardsInactive(request.UserId);
        var newCard = await CreateCard(request.UserId);

        var requireCreditOfferResponse = await _cardService.RequireCreditOffer();
        if (!requireCreditOfferResponse.Success)
        {
            await TriggerStateMachieToNewState(newCard, CardTrigger.RequireCreditOfferFailed);
            return Result.CreateFailed("An error occurred while requiring credit offer.");
        }

        await TriggerStateMachieToNewState(newCard, CardTrigger.RequireCreditOfferSuccess);

        return Result.CreateSuccess();
    }

    public async Task<Result> AcceptCreditOffer(AcceptCreditOfferRequest request)
    {
        var card = await GetCardByUserId(request.UserId);

        InitStateMachineState(card.StateId);

        await _stateMachine.FireAsync(CardTrigger.AcceptCreditOffer);

        var acceptCreditOfferResponse = await _cardService.AcceptCreditOffer();
        if (!acceptCreditOfferResponse.Success)
        {
            await TriggerStateMachieToNewState(card, CardTrigger.AcceptCreditOfferFailed);
            return Result.CreateFailed("An error occurred while accepting credit offer.");
        }

        await TriggerStateMachieToNewState(card, CardTrigger.AcceptCreditOfferSuccess);

        return Result.CreateSuccess();
    }

    public async Task<Result> DeclineCreditOffer(DeclineCreditOfferRequest request)
    {
        var card = await GetCardByUserId(request.UserId);

        InitStateMachineState(card.StateId);

        await _stateMachine.FireAsync(CardTrigger.DeclineCreditOffer);

        var declineCreditOfferResponse = await _cardService.DeclineCreditOffer();
        if (!declineCreditOfferResponse.Success)
        {
            await TriggerStateMachieToNewState(card, CardTrigger.DeclineCreditOfferFailed);
            return Result.CreateFailed("An error occurred while declining credit offer.");
        }

        await TriggerStateMachieToNewState(card, CardTrigger.DeclineCreditOfferSuccess);

        return Result.CreateSuccess();
    }

    public async Task<Result> Pay(PayRequest request)
    {
        var card = await GetCardByUserId(request.UserId);

        InitStateMachineState(card.StateId);

        await _stateMachine.FireAsync(CardTrigger.Pay);

        var declineCreditOfferResponse = await _paymentService.Pay(request);
        if (!declineCreditOfferResponse.Success)
        {
            return Result.CreateFailed("An error occurred while paying balance.");
        }

        card.AddTransaction(request.Amount);
        await _cardDbContext.SaveChangesAsync();

        return Result.CreateSuccess();
    }

    private async Task<Card> GetCardByUserId(int userId)
    {
        var card = await _cardDbContext.Cards
           .Include(t => t.Transactions)
           .Where(c => c.UserId == userId && c.Active)
           .SingleAsync();

        return card;
    }

    private async Task<Card> CreateCard(int userId)
    {
        var card = new Card(userId);
        var cardEntry = await _cardDbContext.Cards.AddAsync(card);
        return cardEntry.Entity;
    }

    private async Task MakePreviousUserCardsInactive(int userId)
    {
        var previousUserCards = await _cardDbContext.Cards
            .Where(c => c.UserId == userId)
            .ToListAsync();
        previousUserCards.ForEach(c => c.InactiveCard());
    }

    private void InitStateMachineState(int state)
    {
        State = (CardState)state;
    }

    private async Task TriggerStateMachieToNewState(Card card, CardTrigger trigger)
    {
        await _stateMachine.FireAsync(trigger);
        card.UpdateState(State);
        await _cardDbContext.SaveChangesAsync();
    }
}
