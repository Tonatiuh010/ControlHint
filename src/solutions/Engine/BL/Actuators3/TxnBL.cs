using Engine.BO;
using Engine.Constants;
using Engine.BO.DocsControl;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BL.Actuators3
{
    public class TxnBL : BaseBL<DocsControlDAL>
    {
        private readonly DocumentBL bl = new DocumentBL();
        private readonly ApproverBL apprBl = new ApproverBL();

        public DocumentTransaction GetTransaction(int documentId, int? approverId = null)
        {
            var txn = Dal.GetDocTransanctions(documentId, approverId);

            CompleteDocTransaction(txn);

            return txn;
        }

        public Result SetDocumentTxn(int documentId, string key) => Dal.CreateTransactionDocument(documentId, key, C.GLOBAL_USER);

        public Result SetApproverTxn(ApproverStep step, int documentId) => Dal.SetApproverTransaction(step, documentId, C.GLOBAL_USER);

        private void CompleteDocTransaction(DocumentTransaction txn)
        {
            if (txn.Document != null && txn.Document.IsValid())
            {
                txn.Document = bl.GetDocument((int)txn?.Document?.Id);
            }

            if (txn.Approvers != null )
            {
                foreach (var stepApprover in txn.Approvers)
                {
                    stepApprover.DocumentDetail = apprBl.GetDocApprover((int)stepApprover.DocumentDetail.Id);
                }
            }            
        }
    }
}
