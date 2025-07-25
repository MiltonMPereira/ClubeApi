
namespace ClubAccessControl.Domain.Entidades
{
    public class TentativaAcesso
    {
        public int Id { get; set; }
        public int SocioId { get; set; }
        public Socio Socio { get; set; } = null!;
        public int AreaClubeId { get; set; }
        public AreaClube AreaClube { get; set; } = null!;
        public DateTime DataHoraAcesso { get; set; }
        public bool Autorizado { get; set; }

        public TentativaAcesso(int socioId, int areaClubeId, bool autorizado)
        {
            SocioId = socioId;
            AreaClubeId = areaClubeId;
            DataHoraAcesso = DateTime.Now;
            Autorizado = autorizado;
        }

        protected TentativaAcesso() { }
    }
}
