using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExam.Models
{

    [Table("Teachers_Registration")]
    public class TeacherRegistration
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WhatsApp { get; set; }
        public int PrimarySubject { get; set; }
        public int SecondarySubject { get; set; }
        public string Location { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public string PO { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedDateTime { get; set; }
       
    }
}