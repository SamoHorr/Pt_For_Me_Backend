namespace Pt_For_Me.Entities
{
    public class Table_User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ProfileURL { get; set; }
        public string Email { get; set; }
        public string Device_Token { get; set; }
        //secure website login 
        public Guid? AuthToken { get; set; }
        public DateTime? Expiration_Date { get; set; }
    }
}
