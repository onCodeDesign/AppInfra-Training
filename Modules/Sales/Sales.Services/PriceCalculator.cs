using AppBoot.DependencyInjection;
using Contracts.Sales.OrderingService;
using Microsoft.Extensions.DependencyInjection;
using Sales.DataModel.SalesLT;
using System;

namespace Sales.Services;

interface IPriceCalculator
{
    decimal CalculateTaxes(OrderRequest o, Customer c);

    decimal CalculateDiscount(OrderRequest o, Customer c);
}

[Service(typeof(IPriceCalculator), ServiceLifetime.Transient)]
class PriceCalculator : IPriceCalculator
{
    public decimal CalculateTaxes(OrderRequest o, Customer c)
    {
        // do actual calculation
        return 10;
    }

    public decimal CalculateDiscount(OrderRequest o, Customer c)
    {
        // do actual calculation
        return 20;
    }
}