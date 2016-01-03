using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CreatoshDAL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        /*
         * Add a new game to the DB.
         * @param name - application name.
         * @param description - application description.
         * @param publish_date - the date the game was published.
         * @param creator_ID - the id of the creator of the game.
         * @param grade_level - the grade level of the game.
         * @param subject_name - the subject of the application.
         * @return GameID. ERROR message if operation failed.
         * */
        [OperationContract]
        string AddGame(string name, string description, string publish_date, string creator_ID, string grade_level, string subject_name);

        /*
         * Update game details and set to publish for others to see.
         * @param gameID - game ID.
         * @param categories - the categories that was decided by the teacher.
         * @param teacher_comment - the comment the teacher wrote on the game.
         * @return SUCCESS or ERROR message.
         * */
        [OperationContract]
        string PublishGame(string gameID, string[] categories, string teacher_comment);
        
        /*
         * Update game details and set to return to creator.
         * @param gameID - game ID.
         * @param teacher_comment - the comment the teacher wrote on the game.
         * @param metrics - list of metrics on which the teacher value the game.
         * @param values - list of values where value[i] is the value of metrics[i].
         * @return SUCCESS or ERROR message.
         * */
        [OperationContract]
        string ReturnGameForReCreate(string gameID, string teacher_comment, string[] metrics, string[] values);

        /*
         * Update game details and set to be checked again by the teacher.
         * @param gameID - game ID.
         * @param name - fixed application name.
         * @param decription - fixed application decription.
         * @param publish_date - the date the game was published.
         * @return SUCCESS or ERROR message.
         * */
        [OperationContract]
        string ReAddGame(string gameID, string name, string description, string publish_date);
        
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.

}
