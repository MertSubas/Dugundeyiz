using Dugundeyiz.Entity;
using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.ViewModels
{
    //public class UserViewModel
    //{
    //    public string UserName { get; set; }
    //    public string Password { get; set; }
    //    public string Name { get; set; }
    //    public string Surname { get; set; }
    //    public string Email { get; set; }
    //    public string Phone { get; set; }
    //    public int Role { get; set; }
    //    public bool Admin { get; set; }
    //} public class UserViewModel
    public class UserSingUpViewModel
    {
        public string UserName { get; set; } // This should match the key sent from JS
        public string Password { get; set; } // This should match the key sent from JS
        public string Name { get; set; } // This should match the key sent from JS
        //public string Surname { get; set; } // This should match the key sent from JS
        public string Email { get; set; } // This should match the key sent from JS
        public string Phone { get; set; } // This should match the key sent from JS
        //public int Role { get; set; } // This should match the key sent from JS
        //public bool Admin { get; set; } // This should match the key sent from JS
    }
    public class UserViewModel
        {
            public string UserName { get; set; } // This should match the key sent from JS
            public string Password { get; set; } // This should match the key sent from JS
            public string Name { get; set; } // This should match the key sent from JS
            public string Surname { get; set; } // This should match the key sent from JS
            public string Email { get; set; } // This should match the key sent from JS
            public string Phone { get; set; } // This should match the key sent from JS
            public int Role { get; set; } // This should match the key sent from JS
            public bool Admin { get; set; } // This should match the key sent from JS
        }
    public class PartnerSignUpViewModel
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string District { get; set; }
    }
    public class ProfileDatatoSend
    {
        public PartnerEditProfile Profile { get; set; }
        public UserPartner UserpartnerInfo { get; set; }
        public string? CityName { get; set; }
        public string? DistrictName { get; set; }



    }
    public class PartnerEditProfile
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public DateTime? WeddingDate { get; set; }
    }

    public class UpdateProfileViewModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public string? CompanyName { get; set; }

        public string? Address { get; set; }

        public string? AddressDesc { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? City { get; set; }

        public string? District { get; set; }
        public string? WeddingDate { get; set; }
    }

}
