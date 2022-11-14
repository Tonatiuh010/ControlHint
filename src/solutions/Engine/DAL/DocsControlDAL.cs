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
            string sSp = SQL.CONTROL_ACCESS_APPROVER_INSERT;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_APPROVER", Approver.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_FULLNAME", Approver.FullName, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_POSITION", Approver.PositionID, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", Approver.DeptoID, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(sSp);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
              (ex, msg) => SetExceptionResult("DocsControlDAL.SetApprover", msg, ex, result),
              () => SetResultInsert(result, Approver)
            );
            return result;
        }

        public ResultInsert SetDocApprover(DocsApprover DocsApprover, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.CONTROL_ACCESS_DOCAPPROVER_INSERT;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_DOCAPPROVER", DocsApprover.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DOCFLOW", DocsApprover.DocFlowID, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_APPROVER", DocsApprover.ApproverID, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_SEQUENCE", DocsApprover.Sequence, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", DocsApprover.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_ACTION", DocsApprover.Action, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.SetDocsApprover", msg, ex, result),
                () => SetResultInsert(result, DocsApprover)
            );

            return result;
        }

        public ResultInsert SetDocFile(DocFile DocFile, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.CONTROL_ACCESS_DOC_FILE_INSERT;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_FILE_ID", DocFile.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DOCUMENT_ID", DocFile.DocumentID, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DOC_IMG", DocFile.DocImg, MySqlDbType.String));
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
            string sSp = SQL.CONTROL_ACCESS_DOC_FLOW_INSERT;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_DOC_FLOW_ID", DocFlow.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_TYPE_ID", DocFlow.TypeID, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_KEY1", DocFlow.Key1, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_KEY2", DocFlow.Key2, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_KEY3", DocFlow.Key3, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_KEY4", DocFlow.Key4, MySqlDbType.String));
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
            string sSp = SQL.CONTROL_ACCESS_DOC_TYPE_INSERT;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

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
            string sSp = SQL.CONTROL_ACCESS_DOCUMENT_INSERT;

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_DOCUMENT_ID", Document.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", Document.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_TYPE_ID", Document.TypeID, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("DocsControlDAL.SetDocument", msg, ex, result),
                () => SetResultInsert(result, Document)
            );
            return result;
        }
    }
}
