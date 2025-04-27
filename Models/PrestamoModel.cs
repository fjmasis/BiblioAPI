﻿namespace BiblioAPI.Models
{
    public class PrestamoModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdLibro { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucionEsperada { get; set; }
        public DateTime? FechaDevolucionReal {  get; set; }
        public string Estado {  get; set; }

    }
}
