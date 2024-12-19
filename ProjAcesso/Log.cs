using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjAcesso
{
    internal class Log
    {
        public DateTime DtAcesso { get; private set; }
        public Usuario Usuario { get; private set; }
        public bool TipoAcesso { get; private set; } 

        public Log(DateTime dtAcesso, Usuario usuario, bool tipoAcesso)
        {
            DtAcesso = dtAcesso;
            Usuario = usuario;
            TipoAcesso = tipoAcesso;
        }

        public override string ToString()
        {
            return $"{DtAcesso} - {Usuario.Nome} - {(TipoAcesso ? "Autorizado" : "Negado")}";
        }
    }
}

