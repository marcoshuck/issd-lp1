using project.domain;

namespace project.services
{
    public interface IUserService
    {
        public User Create(User user);
        public User GetById(int id);
        public User GetByUsername(string username);
        public User GetByEmail(string email);
    }
}