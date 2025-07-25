using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ClubAccessControl.API.DTOs
{
    /// <summary>  
    /// DTO para criação ou atualização de sócio  
    /// </summary>  
    [SwaggerSchema("Dados para criação/atualização de sócio")]
    public class SocioDTO
    {
        /// <summary>  
        /// Nome completo do sócio  
        /// </summary>  
        [Required]
        [SwaggerSchema("Nome do sócio")]
        public string Nome { get; set; } = null!;

        /// <summary>  
        /// ID do plano associado  
        /// </summary>  
        [Required]
        [SwaggerSchema("ID do plano")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do plano inválido")]
        public int PlanoId { get; set; }
    }

    public class SocioUpdateDTO
    {
        /// <summary>  
        /// Nome completo do sócio  
        /// </summary>   
        public string? Nome { get; set; }

        /// <summary>  
        /// ID do plano associado  
        /// </summary>  
        [SwaggerSchema("ID do plano. Envie null para não alterar, ou o número do plano para alteração")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do plano inválido")]
        public int? PlanoId { get; set; }
    }

    /// <summary>
    /// DTO para leitura de dados de sócio
    /// </summary>
    [SwaggerSchema("Dados de sócio para leitura")]
    public class SocioReadDTO
    {
        /// <summary>
        /// ID do sócio
        /// </summary>
        [SwaggerSchema("ID do sócio", ReadOnly = true)]
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do sócio
        /// </summary>
        [SwaggerSchema("Nome do sócio")]
        public string Nome { get; set; } = null!;

        /// <summary>
        /// ID do plano associado
        /// </summary>
        [SwaggerSchema("ID do plano do sócio")]
        public int PlanoId { get; set; }

        /// <summary>
        /// Nome do plano associado
        /// </summary>
        [SwaggerSchema("Nome do plano")]
        public string PlanoNome { get; set; } = null!;

        /// <summary>
        /// Nomes das áreas permitidas pelo plano
        /// </summary>
        [SwaggerSchema("Áreas permitidas pelo plano")]
        public List<string> AreasPermitidas { get; set; } = new();
    }
}
