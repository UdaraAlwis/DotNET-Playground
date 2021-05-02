using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp9DemoApp
{
    public class PersonModel
    {
        public int Id { get; init; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public PersonModel()
        {

        }

        public PersonModel(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        //// DOES NOT WORK - init setter
        //public void SetId(int newId) 
        //{
        //    this.Id = newId;
        //}
    }
}
