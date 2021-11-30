using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentKendaraan.Models
{
    public partial class JenisKendaraan
    {
        public JenisKendaraan()
        {
            Kendaraans = new HashSet<Kendaraan>();
        }

        [Required(ErrorMessage = "Tidak boleh kosong")]
        [RegularExpression("[0-9]*$", ErrorMessage = "Hanya boleh diisi oleh angka")]
        public int IdJenisKendaraan { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public string NamaJenisKendaraan { get; set; }

        public virtual ICollection<Kendaraan> Kendaraans { get; set; }
    }
}
