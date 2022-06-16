using System;
using System.Data;
using project.domain;
using System.Data.SqlClient;

namespace project.persistence
{
    public class UserRepositorySqlServer : IUserRepository
    {
        private readonly string _dsn;

        public UserRepositorySqlServer(string dsn)
        {
            this._dsn = dsn;
        }
        
        public User Create(User user)
        {
            using (SqlConnection db = new SqlConnection(_dsn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usersInsert", db);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@created_at", user.CreatedAt);
                    cmd.Parameters.AddWithValue("@updated_at", user.UpdatedAt);
                    db.Open();
                    user.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    return user;
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }

        public User GetById(int id)
        {
            return Get(Filters.Id, id);
        }

        public User GetByUsername(string username)
        {
            return Get(Filters.Username, username);
        }

        public User GetByEmail(string email)
        {
            return Get(Filters.Email, email);
        }

        public void Migrate()
        {
            throw new NotImplementedException();
        }

        private enum Filters
        {
            Id,
            Username,
            Email
        }

        private User Get(Filters column, object value)
        {
            using (SqlConnection db = new SqlConnection(_dsn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usersGet", db);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    switch (column)
                    {
                        case Filters.Id:
                        {
                            cmd.Parameters.AddWithValue("@id", value);
                            break;
                        }
                        case Filters.Username:
                        {
                            cmd.Parameters.AddWithValue("@username", value);
                            break;
                        }
                        case Filters.Email:
                        {
                            cmd.Parameters.AddWithValue("@email", value);
                            break;
                        }
                        default:
                        {
                            cmd.Parameters.AddWithValue("@id", value);
                            break;
                        }
                    }

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return null;
                    }

                    string username = Convert.ToString(reader["username"]);
                    string email = Convert.ToString(reader["email"]);

                    User user = new User(username, email)
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        CreatedAt = Convert.ToDateTime(reader["created_at"]),
                        UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                    };
                    
                    reader.Close();

                    return user;
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }
    }
}