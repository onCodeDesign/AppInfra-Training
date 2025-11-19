using AppBoot.DependencyInjection;
using Contracts.PersonsManagement;
using Contracts.Sales;
using DataAccess;
using Sales.DataModel.SalesLT;

namespace Sales.Services;

[Service(typeof(ICustomerImportService))]
internal class CustomerImportService(IPersonService personService, IRepository repository) : ICustomerImportService
{
    public CustomerImportResult ImportPersonsAsCustomers()
    {
        var result = new CustomerImportResult();

        var persons = personService.GetAllPersons();
        result.TotalPersonsProcessed = persons.Length;

        using (IUnitOfWork uof = repository.CreateUnitOfWork())
        {
            var existingCustomers = uof.GetEntities<Customer>()
                .ToList();

            foreach (var person in persons)
            {
                // Find matching customer by FirstName and LastName
                var matchingCustomer = existingCustomers
                    .FirstOrDefault(c => 
                        c.FirstName.Equals(person.FirstName, StringComparison.OrdinalIgnoreCase) &&
                        c.LastName.Equals(person.LastName, StringComparison.OrdinalIgnoreCase));

                if (matchingCustomer == null)
                {
                    // No match found - ADD new customer
                    var newCustomer = CreateCustomerFromPerson(person);
                    uof.Add(newCustomer);
                    result.CustomersAdded++;
                }
                else
                {
                    // Match found - check if UPDATE is needed
                    if (person.ModifiedDate > matchingCustomer.ModifiedDate)
                    {
                        // Person is newer - UPDATE customer
                        UpdateCustomerFromPerson(matchingCustomer, person);
                        result.CustomersUpdated++;
                    }
                    else
                    {
                        // Customer is up-to-date - SKIP
                        result.CustomersSkipped++;
                    }
                }
            }

            uof.SaveChanges();
        }

        return result;
    }

    private Customer CreateCustomerFromPerson(PersonData person)
    {
        return new Customer
        {
            NameStyle = person.NameStyle,
            Title = person.Title,
            FirstName = person.FirstName,
            MiddleName = person.MiddleName,
            LastName = person.LastName,
            Suffix = person.Suffix,
            EmailAddress = person.EmailAddress,
            Phone = person.Phone,
            CompanyName = person.CompanyName ?? "Imported from Persons",
            SalesPerson = $"adventure-works\\imported-from-persons-{person.PersonID}",
        };
    }

    private void UpdateCustomerFromPerson(Customer customer, PersonData person)
    {
        customer.NameStyle = person.NameStyle;
        customer.Title = person.Title;
        customer.FirstName = person.FirstName;
        customer.MiddleName = person.MiddleName;
        customer.LastName = person.LastName;
        customer.Suffix = person.Suffix;
        customer.EmailAddress = person.EmailAddress;
        customer.Phone = person.Phone;
        
        if (!string.IsNullOrWhiteSpace(person.CompanyName))
        {
            customer.CompanyName = person.CompanyName;
        }
    }
}
