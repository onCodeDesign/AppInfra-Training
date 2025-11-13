using Sales.DataModel;

namespace Sales.DataModel.SalesLT;

public partial class Product : IAuditable
{
    DateTime IAuditable.ModifiedDate
    {
        get => ModifiedDate;
        set => ModifiedDate = value;
    }
}
