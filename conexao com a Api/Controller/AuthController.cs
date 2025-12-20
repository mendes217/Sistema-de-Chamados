using Api_crudPim.Data;
using Api_crudPim.DTOs;
using Api_crudPim.Models;
using Api_crudPim.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Api_crudPim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email já cadastrado.");

            if (dto.Papel == "Aluno" && string.IsNullOrWhiteSpace(dto.RegistroAluno))
            {
                return BadRequest("O Registro do Aluno (RA) é obrigatório para novos alunos.");
            }

            // 1. Cria a entrada principal (Usuarios)
            var usuario = new Usuarios
            {
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Papel = dto.Papel
                
            };

            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync(); 

               
                if (dto.Papel == "Aluno")
                {
                    _context.Alunos.Add(new Alunos
                    {
                        UsuarioID = usuario.ID,
                        Nome = dto.Nome,
                        RegistroAluno = dto.RegistroAluno, 
                        CursoID = dto.CursoID 
                    });
                }
                else if (dto.Papel == "Funcionario")
                {
                    _context.Funcionarios.Add(new Funcionarios
                    {
                        UsuarioID = usuario.ID,
                        Nome = dto.Nome,
                        MatriculaFuncionario = "FUNC" + usuario.ID.ToString("000")
                    });
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Usuário registrado com sucesso!" });
            }
            catch (Exception ex)
            {
                // Inclui a exceção para ajudar na depuração
                return StatusCode(500, new { error = "Erro interno ao registrar o usuário.", details = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usuario == null) return Unauthorized("Usuário não encontrado.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                return Unauthorized("Senha incorreta.");

            var token = _tokenService.GenerateToken(usuario);

            return Ok(new
            {
                token,
                id = usuario.ID,
                email = usuario.Email,
                papel = usuario.Papel
            });
        }
    }
}