using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Specialist
{
    public class SpecialistsService : ISpecialistsService
    {
        public List<Model.Specialist> GetSpecialist()
        {
            List<Model.Specialist> objSpecialist = new List<Model.Specialist>();
            try
            {
                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {

                    objSpecialist = dbContext.Specialists.ToList();
                }
            }
            catch (Exception ex)
            { }
            return objSpecialist;
        }
    }
}
