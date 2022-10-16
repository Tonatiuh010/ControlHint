using Engine.DAL;
using Engine.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Engine.BL
{
    public class GroupBL : BaseBL
    {        

        public static List<Group> GetGroups(int? id = null, string? code = null)
        {
            var groups = Dal.GetGroups(id, code);

            foreach (var g in groups)
                CompleteGroup(g);

            return groups;
        }

        public static Group? GetStudentGroup(int studentId)
            => Dal.GetStudentGroup(studentId);

        public static Group? GetGroup(int id)
            => GetGroups(id).FirstOrDefault();

        private static void CompleteGroup(Group? g)
        {
            if(g != null && g.IsValid() && g.Period != null && g.Field != null && g.Major != null)
            {
                g.Major = CatalogsBL.GetMajor(g.Major.Id);
                g.Period = CatalogsBL.GetAsset(g.Period.Id);
                g.Field = CatalogsBL.GetAsset(g.Field.Id);
            }
        }

    }
}
