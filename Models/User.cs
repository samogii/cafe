using Cafe.Data;
using Cafe.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Cafe.Models
{
    public class User
    {
        private DataContext _context;

        public User(DataContext context)
        {
            _context = context;
        }
        public User() { }

        public User(string email, string password , string userName)
        {
            Email = email;
            Password = password;
            UserName = userName;
            CreatedAt = DateTime.Now;
            Role = Enums.Role.USER;
        }

        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? EmailConfirmed { get; set; }
        public Role? Role { get; set; }
        public string? Phone { get; set; }
        [AllowNull]
        public DateTime? Birthdate { get; set; }
        public DateTime CreatedAt { get; set; }


        public User GetId(int id)
        {
            return _context.Users.First(x=>x.Id == id);
        }
    }

}
