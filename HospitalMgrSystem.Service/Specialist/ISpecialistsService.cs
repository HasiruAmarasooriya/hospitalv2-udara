using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Specialist
{
    public interface ISpecialistsService
    {
        public List<Model.Specialist> GetSpecialist();
    }
}
