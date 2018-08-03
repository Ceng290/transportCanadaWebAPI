using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using TransCanadaWebAPI.Models;

namespace TransCanadaWebAPI.Repositories
{
    public class CentersRepository : Repository<Centers>
    {
        //========================================================================================
        //
        //========================================================================================
        public override Centers PopulateRecord(IDataReader reader)
        {
            return new Centers(reader);
        }
        //========================================================================================
        //
        //========================================================================================
        public IEnumerable<Centers> Get()
        {
            var sqlCommand = "select a.CenterID, a.CenterTypeID, "
                            + "a.Name, a.StreetAddress, b.Value as CenterTypeValue "
                            + "from Center a, CenterType b "
                            + "where a.CenterTypeId = b.CenterTypeID ";

            return base.Get(sqlCommand);
        }

        //========================================================================================
        //
        //========================================================================================
        public IEnumerable<Centers> Get(int Id)
        {
            var sqlCommand = "select a.CenterID, a.CenterTypeID, "
                            + "a.Name, a.StreetAddress, b.Value as CenterTypeValue "
                            + "from Center a, CenterType b "
                            + "where a.CenterTypeId = b.CenterTypeID and a.CenterID = @Id";

            var parameters = new SqlParameter[] { new SqlParameter("@Id", Id) };

            return base.Get(sqlCommand, parameters);
        }
    }        
}