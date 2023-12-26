using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalMgrSystem.Model;

namespace HospitalMgrSystem.Service.User
{
    public interface IUserService
    {
        public HospitalMgrSystem.Model.User GetUserLogin(HospitalMgrSystem.Model.User user);
    }
}
