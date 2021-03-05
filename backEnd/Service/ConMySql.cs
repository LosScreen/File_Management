using System.Data;
using MySqlConnector;
using System;

namespace backEnd.Service
{
    public class ConMySQL
    {
        // root1234
        private string _conectionString = "server=127.0.0.1;user=root;password=;database=FileMDB";
        private MySqlConnection _conn;

        public ConMySQL()
        {
            this._conn = new MySqlConnection(_conectionString);
            // _conn.Open();
            // _conn.Close();
        }

        public void Open(){
            _conn.Open();
        }

        public void Close(){
            _conn.Close();
        }
        public DataTable get(string sqlCmd)
        {

            MySqlCommand cmd = new MySqlCommand(sqlCmd, _conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            adapter.Fill(dt,"data");
            return dt.Tables["data"];
        }
        public void execute(string sqlCmd){

            MySqlCommand cmd = new MySqlCommand(sqlCmd, _conn);
            cmd.CommandText = sqlCmd;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = _conn;
            cmd.ExecuteNonQuery();

        }


        public DataTable getData(string sqlCmd)
        {
            _conn.Open();

            MySqlCommand cmd = new MySqlCommand(sqlCmd, _conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            adapter.Fill(dt,"data");
            return dt.Tables["data"];

            _conn.Close();
        }

        public void executeQuery(string sqlCmd){
            _conn.Open();

            MySqlCommand cmd = new MySqlCommand(sqlCmd, _conn);
            cmd.CommandText = sqlCmd;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = _conn;
            cmd.ExecuteNonQuery();

            _conn.Close();
        }

        public void delete(string sqlCmd){
            _conn.Open();

            MySqlCommand cmd = new MySqlCommand(sqlCmd, _conn);
            cmd.CommandText = sqlCmd;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = _conn;
            cmd.ExecuteNonQuery();

            _conn.Close();
        }

    }
} 