namespace C3P1.Net.Shared.Data.Admin.UserManagement
{
    public class AppUserWithRoles
    {
        public AppUserDto? User { get; set; }
        public List<string>? Roles { get; set; }
        public bool IsAdmin()
        {
            return IsInRole("Admin");
        }
        public bool IsInRole(string role)
        {
            if (User == null || Roles == null || Roles.Count == 0)
            {
                return false;
            }
            else
            {
                if (Roles.Contains(role))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
