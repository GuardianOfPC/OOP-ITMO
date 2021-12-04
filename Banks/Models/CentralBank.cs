using Banks.Interfaces;

namespace Banks.Models
{
    public class CentralBank
    {
        public CentralBank(IBankRepository bankRepository)
        {
            BankRepository = bankRepository;
        }

        private delegate void AccountHandler(int days);
        private event AccountHandler ChargeInterest;
        private event AccountHandler ChargeCommission;
        public IBankRepository BankRepository { get; }

        private uint DaysFromCentralBankCreation { get; set; }

        public uint ForwardTime(uint value)
        {
            DaysFromCentralBankCreation += value;
            return DaysFromCentralBankCreation;
        }

        public Bank RegisterBank(Bank bank)
        {
            BankRepository.AddBank(bank);
            return bank;
        }
    }
}