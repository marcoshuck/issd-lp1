using System;

namespace project.domain
{
    public class User
    {
        public int Id { get; set; }
        
        private string _username;
        
        private string _email;
        
        public string Username
        {
            get => this._username;
            set
            {
                this._username = value;
                this.UpdatedAt = new DateTime();
            }
        }
        
        public string Email
        {
            get => this._email;
            set
            {
                this._email = value;
                this.UpdatedAt = new DateTime();
            }
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public User(string username, string email)
        {
            CreatedAt = new DateTime();
            UpdatedAt = new DateTime();
            DeletedAt = null;
            this._username = username;
            this._email = email;
        }

        public bool Validate()
        {
            return this._username.Length > 0 && this._email.Length > 0;
        }

        public override string ToString()
        {
            return "ID: " + Id + " | Nombre de usuario: " + Username + " | Correo electrónico: " + Email;
        }
    }
}