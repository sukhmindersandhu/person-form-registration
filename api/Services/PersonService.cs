using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace api
{
    public class PersonService : IPersonService
    {
        private readonly IPersistence persistence;

        public PersonService(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        public async Task<string> SavePerson(Person person)
        {
            return await this.persistence.Save(person);
        }
    }
}
