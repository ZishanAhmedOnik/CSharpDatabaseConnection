using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace TestMySQLConn
{
    class Program
    {
        static List<int> colMaxLength = new List<int>();
        static string connectionString = "server=localhost;user id=root;database=c_sharp_prac;allowuservariables=True";

        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("=========================");
                Console.WriteLine("1: Query");
                Console.WriteLine("2: Insert");
                Console.WriteLine("3: Update");
                Console.WriteLine("4: Delete");
                Console.WriteLine("0: Exit");
                Console.WriteLine("=========================");
                Console.Write("Enter Command: ");

                string input = Console.ReadLine();
                switch(input)
                {
                    case "1":
                        Query();
                        break;

                    case "2":
                        Insert();
                        break;

                    case "3":
                        Update();
                        break;

                    case "4":
                        Delete();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid Command");
                        break;
                }
            } while (true);
        }

        static void Query()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter("select * from student", connection);
            DataTable dataTable = new DataTable();

            connection.Open();
            adapter.Fill(dataTable);
            connection.Close();

            PrintTable(dataTable);
        }

        static void Insert()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            string insertCommand = "INSERT INTO student (ID, NAME) VALUES(@ID, @NAME)";
            MySqlCommand command = new MySqlCommand(insertCommand, connection);

            MySqlParameter idParam = new MySqlParameter("@ID", MySqlDbType.Int16);
            idParam.Value = id;
            MySqlParameter nameParam = new MySqlParameter("@NAME", MySqlDbType.VarChar, 50);
            nameParam.Value = name;

            command.Parameters.Add(idParam);
            command.Parameters.Add(nameParam);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            Query();
        }

        static void Update()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            Console.Write("Enter Id: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            string updateCommand = "UPDATE student SET NAME = @NAME WHERE ID = @ID";

            MySqlCommand command = new MySqlCommand(updateCommand, connection);

            MySqlParameter idParam = new MySqlParameter("@ID", MySqlDbType.Int16);
            idParam.Value = id;
            MySqlParameter nameParam = new MySqlParameter("@NAME", MySqlDbType.VarChar, 50);
            nameParam.Value = name;

            command.Parameters.Add(idParam);
            command.Parameters.Add(nameParam);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            Query();
        }

        static void Delete()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine());

            MySqlParameter idParam = new MySqlParameter("@ID", MySqlDbType.Int16);
            idParam.Value = id;

            MySqlCommand command = new MySqlCommand("DELETE FROM student WHERE ID = @ID", connection);
            command.Parameters.Add(idParam);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            Query();
        }

        static void PrintTable(DataTable dataTable)
        {
            string[,] dataArray = new string[dataTable.Rows.Count, dataTable.Columns.Count];

            for(int i = 0; i < dataTable.Rows.Count; i++)
            {
                for(int j = 0; j < dataTable.Columns.Count; j++)
                {
                    if (dataTable.Rows[i][j] is int)
                    {
                        dataArray[i, j] = dataTable.Rows[i][j] + "";
                    }
                    else
                    {
                        dataArray[i, j] = (string)dataTable.Rows[i][j];
                    }
                }
            }

            initColMaxLenght(dataArray);
            printDashedLine();

            for (int i = 0; i < dataArray.GetLength(0); i++)
            {

                for (int j = 0; j < dataArray.GetLength(1); j++)
                {
                    printNameAdjusted(dataArray[i, j], colMaxLength[j]);
                }
                Console.WriteLine('|');
                printDashedLine();
            }

            colMaxLength.Clear();
        }

        static void initColMaxLenght(string[,] dataArray)
        {
            int maxLen = 0;

            for(int i = 0; i < dataArray.GetLength(1); i++)
            {
                maxLen = 0;
                for(int j = 0; j < dataArray.GetLength(0); j++)
                {
                    if(dataArray[j, i].Length > maxLen)
                    {
                        maxLen = dataArray[j, i].Length;
                    }
                }

                if(maxLen % 2 == 1)
                {
                    maxLen += 1;
                }

                colMaxLength.Add(maxLen);
            }
        }

        static void printDashedLine()
        {
            Console.Write('+');

            foreach(int len in colMaxLength)
            {
                for(int i = 0; i < len + 2; i++)
                {
                    Console.Write('-');
                }
                Console.Write('+');
            }

            Console.WriteLine();
        }

        static void printNameAdjusted(string name, int maxStringLen)
        {
            int spaces = (maxStringLen - name.Length) / 2;

            Console.Write('|');

            for(int i = 0; i <= spaces; i++)
            {
                Console.Write(' ');
            }

            Console.Write(name);

            for (int i = 0; i <= spaces; i++)
            {
                Console.Write(' ');
            }

            if (name.Length % 2 == 1)
            {
                Console.Write(' ');
            }
        }
    }
}
