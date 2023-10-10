using fit_and_fuel.Services;

namespace fit_and_fuel.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        //public string Token { get; set; }
       public IList<string> Roles { get; set; }
    }
}
