namespace TEJ0017_FakturacniSystem.Models.User
{
    public abstract class User
    {
        public int UserId { get; protected set; }
        public string Login { get; protected set; }
        public string Password { get; protected set; }
        public string Name { get; protected set; }
        public string Surname { get; protected set; }
        public string Email { get; protected set; }
        public string? Telephone { get; protected set; }
        public DateTime LastLoginTmstmp { get; protected set; }
        public DateTime RegisteredTmpstmp { get; protected set; }

    }
}
