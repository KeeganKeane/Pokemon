using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient; 
using System.IO;
using System.Net;
using Newtonsoft.Json;


//This Program recieves Pokemon stats from the Pokemon API and sends them to SQL 

namespace CryptoCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //The pokemon we are searching for
            string[] arr = new string []{"mudkip", "pikachu", "geodude", "dragonite", "charmander", "mew"};

            //Open server 
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "pokemon";
            Console.WriteLine(dbCon.IsConnect()); 
            Console.WriteLine();



            for (int i = 0; i < arr.Length; i++)
            {
                WebRequest request = HttpWebRequest.Create("https://pokeapi.co/api/v2/pokemon/" + arr[i]);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string joke_JSON = reader.ReadToEnd();
                Pokemon monster = JsonConvert.DeserializeObject<Pokemon>(joke_JSON);

                var query = "INSERT INTO pokemon.pokemon (`name`, `height`) VALUES(\"" + monster.name + "\", \"" + monster.height + "\")";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                Console.WriteLine(cmd.CommandText);
                cmd.ExecuteNonQuery();
                Console.WriteLine(monster.name);
                Console.WriteLine(monster.height);
                Console.WriteLine(monster.types[0].type.name);
            }


            Console.ForegroundColor = ConsoleColor.Yellow;
            dbCon.Close();
        }
    }

    public class DBConnection
    {
        private DBConnection()
        {
        }

        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server=localhost; database={0}; UID=root; password=Hasegawa@78#", databaseName);
                connection = new MySqlConnection(connstring);
                connection.Open();
            }

            return true;
        }

        public void Close()
        {
            connection.Close();
        }
    }

    public class Ability2
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Ability
    {
        public Ability2 ability { get; set; }
        public bool is_hidden { get; set; }
        public int slot { get; set; }
    }

    public class Form
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Version
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class GameIndice
    {
        public int game_index { get; set; }
        public Version version { get; set; }
    }

    public class Move2
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class MoveLearnMethod
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class VersionGroup
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class VersionGroupDetail
    {
        public int level_learned_at { get; set; }
        public MoveLearnMethod move_learn_method { get; set; }
        public VersionGroup version_group { get; set; }
    }

    public class Move
    {
        public Move2 move { get; set; }
        public List<VersionGroupDetail> version_group_details { get; set; }
    }

    public class Species
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Sprites
    {
        public string back_default { get; set; }
        public object back_female { get; set; }
        public string back_shiny { get; set; }
        public object back_shiny_female { get; set; }
        public string front_default { get; set; }
        public object front_female { get; set; }
        public string front_shiny { get; set; }
        public object front_shiny_female { get; set; }
    }

    public class Stat2
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Stat
    {
        public int base_stat { get; set; }
        public int effort { get; set; }
        public Stat2 stat { get; set; }
    }

    public class Type2
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Type
    {
        public int slot { get; set; }
        public Type2 type { get; set; }
    }

    public class Pokemon
    {
        public List<Ability> abilities { get; set; }
        public int base_experience { get; set; }
        public List<Form> forms { get; set; }
        public List<GameIndice> game_indices { get; set; }
        public int height { get; set; }
        public List<object> held_items { get; set; }
        public int id { get; set; }
        public bool is_default { get; set; }
        public string location_area_encounters { get; set; }
        public List<Move> moves { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public Species species { get; set; }
        public Sprites sprites { get; set; }
        public List<Stat> stats { get; set; }
        public List<Type> types { get; set; }
        public int weight { get; set; }
    }
}
