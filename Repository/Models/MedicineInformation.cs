﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models;

public partial class MedicineInformation
{
    public string MedicineID { get; set; }

    [Required(ErrorMessage = "This field can't be empty.")]
    public string MedicineName { get; set; }

    [Required]
    [MinLength(11)]
    public string ActiveIngredients { get; set; }

    [Required(ErrorMessage = "This field can't be empty.")]
    public string ExpirationDate { get; set; }

    [Required(ErrorMessage = "This field can't be empty.")]
    public string DosageForm { get; set; }

    [Required(ErrorMessage = "This field can't be empty.")]
    public string WarningsAndPrecautions { get; set; }

    [Required(ErrorMessage = "This field can't be empty.")]
    public string ManufacturerID { get; set; }

    public virtual Manufacturer Manufacturer { get; set; }
}