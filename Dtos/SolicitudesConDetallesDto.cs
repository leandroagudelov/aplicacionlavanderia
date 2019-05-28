﻿using System;

namespace Dtos
{
    public class SolicitudesConDetallesDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public bool SuplementoEntrega { get; set; }
        public string Estado { get; set; }
        public ListadoDetallesSolicitudDto ListadoDetallesSolicitud { get; set; }
    }
}
