//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UIWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class bairro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public bairro()
        {
            this.endentrega = new HashSet<endentrega>();
            this.mobiliario = new HashSet<mobiliario>();
            this.cadimob = new HashSet<cadimob>();
            this.condominio = new HashSet<condominio>();
        }
    
        public string siglauf { get; set; }
        public short codcidade { get; set; }
        public short codbairro { get; set; }
        public string descbairro { get; set; }
    
        public virtual cidade cidade { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<endentrega> endentrega { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mobiliario> mobiliario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cadimob> cadimob { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<condominio> condominio { get; set; }
    }
}
