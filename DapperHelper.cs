﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using static Dapper.SqlMapper;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace DapperHelper {
    public class BaseClass {
        public long Id { get; set; }
    }
    public class DapperHelper : IDisposable {
        public DapperHelper () { }
        //This only works for Data Types that inherit from BaseClass, i.e. Classes in our DB
        public static string _connectionString { get; set; }
        public SqlConnection GetConnection (string connectionString = "") => string.IsNullOrWhiteSpace (connectionString) ? new SqlConnection (_connectionString) : new SqlConnection (connectionString);
        public int ConvertBooleanForQuery (bool value) => value ? 1 : 0;

        public IEnumerable<T> Query<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return connection.Query<T> (queryString);
            }
        }
        public IEnumerable<object> Query (string queryString) {
            using (IDbConnection connection = GetConnection ()) {
                return connection.Query<object> (queryString);
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QueryAsync<T> (queryString);
            }
        }
        public async Task<IEnumerable<object>> QueryAsync (string queryString) {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QueryAsync<object> (queryString);
            }
        }
        public object QueryFirst (string queryString) {
            using (IDbConnection connection = GetConnection ()) {
                return connection.QueryFirst (queryString);
            }
        }
        public T QueryFirst<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return connection.QueryFirst<T> (queryString);
            }
        }
        public async Task<object> QueryFirstAsync (string queryString) {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QueryFirstAsync (queryString);
            }
        }
        public async Task<T> QueryFirstAsync<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QueryFirstAsync<T> (queryString);
            }
        }
        public T QueryFirstOrDefault<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return connection.QueryFirstOrDefault<T> (queryString);
            }
        }
        public object QueryFirstOrDefault (string queryString) {
            using (IDbConnection connection = GetConnection ()) {
                return connection.QueryFirstOrDefault (queryString);
            }
        }
        public async Task<T> QueryFirstOrDefaultAsync<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QueryFirstOrDefaultAsync<T> (queryString);
            }
        }
        public async Task<object> QueryFirstOrDefaultAsync (string queryString) {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QueryFirstOrDefaultAsync (queryString);
            }
        }
        public object QueryMultiple<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return connection.QueryMultiple (queryString);
            }
        }
        public async Task<object> QueryMultipleAsync<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QueryMultipleAsync (queryString);
            }
        }
        public T QuerySingle<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return connection.QuerySingle<T> (queryString);
            }
        }
        public object QuerySingle (string queryString) {
            using (IDbConnection connection = GetConnection ()) {
                return connection.QuerySingle (queryString);
            }
        }
        public async Task<T> QuerySingleAsync<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QuerySingleAsync<T> (queryString);
            }
        }
        public async Task<object> QuerySingleAsync (string queryString) {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QuerySingleAsync (queryString);
            }
        }
        public void Execute (string queryString) {
            using (IDbConnection connection = GetConnection ()) {
                connection.Execute (queryString);
            }
        }
        public async Task ExecuteAsync (string queryString) {
            using (IDbConnection connection = GetConnection ()) {
                await connection.ExecuteAsync (queryString);
            }
        }
        public IDataReader ExecuteReader<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return connection.ExecuteReader (queryString);
            }
        }
        public async Task<IDataReader> ExecuteReaderAsync<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.ExecuteReaderAsync (queryString);
            }
        }
        public object ExecuteScalar<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return connection.ExecuteScalar (queryString);
            }
        }
        public async Task<object> ExecuteScalarAsync<T> (string queryString) where T : BaseClass, new () {
            using (IDbConnection connection = GetConnection ()) {
                return await connection.ExecuteScalarAsync (queryString);
            }
        }
        public void Insert<T> (T entity) {
            IEnumerable<string> columns = GetColumns<T> ();
            string stringOfColumns = string.Join (", ", columns);
            string stringOfParameters = string.Join (", ", columns.Select (e => "@" + e));
            string query = $"INSERT INTO {getName<T>()} ({stringOfColumns}) VALUES ({stringOfParameters})";

            using (IDbConnection connection = GetConnection ()) {
                connection.Execute (query, entity);
            }
        }
        public async Task InsertAsync<T> (T entity) {
            IEnumerable<string> columns = GetColumns<T> ();
            string stringOfColumns = string.Join (", ", columns);
            string stringOfParameters = string.Join (", ", columns.Select (e => "@" + e));
            string query = $"INSERT INTO {getName<T>()} ({stringOfColumns}) VALUES ({stringOfParameters})";

            using (IDbConnection connection = GetConnection ()) {
                await connection.ExecuteAsync (query, entity);
            }
        }
        public void Delete<T> (T entity) {
            string query = $"DELETE FROM {getName<T>()} WHERE Id = @Id";

            using (IDbConnection connection = GetConnection ()) {
                connection.Execute (query, entity);
            }
        }
        public async Task DeleteAsync<T> (T entity) {
            string query = $"DELETE FROM {getName<T>()} WHERE Id = @Id";

            using (IDbConnection connection = GetConnection ()) {
                await connection.ExecuteAsync (query, entity);
            }
        }

        public void Update<T> (T entity) {
            IEnumerable<string> columns = GetColumns<T> ();
            string stringOfColumns = string.Join (", ", columns.Select (e => $"{e} = @{e}"));
            string query = $"UPDATE {getName<T>()} SET {stringOfColumns} WHERE Id = @Id";

            using (IDbConnection connection = GetConnection ()) {
                connection.Execute (query, entity);
            }
        }
        public async Task UpdateAsync<T> (T entity) {
            IEnumerable<string> columns = GetColumns<T> ();
            string stringOfColumns = string.Join (", ", columns.Select (e => $"{e} = @{e}"));
            string query = $"UPDATE {getName<T>()} SET {stringOfColumns} WHERE Id = @Id";

            using (IDbConnection connection = GetConnection ()) {
                await connection.ExecuteAsync (query, entity);
            }
        }
        public T GetById<T> (long Id) {
            string query = $"SELECT * FROM {getName<T>()} WHERE Id = {Id}";
            using (IDbConnection connection = GetConnection ()) {
                return connection.QuerySingleOrDefault<T> (query);
            }
        }
        public async Task<T> GetByIdAsync<T> (int Id) {
            string query = $"SELECT * FROM {getName<T>()} WHERE Id = {Id}";
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QueryFirstOrDefaultAsync<T> (query);
            }
        }
        public int Count<T> () {
            string query = $"SELECT COUNT(*) FROM {getName<T>()}";
            using (IDbConnection connection = GetConnection ()) {
                return connection.QuerySingleOrDefault<int> (query);
            }
        }
        public async Task<int> CountAsync<T> () {
            string query = $"SELECT COUNT(*) FROM {getName<T>()}";
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QueryFirstOrDefaultAsync<int> (query);
            }
        }
        public IEnumerable<T> Get<T> () {
            string query = $"SELECT * FROM {getName<T>()}";
            using (IDbConnection connection = GetConnection ()) {
                return connection.Query<T> (query);
            }
        }
        public async Task<IEnumerable<T>> GetAsync<T> () {
            string query = $"SELECT * FROM {getName<T>()}";
            using (IDbConnection connection = GetConnection ()) {
                return await connection.QueryAsync<T> (query);
            }
        }
        public string getName<T> () {
            string Name = typeof (T).Name;
            char[] vowels = { 'a', 'e', 'i', 'o', 'u' };
            return (Name[Name.Length - 1] == 'y') && !vowels.Contains (Name[Name.Length - 2]) ? $"{Name.Substring(0, Name.Length - 1)}ies" : $"{Name}s";
        }
        private IEnumerable<string> GetColumns<T> () =>
            typeof (T).GetProperties ().Where (x => x.Name != "Id" && !x.PropertyType.GetTypeInfo ().IsGenericType && !x.GetGetMethod ().IsVirtual).Select (x => x.Name);

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle (IntPtr.Zero, true);
        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this);
        }~DapperHelper () {
            Dispose (true);
        }
        protected virtual void Dispose (bool disposing) {
            if (disposed) return;
            if (disposing)
                handle.Dispose ();
            disposed = true;
        }
    }
}