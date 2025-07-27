using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Domain.Entidades
{
    public class PlanoAcesso
    {
        public int Id { get; set; }
        public string Nome { get; private set; } = null!;
        public List<AreaClube> AreasPermitidas { get; private set; } = new List<AreaClube>();

        protected PlanoAcesso() { }

        public PlanoAcesso(string nome)
        {
            SetNome(nome);
        }

        public void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do plano não pode ser vazio.");
            Nome = nome;
        }

        public void AdicionarArea(AreaClube area)
        {
            if (area == null)
                throw new ArgumentNullException(nameof(area), "Área não pode ser nula.");

            if (!AreasPermitidas.Any(a => a.Id == area.Id))
            {
                AreasPermitidas.Add(area);
            }
        }

        public bool RemoverArea(int areaId)
        {
            var area = AreasPermitidas.FirstOrDefault(a => a.Id == areaId);
            if (area != null)
            {
                AreasPermitidas.Remove(area);
                return true;
            }
            return false;
        }

        public bool PermiteAcesso(int areaId)
        {
            return AreasPermitidas.Any(a => a.Id == areaId);
        }
    }

}
