using Engine.BO.AccessControl;
using Engine.BO;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BL.Actuators
{
    public class JobBL : BaseBL<AccessControlDAL>
    {
        public List<Job> GetJobs(int? jobId = null) => Dal.GetJobs(jobId);

        public Job? GetJob(int id) => GetJobs(id).FirstOrDefault();

        public ResultInsert SetJob(Job job, string txnUser) => Dal.SetJob(job, txnUser);
    }
}
