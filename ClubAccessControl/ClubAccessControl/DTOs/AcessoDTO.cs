using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ClubAccessControl.API.DTOs
{
    /// <summary>
    /// DTO para registro de tentativa de acesso
    /// </summary>
    [SwaggerSchema("Dados para registro de acesso")]
    public class AcessoDTO
    {
        /// <summary>
        /// ID do sócio tentando acessar
        /// </summary> 
        [Required]
        [SwaggerSchema("ID do sócio")]
        public int SocioId { get; set; }

        /// <summary>
        /// ID da área sendo acessada
        /// </summary>
        [Required]
        [SwaggerSchema("ID da área")]
        public int AreaClubeId { get; set; }
    }

    /// <summary>
    /// Resposta de tentativa de acesso
    /// </summary>
    [SwaggerSchema("Resultado de tentativa de acesso")]
    public class AcessoResponseDTO
    {
        /// <summary>
        /// Indica se o acesso foi autorizado
        /// </summary>
        [SwaggerSchema("Acesso autorizado?")]
        public bool Autorizado { get; set; }

        /// <summary>
        /// Mensagem explicativa
        /// </summary>
        [SwaggerSchema("Mensagem de resultado")]
        public string Mensagem { get; set; } = null!;

        /// <summary>
        /// Dados adicionais da tentativa
        /// </summary>
        [SwaggerSchema("Detalhes da tentativa")]
        public TentativaAcessoDTO? Dados { get; set; }
    }

    [SwaggerSchema("Dados da tentativa de acesso")]
    public class TentativaAcessoDTO
    {
        [SwaggerSchema("ID da tentativa")]
        public int Id { get; set; }

        [SwaggerSchema("ID do sócio")]
        public int SocioId { get; set; }

        [SwaggerSchema("Nome do sócio")]
        public string SocioNome { get; set; } = null!;

        [SwaggerSchema("ID da área")]
        public int AreaClubeId { get; set; }

        [SwaggerSchema("Nome da área")]
        public string AreaNome { get; set; } = null!;

        [SwaggerSchema("Data e hora do acesso")]
        public DateTime DataHoraAcesso { get; set; }

        [SwaggerSchema("Acesso foi autorizado?")]
        public bool Autorizado { get; set; }
    }
}

