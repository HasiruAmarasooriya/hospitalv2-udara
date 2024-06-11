using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Service.Employee
{
    public class EmployeeService
    {


        public HospitalMgrSystem.Model.EmployeeDetail CreateEmployeen(HospitalMgrSystem.Model.EmployeeDetail employee)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (employee.Id == 0)
                {
                    dbContext.employeeDetails.Add(employee);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.EmployeeDetail result = (from p in dbContext.employeeDetails where p.Id == employee.Id select p).SingleOrDefault();
                    result.Id = employee.Id;
                    result.FullName = employee.FullName;
                    result.NIC = employee.NIC;
                    result.MobileNumber = employee.MobileNumber;
                    result.TelephoneNumber = employee.TelephoneNumber;
                    result.DOB = employee.DOB;
                    result.Sex = employee.Sex;
                    result.Religion = employee.Religion;
                    result.Nationality = employee.ModifiedUser;
                    result.Address = employee.Address;
                    result.Status = employee.Status;
                    result.CreateUser = employee.CreateUser;
                    result.ModifiedUser = employee.ModifiedUser;
                    result.CreateDate= employee.CreateDate;
                    result.ModifiedDate = DateTime.Now;


                    dbContext.SaveChanges();
                }
                return dbContext.employeeDetails.Find(employee.Id);
            }
        }

        public List<Model.EmployeeDetail> GetAllEmployeeByStatus()
        {
            List<Model.EmployeeDetail> mtList = new List<Model.EmployeeDetail>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.employeeDetails
                    .Where(o => o.Status == CommonStatus.Active).ToList<Model.EmployeeDetail>();

            }
            return mtList;
        }

        public Model.EmployeeDetail GetAEmployeenByID(int? id)
        {

            Model.EmployeeDetail employee = new Model.EmployeeDetail();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                employee = dbContext.employeeDetails.First(o => o.Id == id);

            }
            return employee;
        }

        public HospitalMgrSystem.Model.EmployeeDetail DeleteEmployee(HospitalMgrSystem.Model.EmployeeDetail employee)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.EmployeeDetail result = (from p in dbContext.employeeDetails where p.Id == employee.Id select p).SingleOrDefault();
                result.Status = CommonStatus.Delete;
                dbContext.SaveChanges();
                return result;
            }
        }

        //public List<Model.Investigation> SearchInvestigation(string value)
        //{
        //    List<Model.Investigation> mtList = new List<Model.Investigation>();
        //    if (value == null) { value = ""; }
        //    using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
        //    {
        //        mtList = dbContext.Investigation
        //            .Include(c => c.InvestigationCategory)
        //            .Include(c => c.InvestigationSubCategory)
        //            .Where(o => (o.InvestigationName.Contains(value) || o.Description.Contains(value)
        //        || o.InvestigationCategory.Investigation.Contains(value)) && o.Status == 0).ToList<Model.Investigation>();

        //    }
        //    return mtList;
        //}
    }
}
