namespace PersonalFinanceTracker;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== QUẢN LÝ CHI TIÊU CÁ NHÂN ===\n");

        var account = CreateAccount();

        while (true)
        {
            ShowMenu(account);
            var choice = Console.ReadLine()?.Trim();

            try
            {
                switch (choice)
                {
                    case "1":
                        HandleIncome(account);
                        break;
                    case "2":
                        HandleExpense(account);
                        break;
                    case "3":
                        ShowBalance(account);
                        break;
                    case "4":
                        ShowHistory(account);
                        break;
                    case "0":
                        Console.WriteLine("\nTạm biệt!");
                        return;
                    default:
                        Console.WriteLine("\nLựa chọn không hợp lệ. Vui lòng thử lại.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nLỗi: {ex.Message}");
            }

            Console.WriteLine("\nNhấn Enter để tiếp tục...");
            Console.ReadLine();
            Console.Clear();
        }
    }

    private static Account CreateAccount()
    {
        Console.Write("Nhập tên tài khoản: ");
        var name = Console.ReadLine() ?? "Tài khoản chính";

        Console.Write("Nhập số dư ban đầu (Enter = 0): ");
        var balanceInput = Console.ReadLine()?.Trim();

        decimal initialBalance = 0;
        if (!string.IsNullOrEmpty(balanceInput) && !decimal.TryParse(balanceInput, out initialBalance))
        {
            Console.WriteLine("Số dư không hợp lệ, mặc định là 0.");
            initialBalance = 0;
        }

        Console.Clear();
        return new Account(name, initialBalance);
    }

    private static void ShowMenu(Account account)
    {
        Console.WriteLine($"Tài khoản: {account.Name} | Số dư: {FormatMoney(account.Balance)}\n");
        Console.WriteLine("1. Thêm thu nhập");
        Console.WriteLine("2. Thêm chi tiêu");
        Console.WriteLine("3. Xem số dư");
        Console.WriteLine("4. Xem lịch sử giao dịch");
        Console.WriteLine("0. Thoát");
        Console.Write("\nChọn chức năng: ");
    }

    private static void HandleIncome(Account account)
    {
        var description = ReadRequiredText("Mô tả thu nhập: ");
        var amount = ReadPositiveAmount("Số tiền: ");
        account.AddIncome(description, amount);
        Console.WriteLine($"\nĐã thêm thu nhập. Số dư hiện tại: {FormatMoney(account.Balance)}");
    }

    private static void HandleExpense(Account account)
    {
        var description = ReadRequiredText("Mô tả chi tiêu: ");
        var amount = ReadPositiveAmount("Số tiền: ");
        account.AddExpense(description, amount);
        Console.WriteLine($"\nĐã ghi chi tiêu. Số dư hiện tại: {FormatMoney(account.Balance)}");
    }

    private static void ShowBalance(Account account)
    {
        Console.WriteLine($"\nSố dư hiện tại: {FormatMoney(account.Balance)}");
    }

    private static void ShowHistory(Account account)
    {
        var transactions = account.GetTransactions();

        Console.WriteLine("\n--- LỊCH SỬ GIAO DỊCH ---");

        if (transactions.Count == 0)
        {
            Console.WriteLine("Chưa có giao dịch nào.");
            return;
        }

        for (var i = 0; i < transactions.Count; i++)
        {
            var t = transactions[i];
            var sign = t.Type == TransactionType.Income ? "+" : "-";
            var typeLabel = t.Type == TransactionType.Income ? "Thu" : "Chi";

            Console.WriteLine($"{i + 1}. [{typeLabel}] {t.Description}: {sign}{FormatMoney(t.Amount)}");
        }
    }

    private static string ReadRequiredText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();

            if (!string.IsNullOrEmpty(input))
                return input;

            Console.WriteLine("Nội dung không được để trống.");
        }
    }

    private static decimal ReadPositiveAmount(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();

            if (decimal.TryParse(input, out var amount) && amount > 0)
                return amount;

            Console.WriteLine("Vui lòng nhập số tiền hợp lệ (lớn hơn 0).");
        }
    }

    private static string FormatMoney(decimal amount) => $"{amount:N0} đ";
}
