namespace SWN.TTR.Security
{
    public class RequestOfficer
    {
        private System.Security.Principal.IPrincipal userContext = null;

        public RequestOfficer(System.Security.Principal.IPrincipal principal)
        {
            this.userContext = principal;
        }        

        public bool CanApprove()
        {
            if (IsUserFinalApprover())
                return true;
            else
                return false;      
        }

        public bool CanDeny()
        {
            if (IsUserFinalApprover())
                return true;
            else
                return false;
        }

        public bool IsUserFinalApprover()
        {
            if (userContext.IsInRole("FINAL"))
                return true;
            else
                return false;
        }

        public bool CanRoute()
        {
            return true;
        }

        public bool CanDelete()
        {
            return true;
        }

        public bool CanViewAllRequests()
        {
            if (userContext.IsInRole("VIEWALL"))
                return true;
            else
                return false;
        }

        public bool CanSave()
        {
            return true;
        }
    }
}