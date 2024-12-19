using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjAcesso
{
    internal class Usuario
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        private List<Ambiente> Ambientes { get; set; } = new List<Ambiente>();

        public Usuario(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public bool ConcederPermissao(Ambiente ambiente)
        {
            if (!Ambientes.Contains(ambiente))
            {
                Ambientes.Add(ambiente);
                return true;
            }
            return false;
        }

        public bool RevogarPermissao(Ambiente ambiente)
        {
            return Ambientes.Remove(ambiente);
        }

        public bool TemPermissao(Ambiente ambiente)
        {
            return Ambientes.Contains(ambiente);
        }

        public override string ToString()
        {
            return $"ID: {Id}, Nome: {Nome}, Permissões: {Ambientes.Count}";
        }
    }
}

