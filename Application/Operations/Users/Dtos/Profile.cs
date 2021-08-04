using System;

namespace Application.Operations.Users.Dtos
{
    public class Profile
    {
        public string IdDocument { get; set; }
        public string Name { get; set; }
        public string FirstLastname { get; set; }
        public string SecondLastname { get; set; }
        public string Role { get; set; }
    }
}