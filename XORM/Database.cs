using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;


namespace XORM
{
    internal class Database : IDisposable
    {
        private static string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["FuhuaConn"].ConnectionString;
        private IDbConnection _connection = null;
        private string _commandText = null;
        private ArrayList _parameters = new ArrayList();
        private bool _disposed;

        public IDataReader GetDataReader()
        {
            using (IDbCommand cmd = getCommand())
            {
                return cmd.ExecuteReader();
            }
        }

        private ArrayList _inoutParameters = new ArrayList();

        private IDbCommand getCommand()
        {
            IDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = _commandText;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parm in _parameters)
            {
                cmd.Parameters.Add(parm);
            }
            //既能输入也能返回的参数
            foreach (SqlParameter parm in _inoutParameters)
            {
                parm.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parm);
            }

            return cmd;
        }

        public IDbCommand GetCommand()
        {
            return getCommand();
        }

        public string CommandText
        {
            set { _commandText = value; }
            get { return _commandText; }
        }

        private IDbConnection connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(_connectionString);
                    _connection.Open();
                }
                return _connection;
            }
        }

        public void AddParameters(string name, object obj)
        {
            if (name.StartsWith("@") == false)
            {
                name = "@" + name;
            }
            SqlParameter p = new SqlParameter(name, obj);
            _parameters.Add(p);
        }

        public void AddInOutParameters(string name, object obj)
        {
            if (name.StartsWith("@") == false)
            {
                name = "@" + name;
            }
            SqlParameter p = new SqlParameter(name, obj);
            _inoutParameters.Add(p);
        }

        public ArrayList Parameters
        {
            get { return _parameters; }
        }

        public void Dispose()
        {
            if (_disposed == true)
            {
                return;
            }
            //	Dispose and close the connection
            Close();
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
            _disposed = true;
        }

        public void Close()
        {
            if (_disposed == true)
            {
                return;
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
    
}
