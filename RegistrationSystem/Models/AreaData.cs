using System.Data;
using System.Data.SqlClient;

namespace RegistrationSystem.Models
{
    public class AreaData
    {
        MSDatabaseCommunication DB;
        public AreaData()
        {
            DatabaseCommunicationFactory DBF = new DatabaseCommunicationFactory();
            DB = DBF.GetDatabaseCommunication(DatabaseEnum.Test30);
        }
        public DataTable GetCityDataToDB(out bool _Success)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.City");
            return DB.GetDBDataResultDataTable(cmd, out _Success);
        }

        public DataTable GetCityDataToDB(out bool _Success, byte _CityID)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.City where CityID = @CityID");
            cmd.Parameters.Add("@CityID", SqlDbType.TinyInt);
            cmd.Parameters["@CityID"].Value = _CityID;
            return DB.GetDBDataResultDataTable(cmd, out _Success);
        }

        public DataTable GetTownDataToDB(out bool _Success, byte _CityID)
        {
            SqlCommand cmd = new SqlCommand( @"SELECT TownID,TownName_TC FROM dbo.Town where CityID = @CityID");
            cmd.Parameters.Add("@CityID", SqlDbType.TinyInt);
            cmd.Parameters["@CityID"].Value = _CityID;
            return DB.GetDBDataResultDataTable(cmd, out _Success);
        }
    }
}