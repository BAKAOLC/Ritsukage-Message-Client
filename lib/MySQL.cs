﻿using System;
using MySql.Data.MySqlClient;

namespace Ritsukage_Message_Client.lib.MySQL
{

    public class MySQLHelper
    {
        private static string mConnStr;
        private MySqlConnection connection;
        private MySqlDataReader lastDataReader;

        public static string Set(string host, int post, string username, string password, string database)
        {
            try
            {
                mConnStr = "server=" + host + ";port=" + post +
                    ";user=" + username + ";password=" + password +
                    ";database=" + database + ";Allow User Variables=True;CharSet=utf8mb4";
            }
            catch (Exception ex)
            {
                return "error: " + ex.ToString();
            }
            return "success";
        }

        private bool Open()
        {
            try
            {
                if (connection != null) connection.Open();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool Close()
        {
            try
            {
                if (connection != null) connection.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string Connect()
        {
            try
            {
                connection = new MySqlConnection(mConnStr);
                if (!Open()) return "error: failed to connect mysql";
            }
            catch
            {
                return "error: failed to connect mysql";
            }
            return "success";
        }

        public string Disconnect()
        {
            if (!Close()) return "error: failed to close mysql connection";
            connection = null;
            return "success";
        }

        public string DoSQLCommand(string command)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(command, connection);
                lastDataReader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                lastDataReader = null;
                return "error: " + ex.ToString();
            }

            return "success";
        }

        public string ExecuteSQLCommand(string command)
        {
            MySqlConnection c = null;
            try
            {
                c = new MySqlConnection(mConnStr);
                c.Open();
                MySqlCommand cmd = new MySqlCommand(command, c);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                return "error: " + ex.ToString();
            }
            finally
            {
                c?.Close();
            }
            return "success";
        }

        public MySqlDataReader GetLastDataReader()
        {
            return lastDataReader;
        }

    }
}
