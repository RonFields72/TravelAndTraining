using System;
using SWN.TTR.Common;

namespace SWN.TTR.Repository.Dto
{
    public class Approval
    {
        public Approval()
        {            
        }        

        private string reviewedBy;
        public string ReviewedBy
        {
            get { return reviewedBy; }
            set 
            {
                if (value.NullToEmptyString().Length > 25)
                    throw new ArgumentException("Reviewed length cannot exceed 25 characters.");
                else
                    reviewedBy = value.NullToEmptyString(); 
            }
        }

        public DateTime ReviewedOn { get; set; }

        public ApprovalStatus Status { get; set; }

        private string comments;

        public string Comments
        {
            get { return comments; }
            set 
            {
                if (value.NullToEmptyString().Length > 100)
                    throw new ArgumentException("Comments length cannot exceed 25 characters.");
                else
                    comments = value.NullToEmptyString(); 
            }
        }	
    }
}