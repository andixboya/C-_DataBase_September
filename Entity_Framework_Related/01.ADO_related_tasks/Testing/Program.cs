using System;
using System.Collections.Generic;
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
            //string[] minionParams = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();

            //string villainName = Console.ReadLine();
            //string countryName = Console.ReadLine();


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

                //4 Add Minion: not finished
                //AddMinion(minionParams, villainName, connection);

                //5 Change Town Names Casing
                //ChangeTownNamesCasing(countryName, connection);

                //6 Remove Villain
                //RemoveVillain(id, connection);

                //7 Get All MinionNames
                //GetAllMinionNames(connection);


            }
        }

        //7. Print All Minion Names 
        private static void GetAllMinionNames(SqlConnection connection)
        {
            List<string> names = new List<string>();
            using (SqlCommand getAllMinionNames = new SqlCommand(Queries.GET_ALL_MINION_NAMES, connection))
            {

                var readData = getAllMinionNames.ExecuteReader();
                while (readData.Read())
                {
                    names.Add((string)readData["Name"]);
                }
            }
            StringBuilder sb = new StringBuilder();


            int forward = 0;
            int backward = names.Count - 1;

            for (int i = 0; i < names.Count; i++)
            {
                if (i % 2 == 0)
                {
                    sb.AppendLine(names[forward++]);
                }
                else
                {
                    sb.AppendLine(names[backward--]);
                }
            }

            Console.WriteLine(sb.ToString().TrimEnd());

        }

        //6 Remove Villain
        private static void RemoveVillain(int id, SqlConnection connection)
        {
            string villainName = string.Empty;

            using (SqlCommand getVillainNameById = new SqlCommand(Queries.GET_VILLAIN_BY_ID, connection))
            {
                getVillainNameById.Parameters.AddWithValue("@Id", id);

                villainName = (string)getVillainNameById.ExecuteScalar();
            }

            if (villainName is null)
            {
                Console.WriteLine("No such villain was found.");
                return;
            }

            int countOfDeletedMinions = 0;

            using (SqlCommand deleteMinionsVillainsRel = new SqlCommand(Queries.DELETE_VILLAIN_FROM_MINIONS_VILLAINS, connection))
            {
                deleteMinionsVillainsRel.Parameters.AddWithValue(@"@villainId", id);

                countOfDeletedMinions = deleteMinionsVillainsRel.ExecuteNonQuery();
            }

            using (SqlCommand deleteVIllainById = new SqlCommand(Queries.DELETE_VILLAIN_BY_ID, connection))
            {
                deleteVIllainById.Parameters.AddWithValue(@"@villainId", id);

                deleteVIllainById.ExecuteNonQuery();
            }

            Console.WriteLine($"{villainName} was deleted.");
            Console.WriteLine($"{countOfDeletedMinions} minions were released.");

        }

        //5 Change Town Names Casing
        private static void ChangeTownNamesCasing(string countryName, SqlConnection connection)
        {
            int countOfAffectedTowns = -1;
            using (SqlCommand changeTownsNames = new SqlCommand(Queries.CHANGE_TOWNS_NAMES_BY_COUNTRY_NAME, connection))
            {
                int countryCode = -1;

                try
                {
                    using (SqlCommand getCountryCodeByCountryName
                    = new SqlCommand("SELECT TOP(1) c.Id FROM Countries AS c WHERE c.Name = @countryName", connection))
                    {
                        getCountryCodeByCountryName.Parameters.AddWithValue(@"@countryName", countryName);

                        countryCode = (int)getCountryCodeByCountryName.ExecuteScalar();
                    }

                    changeTownsNames.Parameters.AddWithValue(@"@countryCode", countryCode);
                    countOfAffectedTowns = changeTownsNames.ExecuteNonQuery();


                }

                catch (Exception e)
                {
                    Console.WriteLine("No towns names were affected!");
                    return;
                }

            }



            using (SqlCommand getTownsByCountryName = new SqlCommand(Queries.GET_TOWNS_NAMES_BY_COUNTRY_NAME, connection))
            {
                getTownsByCountryName.Parameters.AddWithValue(@"@countryName", countryName);

                using (SqlDataReader readData = getTownsByCountryName.ExecuteReader())
                {
                    var result = new List<string>();

                    while (readData.Read())
                    {
                        result.Add(readData["Name"].ToString());
                    }

                    Console.WriteLine($"{countOfAffectedTowns} towns names were affected.");
                    Console.WriteLine($"[{string.Join(", ", result)}]");
                }
            }

        }

        //4 Add Minion /not finished/
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
                    minionId = (int)getMinionIdByName.ExecuteScalar();
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
