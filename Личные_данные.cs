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
    
    public partial class Личные_данные
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Личные_данные()
        {
            this.Сотрудник = new HashSet<Сотрудник>();
            this.Социальная_карта = new HashSet<Социальная_карта>();
        }
    
        public int Код_личных_данных { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public int Серия_паспорта { get; set; }
        public int Номер_паспорта { get; set; }
        public System.DateTime Дата_рождения { get; set; }
        public string Образование { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Сотрудник> Сотрудник { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Социальная_карта> Социальная_карта { get; set; }
    }
}
