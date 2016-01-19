using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace CreatoshDAL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceUser" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceUser.svc or ServiceUser.svc.cs at the Solution Explorer and start debugging.
    public class ServiceUser : IServiceUser
    {
        public string login(int userID)
        {
            Creatosh_DB_Entities model;
            try
            {
                model = new Creatosh_DB_Entities();
                User user = model.Users.Find(userID);
                if (user == null)
                    return "ERROR - User with the ID " + userID + " can not be found in Users table";
                user.online = 1;
                model.SaveChanges();
                int milliseconds = 3000;
                Thread.Sleep(milliseconds);
                return "SUCCESS";
            }
            catch (Exception e)
            {
                return "ERROR - " + e.Message;
            }
        }

        public string logout(int userID)
        {
            Creatosh_DB_Entities model;
            try
            {
                model = new Creatosh_DB_Entities();
                User user = model.Users.Find(userID);
                if (user == null)
                    return "ERROR - User with the ID " + userID + " can not be found in Users table";
                user.online = 0;
                model.SaveChanges();
                int milliseconds = 4000;
                Thread.Sleep(milliseconds);
                return "SUCCESS";
            }
            catch (Exception e)
            {
                return "ERROR - " + e.Message;
            }
        }

        public bool isUserLogin(int userID)
        {
            Creatosh_DB_Entities model;
            try
            {
                model = new Creatosh_DB_Entities();
                User user = model.Users.Find(userID);
                return (user.online == 1);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
