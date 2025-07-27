using Microsoft.AspNetCore.Mvc;
using ClubAccessControl.Application.Interfaces;
using AutoMapper;
using ClubAccessControl.API.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Application.Services;
using ClubAccessControl.Domain.Common;

namespace ClubAccessControl.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de áreas do clube
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Operações relacionadas às áreas do clube")]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;
        private readonly IMapper _mapper;

        public AreaController(IAreaService areaService , IMapper mapper)
        {
            _areaService = areaService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todas as áreas do clube
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista todas as áreas",
                         Description = "Retorna uma lista com todas as áreas cadastradas no sistema.")]
        [SwaggerResponse(200, "Lista de áreas retornada com sucesso", typeof(List<AreaDTO>))]
        public async Task<IActionResult> Listar()
        {
            var resultado = await _areaService.ListarAsync();
            var dtos = _mapper.Map<List<AreaDTO>>(resultado.Valor);
            return Ok(dtos);
        }

        /// <summary>
        /// Obtém uma área específica por ID
        /// </summary>
        /// <param name="id">ID da área</param>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém uma área por ID")]
        [SwaggerResponse(200, "Área encontrada", typeof(AreaDTO))]
        [SwaggerResponse(404, "Área não encontrada")]
        public async Task<IActionResult> ObterPorId(
            [SwaggerParameter("ID da área", Required = true)] int id)
        {
            var resultado = await _areaService.ObterPorIdAsync(id);
            if (!resultado.Sucesso)
                return NotFound(resultado.Erro);

            var dto = _mapper.Map<AreaDTO>(resultado.Valor);
            return Ok(dto);
        }

        /// <summary>
        /// Cria uma nova área
        /// </summary>
        /// <param name="dto">Dados da área</param>
        [HttpPost]
        [SwaggerOperation(Summary = "Cria uma nova área")]
        [SwaggerResponse(201, "Área criada com sucesso", typeof(AreaDTO))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> Criar(
            [FromBody, SwaggerRequestBody("Dados da área a ser criada", Required = true)] AreaDTO dto)
        {

            var area = new AreaClube(dto.Nome); 

            var resultado = await _areaService.CriarAsync(area, dto.PlanosPermitidosIds);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            var outputDto = _mapper.Map<AreaDTO>(resultado.Valor);

            return CreatedAtAction(nameof(ObterPorId), new { id = outputDto.Id }, outputDto);
        }

        /// <summary>
        /// Atualiza uma área existente
        /// </summary>
        /// <param name="id">ID da área</param>
        /// <param name="dto">Dados atualizados da área</param>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza uma área existente")]
        [SwaggerResponse(200, "Área atualizada com sucesso")]
        [SwaggerResponse(400, "Dados inválidos")]
        [SwaggerResponse(404, "Área não encontrada")]
        public async Task<IActionResult> Atualizar(
            [SwaggerParameter("ID da área", Required = true)] int id,
            [FromBody, SwaggerRequestBody("Dados atualizados da área", Required = true)] AreaUpdateDTO dto)
        {
            var resultado = await _areaService.AtualizarAsync(id, dto.Nome, dto.PlanosPermitidosIds);
            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            return Ok(_mapper.Map<AreaDTO>(resultado.Valor));
        }

        /// <summary>
        /// Remove uma área
        /// </summary>
        /// <param name="id">ID da área</param>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove uma área")]
        [SwaggerResponse(204, "Área removida com sucesso")]
        [SwaggerResponse(404, "Área não encontrada")]
        public async Task<IActionResult> Deletar(
            [SwaggerParameter("ID da área", Required = true)] int id)
        {
            var resultado = await _areaService.DeletarAsync(id);
            if (!resultado.Sucesso)
                return NotFound(resultado.Erro);

            return NoContent();
        }

        /// <summary>
        /// Associa um plano a uma área
        /// </summary>
        /// <param name="areaId">ID da área</param>
        /// <param name="planoId">ID do plano</param>
        [HttpPost("{areaId}/planos/{planoId}")]
        [SwaggerOperation(Summary = "Associa um plano a uma área")]
        [SwaggerResponse(204, "Plano associado com sucesso")]
        [SwaggerResponse(400, "Associação inválida ou já existente")]
        [SwaggerResponse(404, "Área ou plano não encontrado")]
        public async Task<IActionResult> AdicionarPlano(
            [SwaggerParameter("ID da área", Required = true)] int areaId,
            [SwaggerParameter("ID do plano", Required = true)] int planoId)
        {
            var resultado = await _areaService.AdicionarPlanoAsync(areaId, planoId);
            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            return NoContent();
        }

        /// <summary>
        /// Remove a associação de um plano a uma área
        /// </summary>
        /// <param name="areaId">ID da área</param>
        /// <param name="planoId">ID do plano</param>
        [HttpDelete("{areaId}/planos/{planoId}")]
        [SwaggerOperation(Summary = "Remove a associação de um plano a uma área")]
        [SwaggerResponse(204, "Associação removida com sucesso")]
        [SwaggerResponse(400, "Associação não existente")]
        [SwaggerResponse(404, "Área ou plano não encontrado")]
        public async Task<IActionResult> RemoverPlano(
            [SwaggerParameter("ID da área", Required = true)] int areaId,
            [SwaggerParameter("ID do plano", Required = true)] int planoId)
        {
            var resultado = await _areaService.RemoverPlanoAsync(areaId, planoId);
            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            return NoContent();
        }
    }
}