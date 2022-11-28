using Engine.BO;
using Engine.BO.DocsControl;
using Engine.Constants;
using Engine.Services;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics.SymbolStore;
using Engine.BO.AccessControl;

namespace Engine.DAL
{
    public class DocsControlDAL : BaseDAL
    {
        public delegate void DALCallback(DocsControlDAL dal);
        private static ConnectionString? _ConnectionString => ConnectionString.InstanceByName(C.DOCS_DB);
        public static DocsControlDAL Instance => new();
        public DocsControlDAL() : base(_ConnectionString) { }

        public ResultInsert SetApprover(Approver Approver, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_APPROVER;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_APPROVER", Approver.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_FULLNAME", Approver.FullName, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_POSITION", Approver.Position?.PositionId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", Approver.Position?.Department?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
              (ex, msg) => SetExceptionResult("DocsControlDAL.SetApprover", msg, ex, result),
              () => SetResultInsert(result, Approver)
            );
            return result;
        }

        /*ESTA MADRE BL*/
        public ResultInsert SetDocApprover(DocApprover DocsApprover, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_DOCS_APPROVER;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_DOCAPPROVER", DocsApprover.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DOCFLOW", DocsApprover.DocFlow.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_APPROVER", DocsApprover.Approver.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_SEQUENCE", DocsApprover.Sequence, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", DocsApprover.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_ACTION", DocsApprover.Action, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.SetDocsApprover", msg, ex, result),
                () => SetResultInsert(result, DocsApprover)
            );

