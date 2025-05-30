using AdminBack.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminBack.Service.IService
{
    public interface IReporteService
    {
        Task<List<ReporteVentaPagoDto>> ObtenerReporteVentas(DateTime inicio, DateTime fin);
    }
}
