using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.MySQL;
using Engine.BO;
using Engine.Constants;
using Engine.Interfaces;
using D = Engine.BL.Delegates;

namespace Engine.DAL
{
    public abstract class BaseDAL : MySqlDataBase
    {
        protected static readonly Validate Validate = Validate.Instance;
        public static D.CallbackExceptionMsg? OnDALError { get; set; }

        protected IConnectionString? ConnString { get; set; }

        protected BaseDAL(IConnectionString? conn) : base(conn?.ConnectionString)
        {
            ConnString = conn;
        }             

        protected void SetResultInsert(ResultInsert result, BaseBO bo)
        {
            int id = (int)GetLastId();

            result.InsertDetails = bo.IsValid() ?
                new InsertStatus(bo) :
                new InsertStatus()
                {
                    Id = id,
                    FromObject = bo.GetType()
                };
        }

        protected int GetLastId()
        {
            int id = 0;
            TransactionBlock(this, () => {

                using var cmd = CreateCommand("SELECT LAST_INSERT_ID()", CommandType.Text);
                var result = cmd.ExecuteScalar().ToString();

                if (result != null)
                {
                    id = int.Parse(result);
                }

            }, (ex, msg) => SetExceptionResult($"{this.GetType()}.GetLastId()", msg, ex));

            return id;
        }        

        protected static void GetResult(IDataParameter pResult, string sSp, Result result)
        {

            try
            {
                if (pResult.Value != null)
                {
                    var res = pResult.Value.ToString();

                    if (string.IsNullOrEmpty(res))
                    {
                        result.Status = C.ERROR;
                        result.Message = $"Procedure {sSp} did not return response value";
                    }
                    else if (res != C.OK)
                    {
                        result.Status = C.ERROR;
                        result.Message = res;
                        throw new ArgumentException($"Error From Internal Procedure DB -> Says: {result.Message}");
                    }
                    else
                    {
                        result.Status = res;
                        result.Message = C.COMPLETE;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                result.Status = C.ERROR;
                result.Message = ex.Message;
            }

        }

        protected static void SetExceptionResult(string actionName, string msg, Exception ex, Result? result = null)
            => OnDALError?.Invoke(ex, $"Exception ({actionName}) - {msg}{ ( result != null? " " + result.ToString() : string.Empty ) }");
        
    }
}
