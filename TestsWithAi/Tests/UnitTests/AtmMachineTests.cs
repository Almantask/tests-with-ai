using Moq;
using Api;
using FluentAssertions;

namespace UnitTests
{
    public class AtmMachineTests
    {
        private readonly AtmMachine _atmMachine;

        private readonly Mock<IBank> _bank;

        public AtmMachineTests()
        {
            _bank = new Mock<IBank>();
            _atmMachine = new AtmMachine(_bank.Object);
        }

        [Fact]
        public void Withdraw_WhenAccountHasEnoughMoneyInBank_ReturnsBiggestNominalFromAtm_AndWithdrawsMoneyFromBank()
        {
            // Arrange
            _bank.Setup(b => b.Withdraw(It.IsAny<int>())).Returns(true);

            // Act
            _atmMachine.AddBills(100, 1);
            Dictionary<int, int> result = _atmMachine.Withdraw(100);

            // Assert
            // dictionary with 1 bill of 100. Equivalence using fluent assertions
            result.Should().BeEquivalentTo(new Dictionary<int, int> { { 100, 1 } });
            _bank.Verify(b => b.Withdraw(100), Times.Once);
        }

        [Fact]
        public void Withdraw_WhenAccountHasNotEnoughMoneyInBank_ThrowsException()
        {
            // Arrange
            _bank.Setup(b => b.Withdraw(It.IsAny<int>())).Returns(false);

            // Act
            Action act = () => _atmMachine.Withdraw(100);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Insufficient funds in bank.");
            _bank.Verify(b => b.Withdraw(100), Times.Once);
        }

        [Fact]
        public void Withdraw_WhenNotEnoughMoneyInAtm_ThrowsException()
        {
            // Arrange
            _bank.Setup(b => b.Withdraw(It.IsAny<int>())).Returns(true);

            // Act
            _atmMachine.AddBills(100, 1);
            Action act = () => _atmMachine.Withdraw(200);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Insufficient funds in ATM.");
            _bank.Verify(b => b.Withdraw(200), Times.Once);
        }
    }
}