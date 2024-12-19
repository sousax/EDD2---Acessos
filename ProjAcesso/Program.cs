using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjAcesso
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cadastro cadastro = new Cadastro();
            cadastro.Download();

            int opcao;
            do
            {
                Console.WriteLine("\n--- Menu ---");
                Console.WriteLine("0. Sair");
                Console.WriteLine("1. Cadastrar ambiente");
                Console.WriteLine("2. Consultar ambiente");
                Console.WriteLine("3. Excluir ambiente");
                Console.WriteLine("4. Cadastrar usuário");
                Console.WriteLine("5. Consultar usuário");
                Console.WriteLine("6. Excluir usuário");
                Console.WriteLine("7. Conceder permissão de acesso");
                Console.WriteLine("8. Revogar permissão de acesso");
                Console.WriteLine("9. Registrar acesso");
                Console.WriteLine("10. Consultar logs");
                Console.Write("Escolha uma opção: ");
                // Verificação de entrada do usuário
                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida! Por favor, insira um número.");
                    continue;
                }

                switch (opcao)
                {
                    case 1:
                        Console.Write("ID do ambiente: ");
                        int idAmbiente = int.Parse(Console.ReadLine());
                        Console.Write("Nome do ambiente: ");
                        string nomeAmbiente = Console.ReadLine();
                        cadastro.AdicionarAmbiente(new Ambiente(idAmbiente, nomeAmbiente));
                        Console.WriteLine("Ambiente cadastrado com sucesso!");
                        break;
                    case 2:
                        Console.Write("ID do ambiente: ");
                        int idConsultaAmbiente = int.Parse(Console.ReadLine());
                        var ambienteConsulta = cadastro.PesquisarAmbiente(idConsultaAmbiente);
                        if (ambienteConsulta != null)
                        {
                            Console.WriteLine($"Ambiente ID: {ambienteConsulta.Id}, Nome: {ambienteConsulta.Nome}");
                        }
                        else
                        {
                            Console.WriteLine("Ambiente não encontrado.");
                        }
                        break;
                    case 3:
                        // Excluir ambiente
                        Console.Write("ID do ambiente a ser excluído: ");
                        int idExcluirAmbiente = int.Parse(Console.ReadLine());
                        var ambienteExcluir = cadastro.PesquisarAmbiente(idExcluirAmbiente);
                        if (ambienteExcluir != null) // Verifica se não há logs
                        {
                            if (cadastro.RemoverAmbiente(ambienteExcluir))
                            {
                                Console.WriteLine("Ambiente excluído com sucesso!");
                            }
                            else
                            {
                                Console.WriteLine("Erro ao excluir ambiente.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ambiente não encontrado ou possui logs, não pode ser excluído.");
                        }
                        break;

                    case 4:
                        Console.Write("ID do usuário: ");
                        int idUsuario = int.Parse(Console.ReadLine());
                        Console.Write("Nome do usuário: ");
                        string nomeUsuario = Console.ReadLine();
                        cadastro.AdicionarUsuario(new Usuario(idUsuario, nomeUsuario));
                        Console.WriteLine("Usuário cadastrado com sucesso!");
                        break;
                    case 5:
                        // Consultar usuário
                        Console.Write("ID do usuário: ");
                        int idConsultaUsuario = int.Parse(Console.ReadLine());
                        var usuarioConsulta = cadastro.PesquisarUsuario(idConsultaUsuario);
                        if (usuarioConsulta != null)
                        {
                            Console.WriteLine($"Usuário ID: {usuarioConsulta.Id}, Nome: {usuarioConsulta.Nome}");
                        }
                        else
                        {
                            Console.WriteLine("Usuário não encontrado.");
                        }
                        break;

                    case 6:
                        // Excluir usuário
                        Console.Write("ID do usuário a ser excluído: ");
                        int idExcluirUsuario = int.Parse(Console.ReadLine());
                        var usuarioExcluir = cadastro.PesquisarUsuario(idExcluirUsuario);
                        if (usuarioExcluir != null) // Verifica se o usuário tem permissões
                        {
                            if (cadastro.RemoverUsuario(usuarioExcluir))
                            {
                                Console.WriteLine("Usuário excluído com sucesso!");
                            }
                            else
                            {
                                Console.WriteLine("Erro ao excluir usuário.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Usuário não encontrado ou tem permissões ativas.");
                        }
                        break;
                    case 7:
                        Console.Write("ID do ambiente: ");
                        int ambienteId = int.Parse(Console.ReadLine());
                        Console.Write("ID do usuário: ");
                        int usuarioId = int.Parse(Console.ReadLine());

                        var ambiente = cadastro.PesquisarAmbiente(ambienteId);
                        var usuario = cadastro.PesquisarUsuario(usuarioId);

                        if (ambiente != null && usuario != null)
                        {
                            if (usuario.ConcederPermissao(ambiente))
                                Console.WriteLine("Permissão concedida!");
                            else
                                Console.WriteLine("Permissão já existente.");
                        }
                        else
                        {
                            Console.WriteLine("Usuário ou ambiente não encontrado.");
                        }
                        break;
                    case 8:
                        // Revogar permissão
                        Console.Write("ID do ambiente: ");
                        int idAmbienteRevogar = int.Parse(Console.ReadLine());
                        Console.Write("ID do usuário: ");
                        int idUsuarioRevogar = int.Parse(Console.ReadLine());

                        var ambienteRevogar = cadastro.PesquisarAmbiente(idAmbienteRevogar);
                        var usuarioRevogar = cadastro.PesquisarUsuario(idUsuarioRevogar);

                        if (ambienteRevogar != null && usuarioRevogar != null)
                        {
                            if (usuarioRevogar.RevogarPermissao(ambienteRevogar))
                                Console.WriteLine("Permissão revogada!");
                            else
                                Console.WriteLine("Permissão não existente.");
                        }
                        else
                        {
                            Console.WriteLine("Usuário ou ambiente não encontrado.");
                        }
                        break;

                    case 9:
                        Console.Write("ID do ambiente: ");
                        int ambId = int.Parse(Console.ReadLine());
                        Console.Write("ID do usuário: ");
                        int usrId = int.Parse(Console.ReadLine());

                        var amb = cadastro.PesquisarAmbiente(ambId);
                        var usr = cadastro.PesquisarUsuario(usrId);

                        if (amb != null && usr != null)
                        {
                            bool autorizado = usr.TemPermissao(amb);
                            amb.RegistrarLog(new Log(DateTime.Now, usr, autorizado));
                            Console.WriteLine($"Acesso {(autorizado ? "Autorizado" : "Negado")}");
                        }
                        else
                        {
                            Console.WriteLine("Usuário ou ambiente não encontrado.");
                        }
                        break;
                    case 10:
                        // Consultar logs
                        Console.Write("ID do ambiente: ");
                        int idAmbienteLogs = int.Parse(Console.ReadLine());
                        Console.WriteLine("1. Logs autorizados\n2. Logs negados\n3. Todos os logs");
                        Console.Write("Escolha a opção de filtragem: ");
                        int filtroLogs = int.Parse(Console.ReadLine());

                        var ambienteLogs = cadastro.PesquisarAmbiente(idAmbienteLogs);
                        if (ambienteLogs != null)
                        {
                            // Lista para armazenar os logs filtrados
                            List<Log> logsFiltrados = new List<Log>();

                            // Loop para filtrar os logs manualmente
                            foreach (var log in ambienteLogs.Logs)
                            {
                                if (filtroLogs == 1 && log.TipoAcesso)  // Logs autorizados
                                {
                                    logsFiltrados.Add(log);
                                }
                                else if (filtroLogs == 2 && !log.TipoAcesso)  // Logs negados
                                {
                                    logsFiltrados.Add(log);
                                }
                                else if (filtroLogs == 3)  // Todos os logs
                                {
                                    logsFiltrados.Add(log);
                                }
                            }

                            // Verificando se encontrou logs e exibindo-os
                            if (logsFiltrados.Count > 0)
                            {
                                foreach (var log in logsFiltrados)
                                {
                                    Console.WriteLine($"{log.DtAcesso} - {log.Usuario.Nome} - {(log.TipoAcesso ? "Autorizado" : "Negado")}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nenhum log encontrado para o filtro selecionado.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ambiente não encontrado.");
                        }
                        break;



                    case 0:
                        cadastro.Upload();
                        Console.WriteLine("Dados salvos. Saindo...");
                        break;

                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            } while (opcao != 0);
            cadastro.Upload(); // Salva os dados ao encerrar
            Console.WriteLine("Dados salvos. Saindo...");
        }
    }
}
    

