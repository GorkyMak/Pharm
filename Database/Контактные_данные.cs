//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pharm.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Контактные_данные
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Контактные_данные()
        {
            this.Сотрудник = new HashSet<Сотрудник>();
        }
    
        public int Код_контактных_данных { get; set; }
        public Nullable<int> Код_адреса { get; set; }
        public string Номер_телефона { get; set; }
        public string Электронная_почта { get; set; }
    
        public virtual Адрес Адрес { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Сотрудник> Сотрудник { get; set; }
    }
}
