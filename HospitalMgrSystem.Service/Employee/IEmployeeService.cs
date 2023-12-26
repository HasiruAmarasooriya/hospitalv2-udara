using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Employee
{
    public interface IEmployeeService
    {
        public HospitalMgrSystem.Model.Employee CreateEmployeen(HospitalMgrSystem.Model.Employee employee);
        public List<Model.Employee> GetAllEmployeeByStatus();
        public Model.Employee GetAEmployeenByID(int? id);
        public HospitalMgrSystem.Model.Employee DeleteEmployee(HospitalMgrSystem.Model.Employee employee);
       // public List<Model.Investigation> SearchInvestigation(string value);
    }
}
