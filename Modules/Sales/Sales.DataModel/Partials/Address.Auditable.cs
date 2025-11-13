using Sales.DataModel;

namespace Sales.DataModel.SalesLT;

public partial class Address : IAuditable
{
    DateTime IAuditable.ModifiedDate
    {
        get => ModifiedDate;
        set => ModifiedDate = value;
    }
}
