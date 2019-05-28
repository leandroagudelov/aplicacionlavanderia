﻿using Dtos;
using Persistencia;
using Persistencia.Entidades;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Negocio
{
    /// <summary>
    /// Casos de prueba: 
    /// </summary>
    /// <returns></returns>
    public class SolicitudesNegocio : ISolicitudesNegocio
    {
        private LavanderiaDbContext _context;

        public SolicitudesNegocio(LavanderiaDbContext context)
        {
            _context = context;
        }
        
        public async Task GuardarSolicitud(GuardarSolicitudDto solicitud)
        {
            var solicitudEntidad = new Solicitudes
            {
                 ClienteId = solicitud.ClienteId,
                 Estado = solicitud.Estado,
                 Fecha = solicitud.Fecha,
                 SuplementoEntrega = solicitud.SuplementoEntrega
            };
            var entry = _context.Solicitudes.Add(solicitudEntidad);
            await _context.SaveChangesAsync();

            foreach(var detalleSolicitud in solicitud.DetallesSolicitud.DetalleSolicitud)
            {
                var detalleSolicitudEntidad = new DetalleSolicitud
                {
                     SolicitudesId = entry.Id,
                     Doblado = detalleSolicitud.Doblado,
                     LavadoPlanchado = detalleSolicitud.LavadoPlanchado,
                     LavadoSeco = detalleSolicitud.LavadoSeco,
                     Planchado = detalleSolicitud.Planchado,
                     Estado = detalleSolicitud.Estado,
                     PrendasClasificacionId = detalleSolicitud.PrendasClasificacionId
                };
                _context.DetalleSolicitud.Add(detalleSolicitudEntidad);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<SolicitudesConDetallesDto>> ObtenerTodasSolicitudes()
        {
            var listaSolicitudConDetalles = new List<SolicitudesConDetallesDto>();
            var solicitudes =  _context.Solicitudes.ToList();
            foreach(var solicitud in solicitudes)
            {
                var listaDetalles = (await EncontrarDetallesSolicitudes(t => t.SolicitudesId == solicitud.Id)).ToList();
                var listaDetallesDto = new List<DetalleSolicitudDto>();

                foreach(var detalle in listaDetalles)
                {
                    var detalleDto = new DetalleSolicitudDto
                    {
                         Id = detalle.Id,
                         Estado = detalle.Estado,
                         Doblado = detalle.Doblado,
                         LavadoPlanchado = detalle.LavadoPlanchado,
                         LavadoSeco = detalle.LavadoSeco,
                         Planchado = detalle.Planchado,
                         PrendasClasificacionId = detalle.PrendasClasificacionId,
                         SolicitudesId = detalle.SolicitudesId
                    };
                    listaDetallesDto.Add(detalleDto);
                }

                var listadoDetalles = new ListadoDetallesSolicitudDto
                {
                     DetalleSolicitud = listaDetallesDto
                };
                var solicitudConDetalle = new SolicitudesConDetallesDto
                {
                     ClienteId = solicitud.ClienteId,
                     Estado = solicitud.Estado,
                     Fecha = solicitud.Fecha,
                     Id = solicitud.Id,
                     SuplementoEntrega = solicitud.SuplementoEntrega,
                    ListadoDetallesSolicitud = listadoDetalles
                };
                listaSolicitudConDetalles.Add(solicitudConDetalle); 
            }
            return await Task.FromResult(listaSolicitudConDetalles);
        }

        public async Task<List<SolicitudesConDetallesDto>> ConsultarSolicitudPorEstado(string estado)
        {
            var solicitudes =  (await EncontrarSolicitudes(t => t.Estado == estado)).ToList();
            var listaSolicitudConDetalles = new List<SolicitudesConDetallesDto>();

            foreach (var solicitud in solicitudes)
            {
                var listaDetalles = (await EncontrarDetallesSolicitudes(t => t.SolicitudesId == solicitud.Id)).ToList();
                var listaDetallesDto = new List<DetalleSolicitudDto>();

                foreach (var detalle in listaDetalles)
                {
                    var detalleDto = new DetalleSolicitudDto
                    {
                        Id = detalle.Id,
                        Estado = detalle.Estado,
                        Doblado = detalle.Doblado,
                        LavadoPlanchado = detalle.LavadoPlanchado,
                        LavadoSeco = detalle.LavadoSeco,
                        Planchado = detalle.Planchado,
                        PrendasClasificacionId = detalle.PrendasClasificacionId,
                        SolicitudesId = detalle.SolicitudesId
                    };
                    listaDetallesDto.Add(detalleDto);
                }

                var listadoDetalles = new ListadoDetallesSolicitudDto
                {
                    DetalleSolicitud = listaDetallesDto
                };
                var solicitudConDetalle = new SolicitudesConDetallesDto
                {
                    ClienteId = solicitud.ClienteId,
                    Estado = solicitud.Estado,
                    Fecha = solicitud.Fecha,
                    Id = solicitud.Id,
                    SuplementoEntrega = solicitud.SuplementoEntrega,
                    ListadoDetallesSolicitud = listadoDetalles
                };
                listaSolicitudConDetalles.Add(solicitudConDetalle);
            }
            return await Task.FromResult(listaSolicitudConDetalles);
        }

        public async Task ActualizarSolicitud(SolicitudDto solicitud)
        {
            var solicitudEntidad = _context.Solicitudes.First(a => a.Id == solicitud.Id);
            solicitudEntidad.ClienteId = solicitud.ClienteId;
            solicitudEntidad.Estado = solicitud.Estado;
            solicitudEntidad.Fecha = solicitud.Fecha;
            solicitudEntidad.SuplementoEntrega = solicitud.SuplementoEntrega;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarSolicitud(int id)
        {
            _context.Solicitudes.Remove(_context.Solicitudes.Single(a => a.Id == id));
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Solicitudes>> EncontrarSolicitudes(Expression<Func<Solicitudes, bool>> expresion)
        {
            IQueryable<Solicitudes> query = _context.Set<Solicitudes>().Where(expresion);
            return await Task.FromResult(query);
        }

        public async Task<IQueryable<DetalleSolicitud>> EncontrarDetallesSolicitudes(Expression<Func<DetalleSolicitud, bool>> expresion)
        {
            IQueryable<DetalleSolicitud> query = _context.Set<DetalleSolicitud>().Where(expresion);
            return await Task.FromResult(query);
        }
    }
}
