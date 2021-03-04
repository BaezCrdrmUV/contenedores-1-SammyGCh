using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonasApp
{
    public class Persona
    {
        public string Curp { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public List<Telefono> Telefonos { get; set; }
        public List<Email> Emails { get; set; }
    }
}
