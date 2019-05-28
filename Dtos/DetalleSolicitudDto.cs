﻿namespace Dtos
{
    public class DetalleSolicitudDto
    {
        public int Id { get; set; }
        public int SolicitudesId { get; set; }
        public int PrendasClasificacionId { get; set; }
        public bool LavadoSeco { get; set; }
        public bool LavadoPlanchado { get; set; }
        public bool Planchado { get; set; }
        public bool Doblado { get; set; }
        public string Estado { get; set; }
    }
}
