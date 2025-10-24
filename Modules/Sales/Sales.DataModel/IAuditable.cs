using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.DataModel;

interface IAuditable
{
    DateTime ModifiedDate { get; set; }
}