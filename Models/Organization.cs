﻿using System.ComponentModel.DataAnnotations;

namespace EmpathyKick.Models
{
    public class Organization
    {
        public Organization() // Add a default constructor
        {
        }
        public Organization(int orgId, string orgName, int addId, string taxId, string logo, string color)
        {
            OrganizationId = orgId;
            OrganizationName = orgName;
            AddressId = addId;
            TaxId = taxId;
            LogoFile = logo;
            ThemeColor = color;
        }
        [Key]
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int AddressId { get; set; }
        public string? TaxId { get; set; }
        public string LogoFile { get; set; }
        public string? ThemeColor { get; set; }
        public virtual Donations Donations { get; set; }
    }

}

