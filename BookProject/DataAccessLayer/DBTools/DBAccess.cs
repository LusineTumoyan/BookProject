using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccessLayer.DBTools
{
    public class DBAccess
    {
        private static readonly string connectionString = "Server=tcp:192.168.107.14,1433; Initial Catalog=PersonDB;User Id=MicBootcamp;Password = mic12345;Persist Security Info=True;";


        public async Task<T> ExecuteSPWithSingleResult<T>(string procedureName, List<SPParam> parameters) where T : IDataMapper, new()
        {
            T obj = default(T);
            try
            {
                return await Task.Run(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(procedureName, connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        for (int i = 0; i < parameters?.Count; i++)
                        {
                            command.Parameters.AddWithValue(parameters[i].Name, parameters[i].Value);
                        }

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();

                            obj = new T();
                            obj.MapEntity(reader);

                            return obj;
                        }
                    }
                }
                );
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public async Task<List<T>> ExecuteSPWithListResult<T>(string procedureName, List<List<SPParam>> parameters) where T : IDataMapper, new()
        {
            List<T> list = null;
            try
            {
                return await Task.Run(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(procedureName, connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        for (int i = 0; i < parameters?.Count; i++)
                        {
                            for (int j = 0; j < parameters[i].Count; j++)
                            {
                                command.Parameters.AddWithValue(parameters[i][j].Name, parameters[i][j].Value);
                            }
                        }

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            list = new List<T>();
                            while (reader.Read())
                            {
                                T obj = new T();
                                obj.MapEntity(reader);
                                list.Add(obj);
                            }
                            return list;
                        }
                    }
                }
                );
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task ExecuteSPWithNoResult<T>(string procedureName, List<SPParam> parameters) where T : IDataMapper
        {
            try
            {
                await Task.Run(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(procedureName, connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        for (int i = 0; i < parameters?.Count; i++)
                        {
                            command.Parameters.AddWithValue(parameters[i].Name, parameters[i].Value);
                        }

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
