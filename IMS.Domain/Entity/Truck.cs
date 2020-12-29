using IMS.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Domain.Entity
{
    public class Truck
    {
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TruckId { get; set; }

        [Required]
        public TruckModel Model { get; set; }

        [Required]
        public int ManufactureYear { get; set; }

        [Required]
        public int ModelYear { get; set; }


    }
}
