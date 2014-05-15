namespace SWN.TTR.Repository.Dto
{
    public class Employee
    {
        public Employee()
            : this("", "", "", "")
        {

        }

        public Employee(string firstName, string lastName, string emailAddress, string networkId)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
            this.NetworkId = networkId;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string NetworkId { get; set; }        

        public string FullName
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }            
        }	

        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }
}