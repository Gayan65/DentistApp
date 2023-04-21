using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DentistApp
{
    internal class Dentist
    {
        public Dentist(int? id, string? name, string? telNum)
        {
            Id = id;
            Name = name;
            TelNum = telNum;
        }

        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? TelNum { get; set; }


        //INSTENT METHODS
        public List<Dentist> GetAllDentists(SqlConnection myConnection)
        {
            
         List<Dentist> dentists = new List<Dentist>();
                    
                try
                {
                    string query = "SELECT * FROM Dentist";
                    SqlCommand sqlCommand = new SqlCommand(query, myConnection);
                    myConnection.Open();
                    using (SqlDataReader sqlDataReder = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReder.HasRows)
                        {
                            while (sqlDataReder.Read())
                            {
                               dentists.Add(new Dentist((int?)sqlDataReder[0], (string?)sqlDataReder[1], (string?)sqlDataReder[2]));
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your table is empty");
                        }
                    }
                    return dentists;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return dentists;
                }
            

        }
    }
}


   
