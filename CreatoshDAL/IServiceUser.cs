using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CreatoshDAL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceUser" in both code and config file together.
    [ServiceContract]
    public interface IServiceUser
    {
         /*
         * Login User to system
         * Doing that by set the fild online from 0 to 1
         * @param userID
         * @return SUCCESS or ERROR message.
         * */
        [OperationContract]
        string login(int userID);


        /*
         * Login User to system
         * Doing that by set the fild online from 1 to 0
         * @param userID
         * @return SUCCESS or ERROR message.
         * */
        [OperationContract]
        string logout(int userID);

        /*
         * Login User to system
         * Doing that by set the fild online from 0 to 1
         * @param userID
         * @return true - if the user login, false - if the user logout.
         * */
        [OperationContract]
        bool isUserLogin(int userID);


    }

    
}
