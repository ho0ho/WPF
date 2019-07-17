using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_oracleDB
{
    class Employees
    {
        public long EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public string JobID { get; set; }
        public double Salary { get; set; }
        public double CommissionPCT { get; set; }
        public long ManagerID { get; set; }
        public Int32 DepartmentID { get; set; }

    }
}
