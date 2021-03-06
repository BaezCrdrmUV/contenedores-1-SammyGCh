using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace PersonasApp
{
    public class Program
    {


        public static void Main(string[] args)
        {
            
            bool salir = false;
            string opcionEntrada = "";

            string infoConnection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            Console.WriteLine(infoConnection);
            do
            {
                if (args.Length == 0)
                {
                    // MostrarMenuEntrada();
                    // Console.ReadLine();

                    Console.WriteLine("\n-- DEBE INGRESAR UNA OPCION --");
                    opcionEntrada = "1";
                }

                if (args.Length > 0 && args[0].Equals("1")|| opcionEntrada.Equals("1"))
                {
                    string opcion = "";
                    Console.WriteLine("\nDigita el número de la opción deseada:");
                    Console.WriteLine("1. Consultar Personas.\n2. Agregar Personas.\n3. Actualizar persona.\n4. Salir");
                    Console.WriteLine("\nOpción: ");
                    opcion = Console.ReadLine();

                    PersonaDataManager personaData = new PersonaDataManager();

                    switch (opcion)
                    {
                        case "1":
                            Console.WriteLine("\nDigita el número de la opción deseada:");
                            Console.WriteLine("\n1. Mostrar todas las personas registradas.\n2. Buscar por CURP.");
                            Console.WriteLine("\nOpción: ");
                            opcion = Console.ReadLine();

                            switch (opcion)
                            {
                                case "1":
                                    Console.WriteLine("\n--- TODAS LAS PERSONAS REGISTRADAS: ---");

                                    List<Persona> todasLasPersonas = personaData.ObtenerTodasLasPersonas();

                                    todasLasPersonas.ForEach(persona =>
                                       MostrarPersonaBuscada(persona)
                                    );

                                    break;
                                case "2":
                                    Console.WriteLine("\nIngresa la CURP de la Persona a buscar:");
                                    string curp = Console.ReadLine();

                                    Persona personaBuscada = null;

                                    try
                                    {
                                        personaBuscada = personaData.ObtenerPersonaPorCurp(curp);
                                    }
                                    catch (MySqlException)
                                    {

                                    }

                                    if (personaBuscada != null)
                                    {
                                        MostrarPersonaBuscada(personaBuscada);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\nLa persona con el CURP: {curp} no fue encontrada.");
                                    }

                                    break;
                                default:
                                    break;
                            }

                            break;
                        case "2":
                            Console.WriteLine("\n---Agregar nueva persona: ----");

                            Persona nuevaPersona = new Persona();

                            Console.WriteLine("\nNombres: ");
                            nuevaPersona.Nombres = Console.ReadLine();
                            Console.WriteLine("\nApellidos: ");
                            nuevaPersona.Apellidos = Console.ReadLine();
                            Console.WriteLine("\nCURP: ");
                            nuevaPersona.Curp = Console.ReadLine();

                            Console.WriteLine("\nIngresa la cantidad de telefonos a registrar: ");
                            string cantidad = Console.ReadLine();

                            nuevaPersona.Telefonos = new List<Telefono>();
                            for (int i = 0; i < int.Parse(cantidad); i++)
                            {
                                Console.Write($"Telefono {i + 1}: ");
                                nuevaPersona.Telefonos.Add(
                                    new Telefono
                                    {
                                        NumeroTelefono = Console.ReadLine()
                                    }
                                );
                            }

                            cantidad = "";
                            Console.WriteLine("\nIngresa la cantidad de emails a registrar: ");
                            cantidad = Console.ReadLine();

                            nuevaPersona.Emails = new List<Email>();
                            for (int i = 0; i < int.Parse(cantidad); i++)
                            {
                                Console.Write($"Email {i + 1}: ");
                                nuevaPersona.Emails.Add(
                                    new Email
                                    {
                                        DireccionEmail = Console.ReadLine()
                                    }
                                );
                            }

                            int registrada = personaData.AgregarNuevaPersona(nuevaPersona);

                            if (registrada == 1)
                            {
                                MostrarPersonaRegistrada(nuevaPersona);
                            }
                            else
                            {
                                Console.WriteLine("Ocurrió un error");
                            }

                            break;
                        case "3":
                            Console.WriteLine("\nIngresa la CURP de la Persona a actualizar:");
                            string curpPersona = Console.ReadLine();

                            Persona personaAActualizar = null;

                            try
                            {
                                personaAActualizar = personaData.ObtenerPersonaPorCurp(curpPersona);
                            }
                            catch (MySqlException)
                            {

                            }

                            if (personaAActualizar != null)
                            {
                                MostrarPersonaBuscada(personaAActualizar);

                                Console.WriteLine("\nSelecciona la opción deseada:");
                                Console.WriteLine("\n1. Actualizar Nombres.\n2. Actualizar Apellidos." +
                                    "\n3. Actualizar Telefono.\n4. Actualizar Email.\n5. Agregar nuevo telefono.\n6. Agregar nuevo email." +
                                    "\n7. Cancelar.");
                                Console.WriteLine("\nOpción: ");
                                string opcionActualizacion = Console.ReadLine();

                                switch (opcionActualizacion)
                                {
                                    case "1":
                                        Console.WriteLine("\nIngresa el nuevo nombre: ");
                                        string nuevoNombre = Console.ReadLine();

                                        personaAActualizar.Nombres = nuevoNombre;

                                        int fueNombreActualizado = personaData.ActualizarNombresDePersona(personaAActualizar.Curp, nuevoNombre);

                                        if (fueNombreActualizado == 1)
                                        {
                                            Console.WriteLine($"\n-- NOMBRES ACTUALIZADOS: {nuevoNombre} --");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\n-- ERROR AL ACTUALIZAR --");
                                        }
                                        break;
                                    case "2":
                                        Console.WriteLine("\nIngresa los nuevos apellidos: ");
                                        string nuevosApellidos = Console.ReadLine();

                                        personaAActualizar.Apellidos = nuevosApellidos;

                                        int fueronApellidosActualizados = personaData.ActualizarApellidosDePersona(personaAActualizar.Curp, nuevosApellidos);

                                        if (fueronApellidosActualizados == 1)
                                        {
                                            Console.WriteLine($"\n-- APELLIDOS ACTUALIZADOS: {nuevosApellidos} --");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\n-- ERROR AL ACTUALIZAR --");
                                        }

                                        break;
                                    case "3":
                                        Console.WriteLine("\nIngresa el ID del telefono a actualizar: ");
                                        string idTelefono = Console.ReadLine();

                                        if (personaAActualizar.Telefonos.Any(telefono => telefono.Id == int.Parse(idTelefono)))
                                        {
                                            Console.WriteLine("\nIngresa el número actualizado: ");
                                            string numeroActualizado = Console.ReadLine();

                                            Telefono telefonoActualizado = personaAActualizar.Telefonos.Find(telefono => telefono.Id == int.Parse(idTelefono));
                                            telefonoActualizado.NumeroTelefono = numeroActualizado;

                                            int seActualizo = personaData.ActualizarTelefonoDePersona(telefonoActualizado);

                                            if (seActualizo == 1)
                                            {
                                                Console.WriteLine($"TELEFONO ACTUALIZADO: {numeroActualizado}");
                                            }
                                            else
                                            {
                                                Console.WriteLine("\n-- ERROR AL ACTUALIZAR --");
                                            }

                                        }
                                        else
                                        {
                                            Console.WriteLine("\n-- El ID no existe --");
                                        }
                                        break;
                                    case "4":
                                        Console.WriteLine("\nIngresa el ID del email a actualizar: ");
                                        string idEmail = Console.ReadLine();
                                        

                                        if (personaAActualizar.Emails.Any(email => email.Id == int.Parse(idEmail)))
                                        {
                                            Console.WriteLine("\nIngresa el email actualizado: ");
                                            string emailActualizado = Console.ReadLine();

                                            Email emailAActualizar = personaAActualizar.Emails.Find(email => email.Id == int.Parse(idEmail));
                                            emailAActualizar.DireccionEmail = emailActualizado;

                                            int seActualizo = personaData.ActualizarEmailDePersona(emailAActualizar);

                                            if (seActualizo == 1)
                                            {
                                                Console.WriteLine($"EMAIL ACTUALIZADO: {emailActualizado}");
                                            }
                                            else
                                            {
                                                Console.WriteLine("\n-- ERROR AL ACTUALIZAR --");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("\n-- El ID no existe --");
                                        }

                                        break;
                                    case "5":
                                        Console.WriteLine("\nIngresa el nuevo telefono: ");
                                        string nuevoTelefono = Console.ReadLine();

                                        int seAgrego = personaData.AgregarNuevoTelefonoAPersona(personaAActualizar.Curp, nuevoTelefono);

                                        if (seAgrego == 1)
                                        {
                                            Console.WriteLine($"-- NUEVO TELEFONO AGREGADO: {nuevoTelefono}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\n-- ERROR AL AGREGAR --");
                                        }
                                        break;
                                    case "6":
                                        Console.WriteLine("\nIngresa el nuevo email: ");
                                        string nuevoEmail = Console.ReadLine();

                                        int agregado = personaData.AgregarNuevoEmailAPersona(personaAActualizar.Curp, nuevoEmail);

                                        if (agregado == 1)
                                        {
                                            Console.WriteLine($"-- NUEVO EMAIL AGREGADO: {nuevoEmail}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\n-- ERROR AL AGREGAR --");
                                        }
                                        break;
                                    case "7":
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"\nLa persona con el CURP: {curpPersona} no fue encontrada.");
                            }

                            break;
                        case "4":
                            salir = true;
                            break;
                        default:
                            salir = true;
                            break;
                    }
                }
                // else if (args.Length > 0 && (args[0].Equals("2") || opcionEntrada.Equals("2")))
                // {
                //     salir = true;
                // }
                // else
                // {
                //     Console.WriteLine("\n-- OPCIÓN INCORRECTA --");
                //     MostrarMenuEntrada();
                //     Console.WriteLine(args.Length);
                //     Console.ReadLine();
                // }
                else
                {
                    salir = true;
                }
            } while (!salir);

        }

        private static void MostrarPersonaBuscada(Persona personaBuscada)
        {
            Console.WriteLine("\n--- PERSONA BUSCADA: ---");
            Console.WriteLine($"CURP: {personaBuscada.Curp} \nNombres: {personaBuscada.Nombres}\nApellidos: {personaBuscada.Apellidos}");
            Console.WriteLine("--Telefonos--");
            personaBuscada.Telefonos.ForEach(telefono =>
                 Console.WriteLine($"ID: {telefono.Id} - Numero: {telefono.NumeroTelefono}")
            );
            Console.WriteLine("--Emails--");
            personaBuscada.Emails.ForEach(email =>
                 Console.WriteLine($"ID: {email.Id} - Email: {email.DireccionEmail}")
            );
        }

        private static void MostrarPersonaRegistrada(Persona nuevaPersona)
        {
            Console.WriteLine("\n---PERSONA REGISTRADA---: ");

            Console.WriteLine($"CURP: {nuevaPersona.Curp} \nNombres: {nuevaPersona.Nombres}\nApellidos: {nuevaPersona.Apellidos}");
            Console.WriteLine("--Telefonos--");
            nuevaPersona.Telefonos.ForEach(telefono =>
                 Console.WriteLine($"-Numero: {telefono.NumeroTelefono}")
            );
            Console.WriteLine("--Emails--");
            nuevaPersona.Emails.ForEach(email =>
                 Console.WriteLine($"-Email: {email.DireccionEmail}")
            );
        }

        private static void MostrarMenuEntrada()
        {
            Console.WriteLine("\nDigita el número de la opción deseada:");
            Console.WriteLine("1. Entrar al programa.\n2. Salir.");
            Console.WriteLine("\nOpción: ");
        }
    }
}
