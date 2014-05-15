namespace SWN.TTR.WebUI
{
    public class TTRRoleProvider : System.Web.Security.RoleProvider
    {
        private readonly string SYSTEM = System.Web.Configuration.WebConfigurationManager.AppSettings["MenuSystem"];
        private readonly string SUBSYSTEM = System.Web.Configuration.WebConfigurationManager.AppSettings["MenuSubSystem"];

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new System.NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            RolesServices.RolesServiceSoapClient client =
                new RolesServices.RolesServiceSoapClient();
            client.ClientCredentials.Windows.AllowedImpersonationLevel =
                System.Security.Principal.TokenImpersonationLevel.Delegation;
            return client.GetRolesForUser(username.Replace(@"SWN\", ""), SYSTEM, SUBSYSTEM);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            RolesServices.RolesServiceSoapClient client =
                new RolesServices.RolesServiceSoapClient();
            client.ClientCredentials.Windows.AllowedImpersonationLevel =
                System.Security.Principal.TokenImpersonationLevel.Delegation;
            return client.GetUsersInRole(roleName, SYSTEM, SUBSYSTEM);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new System.NotImplementedException();
        }
    }
}