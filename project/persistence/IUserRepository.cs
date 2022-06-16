using project.domain;

namespace project.persistence
{
    public interface IUserRepository
    {
        public User Create(User user);
        public User GetById(int id);
        public User GetByUsername(string username);
        public User GetByEmail(string email);
        public void Migrate();
    }
}