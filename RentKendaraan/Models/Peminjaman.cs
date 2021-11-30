using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentKendaraan.Models
{
    public partial class Peminjaman
    {
        public Peminjaman()
        {
            Pengembalians = new HashSet<Pengembalian>();
        }

        [Required(ErrorMessage = "Tidak boleh kosong")]
        [RegularExpression("[0-9]*$", ErrorMessage = "Hanya boleh diisi oleh angka")]
        public int IdPeminjaman { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public DateTime? TglPeminjaman { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public int? IdKendaraan { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public int? IdCostumer { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public int? IdJaminan { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        [RegularExpression("[0-9]*$", ErrorMessage = "Hanya boleh diisi oleh angka")]
        public int? Biaya { get; set; }

        public virtual Customer IdCostumerNavigation { get; set; }
        public virtual Jaminan IdJaminanNavigation { get; set; }
        public virtual Kendaraan IdKendaraanNavigation { get; set; }
        public virtual ICollection<Pengembalian> Pengembalians { get; set; }
    }
}
