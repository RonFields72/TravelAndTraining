using SWN.TTR.Repository.Common;
using SWN.TTR.Repository.Dto;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SWN.TTR.Repository.SqlServer
{
    public class OdsSqlRepository : IOdsRepository
    {
        #region IOdsRepository Members

        public SWN.TTR.Repository.Dto.Employee GetEmployeeByNetworkID(string networkId)
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.OdsConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = "SELECT ISNULL(Preferred_FirstName,''), ISNULL(Last_Name,''), ISNULL(Work_Email_Address,''), ISNULL(Full_Network_Id,'') FROM dbo.v_Entity_Employee_By_HR_Alias WHERE Full_Network_ID = @Id;";
                    cm.CommandType = CommandType.Text;
                    cm.Parameters.AddWithValue("@Id", networkId);
                    cm.Connection.Open();
                    using (SqlDataReader rdr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        Employee result = new Employee();
                        // in case duplicate values come back, just pull the last one
                        while (rdr.Read())
                        {
                            result.FirstName = rdr.GetString(0);
                            result.LastName = rdr.GetString(1);
                            result.EmailAddress = rdr.GetString(2);
                            result.NetworkId = rdr.GetString(3);
                        }
                        return result;
                    }
                }
            }
        }

        public System.Collections.Generic.List<SWN.TTR.Repository.Dto.Employee> GetActiveEmployees()
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.OdsConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.Text;
                    cm.CommandText = @"SELECT ISNULL(Preferred_FirstName,''), ISNULL(Last_Name,''), ISNULL(Work_Email_Address,''), ISNULL(Full_Network_Id,'')
                                        FROM dbo.v_Entity_Employee_Active
                                        WHERE LEN(Work_Email_Address) > 0
                                        AND Work_Email_Address <> 'None';";
                    cm.Connection.Open();
                    using (SqlDataReader rdr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        List<Employee> employess = new List<Employee>();
                        while (rdr.Read())
                        {
                            Employee result = new Employee();
                            result.FirstName = rdr.GetString(0);
                            result.LastName = rdr.GetString(1);
                            result.EmailAddress = rdr.GetString(2);
                            result.NetworkId = rdr.GetString(3);
                            employess.Add(result);
                        }
                        return employess;
                    }
                }
            }
        }

        public SWN.TTR.Repository.Dto.Employee GetEmployeeByWorkEmailAddress(string emailAddress)
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.OdsConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = "SELECT ISNULL(Preferred_FirstName,''), ISNULL(Last_Name,''), ISNULL(Work_Email_Address,''), ISNULL(Full_Network_Id,'') FROM dbo.v_Entity_Employee_By_HR_Alias WHERE Work_Email_Address = @Email;";
                    cm.CommandType = CommandType.Text;
                    cm.Parameters.AddWithValue("@Email", emailAddress);
                    cm.Connection.Open();
                    using (SqlDataReader rdr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        Employee result = new Employee();
                        // in case duplicate values come back, just pull the last one
                        while (rdr.Read())
                        {
                            result.FirstName = rdr.GetString(0);
                            result.LastName = rdr.GetString(1);
                            result.EmailAddress = rdr.GetString(2);
                            result.NetworkId = rdr.GetString(3);
                        }
                        return result;
                    }
                }
            }
        }

        public System.Collections.Generic.List<SWN.TTR.Repository.Dto.Employee> GetEmployeesByRole(string system, string subsystem, string role)
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.OdsConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = @"SELECT   EA.[Preferred_FirstName], EA.[Last_Name], EA.[Work_Email_Address], EA.[Network_Id]
                                        FROM   	dbo.Menu_User MU,
		                                        dbo.Menu_Option MO, 
		                                        dbo.Menu_Option_Security MS,
		                                        dbo.v_Entity_Employee_Active EA
                                        WHERE   MS.[System_Id] = MO.[System_Id]
    	                                        AND MS.[Option_Number] = MO.[Option_Number]
    	                                        AND MS.[Sub_System_Id] = MO.[Sub_System_Id]
   		                                        AND MS.[Capability_Group] = MU.[Capability_Group] 
    	                                        AND MO.[Sub_System_Id] = MU.[Sub_System_Id]
    	                                        AND MO.[System_Id] = MU.[System_Id]
    	                                        AND MU.[User_Id] = EA.[Network_Id]
    	                                        AND MU.[System_Id] = @System_Id
    	                                        AND MU.[Sub_System_Id] = @Sub_System_Id
		                                        AND MU.Capability_Group = @Role 
                                                ORDER BY EA.[Last_Name];";
                    cm.CommandType = CommandType.Text;
                    cm.Parameters.AddWithValue("@System_Id", system);
                    cm.Parameters.AddWithValue("@Sub_System_Id", subsystem);
                    cm.Parameters.AddWithValue("@Role", role);
                    cm.Connection.Open();
                    using (SqlDataReader rdr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        List<Employee> result = new List<Employee>();
                        while (rdr.Read())
                        {
                            Employee employee = new Employee();
                            employee.FirstName = rdr.GetString(0);
                            employee.LastName = rdr.GetString(1);
                            employee.EmailAddress = rdr.GetString(2);
                            employee.NetworkId = rdr.GetString(3);
                            result.Add(employee);
                        }
                        return result;
                    }
                }
            }
        }

        public List<Employee> GetEmployeesLikeFullName(string fullname)
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.OdsConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = @"SELECT Preferred_FirstName, Last_Name, Work_Email_Address, Network_Id
                                        FROM dbo.v_Entity_Employee_Active
                                        WHERE LEN(Work_Email_Address) > 0 
                                        AND Work_Email_Address <> 'None' 
                                        AND Preferred_FirstName + ' ' + Last_Name LIKE(@Full_Name)
                                        ORDER BY Preferred_FirstName;";
                    cm.CommandType = CommandType.Text;
                    cm.Parameters.AddWithValue("@Full_Name", fullname);
                    cm.Connection.Open();
                    using (SqlDataReader rdr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        List<Employee> result = new List<Employee>();
                        while (rdr.Read())
                        {
                            Employee employee = new Employee();
                            employee.FirstName = rdr.GetString(0);
                            employee.LastName = rdr.GetString(1);
                            employee.EmailAddress = rdr.GetString(2);
                            employee.NetworkId = rdr.GetString(3);
                            result.Add(employee);
                        }
                        return result;
                    }
                }
            }
        }

        public Employee GetEmployeeByFullName(string fullname)
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.OdsConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = @"SELECT Preferred_FirstName, Last_Name, Work_Email_Address, Network_Id
                                        FROM dbo.v_Entity_Employee_Active
                                        WHERE LEN(Work_Email_Address) > 0 
                                        AND Work_Email_Address <> 'None' 
                                        AND Preferred_FirstName + ' ' + Last_Name = @Full_Name;";                                        
                    cm.CommandType = CommandType.Text;
                    cm.Parameters.AddWithValue("@Full_Name", fullname);
                    cm.Connection.Open();
                    using (SqlDataReader rdr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (rdr.Read())
                        {
                            Employee employee = new Employee();
                            employee.FirstName = rdr.GetString(0);
                            employee.LastName = rdr.GetString(1);
                            employee.EmailAddress = rdr.GetString(2);
                            employee.NetworkId = rdr.GetString(3);
                            return employee;
                        }
                        else
                            return null;
                    }
                }
            }
        }

        #endregion
    }
}