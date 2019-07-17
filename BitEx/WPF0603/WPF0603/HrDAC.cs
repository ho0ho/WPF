using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF0603
{
    class HrDAC
    {
        private string connectionString;
        private static readonly HrDAC instance = new HrDAC();
        private HrDAC()
        {
            connectionString = Properties.Settings.Default.ConnectionInfo;
        }

        public static HrDAC Instance { get { return instance; } }
        public Employee GetEmployee(long id)
        {
            Employee emp = new Employee();
            string sql = "SELECT * FROM EMPLOYEES WHERE EMPLOYEE_ID = :EMPLOYEE_ID";
            // OracleConnection 객체를 생성한뒤 close 안해줘도 알아서 using문 떠나면 close됨 => using 사용이유
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(sql, conn)
                {
                    CommandType = System.Data.CommandType.Text,
                    BindByName = true
                };
                cmd.Parameters.Add(":EMPLOYEE_ID", OracleDbType.Long).Value = id;
                conn.Open();        // close 꼭 해야함 => 하지만 using문을 썼기 때문에 해줄 필요 X 

                using (OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        // emp.Employee_id = reader.GetIntt64(0); 과 동일
                        //  => reader.GetOrdinal(컬럼명) => 테이블 내에 해당 컬럼명의 인덱스 번호를 리턴 
                        emp.EmployeeID = reader.GetInt64(reader.GetOrdinal("EMPLOYEE_ID"));        // emp.Employee_id = reader.GetIntt64(0);
                        emp.FirstName = reader.IsDBNull(reader.GetOrdinal("FIRST_NAME")) ? "" : reader.GetString(reader.GetOrdinal("FIRST_NAME"));
                        emp.LastName = reader.GetString(reader.GetOrdinal("LAST_NAME"));
                        emp.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                        emp.PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PHONE_NUMBER")) ? "" : reader.GetString(reader.GetOrdinal("PHONE_NUMBER"));
                        emp.HireDate = reader.GetDateTime(reader.GetOrdinal("HIRE_DATE"));
                        emp.JobID = reader.GetString(reader.GetOrdinal("JOB_ID"));
                        emp.Salary = reader.IsDBNull(reader.GetOrdinal("SALARY")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("SALARY"));
                        emp.CommissionPCT = reader.IsDBNull(reader.GetOrdinal("COMMISSION_PCT")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("COMMISION_PCT"));
                        emp.ManagerID = reader.IsDBNull(reader.GetOrdinal("MANAGER_ID")) ? 0 : reader.GetInt64(reader.GetOrdinal("MANAGER_ID"));
                        emp.DepartmentID = reader.IsDBNull(reader.GetOrdinal("DEPARTMENT_ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("DEPARTMENT_ID"));
                    }
                }
            }

            return emp;
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> lemp = new List<Employee>();


            string sql = "SELECT * FROM EMPLOYEES";
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(sql, conn)
                {
                    CommandType = System.Data.CommandType.Text,
                    BindByName = true
                };
                conn.Open();

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee emp = new Employee();
                        emp.EmployeeID = reader.GetInt64(reader.GetOrdinal("EMPLOYEE_ID"));
                        emp.FirstName = reader.IsDBNull(reader.GetOrdinal("FIRST_NAME")) ? "" : reader.GetString(reader.GetOrdinal("FIRST_NAME"));
                        emp.LastName = reader.GetString(reader.GetOrdinal("LAST_NAME"));
                        emp.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                        emp.PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PHONE_NUMBER")) ? "" : reader.GetString(reader.GetOrdinal("PHONE_NUMBER"));
                        emp.HireDate = reader.GetDateTime(reader.GetOrdinal("HIRE_DATE"));
                        emp.JobID = reader.GetString(reader.GetOrdinal("JOB_ID"));
                        emp.Salary = reader.IsDBNull(reader.GetOrdinal("SALARY")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("SALARY"));
                        emp.CommissionPCT = reader.IsDBNull(reader.GetOrdinal("COMMISSION_PCT")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("COMMISSION_PCT"));
                        emp.ManagerID = reader.IsDBNull(reader.GetOrdinal("MANAGER_ID")) ? 0 : reader.GetInt64(reader.GetOrdinal("MANAGER_ID"));
                        emp.DepartmentID = reader.IsDBNull(reader.GetOrdinal("DEPARTMENT_ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("DEPARTMENT_ID"));

                        lemp.Add(emp);
                    }
                }
            }
            return lemp;
        }
    }
}
