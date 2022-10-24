using System;
using System.Collections.Generic;
using System.Linq;
using Engine.BO;
using Engine.BO.AccessControl;
using Engine.Constants;
using Engine.DAL;

namespace Engine.BL.Actuators
{
    public class PositionBL : BaseBL<AccessControlDAL>
    {

        public List<Position> GetPositions(int? positionId = null, int? jobId = null, int? departmentId = null) 
            => Dal.GetPositions(positionId, jobId, departmentId);

        public Position? GetPosition(int id) => GetPositions(positionId: id).FirstOrDefault();

        public ResultInsert SetPosition(Position position, string txnUser) => Dal.SetPosition(position, txnUser);

    }

}