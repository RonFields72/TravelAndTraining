using SWN.TTR.Repository;
using SWN.TTR.Repository.Common;
using SWN.TTR.Repository.Dto;
using System.Collections.Generic;

namespace SWN.TTR
{
    /// <summary>
    /// 
    /// </summary>
    public class OdsFinder
    {
        private IOdsRepository provider = null;

        public OdsFinder()           
        {
            this.provider = DataAccessFactory.CreateOdsRepository();
        }

        public System.Collections.Generic.IList<Employee> GetActiveEmployees()
        {
            return provider.GetActiveEmployees();
        }

        public Employee GetEmployeeByNetworkID(string networkId)
        {
            return provider.GetEmployeeByNetworkID(networkId);
        }

        public Employee GetEmployeeByWorkEmailAddress(string emailAddress)
        {
            return provider.GetEmployeeByWorkEmailAddress(emailAddress);
        }

        public System.Collections.Generic.List<Employee> GetEmployeesByRole(string system, string subsystem, string role)
        {
            return provider.GetEmployeesByRole(system, subsystem, role);
        }

        public List<Employee> GetEmployeesLikeFullName(string fullname)
        {
            return provider.GetEmployeesLikeFullName("%" + fullname + "%");
        }

        public Employee GetEmployeeByFullName(string fullname)
        {
            return provider.GetEmployeeByFullName(fullname);
        }
    }
}