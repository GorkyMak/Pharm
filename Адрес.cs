//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pharm
{
    using System;
    using System.Collections.Generic;
    
    public partial class Адрес
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Адрес()
        {
            this.Контактные_данные = new HashSet<Контактные_данные>();
            this.Склад = new HashSet<Склад>();
        }
    
        public int Код_адреса { get; set; }
        public string Страна { get; set; }
        public string Субъект { get; set; }
        public string Город { get; set; }
        public string Улица { get; set; }
        public string Дом { get; set; }
        public string Квартира { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Контактные_данные> Контактные_данные { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Склад> Склад { get; set; }
    }
}
