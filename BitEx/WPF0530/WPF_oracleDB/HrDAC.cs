using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_oracleDB
{
    class HrDAC
    {
        private string connectionString;
        private static readonly HrDAC instance = new HrDAC();
        private HrDAC()
        {
            connectionString = Properties.Settings.Default.ConnectionString;
            // connectionString = "DATA SOURCE=XE; User Id=c##hr;Password=tiger";
            // 프로젝트 속성 > 설정 > 설정해주고 빌드해보면, 
            // C:\Users\bit\source\repos\WinForm0527\HRDtableWinForm\bin\Debug 내에 HRDtableWinForm.exe.config(.exe.config 파일) 만들어져 있음
            // 이 파일은 => 앞으로 일일히 코드 수정해서 재컴파일해서 만들지 않고, 이 config파일(html)만 수정해서 갖고 다니면
            // 실행파일(.exe)는 실행할 때 .exe.config를 물고 실행하기 때문에 자동으로 적용되게 된다. 
        }

        public static HrDAC Instance { get { return instance; } }
        public Employees GetEmployee(long id)
        {
            Employees emp = new Employees();
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

            /// DB.open()에서 오류남! 
            // 1. C:\app\bit\product\18.0.0\dbhomeXE\network\admin\listenr.ora => 외부 이벤트에 대해 반응하는 파일
            //    자기 컴퓨터 이름 되어 있음 => 이부분을 localhost로 변경하기
            // 2. C:\app\bit\product\18.0.0\dbhomeXE\network\admin\tnsnames.ora 
            //    XE부분의 자기컴퓨터명만 localhost로 바꾸기 
            // 3. sys as sysdba로 접속해서 shutdown immediate; startup; 해주기
            // 4. [환경 변수] - [고급 설정] 에서 시스템변수 추가해주기 
            //    4-1. ORACLE_HOME => C:\app\bit\product\18.0.0\dbhomeXE
            //    4-2. TNS_ADMIN => C:\app\bit\product\18.0.0\dbhomeXE\network\admin 


            return emp;
        }

        public List<Employees> GetEmployees()
        {
            List<Employees> lemp = new List<Employees>();


            string sql = "SELECT * FROM EMPLOYEES";
            // OracleConnection 객체를 생성한뒤 close 안해줘도 알아서 using문 떠나면 close됨 => using 사용이유
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(sql, conn)
                {
                    CommandType = System.Data.CommandType.Text,
                    BindByName = true
                };
                //cmd.Parameters.Add(":EMPLOYEE_ID", OracleDbType.Long).Value = id;
                conn.Open();        // close 꼭 해야함

                using (OracleDataReader reader = cmd.ExecuteReader())   // 하나의 레코드가 아닌, 결과전체를 가져옴
                {
                    while (reader.Read())
                    {
                        Employees emp = new Employees();
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

                        lemp.Add(emp);
                    }
                }
            }

            return lemp;
        }


    }
}
