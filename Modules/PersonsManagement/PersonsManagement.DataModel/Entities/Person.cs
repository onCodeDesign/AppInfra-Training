using System.CodeDom.Compiler;

namespace PersonsManagement.DataModel.Persons;

[GeneratedCode("Manual", "1.0.0.0")]
[Serializable]
public partial class Person
{
    public int PersonID { get; set; }
    public bool NameStyle { get; set; }
    public string? Title { get; set; }
    public string FirstName { get; set; } = "";
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = "";
    public string? Suffix { get; set; }
    public string? EmailAddress { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public string PasswordHash { get; set; } = "";
    public string PasswordSalt { get; set; } = "";
    public Guid Rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }

    public Person()
    {
    }
}
