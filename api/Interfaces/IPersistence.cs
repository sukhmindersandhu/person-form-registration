using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api
{
    public interface IPersistence
    {
        Task<string> Save(Person person);
    }
}
