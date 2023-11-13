﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsManager.Core.Domain.Entities
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)]
        public string? PersonName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(40)]
        public string? Gender { get; set; }

        public Guid? CountryID { get; set; }

        [ForeignKey("CountryID")]
        public Country? Country { get; set; }

        [StringLength(40)]
        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        public string? TIN { get; set; } 

    }
}