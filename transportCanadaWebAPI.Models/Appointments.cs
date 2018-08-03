using System;
using System.Collections.Generic;
using System.Data;

namespace TransCanadaWebAPI.Models
{
    public class Appointments
    {
        public int Id { get; set; }
        public int CenterID { get; set; }
        public string ClientFullName { get; set; }
        public DateTime? Date { get; set; }

        public Centers Centers { get; set; }

        public Appointments() { }

        public Appointments(IDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["AppointmentID"]);
            this.CenterID = Convert.ToInt32(reader["CenterID"]);
            this.ClientFullName = reader["ClientFullName"] as string;
            this.Date = reader["AppointmentDate"] as DateTime?;

            this.Centers = new Centers();
            this.Centers.Id = Convert.ToInt32(reader["CenterID"]);
            this.Centers.Id = this.Centers.Id;
            this.Centers.Name = reader["Name"] as string;
            this.Centers.StreetAddress = reader["StreetAddress"] as string;
            this.Centers.CenterTypeValue = reader["CenterTypeValue"] as string;

        }
    }
}