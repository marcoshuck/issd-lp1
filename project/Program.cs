using System;
using project.persistence;
using project.presentation;
using project.services;

namespace project
{
    class Program
    {
        static void Main(string[] args)
        {
            // Persistence layer - Capa de persistencia.
            IUserRepository repository = new UserRepositorySqlite3("URI=file:./project.db");
            repository.Migrate();
            // IUserRepository repository = new UserRepositorySqlServer();
            
            // Business logic - Capa de logica de negocio.
            IUserService svc = new UserService(repository, Console.Out);
            
            // Presentation layer - Capa de presentación
            IUserUi ui = new UserConsole(Console.In, Console.Out, svc);

            while (ui.Run())
            {
                
            }
        }
    }
}