using SWN.TTR.Repository.Dto;
using System.Collections.Generic;

namespace SWN.TTR.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOdsRepository
    {
        Employee GetEmployeeByNetworkID(string networkId);
        
        List<Employee> GetActiveEmployees();
        
        Employee GetEmployeeByWorkEmailAddress(string emailAddress);

        List<Employee> GetEmployeesByRole(string system, string subsystem, string role);

        List<Employee> GetEmployeesLikeFullName(string fullname);

        Employee GetEmployeeByFullName(string fullName);
    }
}