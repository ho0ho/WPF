using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_oracleDB
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_getEmployeeID_Click(object sender, RoutedEventArgs e)
        {
            long id;
            if (Int64.TryParse(txt_employeeID.Text, out id))
            {
                try
                {
                    Grid2.DataContext = HrDAC.Instance.GetEmployee(id);
                }
                catch
                {
                    MessageBox.Show("Error contacting database.");
                }
            }
            else
            {
                MessageBox.Show("Invalid ID.");
            }


            //Employees emp = new Employees();
            //emp = HrDAC.Instance.GetEmployee(long.Parse(txtBox_EmployeeID.Text));

            //txtBox_FirstName.Text = emp.First_name;
            //txtBox_LastName.Text = emp.Last_name;
            //txtBox_Email.Text = emp.Email;
            //txtBox_PhoneNumber.Text = emp.Phone_number;
            //txtBox_HireDate.Text = emp.Hire_date.ToShortDateString();
            //txtBox_JobID.Text = emp.Job_id;
            //txtBox_Salary.Text = emp.Salary.ToString();
            //txtBox_CommissionPCT.Text = emp.CommissionPCT.ToString();
            //txtBox_ManagerID.Text = emp.Manager_id.ToString();
            //txtBox_DepartmentID.Text = emp.Department_id.ToString();
        }

        private void BtnAllSearch_Click(object sender, EventArgs e)
        {
            //List<Employees> lemp = new List<Employees>();
            //lemp = HrDAC.Instance.GetEmployees();

            //lv_listEmployees.Items.Clear();
            //foreach (Employees emp in lemp)
            //{
            //    ListViewItem lvi = new ListViewItem(emp.Employee_id.ToString());
            //    lvi.SubItems.Add(emp.First_name);
            //    lvi.SubItems.Add(emp.Last_name);
            //    lvi.SubItems.Add(emp.Email);
            //    lvi.SubItems.Add(emp.Phone_number);
            //    lvi.SubItems.Add(emp.Hire_date.ToString());
            //    lvi.SubItems.Add(emp.Job_id.ToString());
            //    lvi.SubItems.Add(emp.Salary.ToString());
            //    lvi.SubItems.Add(emp.CommissionPCT.ToString());
            //    lvi.SubItems.Add(emp.Manager_id.ToString());
            //    lvi.SubItems.Add(emp.Department_id.ToString());
            //    lv_listEmployees.Items.Add(lvi);
            //}

            //lv_listEmployees.EndUpdate();
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
