using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentKendaraan.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Peminjamen = new HashSet<Peminjaman>();
        }
        public int IdCostumer { get; set; }
        [Required(ErrorMessage = "Nama Customer tidak boleh kosong")]
        public string NamaCustomer { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Hanya boleh diisi oleh angka")]
        public string Nik { get; set; }
        [Required(ErrorMessage = "Alamat tidak boleh kosong")]
        public string Alamat { get; set; }
        [MinLength(10, ErrorMessage = "No Hp minimal 10 angka")]
        [MaxLength(13, ErrorMessage = "No Hp maksimal 13 angka")]
        public string NoHp { get; set; }
        [Required(ErrorMessage = "IDGender tidak boleh kosong")]
        public int? IdGender { get; set; }

        public virtual Gender IdGenderNavigation { get; set; }
        public virtual ICollection<Peminjaman> Peminjamen { get; set; }
    }
}
