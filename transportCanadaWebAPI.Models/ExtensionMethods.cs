using System;
using System.Data.SqlClient;

using TransCanadaWebAPI.Models;

namespace TransCanadaWebAPI.Models
{
    public static class ExtensionMethods
    {
        public static SqlParameter[] ToParameterArray(this Centers center)
        {
            return new SqlParameter[] { new SqlParameter("@Id", center.Id)
                                                , new SqlParameter("@Name", center.Name)
                                                , new SqlParameter("@StreetAddress", center.StreetAddress)                                               
                                                , new SqlParameter("@CenterTypeValue", center.CenterTypeValue)
                                                };
        }

        public static SqlParameter[] ToParameterArray(this Appointments appointment)
        {
            return new SqlParameter[]{ new SqlParameter("@Id", appointment.Id)
                                                , new SqlParameter("@CenterID", appointment.CenterID)
                                                , new SqlParameter("@ClientFullName", appointment.ClientFullName ?? (object)DBNull.Value)
                                                , new SqlParameter("@Date", appointment.Date ?? (object)DBNull.Value)
                                                };
        }
    }
}