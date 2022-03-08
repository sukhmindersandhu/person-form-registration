using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api
{
    public interface IPersonService
    {
        Task<string> SavePerson(Person person);
    }
}
