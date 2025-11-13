using Sales.DataModel;
using Sales.DataModel.SalesLT;

namespace Sales.DataModel.SalesLT;

public partial class SalesOrderHeader : IAuditable
{
    DateTime IAuditable.ModifiedDate
    {
        get => ModifiedDate;
        set => ModifiedDate = value;
    }
}
