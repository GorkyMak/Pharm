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
    
    public partial class Социальная_карта
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Социальная_карта()
        {
            this.Заказ = new HashSet<Заказ>();
        }
    
        public int Код_социальной_карты { get; set; }
        public Nullable<int> Код_личных_данных_владельца { get; set; }
        public System.DateTime Срок_действия { get; set; }
        public decimal Скидка { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Заказ> Заказ { get; set; }
        public virtual Личные_данные Личные_данные { get; set; }
    }
}
