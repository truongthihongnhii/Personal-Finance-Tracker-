namespace PersonalFinanceTracker;

public class Account
{
    public string Name { get; }
    public decimal Balance { get; private set; }

    private readonly List<Transaction> _transactions = [];

    public Account(string name, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên tài khoản không được để trống.", nameof(name));

        if (initialBalance < 0)
            throw new ArgumentException("Số dư ban đầu không được âm.", nameof(initialBalance));

        Name = name.Trim();
        Balance = initialBalance;

        if (initialBalance > 0)
        {
            _transactions.Add(new Transaction("Số dư ban đầu", initialBalance, TransactionType.Income));
        }
    }

    public void AddIncome(string description, decimal amount)
    {
        ValidateAmount(amount, "Thu nhập");

        var transaction = new Transaction(description, amount, TransactionType.Income);
        _transactions.Add(transaction);
        Balance += amount;
    }

    public void AddExpense(string description, decimal amount)
    {
        ValidateAmount(amount, "Chi tiêu");

        if (amount > Balance)
            throw new InvalidOperationException("Số dư không đủ để thực hiện giao dịch.");

        var transaction = new Transaction(description, amount, TransactionType.Expense);
        _transactions.Add(transaction);
        Balance -= amount;
    }

    public IReadOnlyList<Transaction> GetTransactions() => _transactions.AsReadOnly();

    private static void ValidateAmount(decimal amount, string label)
    {
        if (amount <= 0)
            throw new ArgumentException($"{label} phải lớn hơn 0.", nameof(amount));
    }
}

public enum TransactionType
{
    Income,
    Expense
}

public record Transaction(string Description, decimal Amount, TransactionType Type);
