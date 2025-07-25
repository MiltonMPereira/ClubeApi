using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ClubAccessControl.API.DTOs
{
    /// <summary>
    /// DTO para representar um plano de acesso
    /// </summary>
    [SwaggerSchema("Plano de acesso do clube")]
    public class PlanoDTO
    {
        /// <summary>
        /// ID do plano
        /// </summary>
        [SwaggerSchema("ID do plano", ReadOnly = true)]
        public int Id { get; set; }

        /// <summary>
        /// Nome do plano
        /// </summary>
        [Required]
        [SwaggerSchema("Nome do plano")] 
        public string Nome { get; set; } = null!;

        /// <summary>
        /// IDs das áreas permitidas por este plano
        /// </summary>
        [SwaggerSchema("IDs das áreas permitidas")]
        public List<int> AreasPermitidasIds { get; set; } = new();
    }

    public class PlanoUpdateDTO
    {
        /// <summary>  
        /// Nome do plano
        /// </summary>  
        [Required]
        public string? Nome { get; set; }

        [SwaggerSchema("IDs das áreas com acesso. Envie null para não alterar, [] para remover todos")]
        public List<int>? AreasPermitidasIds { get; set; }
    }
}