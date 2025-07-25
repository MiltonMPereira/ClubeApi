using Microsoft.AspNetCore.Mvc;
using ClubAccessControl.Application.Interfaces;
using AutoMapper;
using ClubAccessControl.API.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Application.Services;

namespace ClubAccessControl.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de planos de acesso
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Operações relacionadas aos planos de acesso")]
    public class PlanoController : ControllerBase
    {
        private readonly IPlanoService _planoService;
        private readonly IMapper _mapper;

        public PlanoController(IPlanoService service, IMapper mapper)
        {
            _planoService = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria um novo plano de acesso
        /// </summary>
        /// <param name="dto">Dados do plano</param>
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo plano")]
        [SwaggerResponse(201, "Plano criado com sucesso", typeof(PlanoDTO))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> Criar(
            [FromBody, SwaggerRequestBody("Dados do plano a ser criado", Required = true)] PlanoDTO dto)
        {

            if (dto.AreasPermitidasIds == null || !dto.AreasPermitidasIds.Any())
                return BadRequest("Plano deve ter pelo menos uma área permitida.");

            var plano = new PlanoAcesso(dto.Nome);

            var resultado = await _planoService.CriarAsync(plano, dto.AreasPermitidasIds);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            var outputDto = _mapper.Map<PlanoDTO>(resultado.Valor);

            return CreatedAtAction(nameof(ObterPorId), new { id = outputDto.Id }, outputDto);
        }

        /// <summary>
        /// Obtém um plano pelo ID
        /// </summary>
        /// <param name="id">ID do plano</param>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um plano por ID")]
        [SwaggerResponse(200, "Plano encontrado", typeof(PlanoDTO))]
        [SwaggerResponse(404, "Plano não encontrado")]
        public async Task<IActionResult> ObterPorId(
            [SwaggerParameter("ID do plano", Required = true)] int id)
        {
            var result = await _planoService.ObterPorIdAsync(id);
            if (!result.Sucesso)
                return NotFound(result.Erro);

            var dto = _mapper.Map<PlanoDTO>(result.Valor);
            return Ok(dto);
        }

        /// <summary>
        /// Lista todos os planos
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os planos")]
        [SwaggerResponse(200, "Lista de planos retornada", typeof(List<PlanoDTO>))]
        public async Task<IActionResult> Listar()
        {
            var result = await _planoService.ListarAsync();
            var dtoList = _mapper.Map<List<PlanoDTO>>(result.Valor);
            return Ok(dtoList);
        }

        /// <summary>
        /// Atualiza um plano existente
        /// </summary>
        /// <param name="id">ID do plano</param>
        /// <param name="dto">Dados atualizados</param>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um plano")]
        [SwaggerResponse(200, "Plano atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos")]
        [SwaggerResponse(404, "Plano não encontrado")]
        public async Task<IActionResult> Atualizar(
            [SwaggerParameter("ID do plano", Required = true)] int id,
            [FromBody, SwaggerRequestBody("Dados atualizados do plano", Required = true)] PlanoUpdateDTO dto)
        {
            var resultado = await _planoService.AtualizarAsync(id, dto.Nome,dto.AreasPermitidasIds);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            return Ok(_mapper.Map<PlanoDTO>(resultado.Valor));
        }

        /// <summary>
        /// Remove um plano
        /// </summary>
        /// <param name="id">ID do plano</param>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove um plano")]
        [SwaggerResponse(204, "Plano removido")]
        [SwaggerResponse(404, "Plano não encontrado")]
        public async Task<IActionResult> Deletar(
            [SwaggerParameter("ID do plano", Required = true)] int id)
        {
            var result = await _planoService.DeletarAsync(id);
            if (!result.Sucesso)
                return NotFound(result.Erro);

            return NoContent();
        }

        /// <summary>
        /// Adiciona uma área permitida ao plano
        /// </summary>
        /// <param name="planoId">ID do plano</param>
        /// <param name="areaId">ID da área</param>
        [HttpPost("{planoId}/areas/{areaId}")]
        [SwaggerOperation(Summary = "Adiciona área permitida ao plano")]
        [SwaggerResponse(204, "Área adicionada ao plano")]
        [SwaggerResponse(400, "Área já existe no plano")]
        [SwaggerResponse(404, "Plano ou área não encontrado")]
        public async Task<IActionResult> AdicionarArea(
            [SwaggerParameter("ID do plano", Required = true)] int planoId,
            [SwaggerParameter("ID da área", Required = true)] int areaId)
        {
            var result = await _planoService.AdicionarAreaAsync(planoId, areaId);
            if (!result.Sucesso)
                return BadRequest(result.Erro);

            return NoContent();
        }

        /// <summary>
        /// Remove uma área permitida do plano
        /// </summary>
        /// <param name="planoId">ID do plano</param>
        /// <param name="areaId">ID da área</param>
        [HttpDelete("{planoId}/areas/{areaId}")]
        [SwaggerOperation(Summary = "Remove área permitida do plano")]
        [SwaggerResponse(204, "Área removida do plano")]
        [SwaggerResponse(400, "Área não existe no plano")]
        [SwaggerResponse(404, "Plano ou área não encontrado")]
        public async Task<IActionResult> RemoverArea(
            [SwaggerParameter("ID do plano", Required = true)] int planoId,
            [SwaggerParameter("ID da área", Required = true)] int areaId)
        {
            var result = await _planoService.RemoverAreaAsync(planoId, areaId);
            if (!result.Sucesso)
                return BadRequest(result.Erro);

            return NoContent();
        }
    }
}