using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.DAL;
using Engine.BO;
using Engine.BL;

namespace Engine.BL
{
    public class StudentsBL : BaseBL
    {
        public static List<StudentExt> GetStudents(int? id = null) => CompleteStudents(Dal.GetStudents(id));

        public static List<StudentExt> GetGroupStudents(int groupId)
        {
            List<StudentExt> students = Dal.GetGroupStudents(groupId);

            for(int i = 0; i < students.Count; i++)
            {
                StudentExt s = students[i];
                if (s.IsValid() && s != null)
                {
                    var temp = GetStudent(s.Id);
                    if (temp != null)
                    {
                        s = temp;
                        students[i] = s;
                    }                    
                }
            }

            return students;
        }            

        public static StudentExt? GetStudent(int id) => GetStudents(id).FirstOrDefault();

        private static List<StudentExt> CompleteStudents(List<StudentExt> students)
        {
            foreach(var s in students)            
                CompleteStudent(s);            

            return students;
        }

        private static void CompleteStudent(Student? s)
        {
            if(s != null && s.IsValid() && s.Marital != null && s.Genre != null && s.Address != null && s.Scholarly != null && s.Labor != null && s.BirthPlace != null)
            {
                s.Marital = CatalogsBL.GetAsset(s.Marital.Id);
                s.Genre = CatalogsBL.GetAsset(s.Genre.Id);
                s.Address = CatalogsBL.GetAddress(s.Address.Id);
                s.Scholarly = CatalogsBL.GetScholarly(s.Scholarly.Id);
                s.Labor = CatalogsBL.GetLabor(s.Labor.Id);
                s.BirthPlace = CatalogsBL.GetLocation(s.BirthPlace.Id);                
            }
        }

    }
}
