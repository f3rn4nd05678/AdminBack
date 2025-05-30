using AdminBack.Data;
using AdminBack.Models.DTOs;
using Microsoft.EntityFrameworkCore;

public class NotaCreditoService : INotaCreditoService
{
    private readonly AdminDbContext _context;

    public NotaCreditoService(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Registrar(NotaCreditoDto dto)
    {
        var pedido = await _context.PedidosCliente.FindAsync(dto.PedidoId);
        if (pedido == null) return false;

        var nota = new NotaCredito
        {
            PedidoId = dto.PedidoId,
            Monto = dto.Monto,
            Motivo = dto.Motivo,
            Fecha = DateTime.UtcNow
        };

        _context.NotasCredito.Add(nota);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<NotaCreditoDetalleDto>> ObtenerPorPedido(int pedidoId)
    {
        return await _context.NotasCredito
            .Where(n => n.PedidoId == pedidoId)
            .Select(n => new NotaCreditoDetalleDto
            {
                Id = n.Id,
                Fecha = n.Fecha,
                Monto = n.Monto,
                Motivo = n.Motivo
            })
            .ToListAsync();
    }
}
