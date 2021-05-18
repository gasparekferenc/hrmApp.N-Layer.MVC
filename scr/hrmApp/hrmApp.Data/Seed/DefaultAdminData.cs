namespace hrmApp.Data.Seed
{
    public class DefaultAdminData
    {
        public string UserName { get; set; } = "admin@email.com";
        public string Email { get; set; } = "admin@email.com";
        public string Password { get; set; } = "Pa$$word123!";
        public string SurName { get; set; } = "Admin";
        public string ForeName { get; set; } = "User";
        public string PhoneNumber { get; set; } = "06 30 123 4567";
        public string Description { get; set; } = "Default Admin";
    }
}