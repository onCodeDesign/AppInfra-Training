using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.DataModel;

public interface IAuditable
{
    DateTime ModifiedDate { get; set; }
}