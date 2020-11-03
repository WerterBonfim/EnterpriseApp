using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using NSE.Core.Data;

namespace NSE.Clientes.API.Data
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClientesContext _context;

        public ClienteRepository(ClientesContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;


        public void Dispose()
        {
            _context?.Dispose();
        }

        public void Adicionar(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
        }

        public async Task<IEnumerable<Cliente>> Listar()
        {
            return await _context.Clientes
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Cliente> BuscarPorCpf(string cpf)
        {
            return _context.Clientes
                .FirstOrDefaultAsync(x => x.Cpf.Numero == cpf);
        }
    }
}