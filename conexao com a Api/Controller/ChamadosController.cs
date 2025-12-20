using Api_crudPim.Data;
using Api_crudPim.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api_crudPim.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChamadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChamadosController(AppDbContext context)
        {
            _context = context;
        }

        private int? GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            return int.TryParse(userIdClaim, out int id) ? id : null;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetChamados()
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var chamados = await _context.Chamados
              .Where(c => c.UsuarioID == userId.Value)
                      .Select(c => new
                      {
                          c.ID,
                          c.UsuarioID,
                          c.CategoriaID,
                          c.Titulo,
                          c.Descricao,
                          c.Status,
                          c.Prioridade,
                          c.DataCriacao,
                          c.DataAtualizacao
                      })
              .ToListAsync();

            return Ok(chamados);
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChamado(int id)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            
            var chamadoDTO = await _context.Chamados
                .Where(c => c.ID == id && c.UsuarioID == userId.Value)
                .GroupJoin( 
                    _context.Respostas, 
                    chamado => chamado.ID,
                    resposta => resposta.ChamadoID,
                    (chamado, respostasGrupo) => new { chamado, respostasGrupo }
                )
                .SelectMany( 
                    joined => joined.respostasGrupo.DefaultIfEmpty(),
                    (joined, resposta) => new 
                    {
                        
                        joined.chamado.ID,
                        joined.chamado.UsuarioID,
                        joined.chamado.CategoriaID,
                        joined.chamado.Titulo,
                        joined.chamado.Descricao,
                        joined.chamado.Status,
                        joined.chamado.Prioridade,
                        joined.chamado.DataCriacao,
                        joined.chamado.DataAtualizacao,

                        
                        resposta = resposta != null ? new
                        {
                            resposta.ID,
                            resposta.ChamadoID,
                            resposta.UsuarioID,
                            resposta.Mensagem, 
                            resposta.DataEnvio
                        } : null
                    }
                )
                .FirstOrDefaultAsync();

            if (chamadoDTO == null) return NotFound();

            return Ok(chamadoDTO);
        }

        [HttpPost]
        public async Task<ActionResult<Chamados>> PostChamado([FromBody] Chamados chamado)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            chamado.UsuarioID = userId.Value;

            try
            {
                _context.Chamados.Add(chamado);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetChamado), new { id = chamado.ID }, chamado);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "Erro ao salvar chamado.",
                    detalhes = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutChamado(int id, [FromBody] Chamados chamado)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            if (id != chamado.ID)
                return BadRequest();

            var existingChamado = await _context.Chamados
              .FirstOrDefaultAsync(c => c.ID == id && c.UsuarioID == userId.Value);

            if (existingChamado == null) return NotFound();

            _context.Entry(existingChamado).CurrentValues.SetValues(chamado);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChamado(int id)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var chamado = await _context.Chamados
              .FirstOrDefaultAsync(c => c.ID == id && c.UsuarioID == userId.Value);

            if (chamado == null) return NotFound();

            _context.Chamados.Remove(chamado);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}