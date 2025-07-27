using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ClubAccessControl.API.DTOs
{
    /// <summary>
    /// DTO para representação de área do clube
    /// </summary>
    [SwaggerSchema("Área física do clube")]
    public class AreaDTO
    {
        /// <summary>
        /// ID da área
        /// </summary>
        [SwaggerSchema("ID da área", ReadOnly = true)]
        public int Id { get; set; }

        /// <summary>
        /// Nome da área (ex: Piscina, Academia)
        /// </summary>
        [Required]
        [SwaggerSchema("Nome da área")]
        public string Nome { get; set; } = null!;

        /// <summary>
        /// IDs dos planos com acesso permitido
        /// </summary>
        [SwaggerSchema("IDs dos planos com acesso")]
        public List<int> PlanosPermitidosIds { get; set; } = new();
    }

    public class AreaUpdateDTO
    {
        public string? Nome { get; set; }

        [SwaggerSchema("IDs dos planos com acesso. Envie null para não alterar, [] para remover todos")]
        public List<int>? PlanosPermitidosIds { get; set; }
    }
}
