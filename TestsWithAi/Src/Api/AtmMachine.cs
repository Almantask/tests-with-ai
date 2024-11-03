

using UnitTests;

namespace Api
{
    public class AtmMachine
    {
        private IBank bank;
        private Dictionary<int, int> bills;

        public AtmMachine(IBank bank)
        {
            this.bank = bank;
            this.bills = new Dictionary<int, int>();
        }

        public void AddBills(int denomination, int count)
        {
            if (bills.ContainsKey(denomination))
            {
                bills[denomination] += count;
            }
            else
            {
                bills[denomination] = count;
            }
        }

        public Dictionary<int, int> Withdraw(int amount)
        {
            if (!bank.Withdraw(amount))
            {
                throw new InvalidOperationException("Insufficient funds in bank.");
            }

            var result = new Dictionary<int, int>();
            var sortedBills = bills.Keys.OrderByDescending(x => x).ToList();

            foreach (var bill in sortedBills)
            {
                if (amount <= 0) break;
                var count = Math.Min(amount / bill, bills[bill]);
                if (count > 0)
                {
                    result[bill] = count;
                    amount -= count * bill;
                    bills[bill] -= count;
                }
            }

            if (amount > 0)
            {
                throw new InvalidOperationException("Insufficient funds in ATM.");
            }

            return result;
        }
    }
}