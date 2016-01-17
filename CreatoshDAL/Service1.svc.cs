using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CreatoshDAL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string AddGame(string name, string description, string publish_date, string creator_ID, string grade_level, string subject_name)
        {
            return "ERROR - not implemented yet";
        }

        public string PublishGame(string gameID, string[] categories, string teacher_comment)
        {
            return "ERROR - not implemented yet";
        }

        public string ReturnGameForReCreate(string gameID, string teacher_comment, string[] metrics, string[] values)
        {
            return "ERROR - not implemented yet";
        }

        public string ReAddGame(string gameID, string name, string description, string publish_date)
        {
            return "ERROR - not implemented yet";
        }
    }
}
