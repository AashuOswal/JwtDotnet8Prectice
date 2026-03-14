namespace JwtDotnet8Prectice.Models
{
    public class User
    {
        public int Id { get; set; }
        //Name of the user
        public string Name { get; set; } = null!;
        // Username of the user
        public string Email { get; set; } = string.Empty;
        // Password for demo only (in real apps, store hashed passwords)
        public string Password { get; set; } = string.Empty;
        // Roles assigned to the user, comma-separated
        public string Roles { get; set; } = string.Empty;
    }
}
