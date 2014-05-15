using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using SWN.TTR;
using SWN.TTR.Repository.Dto;

namespace SWN.TTR.WebUI.services
{
    /// <summary>
    /// Summary description for EmployeeService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]    
    [System.Web.Script.Services.ScriptService]
    public class EmployeeService : System.Web.Services.WebService
    {
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCompletionList(string prefixText, int count)
        {
            if (count == 0)
                count = 20;
            OdsFinder odsFinder = new OdsFinder();
            List<Employee> employees = odsFinder.GetEmployeesLikeFullName(prefixText);
            int resultCount = count <= employees.Count ? count : employees.Count;
            string[] autoCompleteResults = new string[resultCount];
            for (int i = 0; i < resultCount; ++i)
            {
                autoCompleteResults[i] = 
                    AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(employees[i].ToString(),
                    employees[i].EmailAddress);
            }
            return autoCompleteResults;
        }
    }
}
