using Engine.BO.DocsControl;
using Engine.BO;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Constants;

namespace Engine.BL.Actuators3
{
    public class DocumentBL : BaseBL<DocsControlDAL>
    {
        public ResultInsert SetDocument(Document document) => Dal.SetDocument(document, C.GLOBAL_USER);

        public List<Document> GetDocuments(int? id = null) => Dal.GetDocuments(id);
        public Document? GetDocument(int id) => GetDocuments(id).FirstOrDefault();
        public ResultInsert SetFile(DocFileExt file) => Dal.SetDocFile(file, C.GLOBAL_USER);

        public ResultInsert SetExistingFile(DocFileExt file )
        {

            int docId = (int)file.Document.Id;
            var doc = GetDocument(docId);

            if (doc != null)
            {
                file.Id = doc?.File?.Id;
            }


            return SetFile(file);
        }

        public int GetDocTypeId(string code) => Dal.GetTypeId(code);

    }
}
