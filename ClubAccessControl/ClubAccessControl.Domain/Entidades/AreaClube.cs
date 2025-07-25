using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Domain.Entidades
{
    public class AreaClube
    {
        public int Id { get; set; }
        public string Nome { get; private set; } = null!;
        public ICollection<PlanoAcesso> PlanosPermitidos { get; private set; } = new List<PlanoAcesso>();

        protected AreaClube() { }

        public AreaClube(string nome)
        {
            SetNome(nome);
        }

        public void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome da área não pode ser vazio.");
            Nome = nome;
        }

        public bool AdicionarPlano(PlanoAcesso plano)
        {
            if (plano == null)
                throw new ArgumentNullException(nameof(plano), "Plano não pode ser nulo.");

            if (!PlanosPermitidos.Any(p => p.Id == plano.Id))
            {
                PlanosPermitidos.Add(plano);
                return true;
            }
            return false;
        }

        public bool RemoverPlano(int planoId)
        {
            var plano = PlanosPermitidos.FirstOrDefault(p => p.Id == planoId);
            if (plano != null)
            {
                PlanosPermitidos.Remove(plano);
                return true;
            }
            return false;
        }
    }

}
