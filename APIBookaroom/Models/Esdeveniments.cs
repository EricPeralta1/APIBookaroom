//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APIBookaroom.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Esdeveniments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Esdeveniments()
        {
            this.Entrades = new HashSet<Entrades>();
        }
    
        public int event_id { get; set; }
        public Nullable<int> room_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public int capacity { get; set; }
        public System.DateTime start_date { get; set; }
        public System.DateTime end_date { get; set; }
        public double price { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string event_image { get; set; }
        public int active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entrades> Entrades { get; set; }
        public virtual Sales Sales { get; set; }
        public virtual Usuaris Usuaris { get; set; }
    }
}
