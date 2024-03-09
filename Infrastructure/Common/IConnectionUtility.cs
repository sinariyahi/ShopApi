using Dapper;
using Microsoft.AnalysisServices.AdomdClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public interface IConnectionUtility
    {
        SqlConnection GetConfigDbConneciton();
        SqlConnection GetFilterDbConneciton();
        SqlConnection GetAccessDBConnection();
        DataSet GetDataAdomd(string commandText);
        IEnumerable<dynamic> GetData(string commandText);
    }
    public class ConnectionUtility : IConnectionUtility
    {
        private readonly Configs configs;
        public ConnectionUtility(IOptions<Configs> options)
        {
            this.configs = options.Value;
        }
        public SqlConnection GetConfigDbConneciton()
        {
            return new SqlConnection(configs.DBConnection);
        }

        public SqlConnection GetAccessDBConnection()
        {
            return new SqlConnection(configs.AccessDBConnection);
        }

        public SqlConnection GetFilterDbConneciton()
        {
            return new SqlConnection(configs.DBConnection);
        }

        public IEnumerable<dynamic> GetData(string commandText)
        {
            using (var conn = new SqlConnection(configs.DBConnection))
            {
                return conn.Query<dynamic>(commandText).ToList();
            }
        }

        public DataSet GetDataAdomd(string commandText)
        {
            using (AdomdConnection conn = new AdomdConnection(configs.CubeDBConnection))
            {
                conn.Open();
                AdomdCommand cmd = new AdomdCommand(commandText, conn);
                DataSet dbSet = new DataSet();
                AdomdDataAdapter adapter = new AdomdDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dbSet);
                return dbSet;
            }
        }

    }
}
