using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace api
{
    public class FilePersistence : IPersistence
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public FilePersistence(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> Save(Person person)
        {
            try
            { 
                string path = @$"{hostingEnvironment.ContentRootPath}\Person.txt";
                await File.AppendAllLinesAsync(path, 
                      new[] { $"{person.Id} {person.FirstName} {person.LastName} {person.DOB}" });
                return "Person saved to the file sucessfully!";
            }
            catch(Exception)
            {
                return "Error:Person not saved";
            }
        }
    }
}
