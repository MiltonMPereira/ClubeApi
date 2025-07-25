using Microsoft.AspNetCore.Mvc;
using ClubAccessControl.Application.Interfaces;
using ClubAccessControl.API.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using ClubAccessControl.Domain.Entidades;
using AutoMapper;

namespace ClubAccessControl.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de acessos
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Controle de acesso às áreas do clube")]
    public class AcessoController : ControllerBase
    {
        private readonly IAcessoService _service;
        private readonly IMapper _mapper;

        public AcessoController(IAcessoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Registra tentativa de acesso a uma área
        /// </summary>
        /// <param name="dto">Dados da tentativa</param>
        [HttpPost("acessar")]
        [SwaggerOperation(Summary = "Registra tentativa de acesso")]
        [SwaggerResponse(200, "Tentativa registrada", typeof(AcessoResponseDTO))]
        [SwaggerResponse(400, "Acesso negado")]
        [SwaggerResponse(404, "Sócio ou área não encontrada")]
        public async Task<IActionResult> RegistrarTentativa(
            [FromBody, SwaggerRequestBody("Dados da tentativa de acesso", Required = true)] AcessoDTO dto)
        {
            var resultado = await _service.RegistrarTentativaAsync(dto.SocioId, dto.AreaClubeId);

            var acessoResponseDTO = new AcessoResponseDTO
            {
                Autorizado = resultado.Sucesso,
                Mensagem = resultado.Sucesso ? "Acesso autorizado." : resultado.Erro ?? "Erro desconhecido.",
                Dados = resultado.Sucesso ? _mapper.Map<TentativaAcessoDTO>(resultado.Valor) : null
            };

            return Ok(acessoResponseDTO);
        }

        /// <summary>
        /// Obtém histórico de acessos de um sócio
        /// </summary>
        /// <param name="socioId">ID do sócio</param>
        [HttpGet("socio/{socioId}")]
        [SwaggerOperation(Summary = "Histórico de acessos por sócio")]
        [SwaggerResponse(200, "Histórico retornado", typeof(List<TentativaAcesso>))]
        [SwaggerResponse(404, "Sócio não encontrado")]
        public async Task<IActionResult> ObterHistoricoSocio(
            [SwaggerParameter("ID do sócio", Required = true)] int socioId)
        {
            var resultado = await _service.ObterHistoricoSocioAsync(socioId);
            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Obtém histórico de acessos a uma área
        /// </summary>
        /// <param name="areaId">ID da área</param>
        [HttpGet("area/{areaId}")]
        [SwaggerOperation(Summary = "Histórico de acessos por área")]
        [SwaggerResponse(200, "Histórico retornado", typeof(List<TentativaAcesso>))]
        [SwaggerResponse(404, "Área não encontrada")]
        public async Task<IActionResult> ObterHistoricoArea(
            [SwaggerParameter("ID da área", Required = true)] int areaId)
        {
            var resultado = await _service.ObterHistoricoAreaAsync(areaId);
            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            return Ok(resultado.Valor);
        }
    }
}