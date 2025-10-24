using AppBoot.DependencyInjection;
using AppBoot.SystemEx.Priority;
using Contracts.Sales.OrderingService;
using Sales.DataModel.SalesLT;

namespace Sales.Services;

[Service(typeof(IApprovalService))]
class CompositeApprovalService(IEnumerable<IApprovalServiceStep> approvals) : IApprovalService
{
    private readonly IEnumerable<IApprovalService> approvals = approvals;

    public bool Approve(ApproveRequest approveRequest)
    {
        foreach (var approval in approvals)
        {
            bool isOk = approval.Approve(approveRequest);
            if (!isOk)
                return false;
        }

        return true;
    }
}

interface IApprovalServiceStep : IApprovalService
{
}

[Service(typeof(IApprovalServiceStep))]
[Priority(Int32.MaxValue)]
class BannedCustomer : IApprovalServiceStep
{
    public bool Approve(ApproveRequest approveRequest)
    {
        if (IsBanned(approveRequest.Customer))
            return false;

        return true;
    }

    private bool IsBanned(Customer customer)
    {
        return false;
    }
}

[Service(typeof(IApprovalServiceStep))]
class PriceForCustomer : IApprovalServiceStep
{
    public bool Approve(ApproveRequest approveRequest)
    {
        // check if the order price is to high for the trust we have in this customer
        return true;
    }
}

internal class ApproveRequest
{
    public Customer Customer { get; set; }
    public OrderRequest Order { get; set; }
    public decimal Taxes { get; set; }
    public decimal Discount { get; set; }
}