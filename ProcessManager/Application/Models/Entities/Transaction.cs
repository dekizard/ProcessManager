namespace ProcessManager.Application.Models.Entities;

public class Transaction
{
    public int Id { get; private set; }
    public int CardId { get; private set; }
    public decimal Amount { get; private set; }

    public Transaction(decimal amount)
    {
        Amount = amount;
    }
}
