using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    interface IPersonManager
    {
        bool changeSubscription(string personName, bool subscribe);
        bool Add(Person person);
        bool Update(Person person);
        List<Person> GetAllEmplo();
        List<Person> GetAllCusto();
        List<Person> GetAll();
        List<Person> GetAllWithSubscribtion();
        Person GetByName(string name);
        bool NameExists(string name);
    }
}
