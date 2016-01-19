using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.Entity;


namespace CreatoshDAL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ServiceGame : IServiceGame
    {
        int gameID ;
        
        public string AddGame(string name, string description, DateTime publish_date, int creator_ID,
            int grade_level, string template_name, string subject_name)
        {
            Creatosh_DB_Entities model = null;

            gameID=9;
            try
            {
                model = new Creatosh_DB_Entities();
                Game added = model.Games.Add(new Game() { game_ID=gameID ,name = name, description=description, publish_date=publish_date, creator_ID=creator_ID, amount_of_likes=0,
                    grade_level=grade_level, template_name=template_name, teacher_comment="", subject_name=subject_name,status =0});
                model.SaveChanges();
                return added.game_ID.ToString();
            }
            catch (Exception e){
                return "ERROR - " + e.Message;
            }
        }

        /// <summary>
        /// This function return the next available GameID and sets the next GameID
        /// </summary>
        /// <returns></returns>
        private int getNextGameID()
        {
            //lock

            //read from blob

            //write to blob

            //unlock
            return gameID++; //stub
        }

       public string PublishGame(int gameID, string[] categories, string teacher_comment)
        {
            Creatosh_DB_Entities model;
            string message = "";
            try
            {
                model = new Creatosh_DB_Entities();
                Game gameToSet = model.Games.Find(gameID);
                if (gameToSet == null)
                    return "ERROR - Game with the ID " + gameID + " can not be found in Games table";
                
                /* Appdate the  teacher's comment*/
                gameToSet.teacher_comment = teacher_comment;
                gameToSet.status = 2;
                model.SaveChanges();

                /* Add GameID to all Categories*/
                foreach (string category in categories)
                {
                    Category c = model.Categories.Find(category);
                    if (c == null)
                        message += "\nCategory " + category + " can not be found in Categories table";
                    else
                    {
                       /* Games_In_Categories added = model.Games_In_Categories.Add(new Games_In_Categories()
                        {
                            game_ID = gameID,
                            categoryName = category
                        });*/
                        model.SaveChanges();   
                    }

                }
                if (message == "")
                    message = "The Game added to all categories successfully";
                return "SUCCESS - " + message;
            }
            catch(Exception e)
            {
                return "ERROR - " + e.Message;
            }
        }

        public string ReturnGameForReCreate(int gameID, string teacher_comment, string[] metrics, int[] values)
        {
            Creatosh_DB_Entities model;
            
            try
            {
                model = new Creatosh_DB_Entities();
                
                Game gameToSet = model.Games.Find(gameID);
                if (gameToSet == null)
                    return "ERROR - Game with the ID " + gameID + " can not be found in Games table";

                /* Appdate the  teacher's comment*/
                gameToSet.teacher_comment = teacher_comment;
                model.SaveChanges();

                /* Add values to mesures in game */
                for (int i = 0; i < metrics.Length; i++)
                {
                    Measures_Values_In_Game added = model.Measures_Values_In_Game.Add(new Measures_Values_In_Game()
                {
                    game_ID = gameID,
                    subject_name = gameToSet.subject_name,
                    measure = metrics[i],
                    value = values[i]
                });


                model.SaveChanges();
                }
                return "SUCCESS";
            }
            catch (Exception e)
            {
                return "ERROR - " + e.Message;
            }
        }
        
        public string ReAddGame(int gameID, string name, string description, DateTime publish_date)
        {
            Creatosh_DB_Entities model;
            try
            {
                model = new Creatosh_DB_Entities();
                Game gameToSet = model.Games.Find(gameID);
                if (gameToSet == null)
                    return "ERROR - Game with the ID " + gameID + " can not be found in Games table";
                gameToSet.name = name;
                gameToSet.description = description;
                gameToSet.publish_date = publish_date;

                return "SUCCESS";

            }
            catch (Exception e)
            {
                return "ERROR - " + e.Message;
            }
        }

        
    }
}
