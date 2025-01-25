using Payments.Application;
using Payments.Infrastructure;

namespace Payments.Tests;

public class PaymentRepositoryTest
{
    //[Fact]
    //public async Task GetPaymentsByUserIdAsync_ShouldReturnPayments_WhenPaymentsExist()
    //{
    //    // Arrange
    //    var userId = Guid.NewGuid();
    //    var payments = new List<Domain.Payment>
    //{
    //    new Domain.Payment { PaymentID = Guid.NewGuid(), AmountPaid = 100, PaymentDate = DateTime.UtcNow }
    //};
    //    var mockRepository = new Mock<IPaymentRepository>();
    //    mockRepository.Setup(repo => repo.GetPaymentsByUserIdAsync(userId)).ReturnsAsync(payments);

    //    var paymentService = new PaymentService(mockRepository.Object);

    //    // Act
    //    var result = await paymentService.GetPaymentsByUserIdAsync(userId);

    //    //// Assert
    //    //Assert.NotNull(result);
    //    //Assert.Single(result);
    //    //Assert.Equal(100, result.First().AmountPaid);
    //}
}
