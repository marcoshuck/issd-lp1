using System;
using System.IO;
using project.domain;
using project.services;

namespace project.presentation
{
    public class UserConsole : IUserUi
    {
        private readonly TextWriter writer;
        private readonly TextReader reader;
        private readonly IUserService service;

        public UserConsole(TextReader reader, TextWriter writer, IUserService svc)
        {
            this.reader = reader;
            this.writer = writer;
            this.service = svc;
        }
        
        public void Create()
        {
            try
            {
                writer.WriteLine("Nombre de usuario:");
                string username = reader.ReadLine();
                writer.WriteLine("Correo electrónico:");
                string email = reader.ReadLine();
                User user = service.Create(new User(username, email));
                writer.WriteLine("Usuario creado: {0} {1}", user.Username, user.Email);
            }
            catch (Exception e)
            {
                writer.WriteLine("Fallo al crear usuario: {0}", e);
                throw;
            }
        }

        private enum MenuOptionsGet
        {
            Id = 1,
            Username,
            Email,
            GoBack,
        }
        
        public void Get()
        {
            try
            {
                writer.WriteLine("¿Qué filtro desea aplicar?");
                writer.WriteLine("{0}) A través del ID", MenuOptionsGet.Id.GetHashCode());
                writer.WriteLine("{0}) A traves del nombre de usuario", MenuOptionsGet.Username.GetHashCode());
                writer.WriteLine("{0}) A través del correo electrónico", MenuOptionsGet.Email.GetHashCode());
                writer.WriteLine("{0}) Volver atrás",  MenuOptionsGet.GoBack.GetHashCode());
                
                MenuOptionsGet selectedOption = (MenuOptionsGet)Convert.ToInt32(reader.ReadLine());

                User user = null;
                
                switch (selectedOption)
                {
                    case MenuOptionsGet.Id:
                    {
                        writer.WriteLine("ID:");
                        int id = Convert.ToInt32(reader.ReadLine());
                        user = this.service.GetById(id);
                        break;
                    }
                    case MenuOptionsGet.Username:
                    {
                        writer.WriteLine("Nombre de usuario:");
                        string username = reader.ReadLine();
                        user = this.service.GetByUsername(username);
                        break;
                    }
                    case MenuOptionsGet.Email:
                    {
                        writer.WriteLine("Correo electrónico:");
                        string email = reader.ReadLine();
                        user = this.service.GetByEmail(email);
                        break;
                    }
                    case MenuOptionsGet.GoBack:
                    {
                        writer.WriteLine("Volviendo atrás");
                        break;
                    }
                }

                if (user != null)
                {
                    writer.WriteLine("Usuario: {0}", user);
                }
            }
            catch (Exception e)
            {
                writer.WriteLine("Fallo al obtener usuario: {0}", e);
                throw;
            }
        }

        enum MenuOptionsMain
        {
            Create = 1,
            Get,
            Quit
        }

        public bool Run()
        {
            writer.WriteLine("¿Que operacion desea realizar?");
            writer.WriteLine("{0}) Crear un nuevo usuario", MenuOptionsMain.Create.GetHashCode());
            writer.WriteLine("{0}) Obtener un usuario", MenuOptionsMain.Get.GetHashCode());
            writer.WriteLine("{0}) Salir", MenuOptionsMain.Quit.GetHashCode());
            MenuOptionsMain selectedOption = (MenuOptionsMain)Convert.ToInt32(reader.ReadLine());

            switch (selectedOption)
            {
                case MenuOptionsMain.Create:
                {
                    Create();
                    break;
                }
                case MenuOptionsMain.Get:
                {
                    Get();
                    break;
                }
                case MenuOptionsMain.Quit:
                {
                    return false;
                }
                default:
                {
                    writer.WriteLine("Ninguna opcion valida elegida. Reintentar.");
                    return true;
                }
            }

            return true;
        }
    }
}