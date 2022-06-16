using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using project.domain;

namespace project.persistence
{
    public class UserRepositorySqlite3 : IUserRepository
    {
        private readonly string _dsn;

        public UserRepositorySqlite3(string dsn)
        {
            _dsn = dsn;
        }

        public void Migrate()
        {
            using (SQLiteConnection db = new SQLiteConnection(_dsn))
            {
                try
                {
                    db.Open();
                    this.dropTable(db);
                    this.createTable(db);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void createTable(SQLiteConnection db)
        {
            SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE users (id integer primary key autoincrement, username text, email text, created_at text, updated_at text, deleted_at text DEFAULT NULL)", db);
            cmd.ExecuteNonQuery();
        }

        private void dropTable(SQLiteConnection db)
        {
            SQLiteCommand cmd = new SQLiteCommand("DROP TABLE IF EXISTS users", db);
            cmd.ExecuteNonQuery();
        }

        public User Create(User user)
        {
            using (SQLiteConnection db = new SQLiteConnection(_dsn))
            {
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand(this.generateCreateQuery(user), db);
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

        private string generateCreateQuery(User user)
        {
            return @"INSERT INTO users (username, email, created_at, updated_at) VALUES ('" + user.Username + "', '" + user.Email + "', '" + user.CreatedAt.ToLongDateString() + "', '" + user.UpdatedAt.ToLongDateString() + "')";
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

        private enum Filters
        {
            Id,
            Username,
            Email
        }

        private User Get(Filters column, object value)
        {
            using (SQLiteConnection db = new SQLiteConnection(_dsn))
            {
                try
                {
                    string query;
                    switch (column)
                    {
                        case Filters.Id:
                        {
                            query = generateGetQuery("id", value);
                            break;
                        }
                        case Filters.Username:
                        {
                            query = generateGetQuery("username", value);
                            break;
                        }
                        case Filters.Email:
                        {
                            query = generateGetQuery("email", value);
                            break;
                        }
                        default:
                        {
                            query = generateGetQuery("id", value);
                            break;
                        }
                    }
                    
                    SQLiteCommand cmd = new SQLiteCommand(query, db);

                    db.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return null;
                    }

                    string username = Convert.ToString(reader["username"]);
                    string email = Convert.ToString(reader["email"]);

                    User user = new User(username, email)
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        CreatedAt = DateTime.Parse(Convert.ToString(reader["created_at"])),
                        UpdatedAt = DateTime.Parse(Convert.ToString(reader["updated_at"]))
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

        private string generateGetQuery(string col, object val)
        {
            return "SELECT * FROM users WHERE " + @col + " = '" + @val + "' AND deleted_at IS NULL LIMIT 1";
        }
    }
}