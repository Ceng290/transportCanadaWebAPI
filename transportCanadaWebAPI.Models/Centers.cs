using System;
using System.Data;

namespace TransCanadaWebAPI.Models
{
    public class Centers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string CenterTypeValue { get; set; }

        public Centers() { }

        public Centers(IDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["CenterID"]);
            this.Name = reader["Name"].ToString();
            this.StreetAddress = reader["StreetAddress"].ToString();
            this.CenterTypeValue = reader["CenterTypeValue"].ToString();
        }

    }
}