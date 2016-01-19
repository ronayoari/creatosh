using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFAzureDALServiceWebRole
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string CreateTable(string tableName);

        [OperationContract]
        string DeleteTable(string tableName);
        [OperationContract]
        string AddEntitiesToTable(GameSequenceEntityX[] gameSeqs, string tableName);

        [OperationContract]
        string AddEntityToTable(GameSequenceEntityX gameSeq, string tableName);
        
        [OperationContract]
        List<GameSequenceEntityX> RetrieveAllGameSequenceEntities(string gameID, string tableName);

        [OperationContract]
        GameSequenceEntityX RetrieveGameSequenceEntities(string gameID, string seq, string tableName);

        [OperationContract]
        string UpdateEntityInTable(GameSequenceEntityX gameSeq, string tableName);

        [OperationContract]
        string DeleteEntityInTable(string gameID, string seq, string tableName);
    }

        public class GameSequenceEntity : TableEntity
        {
            public GameSequenceEntity(string gameID, string seq, string template)
            {
                this.PartitionKey = gameID;
                this.RowKey = seq;
                this.Template = template;
            }

            public GameSequenceEntity(GameSequenceEntityX gameSeq) : this(gameSeq.GameID, gameSeq.Sequence, gameSeq.Template)
            {
                
            }

            public GameSequenceEntity() { }

            public string Template { get; set; }
        }

        [DataContract(Name = "GameSequenceEntity")]
        [KnownType(typeof(MultiChoiceSequenceEntityX))]
        public class GameSequenceEntityX
        {

            public GameSequenceEntityX(string gameID, string seq, string template)
            {
                this.GameID = gameID;
                this.Sequence = seq;
                this.Template = template;
            }

            public GameSequenceEntityX() { }

            public static GameSequenceEntityX GameSequenceEntityXFactory(GameSequenceEntity entity)
            {
                GameSequenceEntityX gameSeq = null;
                if (entity.Template.Equals("None"))
                {
                    gameSeq = new GameSequenceEntityX(entity.PartitionKey, entity.RowKey, entity.Template);
                }
                else if (entity.Template.Equals("Multiple Choice"))
                {
                    if (entity is MultiChoiceSequenceEntity)
                    {
                        var multiEntity = (MultiChoiceSequenceEntity)entity;
                        gameSeq = new MultiChoiceSequenceEntityX(multiEntity.PartitionKey, multiEntity.RowKey, multiEntity.BlobName,
                            multiEntity.Question, multiEntity.CorrectAnswer, multiEntity.WrongAnswer1, multiEntity.WrongAnswer2, multiEntity.WrongAnswer3);
                    }
                    else
                    {
                        gameSeq = null;
                    }
                }
                else
                {
                    gameSeq = null;
                }
                return gameSeq;
            }
            [DataMember]
            public string GameID { get; set; }
            [DataMember]
            public string Sequence { get; set; }
            [DataMember]
            public string Template { get; set; }
        }

        public class MultiChoiceSequenceEntity : GameSequenceEntity
        {
            public MultiChoiceSequenceEntity(string gameID, string seq, string blobName, string q, string ca, string wa1, string wa2, string wa3)
                : base(gameID, seq, "Multiple Choice")
            {
                BlobName = blobName;
                Question = q;
                CorrectAnswer = ca;
                WrongAnswer1 = wa1;
                WrongAnswer2 = wa2;
                WrongAnswer3 = wa3;
            }

            public MultiChoiceSequenceEntity(MultiChoiceSequenceEntityX multiEntity) : this(multiEntity.GameID, multiEntity.Sequence, multiEntity.BlobName,
                multiEntity.Question, multiEntity.CorrectAnswer, multiEntity.WrongAnswer1, multiEntity.WrongAnswer2, multiEntity.WrongAnswer3)
            {

            }
            public MultiChoiceSequenceEntity() { }

            public string BlobName { get; set; }

            public string Question { get; set; }

            public string CorrectAnswer { get; set; }

            public string WrongAnswer1 { get; set; }

            public string WrongAnswer2 { get; set; }

            public string WrongAnswer3 { get; set; }

        }

        [DataContract(Name = "MultiChoiceSequenceEntity")]
        public class MultiChoiceSequenceEntityX : GameSequenceEntityX
        {
            public MultiChoiceSequenceEntityX(string gameID, string seq, string blobName, string q, string ca, string wa1, string wa2, string wa3)
                : base(gameID, seq, "Multiple Choice")
            {
                BlobName = blobName;
                Question = q;
                CorrectAnswer = ca;
                WrongAnswer1 = wa1;
                WrongAnswer2 = wa2;
                WrongAnswer3 = wa3;
            }

            public MultiChoiceSequenceEntityX() : base() { }

            [DataMember]
            public string BlobName { get; set; }
            [DataMember]
            public string Question { get; set; }
            [DataMember]
            public string CorrectAnswer { get; set; }
            [DataMember]
            public string WrongAnswer1 { get; set; }
            [DataMember]
            public string WrongAnswer2 { get; set; }
            [DataMember]
            public string WrongAnswer3 { get; set; }

        }
}
