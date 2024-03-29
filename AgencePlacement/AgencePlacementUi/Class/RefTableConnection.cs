﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using DataBaseConnection.Table;
using AgencePlacementUi.Class;

namespace DataBaseConnection
{
    public class RefTableConnection
    {
        private MySqlConnection databaseConnection;
        public RefTableConnection()
        {
            string server = "localhost";
            string database = "agence";
            string uid = "root";
            string password = "";
            string connectionString =

             "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

          
            

           
            databaseConnection = new MySqlConnection(connectionString);
           
        }
        public int GetAccountId(string tableName, string email)
        {
            int result;
            if (OpenConnection())
            {
                //Console.WriteLine("connected");
            }


            string query = $"SELECT id FROM {tableName} where email = @email";





            try
            {

                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                cmd.Parameters.AddWithValue("@email", email);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    result = int.Parse(dataReader["id"].ToString());
                }
                else
                {
                    result = 0;
                }
               


            }
            catch
            {
                //Console.Write("error");
                CloseConnection();

                return 0;
            }
            CloseConnection();
            return result;
        }
        public bool AccountExists(string tableName, string email)
        {
            bool result;
            if (OpenConnection())
            {
                //Console.WriteLine("connected");
            }






            try
            {

                MySqlCommand cmd = databaseConnection.CreateCommand();
                cmd.CommandText = $"SELECT id FROM {tableName} where email = @email";
            cmd.Parameters.AddWithValue("@email", email);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }


            }
            catch
            {
                //Console.Write("error");
                CloseConnection();

                return false;
            }
            CloseConnection();
            return result;
        }
        public string GetDefaultValue(string tableName)
        {
            string result;
            if (OpenConnection())
            {
                //Console.WriteLine("connected");
            }


            string query = $"SELECT TOP 1 nom FROM {tableName}";




            try
            {

                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                dataReader.Read();
                result = dataReader["nom"].ToString();



            }
            catch
            {
                //Console.Write("error");
                CloseConnection();

                return "aucun";
            }
            CloseConnection();
            return result;
        }
        private List<string> GetTable(string tableName)
        {
            if (OpenConnection())
            {
                //Console.WriteLine("connected");
            }

            List<string> result = new List<string>();

            string query = $"SELECT nom FROM {tableName}";




            try
            {

                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    result.Add(dataReader["nom"].ToString());

                }
            }
            catch
            {
                //Console.Write("error");
                CloseConnection();
                return new List<string> { "no result" };
            }
            CloseConnection();
            return result;
        }
        public List<MatchCandidat> GetMatchCandidats(int id)
        {
            List<MatchCandidat> m = new List<MatchCandidat>();
            if (OpenConnection())
            {
                //Console.WriteLine("connected");
            }


            string query = $"SELECT o.nom, o.ID FROM CORRESPONDANCE c inner join offer o on o.ID = c.OFFRE where candidat = {id} group by c.ID";




            try
            {

                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list

                while (dataReader.Read())
                {
                    MatchCandidat c = new MatchCandidat();
                    c.data.Add("id", dataReader["id"].ToString());
                    c.data.Add("nom", dataReader["nom"].ToString());
                    m.Add(c);

                }

            }
            catch
            {
                //Console.Write("error");
                CloseConnection();

                return null;
            }

            CloseConnection();
            return m;
        }
        public void UpdateCandidat(Candidat c)
        {
            if (OpenConnection())
            {

            }
            MySqlCommand cmd;
            ///try
            {
                cmd = databaseConnection.CreateCommand();
                cmd.CommandText = "update candidat " +
                    "set nom = @nom , " +
                    "prenom = @prenom ," +

                    "descri = @descri ," +
                                        "titre = @titre ," +
                                                            "pass = @pass ," +
                    "email = @email ," +
                                        "telephone = @telephone ," +
                    "com_pref = (SELECT ID from TYPE_COMMUNICATION WHERE nom = @com_pref) ," +
                    "poste = (SELECT ID from poste WHERE nom = @poste) ," +
                                        "domaine = (SELECT ID from domaine WHERE nom = @domaine) ," +
                    "region = (SELECT ID from region WHERE nom = @region) ," +
                                        "diplome = (SELECT ID from diplome WHERE nom = @diplome) ," +
                                                           "permis = (SELECT ID from permis_conduire WHERE nom = @permis_conduire) ," +
                    "langue = (SELECT ID from langue WHERE nom = @langue) ," +
                                        "experience = (SELECT ID from experience WHERE nom = @experience) ," +
                    "salaire = (SELECT ID from salaire WHERE nom = @salaire) ," +
                    "horaire = (SELECT ID from horaire WHERE nom = @horaire) " +

                    "where id = @id";
                cmd.Parameters.AddWithValue("@nom", c.data["nom"]);
                cmd.Parameters.AddWithValue("@prenom", c.data["prenom"]);

                cmd.Parameters.AddWithValue("@descri", c.data["descri"]);
                cmd.Parameters.AddWithValue("@pass", c.data["pass"]);
                cmd.Parameters.AddWithValue("@titre", c.data["titre"]);
                cmd.Parameters.AddWithValue("@email", c.data["email"]);
                cmd.Parameters.AddWithValue("@telephone", c.data["telephone"]);
                cmd.Parameters.AddWithValue("@com_pref", c.data["com_pref"]);

                cmd.Parameters.AddWithValue("@poste", c.data["poste"]);
                cmd.Parameters.AddWithValue("@domaine", c.data["domaine"]);
                cmd.Parameters.AddWithValue("@region", c.data["region"]);
                cmd.Parameters.AddWithValue("@diplome", c.data["diplome"]);
                cmd.Parameters.AddWithValue("@permis_conduire", c.data["permis_conduire"]);
                cmd.Parameters.AddWithValue("@langue", c.data["langue"]);
                cmd.Parameters.AddWithValue("@experience", c.data["experience"]);
                cmd.Parameters.AddWithValue("@salaire", c.data["salaire"]);
                cmd.Parameters.AddWithValue("@horaire", c.data["horaire"]);
                cmd.Parameters.AddWithValue("@id", c.data["id"]);
                string id = c.data["id"];
                int.Parse(id);

                cmd.ExecuteNonQuery();
                CloseConnection();
            }
            //catch
            {
                CloseConnection();
            }

        }
        public List<Offer> GetOffers(int id)
        {
            List<Offer> o = new List<Offer>();
            if (OpenConnection())
            {
                //Console.WriteLine("connected");
            }


            string query = $"SELECT *, COUNT(c.OFFRE) as application FROM offer o left join correspondance c on c.OFFRE = o.ID where o.id = {id}";




            try
            {

                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list

                while (dataReader.Read() )
                {
                    Offer c = new Offer();
                    c.data.Add("nom", dataReader["nom"].ToString());
                    c.data.Add("id_employeur", dataReader["id_employeur"].ToString());
                    c.data.Add("id", dataReader["id"].ToString());
                    c.data.Add("descri", dataReader["descri"].ToString());
                    c.data.Add("poste", dataReader["poste"].ToString());
                    c.data.Add("domaine", dataReader["domaine"].ToString());
                    c.data.Add("region", dataReader["region"].ToString());
                    c.data.Add("diplome", dataReader["diplome"].ToString());
                    c.data.Add("permis_conduire", dataReader["permis"].ToString());
                    c.data.Add("langue", dataReader["langue"].ToString());
                    c.data.Add("experience", dataReader["experience"].ToString());
                    c.data.Add("salaire", dataReader["salaire"].ToString());
                    c.data.Add("horaire", dataReader["horaire"].ToString());
                    c.data.Add("application", dataReader["application"].ToString());

                    o.Add(c);
                }

            }
            catch
            {
                //Console.Write("error");
                CloseConnection();

                return null;
            }

            CloseConnection();
            return o;
        }
        public Offer GetOffer(int id)
        {
            Offer c = new Offer();
            if (OpenConnection())
            {
                //Console.WriteLine("connected");
            }


            string query = $"SELECT * , COUNT(c.OFFRE) as application FROM offer o left join correspondance c on c.OFFRE = o.ID where o.id = {id}";




           try
            {

                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list

                bool readed = false;
                while (dataReader.Read() && readed == false)
                {

                    readed = true;
                    c.data.Add("nom", dataReader["nom"].ToString());
                    c.data.Add("id_employeur", dataReader["id_employeur"].ToString());
                    c.data.Add("id", dataReader["id"].ToString());
                    c.data.Add("descri", dataReader["descri"].ToString());
                    c.data.Add("poste", dataReader["poste"].ToString());
                    c.data.Add("domaine", dataReader["domaine"].ToString());
                    c.data.Add("region", dataReader["region"].ToString());
                    c.data.Add("diplome", dataReader["diplome"].ToString());
                    c.data.Add("permis_conduire", dataReader["permis"].ToString());
                    c.data.Add("langue", dataReader["langue"].ToString());
                    c.data.Add("experience", dataReader["experience"].ToString());
                    c.data.Add("salaire", dataReader["salaire"].ToString());
                    c.data.Add("horaire", dataReader["horaire"].ToString());
                    c.data.Add("application", dataReader["application"].ToString());

                }

            }
            catch
            {
                //Console.Write("error");
                CloseConnection();

                return null;
            }

            CloseConnection();
            return c;
        }
        public Candidat GetEmployeur(int id)
        {
            Candidat c = new Candidat();
            if (OpenConnection())
            {
                //Console.WriteLine("connected");
            }


            string query = $"SELECT * FROM EMPLOYEUR where id = {id}";




            try
            {

                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list

                bool readed = false;
                while (dataReader.Read() && readed == false)
                {

                    readed = true;
                    c.data.Add("nom", dataReader["nom"].ToString());
                    c.data.Add("prenom", dataReader["prenom"].ToString());
                    c.data.Add("descri", dataReader["descri"].ToString());
                    c.data.Add("pass", dataReader["pass"].ToString());
                    c.data.Add("id", dataReader["ID"].ToString());
                    c.data.Add("titre", dataReader["titre"].ToString());
                    c.data.Add("email", dataReader["email"].ToString());
                    c.data.Add("telephone", dataReader["telephone"].ToString());
                    c.data.Add("com_pref", dataReader["com_pref"].ToString());
                    c.data.Add("entreprise_descri", dataReader["entreprise_descri"].ToString());
                    

                }

            }
            catch
            {
                //Console.Write("error");
                CloseConnection();

                return null;
            }

            CloseConnection();
            return c;
        }
        public Candidat GetCandidat(int id)
        {
            Candidat c = new Candidat();
            if (OpenConnection())
            {
                //Console.WriteLine("connected");
            }


            string query = $"SELECT * FROM CANDIDAT where id = {id}";




            try
            {

                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list

                bool readed = false;
                while (dataReader.Read() && readed == false)
                {

                    readed = true;
                    c.data.Add("nom", dataReader["nom"].ToString());
                    c.data.Add("prenom", dataReader["prenom"].ToString());
                    c.data.Add("descri", dataReader["descri"].ToString());
                    c.data.Add("pass", dataReader["pass"].ToString());
                    c.data.Add("id", dataReader["ID"].ToString());
                    c.data.Add("titre", dataReader["titre"].ToString());
                    c.data.Add("email", dataReader["email"].ToString());
                    c.data.Add("telephone", dataReader["telephone"].ToString());
                    c.data.Add("com_pref", dataReader["com_pref"].ToString());
                    c.data.Add("poste", dataReader["poste"].ToString());
                    c.data.Add("domaine", dataReader["domaine"].ToString());
                    c.data.Add("region", dataReader["region"].ToString());
                    c.data.Add("diplome", dataReader["diplome"].ToString());
                    c.data.Add("permis_conduire", dataReader["permis"].ToString());
                    c.data.Add("langue", dataReader["langue"].ToString());
                    c.data.Add("experience", dataReader["experience"].ToString());
                    c.data.Add("salaire", dataReader["salaire"].ToString());
                    c.data.Add("horaire", dataReader["horaire"].ToString());
                    
                }
                
            }
            catch
            {
                //Console.Write("error");
                CloseConnection();
                
                return null;
            }
          
            CloseConnection();
            Dictionary<string, string> clone = c.data;
            foreach (KeyValuePair<string, string> pair in c.data.ToList())
            {
                if (pair.Value.Equals(null))
                {
                    clone[pair.Key] = GetDefaultValue(pair.Key);
                }
                else if(pair.Key != "id")
                {
                    string s = pair.Value;

                    try
                    {
                        int i = int.Parse(pair.Value);

                        try
                        {

                            clone[pair.Key] = GetFieldById(i, pair.Key);
                        }
                        catch
                        {
                            clone[pair.Key] = s;
                        }
                    }
                    catch
                    {
                        clone[pair.Key] = s;
                        continue;
                    }
                }
            }
            c.data = clone;
            return c;
        }
        public string GetFieldById(int id, string tableName)
        {
           
                if (OpenConnection())
                {
                    //Console.WriteLine("connected");
                }
            
            

            string result ;

            string query = $"SELECT nom FROM {tableName} where id = {id}";




            try
            {

                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                dataReader.Read();
                    result = dataReader["nom"].ToString();

                
            }
            catch
            {
                //Console.Write("error");
                CloseConnection();
                return "Error";
            }
            CloseConnection();
            return result;
        }
    
        public List<string> GetExperience()
        {
            return GetTable("EXPERIENCE");
        }
        public List<string> GetLangue()
        {
            return GetTable("LANGUE");
        }
        public List<string> GetDomaine()
        {
            return GetTable("DOMAINE");
        }
        public List<string> GetRegion()
        {
            return GetTable("REGION");
        }
        public List<string> GetDiplome()
        {
            return GetTable("DIPLOME");
        }
        public List<string> GetTypeCommunication()
        {
            return GetTable("TYPE_COMMUNICATION");
        }
        public List<string> GetSalaire()
        {
            return GetTable("SALAIRE");
        }
        public List<string> GetHoraire()
        {
            return GetTable("HORAIRE");
        }
        public List<string> GetPermisConduire()
        {
            return GetTable("PERMIS_CONDUIRE");
        }
        public List<string> GetPoste()
        {
            return GetTable("POSTE");
        }
        private bool OpenConnection()
        {
            try
            {
                databaseConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
              
                switch (ex.Number)
                {
                    case 0:
                   //     Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                     //   Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
        private bool CloseConnection()
        {
            try
            {
                databaseConnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
              //  Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
