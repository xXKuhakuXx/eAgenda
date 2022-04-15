﻿using eAgenda.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.ModuloTarefa
{
    public class TelaCadastroTarefa : TelaBase
    {
        private RepositorioTarefa repositorioTarefa;
        private RepositorioItem repositorioItem;
        private Notificador notificador;

        public TelaCadastroTarefa(RepositorioTarefa repositorioTarefa, RepositorioItem repositorioItem, Notificador notificador)
            : base("Cadastro de Tarefa")
        {
            this.repositorioTarefa = repositorioTarefa;
            this.repositorioItem = repositorioItem;
            this.notificador = notificador;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para Adicionar novos itens");
            Console.WriteLine("Digite 6 para Alterar status dos itens de uma tarefa");
            Console.WriteLine("Digite 7 para Visualizar por prioridade");
            Console.WriteLine("Digite 8 para Visualizar finalizadas");
            Console.WriteLine("Digite 9 para Visualizar tarefas agrupadas");

            Console.WriteLine("Digite s para sair");

            string opcao = MetodosAuxiliares.ValidarInputString("Opção: ");

            return opcao;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Tarefa");

            Tarefa novaTarefa = ObterTarefa();

            repositorioTarefa.Inserir(novaTarefa);

            notificador.ApresentarMensagem("Tarefa cadastrada com sucesso!", TipoMensagem.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Tarefa");

            bool temTarefasCadastradas = VisualizarRegistros("Pesquisando");

            if (temTarefasCadastradas == false)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa cadastrada para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroTarefa = ObterNumeroRegistro();

            Tarefa tarefaAtualizada = ObterTarefa();
            Tarefa tarefaAntiga = repositorioTarefa.SelecionarRegistro(numeroTarefa);

            tarefaAtualizada.DataCriacao = tarefaAntiga.DataCriacao;

            bool conseguiuEditar = repositorioTarefa.Editar(numeroTarefa, tarefaAtualizada);

            if (!conseguiuEditar)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Tarefa editada com sucesso!", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Tarefa");

            bool temTarefasRegistradas = VisualizarRegistros("Pesquisando");

            if (temTarefasRegistradas == false)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa cadastrada para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroTarefa = ObterNumeroRegistro();

            if (TarefaPodeExcluir(numeroTarefa) == false)
            {
                notificador.ApresentarMensagem("Tarefas pendentes não podem ser excluidas.", TipoMensagem.Atencao);
                return;
            }

            bool conseguiuExcluir = repositorioTarefa.Excluir(numeroTarefa);

            if (!conseguiuExcluir)
                notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Tarefa excluída com sucesso", TipoMensagem.Sucesso);
        }

        public bool TarefaPodeExcluir(int numeroTarefa)
        {
            Tarefa tarefaSelecionada = repositorioTarefa.SelecionarRegistro(x => x.id == numeroTarefa);
            if(tarefaSelecionada.Status == true)
                return true;

            return false;
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Tarefas");

            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Tarefa tarefa in tarefas)
                Console.WriteLine(tarefa.ToString());

            Console.ReadLine();

            return true;
        }

        public bool VisualizarRegistrosSimplificados()
        {
            MostrarTitulo("Visualização de Tarefas");

            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Tarefa tarefa in tarefas)
            {
                Console.WriteLine("ID: " + tarefa.id);
                Console.WriteLine("Título: " + tarefa.Titulo);
                Console.WriteLine("Prioridade: " + tarefa.TipoRelevancia);
                Console.WriteLine("Data de Conclusão: " + tarefa.DataConclusao);
                Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
            }

            Console.ReadLine();

            return true;
        }

        public bool VisualizarTarefasAgrupadas()
        {
            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            MostrarTitulo("Tarefas agrupadas");

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            Console.WriteLine("TAREFAS PENDENTES");
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.Status == false)
                {
                    Console.WriteLine("ID: " + tarefa.id);
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Prioridade: " + tarefa.TipoRelevancia);
                    Console.WriteLine("Data de Conclusão: " + tarefa.DataConclusao);
                    Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
                }
            }
            Console.ResetColor();

            Console.WriteLine("\nTAREFAS FINALIZADAS");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.Status == true)
                {
                    Console.WriteLine("ID: " + tarefa.id);
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Prioridade: " + tarefa.TipoRelevancia);
                }
            }
            Console.ResetColor();

            Console.ReadLine();

            return true;
        }

        public bool VisualizarTarefasFinalizadas()
        {
            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            MostrarTitulo("Tarefas finalizadas");

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.Status == true)
                {
                    Console.WriteLine("ID: " + tarefa.id);
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Prioridade: " + tarefa.TipoRelevancia);
                    Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
                }
            }
            Console.ResetColor();

            Console.ReadLine();

            return true;
        }

        public bool VisualizarTarefasPorPrioridade()
        {
            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            MostrarTitulo("Tarefas por prioridade (somente pendentes)");

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            Console.WriteLine("PRIORIDADE ALTA");
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.TipoRelevancia == TipoRelevancia.Alta && tarefa.Status == false)
                {
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Data de Conclusão: " + tarefa.DataConclusao);
                    Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
                }
            }
            Console.ResetColor();

            Console.WriteLine("PRIORIDADE MÉDIA");
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.TipoRelevancia == TipoRelevancia.Media && tarefa.Status == false)
                {
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Data de Conclusão: " + tarefa.DataConclusao);
                    Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
                }
            }
            Console.ResetColor();

            Console.WriteLine("PRIORIDADE BAIXA");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.TipoRelevancia == TipoRelevancia.Baixa && tarefa.Status == false)
                {
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Data de Conclusão: " + tarefa.DataConclusao);
                    Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
                }
            }
            Console.ResetColor();

            Console.ReadLine();

            return true;
        }

        public void AlterarStatusItens()
        {
            bool temTarefasDisponiveis = VisualizarRegistrosSimplificados();

            if (!temTarefasDisponiveis)
            {
                notificador.ApresentarMensagem("Você precisa cadastrar uma tarefa antes de editar um item!", TipoMensagem.Atencao);
                return;
            }

            int numTarefaSelecionada = MetodosAuxiliares.ValidarInputInt("Digite o número da tarefa: ");

            Console.WriteLine();

            Tarefa tarefaSelecionada = repositorioTarefa.SelecionarRegistro(x => x.id == numTarefaSelecionada);

            List<Item> items = tarefaSelecionada.Itens;

            foreach (Item item in items)
            {
                Console.WriteLine("Deseja alterar o status deste item?");
                Console.WriteLine(item.ToString()); 
                Console.WriteLine("1 - Sim\n0 - Não");
                int op = MetodosAuxiliares.ValidarInputInt("Digite: ");
                if (op == 1)
                    item.AlterarStatus();
            }

            Console.ReadKey();
        }

        private Tarefa ObterTarefa()
        {
            string titulo = MetodosAuxiliares.ValidarInputString("Digite o título da tarefa: ");

            DateTime dataConclusao = MetodosAuxiliares.ValidarInputDate("Digite a data de conclusão: ");

            Console.Write("Escolha a relevância\n");
            TipoRelevancia tipoRelevancia = ObterTipoRelevancia();

            Console.WriteLine("Cadastrando Items: ");
            List<Item> items = ObterItens();

            Tarefa novaTarefa = new Tarefa(titulo, 
                dataConclusao,
                tipoRelevancia,
                items
                );

            return novaTarefa;
        }

        private TipoRelevancia ObterTipoRelevancia()
        {
            Console.WriteLine("Escolha um grau de relevancia:\n\t0- Alta \n\t1- Média \n\t2- Baixa");

            int relevancia;
            while (true)
            {
                relevancia = MetodosAuxiliares.ValidarInputInt("Digite a opção: ");
                if (relevancia != 0 || relevancia != 1 || relevancia != 2)
                    break;
                else
                    continue;
            }
            TipoRelevancia tipoRelevancia = (TipoRelevancia)relevancia;

            return tipoRelevancia;
        }

        public void AdicionarNovosItens()
        {
            MostrarTitulo("Inserindo novos itens");

            bool temTarefasCadastradas = VisualizarRegistros("Pesquisando");

            if (temTarefasCadastradas == false)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa cadastrada para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroTarefa = ObterNumeroRegistro();
            Tarefa tarefaSelecionada = repositorioTarefa.SelecionarRegistro(numeroTarefa);

            while (true)
            {
                string descricao = MetodosAuxiliares.ValidarInputString("Digite a descrição: ");

                Item novoItem = new Item(descricao);
                tarefaSelecionada.Itens.Add(novoItem);

                Console.WriteLine("Item cadastrado com sucesso, deseja criar mais itens?");
                int op = MetodosAuxiliares.ValidarInputInt("1 - Sim\n0 - Não\n");
                if (op == 1)
                    continue;
                else
                    break;
            }
        }

        private List<Item> ObterItens()
        {
            RepositorioItem repositorioItem = new RepositorioItem();

            while (true)
            {
                string descricao = MetodosAuxiliares.ValidarInputString("Digite a descrição: ");

                Item novoItem = new Item(descricao);
                repositorioItem.Inserir(novoItem);

                Console.WriteLine("Item cadastrado com sucesso, deseja criar mais itens?");
                int op = MetodosAuxiliares.ValidarInputInt("1 - Sim\n0 - Não\n");
                if (op == 1)
                    continue;
                else
                    break;
            }

            return repositorioItem.SelecionarTodos();
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                numeroRegistro = MetodosAuxiliares.ValidarInputInt("Digite o ID da tarefa que deseja editar: ");

                numeroRegistroEncontrado = repositorioTarefa.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    notificador.ApresentarMensagem("ID da tarefa não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }

    }
}
