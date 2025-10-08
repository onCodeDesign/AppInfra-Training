namespace Sales.Services;

internal interface IApprovalService
{
    bool Approve(ApproveRequest approveRequest);
}