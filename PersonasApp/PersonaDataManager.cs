using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PersonasApp
{
    public class PersonaDataManager
    {
        private Persona persona;
        private DatabaseConnection connection;
        private MySqlCommand query;
        private MySqlDataReader reader;
        private MySqlConnection mySqlConnection;
        private List<Telefono> telefonos;
        private List<Email> emails;

        public PersonaDataManager()
        {
            connection = new DatabaseConnection();
        }

        public Persona ObtenerPersonaPorCurp(string curp)
        {
            persona = null;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "SELECT * FROM Persona WHERE curp = @curp"
                };

                MySqlParameter curpParameter = new MySqlParameter("@curp", MySqlDbType.String)
                {
                    Value = curp
                };

                query.Parameters.Add(curpParameter);

                reader = query.ExecuteReader();

                while (reader.Read())
                {
                    persona = new Persona
                    {
                        Curp = reader.GetString(0),
                        Nombres = reader.GetString(1),
                        Apellidos = reader.GetString(2),
                        Telefonos = ObtenerTelefonosDePersona(curp),
                        Emails = ObtenerEmailsPorPersona(curp)
                    };
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                connection.CloseConnection();
            }

            return persona;
        }

        public List<Persona> ObtenerTodasLasPersonas()
        {
            List<Persona> personas = null;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "SELECT * FROM Persona"
                };

                reader = query.ExecuteReader();

                personas = new List<Persona>();
                while (reader.Read())
                {
                    personas.Add(
                        new Persona
                        {
                            Curp = reader.GetString(0),
                            Nombres = reader.GetString(1),
                            Apellidos = reader.GetString(2),
                            Telefonos = ObtenerTelefonosDePersona(reader.GetString(0)),
                            Emails = ObtenerEmailsPorPersona(reader.GetString(0))
                        }
                    );
                }
            }
            catch (MySqlException)
            {

            }
            finally
            {
                connection.CloseConnection();
            }

            return personas;
        }

        private List<Telefono> ObtenerTelefonosDePersona(string curp)
        {
            telefonos = null;
            MySqlDataReader telefonoReader;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "SELECT idtelefono, telefono FROM Telefono WHERE Telefono.curp = @curp"
                };

                MySqlParameter curpParameter = new MySqlParameter("@curp", MySqlDbType.String)
                {
                    Value = curp
                };

                query.Parameters.Add(curpParameter);

                telefonoReader = query.ExecuteReader();
                telefonos = new List<Telefono>();

                while (telefonoReader.Read())
                {
                    telefonos.Add(
                       new Telefono
                       {
                           Id = telefonoReader.GetInt32(0),
                           NumeroTelefono = telefonoReader.GetString(1)
                       } 
                    );
                }
            }
            catch (MySqlException)
            {

            }
            finally
            {
                connection.CloseConnection();
            }

            return telefonos;
        }

        private List<Email> ObtenerEmailsPorPersona(string curp)
        {
            emails = null;
            MySqlDataReader emailReader;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "SELECT idemail, email FROM Email WHERE Email.curp = @curp"
                };

                MySqlParameter curpParameter = new MySqlParameter("@curp", MySqlDbType.String)
                {
                    Value = curp
                };

                query.Parameters.Add(curpParameter);

                emailReader = query.ExecuteReader();
                emails = new List<Email>();

                while (emailReader.Read())
                {
                    emails.Add(
                       new Email
                       {
                           Id = emailReader.GetInt32(0),
                           DireccionEmail = emailReader.GetString(1)
                       }
                    );
                }
            }
            catch (MySqlException)
            {

            }
            finally
            {
                connection.CloseConnection();
            }

            return emails;
        }

        public int AgregarNuevaPersona(Persona nuevaPersona)
        {
            int agregado = 0;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "INSERT INTO Persona (curp, nombres, apellidos) VALUES (@curp, @nombres, @apellidos)"
                };

                MySqlParameter curpParameter = new MySqlParameter("@curp", MySqlDbType.String)
                {
                    Value = nuevaPersona.Curp
                };

                MySqlParameter nombres = new MySqlParameter("@nombres", MySqlDbType.String)
                {
                    Value = nuevaPersona.Nombres
                };

                MySqlParameter apellidos = new MySqlParameter("@apellidos", MySqlDbType.String)
                {
                    Value = nuevaPersona.Apellidos
                };

                query.Parameters.Add(curpParameter);
                query.Parameters.Add(nombres);
                query.Parameters.Add(apellidos);

                agregado = query.ExecuteNonQuery();

                nuevaPersona.Telefonos.ForEach(telefono =>
                    AgregarTelefonoAPersona(telefono.NumeroTelefono, nuevaPersona.Curp)
                );

                nuevaPersona.Emails.ForEach(email =>
                    AgregarEmailAPersona(email.DireccionEmail, nuevaPersona.Curp)
                );
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                connection.CloseConnection();
            }

            return agregado;
        }

        private void AgregarTelefonoAPersona(string telefono, string curp)
        {
            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "INSERT INTO Telefono (telefono, curp) VALUES (@telefono, @curp)"
                };

                MySqlParameter curpParameter = new MySqlParameter("@curp", MySqlDbType.String)
                {
                    Value = curp
                };

                MySqlParameter telefonoParameter = new MySqlParameter("@telefono", MySqlDbType.VarString)
                {
                    Value = telefono
                };

                query.Parameters.Add(curpParameter);
                query.Parameters.Add(telefonoParameter);
                query.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                connection.CloseConnection();
            }
        }

        private void AgregarEmailAPersona(string email, string curp)
        {
            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "INSERT INTO Email (email, curp) VALUES (@email, @curp)"
                };

                MySqlParameter curpParameter = new MySqlParameter("@curp", MySqlDbType.String)
                {
                    Value = curp
                };

                MySqlParameter emailParameter = new MySqlParameter("@email", MySqlDbType.VarString)
                {
                    Value = email
                };

                query.Parameters.Add(curpParameter);
                query.Parameters.Add(emailParameter);
                query.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                connection.CloseConnection();
            }
        }

        public int ActualizarNombresDePersona(string curp, string nombreActualizado)
        {
            int actualizado = 0;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "UPDATE Persona SET nombres = @nombres WHERE curp = @curp"
                };

                MySqlParameter curpParameter = new MySqlParameter("@curp", MySqlDbType.String)
                {
                    Value = curp
                };

                MySqlParameter nombre = new MySqlParameter("@nombres", MySqlDbType.String)
                {
                    Value = nombreActualizado
                };

                query.Parameters.Add(curpParameter);
                query.Parameters.Add(nombre);
                actualizado = query.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                connection.CloseConnection();
            }

            return actualizado;
        }

        public int ActualizarApellidosDePersona(string curp, string apellidosActualizados)
        {
            int actualizado = 0;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "UPDATE Persona SET apellidos = @apellidos WHERE curp = @curp"
                };

                MySqlParameter curpParameter = new MySqlParameter("@curp", MySqlDbType.String)
                {
                    Value = curp
                };

                MySqlParameter apellidos = new MySqlParameter("@apellidos", MySqlDbType.String)
                {
                    Value = apellidosActualizados
                };

                query.Parameters.Add(curpParameter);
                query.Parameters.Add(apellidos);
                actualizado = query.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                connection.CloseConnection();
            }

            return actualizado;
        }

        public int ActualizarTelefonoDePersona(Telefono telefonoActualizado)
        {
            int actualizado = 0;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "UPDATE Telefono SET telefono = @telefono WHERE idtelefono = @idtelefono"
                };

                MySqlParameter telefono = new MySqlParameter("@telefono", MySqlDbType.String)
                {
                    Value = telefonoActualizado.NumeroTelefono
                };

                MySqlParameter idTelefono = new MySqlParameter("@idtelefono", MySqlDbType.Int32)
                {
                    Value = telefonoActualizado.Id
                };

                query.Parameters.Add(telefono);
                query.Parameters.Add(idTelefono);
                actualizado = query.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                connection.CloseConnection();
            }

            return actualizado;
        }

        public int ActualizarEmailDePersona(Email emailActualizado)
        {
            int actualizado = 0;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "UPDATE Email SET email = @email WHERE idemail = @idemail"
                };

                MySqlParameter email = new MySqlParameter("@email", MySqlDbType.String)
                {
                    Value = emailActualizado.DireccionEmail
                };

                MySqlParameter idEmail = new MySqlParameter("@idemail", MySqlDbType.Int32)
                {
                    Value = emailActualizado.Id
                };

                query.Parameters.Add(email);
                query.Parameters.Add(idEmail);
                actualizado = query.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                connection.CloseConnection();
            }

            return actualizado;
        }

        public int AgregarNuevoTelefonoAPersona(string curp, string nuevoTelefono)
        {
            int agregado = 0;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "INSERT INTO Telefono (telefono, curp) VALUES (@telefono, @curp)"
                };

                MySqlParameter telefono = new MySqlParameter("@telefono", MySqlDbType.String)
                {
                    Value = nuevoTelefono
                };

                MySqlParameter curpParam = new MySqlParameter("@curp", MySqlDbType.String)
                {
                    Value = curp
                };

                query.Parameters.Add(telefono);
                query.Parameters.Add(curpParam);
                agregado = query.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                connection.CloseConnection();
            }

            return agregado;
        }

        public int AgregarNuevoEmailAPersona(string curp, string nuevoEmail)
        {
            int agregado = 0;

            try
            {
                mySqlConnection = connection.OpenConnection();
                query = new MySqlCommand("", mySqlConnection)
                {
                    CommandText = "INSERT INTO Email (email, curp) VALUES (@email, @curp)"
                };

                MySqlParameter email = new MySqlParameter("@email", MySqlDbType.String)
                {
                    Value = nuevoEmail
                };

                MySqlParameter curpParam = new MySqlParameter("@curp", MySqlDbType.String)
                {
                    Value = curp
                };

                query.Parameters.Add(email);
                query.Parameters.Add(curpParam);
                agregado = query.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                connection.CloseConnection();
            }

            return agregado;
        }
    }
}
