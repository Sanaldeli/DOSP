using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DOSP.Models
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=DOSPEntities")
        {
        }

        public virtual DbSet<Kategori> Kategoris { get; set; }
        public virtual DbSet<Kullanici> Kullanicis { get; set; }
        public virtual DbSet<Kutuphane> Kutuphanes { get; set; }
        public virtual DbSet<Oyun> Oyuns { get; set; }
        public virtual DbSet<Rapor> Rapors { get; set; }
        public virtual DbSet<Sepet> Sepets { get; set; }
        public virtual DbSet<sepetOyun> SepetOyuns { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Yapimci> Yapimcis { get; set; }
        public virtual DbSet<RaporKategori> RaporKategoris { get; set; }
    }
}