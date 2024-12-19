using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjAcesso
{
    internal class Ambiente
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public Queue<Log> Logs { get; set; } = new Queue<Log>();

        public Ambiente(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public void RegistrarLog(Log log)
        {
            if (Logs.Count >= 100)
                Logs.Dequeue(); // Remove o log mais antigo para manter o limite
            Logs.Enqueue(log);
        }

        public List<Log> ConsultarLogs(bool? tipoAcesso = null)
        {
            return tipoAcesso == null
                ? Logs.ToList()
                : Logs.Where(log => log.TipoAcesso == tipoAcesso).ToList();
        }

        public override string ToString()
        {
            return $"ID: {Id}, Nome: {Nome}, Logs: {Logs.Count}";
        }
    }
}

