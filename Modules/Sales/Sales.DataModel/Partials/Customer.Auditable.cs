using Sales.DataModel;

namespace Sales.DataModel.SalesLT;

public partial class Customer : IAuditable
{
    DateTime IAuditable.ModifiedDate
    {
        get => ModifiedDate;
        set => ModifiedDate = value;
    }
}
