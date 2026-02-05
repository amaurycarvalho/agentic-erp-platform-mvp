namespace ErpAcl.Domain.Interfaces;

using ErpAcl.Domain.Models;

public interface IOrderGateway
{
    Order Create(Order order);
}

