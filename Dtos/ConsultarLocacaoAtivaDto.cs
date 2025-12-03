using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockAi.Dtos
{
    public class ConsultarLocacaoAtivaDto
    {
        public int Id { get; set; }
        public float Valor { get; set; }
        public string Situacao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public PropostaLocacaoDto PropostaLocacao { get; set; }
    }

     public class PropostaLocacaoDto
    {
        public int Id { get; set; }
        public PlanoLocacaoDto PlanoLocacao { get; set; }
        public ObjetoDto Objeto { get; set; }
    }

    public class PlanoLocacaoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public class ObjetoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

}