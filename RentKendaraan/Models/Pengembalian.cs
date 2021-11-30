using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentKendaraan.Models
{
    public partial class Pengembalian
    {
        [Required(ErrorMessage = "Tidak boleh kosong")]
        [RegularExpression("[0-9]*$", ErrorMessage = "Hanya boleh diisi oleh angka")]
        public int IdPengembalian { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public DateTime? TglPengembalian { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public int? IdPeminjaman { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public int? IdKondisi { get; set; }
        [RegularExpression("[0-9]*$", ErrorMessage = "Hanya boleh diisi oleh angka")]
        public int? Denda { get; set; }

        public virtual KondisiKendaraan IdKondisiNavigation { get; set; }
        public virtual Peminjaman IdPeminjamanNavigation { get; set; }
    }
}
