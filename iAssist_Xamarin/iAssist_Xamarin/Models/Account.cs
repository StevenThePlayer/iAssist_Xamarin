using System;
using System.Collections.Generic;
using System.Text;

namespace iAssist_Xamarin.Models
{
    public class RegisterModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class ViewProfileModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
    }

    public class EditProfileModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        //public string ProfilePicture { get; set; }
        public string Userid { get; set; }
    }

    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }


    //Get Address
    public class UserAddress
    {
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }


    public class UserRole
    {
        public string Role { get; set; }
        public int WorkerId { get; set; }
    }
}
