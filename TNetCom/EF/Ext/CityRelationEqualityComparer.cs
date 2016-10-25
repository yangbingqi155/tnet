using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCom.EF
{
    public class CityRelationEqualityComparer : IEqualityComparer<CityRelation>
    {
        private CityRelationEqualityComparer() { }
        public static CityRelationEqualityComparer Instance = new CityRelationEqualityComparer();
        public bool Equals(CityRelation x, CityRelation y)
        {
            return (x.idcity == y.idcity && x.idmodule == y.idmodule && x.moduletype == y.moduletype);
        }

        public int GetHashCode(CityRelation model)
        {
            return model.ToString().GetHashCode();
        }
    }
}
