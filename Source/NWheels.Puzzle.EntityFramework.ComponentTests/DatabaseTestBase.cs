﻿using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWheels.Puzzle.EntityFramework.ComponentTests
{
    [TestFixture]
    public abstract class DatabaseTestBase
    {
        public const string DatabaseName = "NWheelsEFTests";

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        [SetUp]
        public void BaseSetUp()
        {
            this.CompiledModel = null;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public void DropAndCreateTestDatabase()
        {
            using ( var connection = new SqlConnection(this.MasterConnectionString) ) 
            {
                connection.Open();

                var cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format(@"
	                IF EXISTS(SELECT * FROM sys.databases WHERE name='{0}')
	                BEGIN
		                ALTER DATABASE [{0}]
		                SET SINGLE_USER
		                WITH ROLLBACK IMMEDIATE
		                DROP DATABASE [{0}]
	                END

	                DECLARE @FILENAME AS VARCHAR(255)

	                SET @FILENAME = CONVERT(VARCHAR(255), SERVERPROPERTY('instancedefaultdatapath')) + '{0}';

	                EXEC ('CREATE DATABASE [{0}] ON PRIMARY 
		                (NAME = [{0}], 
		                FILENAME =''' + @FILENAME + ''', 
		                SIZE = 25MB, 
		                MAXSIZE = 50MB, 
		                FILEGROWTH = 5MB )')", 
                    DatabaseName);

                cmd.ExecuteNonQuery();
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public DbConnection CreateDbConnection()
        {
            var dbFactory = DbProviderFactories.GetFactory(this.ConnectionStringProviderName);
            var connection = dbFactory.CreateConnection();
            connection.ConnectionString = this.ConnectionString;
            return connection;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string MasterConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["master"].ConnectionString;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["test"].ConnectionString;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string ConnectionStringProviderName
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["test"].ProviderName;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected string GetCommaSeparatedColumnList(DataTable table)
        {
            return string.Join(",", table.Columns.Cast<DataColumn>().Select(c => c.ColumnName + ":" + c.DataType.Name).ToArray());
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected DataTable SelectFromTable(string tableName)
        {
            using ( var connection = (SqlConnection)CreateDbConnection() )
            {
                connection.Open();

                var adapter = new SqlDataAdapter("SELECT * FROM " + tableName, connection);
                var table = new DataTable();
                adapter.Fill(table);
                
                return table;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected void CreateTestDatabaseObjects()
        {
            using ( var connection = CreateDbConnection() )
            {
                var objectContext = CompiledModel.CreateObjectContext<ObjectContext>(connection);
                var script = objectContext.CreateDatabaseScript();

                using ( var command = connection.CreateCommand() )
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = script;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected DbCompiledModel CompiledModel { get; set; }
    }
}
