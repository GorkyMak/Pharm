namespace Pharm.Database
{
    public partial class Адрес
    {
        public override string ToString()
        {
            return string.Concat(Субъект, ", г. " + Город, ", ул. " + Улица, ", д. " + Дом, 
                Квартира != null ? ", кв. " + Квартира : "");
        }
    }
}