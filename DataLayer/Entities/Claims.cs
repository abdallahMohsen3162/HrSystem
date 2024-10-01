using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Claims
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim(AuthConstants.Employee.Show, AuthConstants.Employee.Show),
            new Claim(AuthConstants.Employee.Add, AuthConstants.Employee.Add),
            new Claim(AuthConstants.Employee.Edit, AuthConstants.Employee.Edit),
            new Claim(AuthConstants.Employee.Delete, AuthConstants.Employee.Delete),

            new Claim(AuthConstants.Attendance.Show, AuthConstants.Attendance.Show),
            new Claim(AuthConstants.Attendance.Add, AuthConstants.Attendance.Add),
            new Claim(AuthConstants.Attendance.Edit, AuthConstants.Attendance.Edit),
            new Claim(AuthConstants.Attendance.Delete, AuthConstants.Attendance.Delete),

            new Claim(AuthConstants.Progress.Show, AuthConstants.Progress.Show),
            new Claim(AuthConstants.Progress.Add, AuthConstants.Progress.Add),
            new Claim(AuthConstants.Progress.Edit, AuthConstants.Progress.Edit),
            new Claim(AuthConstants.Progress.Delete, AuthConstants.Progress.Delete),

            new Claim(AuthConstants.GeneralSettings.Show, AuthConstants.GeneralSettings.Show),
            new Claim(AuthConstants.GeneralSettings.Add, AuthConstants.GeneralSettings.Add),
            new Claim(AuthConstants.GeneralSettings.Edit, AuthConstants.GeneralSettings.Edit),
            new Claim(AuthConstants.GeneralSettings.Delete, AuthConstants.GeneralSettings.Delete),

            new Claim(AuthConstants.Permissions.Show, AuthConstants.Permissions.Show),
            new Claim(AuthConstants.Permissions.Add, AuthConstants.Permissions.Add),
            new Claim(AuthConstants.Permissions.Edit, AuthConstants.Permissions.Edit),
            new Claim(AuthConstants.Permissions.Delete, AuthConstants.Permissions.Delete),

            new Claim(AuthConstants.Salary.Show, AuthConstants.Salary.Show),
            new Claim(AuthConstants.Salary.Add, AuthConstants.Salary.Add),
            new Claim(AuthConstants.Salary.Edit, AuthConstants.Salary.Edit),
            new Claim(AuthConstants.Salary.Delete, AuthConstants.Salary.Delete),

            new Claim(AuthConstants.Group.Show, AuthConstants.Group.Show),
            new Claim(AuthConstants.Group.Add, AuthConstants.Group.Add),
            new Claim(AuthConstants.Group.Edit, AuthConstants.Group.Edit),
            new Claim(AuthConstants.Group.Delete, AuthConstants.Group.Delete),

            new Claim(AuthConstants.Department.Show, AuthConstants.Department.Show),
            new Claim(AuthConstants.Department.Add, AuthConstants.Department.Add),
            new Claim(AuthConstants.Department.Edit, AuthConstants.Department.Edit),
            new Claim(AuthConstants.Department.Delete, AuthConstants.Department.Delete),


        };
    }



}


//public class Department
//{
//    public const string Show = "showDepartment";
//    public const string Add = "addDepartment";
//    public const string Edit = "editDepartment";
//    public const string Delete = "deleteDepartment";

//}