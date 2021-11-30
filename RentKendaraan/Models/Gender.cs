using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentKendaraan.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Customers = new HashSet<Customer>();
        }

        [Required(ErrorMessage = "Tidak boleh kosong")]
        public int IdGender { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public string NamaGender { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
