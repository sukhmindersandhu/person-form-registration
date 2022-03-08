using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api
{
    public class Person
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 255)]
        public string FirstName { get; set; }

        [StringLength(maximumLength: 255)]
        public string LastName { get; set; }

        public DateTime DOB { get; set; }
    }
}
