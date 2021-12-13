using Banks.Interfaces;

namespace Banks.Models
{
    public class CentralBank
    {
        public CentralBank(IBankRepository bankRepository)
        {
            BankRepository = bankRepository;
        }

        public event Bank.AccountHandler AddInterest;
        public event Bank.AccountHandler ChargeCommission;
        public IBankRepository BankRepository { get; }

        public uint DaysFromCentralBankCreation { get; set; }

        public uint ForwardTime(uint value)
        {
            DaysFromCentralBankCreation += value;
            if ((DaysFromCentralBankCreation % 30) == 0)
            {
                ChargeCommission?.Invoke();
                AddInterest?.Invoke();
            }

            return DaysFromCentralBankCreation;
        }

        public void TransferMoney(Client client1, Client )

        public Bank RegisterBank(Bank bank)
        {
            BankRepository.AddBank(bank);
            bank.CentralBank = this;
            return bank;
        }
    }
}