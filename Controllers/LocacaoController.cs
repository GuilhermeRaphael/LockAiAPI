using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LockAi.Data;
using LockAi.Models;
using LockAi.Models.Enuns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; 

namespace LockAi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocacaoController : ControllerBase
    {
        private DataContext _context;

        public LocacaoController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocacaoById(int id)
        {
            try
            {
                Locacao locacaoId = await _context.Locacoes.FirstOrDefaultAsync(r => r.Id == id);

                if (locacaoId == null)
                    return NotFound($"Locacão {id} não encontrada.");

                return Ok(locacaoId);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Erro ao buscar locacão. {ex.Message}");
            }
        }

        [HttpPut("{id}/cancelar")]
        
        public async Task<IActionResult> Cancelar (int id)
        {
            try
            {
                var locacaoId = await _context.Locacoes
                    .Include(l => l.PropostaLocacao)
                    .ThenInclude(p => p.Objeto)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (locacaoId == null)
                    return NotFound($"Locacao {id} não encontrada.");
                if (locacaoId.Situacao == SituacaoLocacaoEnum.Cancelada)
                return BadRequest("A locação já está cancelada.");

                locacaoId.Situacao = SituacaoLocacaoEnum.Cancelada;

                var objeto = locacaoId.PropostaLocacao.Objeto;
                objeto.Situacao = SituacaoObjetoEnum.Revisao;
                objeto.DtAtualizao = DateTime.Now;
                var usuario = await GetUsuarioLogadoAsync();
                objeto.IdUsuarioAtualizacao = usuario.Id;

                await _context.SaveChangesAsync();

                return Ok(locacaoId);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro ao cancelar locacão {id}. {ex.Message}");
            }
        }
        
        private async Task<Usuario> GetUsuarioLogadoAsync()
        {
            var userIdClaim =  User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            int userId = int.Parse(userIdClaim.Value);

            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == userId);
        }   

          [HttpGet("GetLocacaoAtivaPorUsuario")]
            public async Task<IActionResult> GetLocacaoAtivaPorUsuario()
            {
                try
                {
                    // ⚠️ PASSO 1: Obter o ID do Usuário Logado de forma segura
                    var usuario = await GetUsuarioLogadoAsync(); // Método temporário (ID=1)
                    // Se o JWT estivesse funcionando, seria algo como:
                    // var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                    if (usuario == null)
                    {
                        // Se o ID do usuário não for encontrado no token ou no mock.
                        return Unauthorized("Usuário não autenticado ou ID de usuário inválido.");
                    }

                    // PASSO 2: Buscar a locação ATIVA (Aprovada ou Em Andamento)
                    var locacaoAtiva = await _context.Locacoes
                        .Include(l => l.PropostaLocacao)
                            .ThenInclude(p => p.PlanoLocacao)
                        .FirstOrDefaultAsync(l =>
                            l.IdUsuario == usuario.Id &&
                            l.Situacao == SituacaoLocacaoEnum.Ativa
                        );


                    if (locacaoAtiva == null)
                    {
                        // Retorna 404 para indicar que não há conteúdo ativo, conforme o frontend espera
                        return NotFound("Nenhuma locação ativa encontrada para este usuário."); 
                    }

                    // PASSO 3: Retorna a locação
                    return Ok(locacaoAtiva);
                }
                catch (System.Exception ex)
                {
                    return BadRequest($"Erro ao buscar locação ativa. {ex.Message}");
                }
            }

    }
}