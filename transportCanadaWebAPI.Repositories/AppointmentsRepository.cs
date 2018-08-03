using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using TransCanadaWebAPI.Models;

namespace TransCanadaWebAPI.Repositories
{
    public class AppointmentsRepository : Repository<Appointments>
    {
    
        //========================================================================================
        //
        //========================================================================================
        public override Appointments PopulateRecord(IDataReader reader)
        {
            return new Appointments(reader);
        }

        //========================================================================================
        // The following section of code retrieves all the Appointments from the database. WORKING
        //========================================================================================
        public IEnumerable<Appointments> Get()
        {
            var sqlCommand = "SELECT APPOINTMENTS.AppointmentID, "
                            + "APPOINTMENTS.ClientFullName, "
                            + "APPOINTMENTS.AppointmentDate, "
                            + "CENTER.CenterID, "
                            + "CENTER.Name, "
                            + "CENTER.StreetAddress, "
                            + "CENTERTYPE.Value as CenterTypeValue "
                            + "FROM Appointments APPOINTMENTS "
                            + "INNER JOIN Center CENTER ON APPOINTMENTS.CenterID = CENTER.CenterID "
                            + "INNER JOIN CenterType CENTERTYPE ON CENTERTYPE.CenterTypeID = Center.CenterTypeID "
                            + "WHERE APPOINTMENTS.CenterId = Center.CenterID";

            return base.Get(sqlCommand);
        }

        //==============================================================================================
        // The following section of code retrieves all the Appointments By ID from the database. WORKING
        //==============================================================================================
        public IEnumerable<Appointments> Get(int Id)
        {
            var sqlCommand = "SELECT APPOINTMENTS.AppointmentID, "
                            + "APPOINTMENTS.ClientFullName, "
                            + "Convert(date,APPOINTMENTS.AppointmentDate), "
                            + "CENTER.CenterID, "
                            + "CENTER.Name, "
                            + "CENTER.StreetAddress, "
                            + "CENTERTYPE.Value as CenterTypeValue "
                            + "FROM Appointments APPOINTMENTS "
                            + "INNER JOIN Center CENTER ON APPOINTMENTS.CenterID = CENTER.CenterID "
                            + "INNER JOIN CenterType CENTERTYPE ON CENTERTYPE.CenterTypeID = Center.CenterTypeID "
                            + "WHERE APPOINTMENTS.CenterId = Center.CenterID "
                            + "AND APPOINTMENTS.AppointmentID = @ID "; 

            var parameters = new SqlParameter[] { new SqlParameter("@ID", Id) };
            return base.Get(sqlCommand, parameters);
        }

        //========================================================================================
        // POST Command
        //========================================================================================
        public void Add(Appointments appointments)
        {
            var sqlCommand = "If Not Exists(select 1 from Appointments where AppointmentDate = Convert(date, getdate()) and CenterId = @CenterID)"
                            + "INSERT INTO Appointments (CenterID, ClientFullName, AppointmentDate) "
                            + "VALUES (@CenterID, @ClientFullName, CONVERT(datetime,@Date)); "
                            + "SELECT SCOPE_IDENTITY() ";

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

            try
            {
                var Id = base.ExecuteScalar(sqlCommand, appointments.ToParameterArray(), CommandType.Text, sqlConnection, sqlTransaction);

                if (Id == DBNull.Value)
                    throw new NullReferenceException("Order creation failed");

                appointments.Id = Convert.ToInt32(Id);
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();
                sqlConnection.Close();
                throw;
            }

            sqlTransaction.Commit();
            sqlConnection.Close();
        }

        //========================================================================================
        // PUT Command
        //========================================================================================
        public void Update(Appointments appointments)
        {
            var sqlCommand = "UPDATE Appointments SET CenterID = @CenterID, ClientFullName = @ClientFullName, AppointmentDate = CONVERT(datetime,@Date) WHERE AppointmentID = @Id ";

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

            try
            {                
                base.ExecuteScalar(sqlCommand, appointments.ToParameterArray(), CommandType.Text, sqlConnection, sqlTransaction);
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                sqlConnection.Close();
                throw e;
            }

            sqlTransaction.Commit();
            sqlConnection.Close();

            base.ExecuteNonQuery(sqlCommand, appointments.ToParameterArray());
        }

        //========================================================================================
        // DELETE Command. The following code deletes the appointment from the database.  WORKING
        //========================================================================================
        public void Delete(int Id)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

            try
            {
                var sqlCommand = "DELETE FROM Appointments WHERE AppointmentID = @Id  ";
                var parameters = new SqlParameter[] { new SqlParameter("@Id", Id) };
                ExecuteNonQuery(sqlCommand, parameters, CommandType.Text, sqlConnection, sqlTransaction);
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();
                sqlConnection.Close();
                throw;
            }

            sqlTransaction.Commit();
            sqlConnection.Close();
        }
    }
}