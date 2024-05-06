using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    public class User
    {
        
        public int UserId {  get; set; }
        
        public string Username { get; set; }    
        public string Password { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }     
        public Guid ActivationCode { get; set; }
    }
}