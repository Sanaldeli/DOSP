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
    
    public partial class Report
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Report()
        {
            this.AdminReports = new HashSet<AdminReport>();
        }
    
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
        public DateTime ReportDate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public int GameID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminReport> AdminReports { get; set; }
        public virtual Game Game { get; set; }
        public virtual ReportCategory ReportCategory { get; set; }
        public virtual User User { get; set; }
    }
}
