namespace Sales.DataModel;

public interface IAuditable
{
    DateTime ModifiedDate { get; set; }
}
