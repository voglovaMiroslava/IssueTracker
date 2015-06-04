using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    interface IPersonManager
    {
        void changeSubscription(string personName, bool subscribe);
        void Add(Person person);
        void Update(Person person);
        List<Person> GetAllEmplo();
        List<Person> GetAllCusto();
        List<Person> GetAll();
        Person GetByName(string name);
        bool NameExists(string name);
    }
}
