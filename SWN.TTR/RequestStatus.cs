namespace SWN.TTR
{
    public enum RequestStatus
    {
        [System.ComponentModel.Description("New")]
        New,        
        [System.ComponentModel.Description("Pending Approval")]
        PendingApproval,
        [System.ComponentModel.Description("Approved")]
        Approved,
        [System.ComponentModel.Description("Denied")]
        Denied
    }
}