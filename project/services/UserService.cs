using System;
using System.IO;
using project.domain;
using project.persistence;

namespace project.services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;
        private readonly TextWriter _logger;

        public UserService(IUserRepository repository, TextWriter logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public User Create(User user)
        {
            try
            {
                if (!user.Validate())
                {
                    throw new Exception("user not valid");
                }

                return _repository.Create(user);
            }
            catch (Exception e)
            {
                _logger.WriteLine("Error: {0}", e);
                throw;
            }
        }

        public User GetById(int id)
        {
            try
            {
                User user = _repository.GetById(id);
                if (user == null)
                {
                    _logger.WriteLine("No user found with id: {0}", id);
                }
                return user;
            }
            catch (Exception e)
            {
                _logger.WriteLine("Error: {0}", e);
                throw;
            }
        }

        public User GetByUsername(string username)
        {
            try
            {
                User user = _repository.GetByUsername(username);
                if (user == null)
                {
                    _logger.WriteLine("No user found with username: {0}", username);
                }
                return user;
            }
            catch (Exception e)
            {
                _logger.WriteLine("Error: {0}", e);
                throw;
            }
        }

        public User GetByEmail(string email)
        {
            try
            {
                User user = _repository.GetByEmail(email);
                if (user == null)
                {
                    _logger.WriteLine("No user found with email: {0}", email);
                }
                return user;
            }
            catch (Exception e)
            {
                _logger.WriteLine("Error: {0}", e);
                throw;
            }
        }
    }
}