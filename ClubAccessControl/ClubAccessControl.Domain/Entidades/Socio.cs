using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Domain.Entidades
{
    public class Socio
    {
        public int Id { get; set; }
        public string Nome { get; private set; } = null!;
        public int PlanoId { get; private set; }
        public PlanoAcesso Plano { get; private set; } = null!;

        protected Socio() { }

        public Socio(string nome, int planoId)
        {
            SetNome(nome);
            SetPlano(planoId);
        }

        public void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do sócio não pode ser vazio.");
            Nome = nome;
        }

        public void SetPlano(int planoId)
        {
            if (planoId <= 0)
                throw new ArgumentException("PlanoId inválido.");
            PlanoId = planoId;
        }

        public bool PodeAcessar(AreaClube area)
        {
            if (Plano == null || area == null)
                return false;

            return Plano.PermiteAcesso(area.Id);
        }

        public void AlterarPlano(int novoPlanoId, PlanoAcesso novoPlano)
        {
            if (novoPlanoId <= 0)
                throw new ArgumentException("ID do plano inválido.");

            if (novoPlano == null)
                throw new ArgumentNullException(nameof(novoPlano));

            SetPlano(novoPlanoId);
            Plano = novoPlano;
        }
    }
}
