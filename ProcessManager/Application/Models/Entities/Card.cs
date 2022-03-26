using ProcessManager.Application.Models.Entities;
using ProcessManager.Application.Models.Entities.Enums;

namespace ProcessManager.Application.Models;

public class Card
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public bool Active { get; private set; }
    public int StateId { get; private set; }
    private List<Transaction> _transactions = new();
    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();

    public Card(int userId)
    {
        UserId = userId;
        Active = true;
    }

    public void UpdateState(CardState state)
    {
        StateId = (int)state;
    }

    public void AddTransaction(decimal amount)
    {
        _transactions.Add(new Transaction(amount));
    }

    public void InactiveCard()
    {
        Active = false;
    }
}
