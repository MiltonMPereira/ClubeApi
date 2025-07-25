using Microsoft.AspNetCore.Mvc;
using ClubAccessControl.Application.Interfaces;
using AutoMapper;
using ClubAccessControl.API.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using ClubAccessControl.Domain.Entidades;

namespace ClubAccessControl.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de sócios
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Operações relacionadas aos sócios do clube")]
    public class SocioController : ControllerBase
    {
        private readonly ISocioService _socioService;
        private readonly IMapper _mapper;

        public SocioController(ISocioService service, IMapper mapper)
        {
            _socioService = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria um novo sócio
        /// </summary>
        /// <param name="dto">Dados do sócio</param>
        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra um novo sócio")]
        [SwaggerResponse(201, "Sócio criado com sucesso", typeof(SocioReadDTO))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> Criar(
            [FromBody, SwaggerRequestBody("Dados do sócio a ser criado", Required = true)] SocioDTO dto)
        {
            var socio = _mapper.Map<Socio>(dto);
            var result = await _socioService.CriarAsync(socio);

            if (!result.Sucesso)
                return BadRequest(result.Erro);

            var outputDto = _mapper.Map<SocioReadDTO>(result.Valor);
            return CreatedAtAction(nameof(ObterPorId), new { id = outputDto.Id }, outputDto);
        }

        /// <summary>
        /// Obtém um sócio pelo ID
        /// </summary>
        /// <param name="id">ID do sócio</param>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um sócio por ID")]
        [SwaggerResponse(200, "Sócio encontrado", typeof(SocioReadDTO))]
        [SwaggerResponse(404, "Sócio não encontrado")]
        public async Task<IActionResult> ObterPorId(
            [SwaggerParameter("ID do sócio", Required = true)] int id)
        {
            var result = await _socioService.ObterPorIdAsync(id);
            if (!result.Sucesso)
                return NotFound(result.Erro);

            var dto = _mapper.Map<SocioReadDTO>(result.Valor);
            return Ok(dto);
        }

        /// <summary>
        /// Lista todos os sócios
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os sócios")]
        [SwaggerResponse(200, "Lista de sócios retornada", typeof(List<SocioReadDTO>))]
        public async Task<IActionResult> Listar()
        {
            var result = await _socioService.ListarAsync();
            var dtoList = _mapper.Map<List<SocioReadDTO>>(result.Valor);
            return Ok(dtoList);
        }

        /// <summary>
        /// Atualiza um sócio existente
        /// </summary>
        /// <param name="id">ID do sócio</param>
        /// <param name="dto">Dados atualizados</param>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um sócio")]
        [SwaggerResponse(200, "Sócio atualizado")]
        [SwaggerResponse(400, "Dados inválidos")]
        [SwaggerResponse(404, "Sócio não encontrado")]
        public async Task<IActionResult> Atualizar(
            [SwaggerParameter("ID do sócio", Required = true)] int id,
            [FromBody, SwaggerRequestBody("Dados atualizados do sócio", Required = true)] SocioUpdateDTO dto)
        { 
            var resultado = await _socioService.AtualizarAsync(id, dto.Nome, dto.PlanoId);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            return Ok(_mapper.Map<SocioDTO>(resultado.Valor));
        }

        /// <summary>
        /// Remove um sócio
        /// </summary>
        /// <param name="id">ID do sócio</param>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove um sócio")]
        [SwaggerResponse(204, "Sócio removido")]
        [SwaggerResponse(404, "Sócio não encontrado")]
        public async Task<IActionResult> Deletar(
            [SwaggerParameter("ID do sócio", Required = true)] int id)
        {
            var result = await _socioService.DeletarAsync(id);
            if (!result.Sucesso)
                return NotFound(result.Erro);

            return NoContent();
        }

        /// <summary>
        /// Altera o plano de um sócio
        /// </summary>
        /// <param name="socioId">ID do sócio</param>
        /// <param name="novoPlanoId">ID do novo plano</param>
        [HttpPut("{socioId}/plano/{novoPlanoId}")]
        [SwaggerOperation(Summary = "Altera o plano de um sócio")]
        [SwaggerResponse(204, "Plano alterado")]
        [SwaggerResponse(400, "Plano inválido")]
        [SwaggerResponse(404, "Sócio não encontrado")]
        public async Task<IActionResult> AlterarPlano(
            [SwaggerParameter("ID do sócio", Required = true)] int socioId,
            [SwaggerParameter("ID do novo plano", Required = true)] int novoPlanoId)
        {
            var result = await _socioService.AlterarPlanoAsync(socioId, novoPlanoId);
            if (!result.Sucesso)
                return BadRequest(result.Erro);

            return NoContent();
        }
    }
}