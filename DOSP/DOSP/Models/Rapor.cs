//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DOSP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rapor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rapor()
        {
            this.adminRapors = new HashSet<adminRapor>();
        }
    
        public int RaporID { get; set; }
        public int KategoriID { get; set; }
        public int KullaniciID { get; set; }
        public DateTime RaporTarihi { get; set; }
        public string aciklama { get; set; }
        public int OyunID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<adminRapor> adminRapors { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual Oyun Oyun { get; set; }
        public virtual RaporKategori RaporKategori { get; set; }
    }
}