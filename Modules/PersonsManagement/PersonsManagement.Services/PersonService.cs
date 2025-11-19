using AppBoot.DependencyInjection;
using Contracts.PersonsManagement;
using DataAccess;
using PersonsManagement.DataModel.Persons;

namespace PersonsManagement.Services;

[Service(typeof(IPersonService))]
internal class PersonService(IRepository repository) : IPersonService
{
    public int AddPerson(PersonData personData)
    {
        using (IUnitOfWork uof = repository.CreateUnitOfWork())
        {
            var person = new Person
            {
                NameStyle = personData.NameStyle,
                Title = personData.Title,
                FirstName = personData.FirstName,
                MiddleName = personData.MiddleName,
                LastName = personData.LastName,
                Suffix = personData.Suffix,
                EmailAddress = personData.EmailAddress,
                Phone = personData.Phone,
                CompanyName = personData.CompanyName,
            };

            uof.Add(person);
            uof.SaveChanges();

            return person.PersonID;
        }
    }

    public PersonData[] GetAllPersons()
    {
        var persons = repository.GetEntities<Person>()
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Select(p => new PersonData
            {
                PersonID = p.PersonID,
                NameStyle = p.NameStyle,
                Title = p.Title,
                FirstName = p.FirstName,
                MiddleName = p.MiddleName,
                LastName = p.LastName,
                Suffix = p.Suffix,
                EmailAddress = p.EmailAddress,
                Phone = p.Phone,
                CompanyName = p.CompanyName,
                ModifiedDate = p.ModifiedDate
            })
            .ToArray();

        return persons;
    }
}
