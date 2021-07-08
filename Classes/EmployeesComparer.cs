using Pharm.Database;
using System.Collections.Generic;

namespace Pharm.Classes
{
    public class EmployeesComparer : IEqualityComparer<Сотрудник>
    {
        public bool Equals(Сотрудник x, Сотрудник y)
        {
            if (x == null || y == null)
                return x == y;

            return x.Код_сотрудника == y.Код_сотрудника;
        }

        public int GetHashCode(Сотрудник obj)
        {
            return obj.Код_сотрудника.GetHashCode();
        }
    }
}
