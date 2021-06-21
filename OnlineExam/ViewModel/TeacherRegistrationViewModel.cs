using OnlineExam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineExam.ViewModel
{
    public class TeacherRegistrationViewModel
    {

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "WhatsApp No")]
        public string WhatsApp { get; set; }

        [Required]
        [Display(Name = "Primary Subject")]
        public int PrimarySubject { get; set; }

        [Required]
        [Display(Name = "Secondary Subject")]
        public int SecondarySubject { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "PO")]
        public string PO { get; set; }

        [Required]
        [Display(Name = "District")]
        public string District { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }
    }
}