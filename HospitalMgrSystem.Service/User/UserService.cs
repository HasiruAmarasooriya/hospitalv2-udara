using System.Security.Cryptography;
using System.Text;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Service.User
{
    public class UserService : IUserService
    {
        public HospitalMgrSystem.Model.User GetUserLogin(HospitalMgrSystem.Model.User user)
        {

            Model.User objUser = new Model.User();

            try
            {
                string encodingString = MD5Hash(user.Password).ToUpper();

                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {

                    objUser = dbContext.Users.Where(o => o.UserName == user.UserName && o.Password == encodingString && o.Status == 0).FirstOrDefault();
                }
            }
            catch (Exception ex)
            { }
            return objUser;
        }


        public bool ValidateUserById(Model.User user)
        {

            Model.User objUser = new Model.User();

            try
            {
                string encodingString = MD5Hash(user.Password).ToUpper();

                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {

                    objUser = dbContext.Users.Where(o => o.Id == user.Id && o.Password == encodingString && o.Status == 0).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            if (objUser != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public Model.User CreateUser(Model.User user)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                if (user.Id == 0)
                {
                    string encodingString = MD5Hash(user.Password).ToUpper();
                    user.Password = encodingString;
                    user.CreateDate = DateTime.Now;
                    user.ModifiedDate = DateTime.Now;
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }
                else
                {
                    Model.User result = (from p in dbContext.Users where p.Id == user.Id select p).SingleOrDefault();
                    result.Id = user.Id;
                    result.FullName = user.FullName;
                    result.MobileNumber = user.MobileNumber;
                    result.EmailAddr = user.EmailAddr;
                    result.Status = user.Status;
                    result.CreateUser = user.CreateUser;
                    result.ModifiedUser = user.ModifiedUser;
                    result.CreateDate = user.CreateDate;
                    result.ModifiedDate = DateTime.Now;


                    dbContext.SaveChanges();
                }
                return dbContext.Users.Find(user.Id);
            }
        }

        public static bool ValidateUserAdmin(int userId)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                Model.User user = (from p in dbContext.Users where p.Id == userId select p).SingleOrDefault();

                if (user.userRole == UserRole.ADMIN)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }


        public Model.User ChangeUserPw(Model.User user)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                string encodingString = MD5Hash(user.Password).ToUpper();
                user.Password = encodingString;
                if (user.Id != 0)
                {

                    Model.User result = (from p in dbContext.Users where p.Id == user.Id select p).SingleOrDefault();
                    result.Password = user.Password;
                    result.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
                return dbContext.Users.Find(user.Id);
            }
        }

        public HospitalMgrSystem.Model.User GetUserByID(int Id)
        {

            Model.User objUser = new Model.User();

            try
            {

                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {

                    objUser = dbContext.Users.Where(o => o.Id == Id && o.Status == 0).FirstOrDefault();
                }
            }
            catch (Exception ex)
            { }
            return objUser;
        }

        public List<Model.User> GetAllUsers()
        {
            List<Model.User> mtList = new List<Model.User>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Users.Where(o => o.Status == 0).ToList<Model.User>();

            }
            return mtList;
        }

        public List<Model.User> GetUsersByRole(UserRole userRole)
        {
            List<Model.User> mtList = new List<Model.User>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Users.Where(o => o.Status == 0 && o.userRole == userRole).ToList<Model.User>();

            }
            return mtList;
        }
        public HospitalMgrSystem.Model.User DeleteUser(HospitalMgrSystem.Model.User user)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.User result = (from p in dbContext.Users where p.Id == user.Id select p).SingleOrDefault();
                result.Status = 1;
                dbContext.SaveChanges();
                return result;
            }
        }

        private string MD5Hash(string text)
        {
            _ = new UTF8Encoding(true).GetBytes(text);
            MD5 md5 = new MD5CryptoServiceProvider();
            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.Unicode.GetBytes(text));
            //get hash result after compute it
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}
