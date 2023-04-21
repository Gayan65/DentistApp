using DentistApp;
using System.Data.SqlClient;
using System.Numerics;
using System.Text.RegularExpressions;


//VARIABLES
bool more;
bool addMember;
int choice;
string name, mobile;
Regex nameFormat = new Regex(@"[^a-zA-Z\s]");
Regex mobileFormat = new Regex("^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}$");
Dentist dentistObj = new Dentist(null, null, null);

//CONNECTION STRING
string connectionString = @"Data Source = Gayan\SQLEXPRESS;Initial Catalog=Dentist;Integrated Security=true;";
SqlConnection myConnection = new SqlConnection(connectionString);

//APP START
do
{
    Console.Clear();
    //APP HEADER
    Console.WriteLine();
    Console.WriteLine("WELCOME, DENTIST APPLICATION SYSTEM");
    Console.WriteLine();
    Console.WriteLine("Select an option 1...4");
    Console.WriteLine();
    Console.WriteLine("Enter 1 if you want to SHOW ALL.");
    Console.WriteLine("Enter 2 if you want to ADD A NEW DENTIST.");
    Console.WriteLine("Enter 3 if you want to FIND DENTIST.");
    Console.WriteLine("Enter 3 if you want to MODIFY INFORMATION.");
    Console.WriteLine("Enter 4 if you want to REMOVE INFORMATION.");
    Console.WriteLine("Enter 5 if you want to DO NOTHING.");
    Console.WriteLine();
    Console.Write("Make your choice : ");
    string recieved = Console.ReadLine();

    //PASSING RIGHT INPUTS
    while (!Int32.TryParse(recieved, out choice) || choice < 1 || choice > 5)
    {
        Console.Write("Invalid input, Make your choice : ");
        recieved = Console.ReadLine();
    }

    //CONDITION AS PER THE USER PREFERENCE
    if (choice.Equals(1))
    {

        Console.Clear();
        List<Dentist> dentistList = new List<Dentist>();
        Console.WriteLine("CHOICE : S H O W   A L L");
        //desplay dentist list
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("ID   | NAME OF THE DENTIST       | MOBILE      ");
        Console.WriteLine("------------------------------------------------------------");

        dentistList = dentistObj.GetAllDentists(myConnection);
        foreach (var item in dentistList)
        {
            Console.WriteLine(String.Format("{0, -4} | {1, -25} | {2, -15}",item.Id, item.Name, item.TelNum));
        }
            
    }

    else if (choice.Equals(2))
    {
        Console.Clear();
        Console.WriteLine("CHOISE : N E W    D E N T I S T");
        Console.WriteLine();
        Console.Write("Name :");
        name = Console.ReadLine();
        while (String.IsNullOrWhiteSpace(name) || nameFormat.IsMatch(name))
        {
            Console.Write("Name :");
            name = Console.ReadLine();
        }
        Console.Write("Mobile :");
        mobile = Console.ReadLine();
        while (String.IsNullOrWhiteSpace(mobile) || !mobileFormat.IsMatch(mobile))
        {
            Console.Write("Mobile :");
            mobile = Console.ReadLine();
        }
        //add member method
        dentistObj = new Dentist(null, name, mobile);
        dentistObj.InsertDentist(myConnection, dentistObj);
    }

    else if(choice.Equals(3))
    {
        Console.Clear();
        Console.WriteLine("CHOICE : F I N D    D E N T I S T");
        Console.WriteLine();
        Console.Write("Enter the Name of the dentist you looking for : ");
        name = Console.ReadLine();
        while (String.IsNullOrWhiteSpace(name) || nameFormat.IsMatch(name))
        {
            Console.Write("Name :");
            name = Console.ReadLine();
        }
        //FIND dentist method
        List<Dentist> dentistList = new List<Dentist>();
        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("ID   | NAME OF THE DENTIST       | MOBILE      ");
        Console.WriteLine("------------------------------------------------------------");
        dentistList = dentistObj.FindDentist(myConnection, name);
       
        foreach (var item in dentistList)
        {
            Console.WriteLine(String.Format("{0, -4} | {1, -25} | {2, -15}", item.Id, item.Name, item.TelNum));
        }
    }
    //else if (choice.Equals(3))
    //{
    //    Console.Clear();
    //    Console.WriteLine("CHOICE : M O D I F Y    D E N T I S T");
    //    Console.WriteLine();
    //    Console.Write("Enter the Name of the dentist you looking for : ");
    //    name = Console.ReadLine();
    //    while (String.IsNullOrWhiteSpace(name) || nameFormat.IsMatch(name))
    //    {
    //        Console.Write("Name :");
    //        name = Console.ReadLine();
    //    }
    //    //modify dentist method
    //}

    else if (choice.Equals(5))
    {
        Console.Clear();
        Console.WriteLine("CHOICE : R E M O V E    D E N T I S T");
        Console.WriteLine();
        Console.Write("Enter the Name of the dentist you looking for : ");
        name = Console.ReadLine();
        while (String.IsNullOrWhiteSpace(name) || nameFormat.IsMatch(name))
        {
            Console.Write("Name :");
            name = Console.ReadLine();
        }
        //delete dentist method;
        dentistObj.DeleteDentist(myConnection, name);
    }

    else
        Console.Write("You choosed want nothing, ");

    Console.WriteLine();
    Console.Write("Continue (Y/N) : ");
    recieved = Console.ReadLine().ToUpper();
    while (string.IsNullOrWhiteSpace(recieved) || !recieved.Equals("Y") && !recieved.Equals("N"))
    {
        Console.Write("Invalid input, Continue (Y/N) : ");
        recieved = Console.ReadLine().ToUpper();

    }
    if (recieved.StartsWith("Y"))
        more = true;
    else
        more = false;
} while (more);