            return result;
        }

        public ResultInsert SetDocFile(DocFileExt DocFile, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_DOCS_FILE;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_FILE_ID", DocFile.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DOCUMENT_ID", DocFile.Document.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DOC_IMG", DocFile.HexImg, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DOC_OBJ", DocFile.Params, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.SetDocFile", msg, ex, result),
                () => SetResultInsert(result, DocFile)
            );

            return result;
        }

        public ResultInsert SetDocFlow(DocFlow DocFlow, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_DOCS_FLOW;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_DOC_FLOW_ID", DocFlow.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_TYPE_ID", DocFlow?.DocType?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_KEY1", DocFlow?.Key1, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_KEY2", DocFlow?.Key2, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_KEY3", DocFlow?.Key3, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_KEY4", DocFlow?.Key4, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.SetDocFlow", msg, ex, result),
                () => SetResultInsert(result, DocFlow)
            );
            return result;
        }
        
        public ResultInsert SetDocType(DocType DocType, string txnUser)
        {
            ResultInsert result = new ();
            string sSp = SQL.SET_DOCS_TYPE;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_TYPE_ID", DocType.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_TYPE_CODE", DocType.TypeCode, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.SetDocType", msg, ex, result),
                ()=> SetResultInsert(result, DocType)
            );
            return result;
        }

        public ResultInsert SetDocument(Document Document, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_DOCUMENT;

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_DOCUMENT_ID", Document.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", Document.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_TYPE_ID", Document.DocType?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.SetDocument", msg, ex, result),
                () => SetResultInsert(result, Document)
            );
            return result;
        }

        public List<Document> GetDocuments(int? DocumentId)
        {
            List<Document> model = new();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_DOCUMENTS, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DOCUMENT_ID", DocumentId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new Document()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["DOCUMENT_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        DocType = new()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["TYPE_ID"]),
                            TypeCode = Validate.getDefaultStringIfDBNull(reader["TYPE_CODE"])
                        },
                        File = new ()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["FILE_ID"]),
                            DocImg = Validate.getDefaultBytesIfDBNull(reader["DOC_IMG"]),
                            Params = Validate.getDefaultStringIfDBNull(reader["DOCUMENT_OBJ"])
                        }

                    });
                }
                reader.Close();
            },
                (ex, msg) => SetExceptionResult("DocsControl.GetDocuments", msg, ex)
            );
            return model;
        }

        public List<DocFlow> GetDocFlows(int? DocFlowId)
        {
            List<DocFlow> model = new();

            TransactionBlock(this, ()=>
            {
                using var cmd = CreateCommand(SQL.GET_FLOWS, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DOC_FLOW_ID", DocFlowId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new DocFlow()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["DOC_FLOW_ID"]),
                        DocType = new()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["TYPE_ID"]),
                            TypeCode = Validate.getDefaultStringIfDBNull(reader["TYPE_CODE"])
                        },
                        Key1 = Validate.getDefaultStringIfDBNull(reader["KEY1"]),
                        Key2 = Validate.getDefaultStringIfDBNull(reader["KEY2"]),
                        Key3 = Validate.getDefaultStringIfDBNull(reader["KEY3"]),
                        Key4 = Validate.getDefaultStringIfDBNull(reader["KEY4"])
                    });
                }
                reader.Close();
            },
                (ex, msg) => SetExceptionResult("DocsControl.GetDocFlows", msg, ex)
            );
            return model;
        }

        public List<DocApprover> GetFlowsApprover(int? DocsApprover)
        {
            List<DocApprover> model = new();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_FLOWS_APPROVERS, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DOC_FLOW_ID", DocsApprover, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new DocApprover()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["DOC_APPROVER_ID"]),
                        DocFlow = new()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["DOC_FLOW_ID"]),
                            DocType = new()
                            {
                                Id = Validate.getDefaultIntIfDBNull(reader["TYPE_ID"]),
                                
                            },
                            Key1 = Validate.getDefaultStringIfDBNull(reader["KEY1"]),
                            Key2 = Validate.getDefaultStringIfDBNull(reader["KEY2"]),
                            Key3 = Validate.getDefaultStringIfDBNull(reader["KEY3"]),
                            Key4 = Validate.getDefaultStringIfDBNull(reader["KEY4"])
                        },
                        Approver = new()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["APPROVER_ID"]),
                        },
                        Sequence = Validate.getDefaultIntIfDBNull(reader["SEQUENCE"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        Action = Validate.getDefaultStringIfDBNull(reader["ACTION"])
                    }) ;
                }
                reader.Close();
            },
                (ex,msg) => SetExceptionResult("DocsControlDAL.GetFlowsApprover", msg, ex)
            );
            return model;
        }
        public List<Approver> GetApprovers(int? Approver)
        {
            List<Approver> model = new List<Approver> ();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_APPROVERS, CommandType.StoredProcedure);
                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_APPROVER_ID", Approver, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new Approver()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["APPROVER_ID"]),
                        FullName = Validate.getDefaultStringIfDBNull(reader["FULL_NAME"]),
                        Position = new Position()
                        {
                            PositionId = Validate.getDefaultIntIfDBNull(reader["POSITION_ID"]),
                            Department = new Department()
                            {
                                Id = Validate.getDefaultIntIfDBNull(reader["DEPTO_ID"]),
                            }
                        }
                    });
                }
                reader.Close();
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.GetApprovers", msg, ex)
            );
            return model;
        }

        public DocumentTransaction GetDocTransanctions(int documentId, int? approverId)
        {
            DocumentTransaction model = new DocumentTransaction()
            {
                Document = null,
                Approvers = new List<ApproverStep>()
            };            

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_DOC_TXN, CommandType.StoredProcedure);
                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DOCUMENT_ID", documentId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_APPROVER_ID", approverId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Document ??= new Document()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["DOCUMENT_ID"]),
                        DocType = new DocType()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["TYPE_ID"]),
                            TypeCode = Validate.getDefaultStringIfDBNull(reader["TYPE_CODE"])
                        }
                    };

                    model.Approvers.Add(new ApproverStep()
                    {
                        DocumentDetail = new DocApprover()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["DOC_APPROVER_ID"]),                            
                            Approver = new()
                            {
                                Id = Validate.getDefaultIntIfDBNull(reader["APPROVER_ID"]),
                                FullName = Validate.getDefaultStringIfDBNull(reader["FULL_NAME"]),                                
                            },
                            Sequence = Validate.getDefaultIntIfDBNull(reader["SEQUENCE"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),                           
                        },
                        Comments = Validate.getDefaultStringIfDBNull(reader["COMMENTS"]),
                        Depto = Validate.getDefaultStringIfDBNull(reader["DEPTO"]),
                        Status = Validate.getDefaultStringIfDBNull(reader["STATUS_TXN"])
                    });
                }
                reader.Close();
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.GetDocTransanctions", msg, ex)
            );
            return model;
        }        

        public int GetTypeId(string code)
        {
            int id = 0;
            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(QueryGetDocTypeId(code), CommandType.Text);

                id = int.Parse(cmd.ExecuteScalar().ToString());
                
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.GetTypeId", msg, ex)
            );


            return id;
        }

        public Result CreateTransactionDocument(int documentId, string key, string txnUser)
        {
            Result result = new();
            string sSp = SQL.SET_DOCUMENT_TXN;

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_DOCUMENT_ID", documentId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_KEY", key, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.CreateTransactionDocument", msg, ex, result)
            );
            return result;
        }

        public Result SetApproverTransaction(ApproverStep transaction, int documentId, string txnUser)
        {
            Result result = new();
            string sSp = SQL.SET_DOCUMENT_TXN;

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);
                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_DOCUMENT_ID", documentId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_APPROVER_ID", transaction?.DocumentDetail?.Approver?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DOC_APP_ID", transaction?.DocumentDetail?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_STATUS", transaction?.Status, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_COMMENTS", transaction?.Comments, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.SetApproverTransaction", msg, ex, result)
            );
            return result;
        }

        #region Crazy Queries

        private static string QueryGetDocTypeId(string code)
        {
            return $"SELECT TYPE_ID FROM DOC_TYPE WHERE TYPE_CODE = '{code}'";
        }

        #endregion
    }
}