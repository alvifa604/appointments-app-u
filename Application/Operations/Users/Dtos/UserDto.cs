using System;

namespace Application.Operations.Users.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string IdDocument { get; set; }
        public string Name { get; set; }
        public string FirstLastname { get; set; }
        public string SecondLastname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}