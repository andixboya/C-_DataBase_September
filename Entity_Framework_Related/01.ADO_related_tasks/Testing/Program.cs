using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Testing
{
    class Program
    {
        static void Main()
        {

            //int id = int.Parse(Console.ReadLine());


            string[] minionParams = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();

            string villainName = Console.ReadLine();


            using (SqlConnection connection = new SqlConnection(Settings.CONNECTION_STRING))
            {
                connection.Open();


                //1. Initial Setup 
                //CreateMinionsDataBase(connection);
                UseMinionDb(connection);
                //CreateTables(connection);
                //InsertDataIntoTables(connection);

                //2 Villain Names
                //GetVillainNamesWithMoreThan3Minions(connection);

                //3 Minion Names
                //GetMinionNamesByVillain(connection, id);

                //4 Add Minion:
                AddMinion(minionParams, villainName, connection);


            }
        }

        //4. not finished
        private static void AddMinion(string[] minionParams, string villainName, SqlConnection connection)
        {
            string minionName = minionParams[0];
            int minionAge = int.Parse(minionParams[1]);
            string minionTown = minionParams[2];

            int minionId = -1;

            try
            {
                using (SqlCommand getMinionIdByName = new SqlCommand(Queries.GET_MINION_ID_BY_NAME, connection))
                {

                    getMinionIdByName.Parameters.AddWithValue("@Name", minionName);

                    minionId = (int) getMinionIdByName.ExecuteScalar() ;
                }

            }

            catch (Exception)
            {
                Console.WriteLine("No such minion in Db!");
                return;
            }


        }



        // 3. Minion Names 
        private static void GetMinionNamesByVillain(SqlConnection connection, int villainId)
        {
            StringBuilder sb = new StringBuilder();
            string villainName = string.Empty;

            using (SqlCommand getVillainById = new SqlCommand(Queries.GET_VILLAIN_BY_ID, connection))
            {
                getVillainById.Parameters.AddWithValue("@Id", villainId);

                villainName = (string)getVillainById.ExecuteScalar();
            }

            if (villainName is null)
            {
                sb.AppendLine($"No villain with {villainId} exists in the database.");
                return;
            }

            sb.AppendLine($"Villain: {villainName}");

            using (SqlCommand getMinionNames = new SqlCommand(Queries.GET_MINIONS_NAMES, connection))
            {
                getMinionNames.Parameters.AddWithValue("@Id", villainId);
                SqlDataReader readData = getMinionNames.ExecuteReader();

                int indexer = 0;
                while (readData.Read())
                {
                    var currentMinionName = (string)readData["Name"];
                    var currentMinionAge = (int)readData["Age"];

                    sb.AppendLine($"{++indexer}. {currentMinionName} {currentMinionAge}");
                }

                if (indexer == 0)
                {
                    sb.AppendLine("(no minions)");
                }
            }

            Console.WriteLine(sb.ToString().TrimEnd());
            sb.Clear();
        }

        private static void UseMinionDb(SqlConnection connection)
        {
            using (SqlCommand useDb = new SqlCommand("USE MinionsDb", connection))
            {
                useDb.ExecuteNonQuery();
            }

        }

        //2 Villain Names with more than 3 minions 
        private static void GetVillainNamesWithMoreThan3Minions(SqlConnection connection)
        {
            using (SqlCommand getAllVillains = new SqlCommand(Queries.GET_ALL_VILLAIN_NAMES, connection))
            {
                using (SqlDataReader readData = getAllVillains.ExecuteReader())
                {
                    while (readData.Read())
                    {
                        var villainName = readData["Name"];
                        var minionsCount = readData["MinionsCount"];
                        Console.WriteLine($"{villainName} - {minionsCount}");

                    }
                }
            }


        }

        private static void InsertDataIntoTables(SqlConnection connection)
        {
            foreach (var query in Queries.INSERT_INFO)
            {
                using (SqlCommand current = new SqlCommand(query, connection))
                {
                    var rowsAffected = current.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected);
                }
            }


        }

        private static void CreateTables(SqlConnection connection)
        {
            foreach (var query in Queries.CREATE_TABLES)
            {
                using (SqlCommand current = new SqlCommand(query, connection))
                {
                    current.ExecuteNonQuery();
                }
            }
        }

        private static void CreateMinionsDataBase(SqlConnection connection)
        {
            using (SqlCommand createCmd = new SqlCommand(Queries.CREATE_MINIONS_DB, connection))
            {
                createCmd.ExecuteNonQuery();
            }
        }
    }
}
