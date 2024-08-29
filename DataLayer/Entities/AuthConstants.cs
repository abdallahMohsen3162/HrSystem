using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class AuthConstants
    {
        public class Employee
        {
            public const string Show = "showEmployee";
            public const string Add = "addEmployee";
            public const string Edit = "editEmployee";
            public const string Delete = "deleteEmployee";
        }
        public class Attendance
        {
            public const string Show = "showAttendance";
            public const string Add = "addAttendance";
            public const string Edit = "editAttendance";
            public const string Delete = "deleteAttendance";
        }
        public class Progress
        {
            public const string Show = "showProgress";
            public const string Add = "addProgress";
            public const string Edit = "editProgress";
            public const string Delete = "deleteProgress";
        }

        public class GeneralSettings 
        { 
            public const string Show = "showGeneralSettings";
            public const string Add = "addGeneralSettings";
            public const string Edit = "editGeneralSettings";
            public const string Delete = "deleteGeneralSettings";
        }

        public class Permissions
        {
            public const string Show = "showPremissions";
            public const string Add = "addPremissions";
            public const string Edit = "editPremissions";
            public const string Delete = "deletePremissions";
        }

        public class Salary
        {
            public const string Show = "showSalary";
            public const string Add = "addSalary";
            public const string Edit = "editSalary";
            public const string Delete = "deleteSalary";
        }

        public class Group
        {
            public const string Show = "showGroup";
            public const string Add = "addGroup";
            public const string Edit = "editGroup";
            public const string Delete = "deleteGroup";
        }

    }
}
