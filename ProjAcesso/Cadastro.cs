using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjAcesso
{
    internal class Cadastro
    {
        private List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        private List<Ambiente> Ambientes { get; set; } = new List<Ambiente>();
        private const string FilePath = "dados.txt"; 

        public void AdicionarUsuario(Usuario usuario)
        {
            Usuarios.Add(usuario);
        }

        public bool RemoverUsuario(Usuario usuario)
        {
            if (!Usuarios.Contains(usuario))
                return false;

            if (usuario.TemPermissao(null))
                return false;

            return Usuarios.Remove(usuario);
        }

        public Usuario PesquisarUsuario(int id)
        {
            return Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public void AdicionarAmbiente(Ambiente ambiente)
        {
            Ambientes.Add(ambiente);
        }

        public bool RemoverAmbiente(Ambiente ambiente)
        {
            return Ambientes.Remove(ambiente);
        }

        public Ambiente PesquisarAmbiente(int id)
        {
            return Ambientes.FirstOrDefault(a => a.Id == id);
        }

        public void Upload()
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                // Salvar usuários
                writer.WriteLine("[Usuarios]");
                foreach (var usuario in Usuarios)
                {
                    writer.WriteLine($"{usuario.Id};{usuario.Nome}");
                }

                // Salvar ambientes e logs
                writer.WriteLine("[Ambientes]");
                foreach (var ambiente in Ambientes)
                {
                    writer.WriteLine($"{ambiente.Id};{ambiente.Nome}");

                    // Salvar logs do ambiente
                    writer.WriteLine("[Logs]");
                    foreach (var log in ambiente.ConsultarLogs())
                    {
                        writer.WriteLine($"{log.DtAcesso};{log.Usuario.Id};{log.TipoAcesso}");
                    }
                }
            }
        }

        public void Download()
        {
            if (!File.Exists(FilePath)) return;

            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line;
                string currentSection = "";
                Ambiente ambienteAtual = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("["))
                    {
                        currentSection = line;
                        ambienteAtual = null; 
                        continue;
                    }

                    switch (currentSection)
                    {
                        case "[Usuarios]":
                            var usuarioData = line.Split(';');
                            if (usuarioData.Length == 2)
                            {
                                var usuario = new Usuario(int.Parse(usuarioData[0]), usuarioData[1]);
                                Usuarios.Add(usuario);
                            }
                            break;

                        case "[Ambientes]":
                            var ambienteData = line.Split(';');
                            if (ambienteData.Length == 2)
                            {
                                ambienteAtual = new Ambiente(int.Parse(ambienteData[0]), ambienteData[1]);
                                Ambientes.Add(ambienteAtual);
                            }
                            break;

                        case "[Logs]":
                            if (ambienteAtual != null)
                            {
                                var logData = line.Split(';');
                                if (logData.Length == 3 && DateTime.TryParse(logData[0], out DateTime dtAcesso))
                                {
                                    var usuarioLog = Usuarios.FirstOrDefault(u => u.Id == int.Parse(logData[1]));
                                    var tipoAcesso = bool.Parse(logData[2]);

                                    if (usuarioLog != null)
                                    {
                                        ambienteAtual.RegistrarLog(new Log(dtAcesso, usuarioLog, tipoAcesso));
                                    }
                                }
                                else
                                {
                                   
                                    Console.WriteLine($"Dados inválidos: {line}");
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}

