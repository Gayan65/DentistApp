using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;

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
                myConnection.Close();
                return dentists;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return dentists;
            }
        }

        public void InsertDentist(SqlConnection myConnection, Dentist dentist)
        {
            string insertNew = "INSERT INTO Dentist (Name, TelNum) VALUES (@newName, @newTelNum)";


            SqlCommand sqlCommand = new SqlCommand(insertNew, myConnection);
            myConnection.Open();

            SqlParameter sqlParameter = new SqlParameter
            {
                ParameterName = "@newName",
                Value = dentist.Name,
                SqlDbType = System.Data.SqlDbType.NVarChar
            };
            sqlCommand.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter
            {
                ParameterName = "@newTelNum",
                Value = dentist.TelNum,
                SqlDbType = System.Data.SqlDbType.NVarChar
            };
            sqlCommand.Parameters.Add(sqlParameter);

            int numberOfRaws = sqlCommand.ExecuteNonQuery();
            if (numberOfRaws > 0)
                Console.WriteLine("Added suessfully new doctor");
            myConnection.Close();
        }

        public List<Dentist> FindDentist(SqlConnection myConnection, string name)
        {
            List<Dentist> dentists = new List<Dentist>();
            try
            {
                string findOne = "SELECT * FROM Dentist WHERE Name LIKE '%'+@personOfIterest+'%'";


                SqlCommand sqlCommand = new SqlCommand(findOne, myConnection);
                myConnection.Open();

                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = "@personOfIterest",
                    Value = name,
                    SqlDbType = System.Data.SqlDbType.NVarChar
                };
                sqlCommand.Parameters.Add(sqlParameter);

                //ARE THERE ANY ?

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
                        Console.WriteLine("No such an employee");
                    }
                    myConnection.Close();
                }
                return dentists;
            }
            catch (Exception ex)
            {

                Console.WriteLine("WWWHAT?");
                Console.WriteLine(ex.Message);
                return dentists;
            }
        }
    }
}


   
