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
    [SwaggerTag("Controle de acesso �s �reas do clube")]
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
        /// Registra tentativa de acesso a uma �rea
        /// </summary>
        /// <param name="dto">Dados da tentativa</param>
        [HttpPost("acessar")]
        [SwaggerOperation(Summary = "Registra tentativa de acesso")]
        [SwaggerResponse(200, "Tentativa registrada", typeof(AcessoResponseDTO))]
        [SwaggerResponse(400, "Acesso negado")]
        [SwaggerResponse(404, "S�cio ou �rea n�o encontrada")]
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
        /// Obt�m hist�rico de acessos de um s�cio
        /// </summary>
        /// <param name="socioId">ID do s�cio</param>
        [HttpGet("socio/{socioId}")]
        [SwaggerOperation(Summary = "Hist�rico de acessos por s�cio")]
        [SwaggerResponse(200, "Hist�rico retornado", typeof(List<TentativaAcesso>))]
        [SwaggerResponse(404, "S�cio n�o encontrado")]
        public async Task<IActionResult> ObterHistoricoSocio(
            [SwaggerParameter("ID do s�cio", Required = true)] int socioId)
        {
            var resultado = await _service.ObterHistoricoSocioAsync(socioId);
            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Obt�m hist�rico de acessos a uma �rea
        /// </summary>
        /// <param name="areaId">ID da �rea</param>
        [HttpGet("area/{areaId}")]
        [SwaggerOperation(Summary = "Hist�rico de acessos por �rea")]
        [SwaggerResponse(200, "Hist�rico retornado", typeof(List<TentativaAcesso>))]
        [SwaggerResponse(404, "�rea n�o encontrada")]
        public async Task<IActionResult> ObterHistoricoArea(
            [SwaggerParameter("ID da �rea", Required = true)] int areaId)
        {
            var resultado = await _service.ObterHistoricoAreaAsync(areaId);
            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            return Ok(resultado.Valor);
        }
    }
}