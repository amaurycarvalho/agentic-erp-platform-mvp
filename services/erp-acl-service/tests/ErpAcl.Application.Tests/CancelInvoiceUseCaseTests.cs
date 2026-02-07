using ErpAcl.Application.UseCases;
using ErpAcl.Domain.Interfaces;
using ErpAcl.Domain.Models;
using Moq;
using Xunit;

public class CancelInvoiceUseCaseTests
{
    private readonly Mock<IInvoiceGateway> _invoiceGatewayMock;
    private readonly CancelInvoiceUseCase _useCase;

    public CancelInvoiceUseCaseTests()
    {
        _invoiceGatewayMock = new Mock<IInvoiceGateway>();
        _useCase = new CancelInvoiceUseCase(_invoiceGatewayMock.Object);
    }

    [Fact]
    public void Should_Cancel_Invoice_When_Valid()
    {
        var invoice = new Invoice
        {
            Id = "INV-001",
            IsCancelled = false
        };

        _invoiceGatewayMock
            .Setup(g => g.GetById("INV-001"))
            .Returns(invoice);

        _useCase.Execute("INV-001", "Customer request");

        _invoiceGatewayMock.Verify(
            g => g.Cancel("INV-001", "Customer request"),
            Times.Once
        );
    }

    [Fact]
    public void Should_Throw_When_Invoice_Not_Found()
    {
        _invoiceGatewayMock
            .Setup(g => g.GetById(It.IsAny<string>()))
            .Returns((Invoice?)null);

        Assert.Throws<InvalidOperationException>(() =>
            _useCase.Execute("INV-404", "Reason")
        );
    }

    [Fact]
    public void Should_Throw_When_Invoice_Already_Cancelled()
    {
        var invoice = new Invoice
        {
            Id = "INV-002",
            IsCancelled = true
        };

        _invoiceGatewayMock
            .Setup(g => g.GetById("INV-002"))
            .Returns(invoice);

        Assert.Throws<InvalidOperationException>(() =>
            _useCase.Execute("INV-002", "Reason")
        );
    }

    [Fact]
    public void Should_Throw_When_Reason_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() =>
            _useCase.Execute("INV-001", "")
        );
    }
}

