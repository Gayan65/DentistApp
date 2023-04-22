using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Text.RegularExpressions;

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

        public void DeleteDentist(SqlConnection myConnection, string name)
        {


            try
            {
                string findOne = "SELECT * FROM Dentist WHERE Name LIKE '%'+@personOfIterest+'%'";

                SqlCommand sqlCommandOne = new SqlCommand(findOne, myConnection);
                myConnection.Open();

                SqlParameter sqlParameterOne = new SqlParameter
                {
                    ParameterName = "@personOfIterest",
                    Value = name,
                    SqlDbType = System.Data.SqlDbType.NVarChar
                };
                sqlCommandOne.Parameters.Add(sqlParameterOne);

                //ARE THERE ANY ?

                using (SqlDataReader sqlDataReder = sqlCommandOne.ExecuteReader())
                {
                    Regex nameFormat = new Regex(@"[^a-zA-Z\s]");
                   
                    if (sqlDataReder.HasRows)
                    {
                        
                        while (sqlDataReder.Read())
                        {
                            Console.WriteLine("Id : {0}, Name {1}, TeleNu : {2}", sqlDataReder[0], sqlDataReder[1], sqlDataReder[2]);
                        }
                        sqlDataReder.Close();
                        Console.Write("Give the right name to be deleted : ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newName))
                        {
                            while (nameFormat.IsMatch(newName) || string.IsNullOrEmpty(newName))
                            {
                                Console.Write("Invalid Name format : ");
                                newName = Console.ReadLine();
                            }

                            string deleteOne = "DELETE FROM Dentist WHERE Name = @personDelete;";

                            SqlCommand sqlCommand = new SqlCommand(deleteOne, myConnection);
       
                            SqlParameter sqlParameter = new SqlParameter
                            {
                                ParameterName = "@personDelete",
                                Value = newName,
                                SqlDbType = System.Data.SqlDbType.NVarChar
                            };
                            sqlCommand.Parameters.Add(sqlParameter);

                            int numberOfRows = sqlCommand.ExecuteNonQuery();
                            //NOTICE: Go back to previous cases, and make a change!
                            if (numberOfRows > 0)
                                Console.WriteLine("Successfully removed information.");
                            else if (numberOfRows == 0)
                                Console.WriteLine("No such employee in the company.");
                            myConnection.Close();
                        }
                        myConnection.Close();
                    }
                    else
                    {
                        Console.WriteLine("No such an employee");
                        myConnection.Close();
                    }
                }
              }
            catch (Exception ex)
            {

                Console.WriteLine("WWWHAT?");
                Console.WriteLine(ex.Message);
                myConnection.Close();
            }

        }

        public void UpdateDentist(SqlConnection myConnection, string name)
        {
            bool namechanged = false;
            bool teleNumChanged = false;
            try
            {
                string findOne = "SELECT * FROM Dentist WHERE Name LIKE '%'+@personOfIterest+'%'";

                SqlCommand sqlCommandOne = new SqlCommand(findOne, myConnection);
                myConnection.Open();

                SqlParameter sqlParameterOne = new SqlParameter
                {
                    ParameterName = "@personOfIterest",
                    Value = name,
                    SqlDbType = System.Data.SqlDbType.NVarChar
                };
                sqlCommandOne.Parameters.Add(sqlParameterOne);

                //ARE THERE ANY ?

                using (SqlDataReader sqlDataReder = sqlCommandOne.ExecuteReader())
                {
                    Regex nameFormat = new Regex(@"[^a-zA-Z\s]");
                    Regex mobileFormat = new Regex("^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}$");

                    if (sqlDataReder.HasRows)
                    {

                        while (sqlDataReder.Read())
                        {
                            Console.WriteLine("Id : {0}, Name {1}, TeleNu : {2}", sqlDataReder[0], sqlDataReder[1], sqlDataReder[2]);
                        }
                        sqlDataReder.Close();
                        Console.WriteLine("Enter the right name to be modified find in the lis above.");
                        Console.WriteLine("Leave the fied blank which is not being modified.");
                        Console.Write("Select and type the right name to be modified : ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newName))
                        {
                            while (nameFormat.IsMatch(newName) || string.IsNullOrEmpty(newName))
                            {
                                Console.Write("Invalid Name format : ");
                                newName = Console.ReadLine();
                            }

                            Console.WriteLine("Enter the new name : ");
                            string changedName = Console.ReadLine();

                            if(String.IsNullOrEmpty(changedName))
                            {
                                newName = newName;
                                namechanged = false;
                            }
                            else
                            {
                                while (nameFormat.IsMatch(changedName) || string.IsNullOrEmpty(changedName))
                                {
                                    Console.Write("Invalid Name format : ");
                                    changedName = Console.ReadLine();
                                }
                                newName = changedName;
                                namechanged= true;

                            }

                            Console.WriteLine("Enter the new telephone number : ");
                            string changedTeleNUm = Console.ReadLine();

                            if (String.IsNullOrEmpty(changedTeleNUm))
                            {
                               teleNumChanged = false;
                            }
                            else
                            {
                                while (mobileFormat.IsMatch(changedTeleNUm) || string.IsNullOrEmpty(changedTeleNUm))
                                {
                                    Console.Write("Invalid mobile number format : ");
                                    changedTeleNUm = Console.ReadLine();
                                }
                                teleNumChanged = true;
                            }



                            string updateOne = "UPDATE Dentist SET TelNum=@newTelNum WHERE Name=@dName";

                            SqlCommand sqlCommand = new SqlCommand(updateOne, myConnection);

                            SqlParameter sqlParameter = new SqlParameter
                            {
                                ParameterName = "@dName",
                                Value = newName,
                                SqlDbType = System.Data.SqlDbType.NVarChar
                            };
                            sqlCommand.Parameters.Add(sqlParameter);

                            sqlParameter = new SqlParameter
                            {
                                ParameterName = "@newTelNum",
                                Value = changedTeleNUm,
                                SqlDbType = System.Data.SqlDbType.NVarChar
                            };
                            sqlCommand.Parameters.Add(sqlParameter);



                            int numberOfRows = sqlCommand.ExecuteNonQuery();
                            //NOTICE: Go back to previous cases, and make a change!
                            if (numberOfRows > 0)
                                Console.WriteLine("Successfully modified information.");
                            else if (numberOfRows == 0)
                                Console.WriteLine("No such employee in the company.");
                            myConnection.Close();
                        }
                        myConnection.Close();
                    }
                    else
                    {
                        Console.WriteLine("No such an employee");
                        myConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("WWWHAT?");
                Console.WriteLine(ex.Message);
                myConnection.Close();
            }
        }
        
    }
}


   
