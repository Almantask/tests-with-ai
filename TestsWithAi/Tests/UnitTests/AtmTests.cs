using Api;
using FluentAssertions;
using Moq;

namespace UnitTests
{
    public class AtmTests
    {
        private readonly Atm _atm;
        private Mock<IBank> _bankMock;

        public AtmTests()
        {
            _bankMock = new Mock<IBank>();
            _atm = new Atm(_bankMock.Object);
        }

        [Fact]
        public void WithdrawWhenAccountHasEnoughMoneyInBankAndAtmHasEnoughReturnsBiggestNominals()
        {
            _bankMock.Setup(b => b.GetBalance(It.IsAny<string>())).Returns(15949879);
            _atm.AddNominals(new Dictionary<int, int>
            {
                { 100, 100 },
                { 50, 200 },
                { 20, 300 },
                { 10, 400 },
                { 5, 500 },
                { 2, 600 },
                { 1, 700 }
            });

            var result = _atm.Withdraw("123456", 1000);

            result.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                { 100, 10 }
            });
        }
    }
}