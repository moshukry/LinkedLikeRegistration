using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinkedLikeRegistration.Models;

namespace LinkedLikeRegistration.ViewModels
{
    public class User_Skills
    {
        public User user { get; set; }
        public List<Skill> skills { get; set; }
    }
}