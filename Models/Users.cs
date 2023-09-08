namespace anket_kazan.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Balance { get; set; }
        public List<Answer> Answer { get; set; }
    }
}
