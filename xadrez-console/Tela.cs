using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro.Entities;
using tabuleiro.Enums;
using xadrez;
using xadrez.Entities;

namespace xadrez_console
{
    class Tela
    {
        public static void ImprimirPartida(PartidaXadrez partida)
        {
            ImprimirTabuleiro(partida.Tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine($"\nTurno: {partida.Turno}");

            if (!partida.Terminada)
            {
                Console.WriteLine($"Aguardando jogada: {partida.JogadorAtual}");

                if (partida.Xeque)
                    Console.WriteLine("XEQUE!!!");
            }
            else
            {
                Console.WriteLine($"XEQUE-MATE!!! \nVencedor: {partida.JogadorAtual}");
            }
        }

        public static void ImprimirPecasCapturadas(PartidaXadrez partida)
        {
            Console.WriteLine("Peças capturadas:");
            Console.Write("Brancas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Branca));
            Console.Write("\nPretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            ImprimirConjunto(partida.PecasCapturadas(Cor.Preta));
            Console.ForegroundColor = aux;
        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto)
        {
            Console.Write("[");
            conjunto.ToList().ForEach(x => Console.Write($"{x} "));
            Console.Write("]");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoMarcacoes = ConsoleColor.Blue;

            for (int l = 0; l < tab.Linhas; l++)
            {
                Console.BackgroundColor = fundoMarcacoes;
                Console.Write($"{8-l} ");
                Console.BackgroundColor = fundoOriginal;

                for (int c = 0; c < tab.Colunas; c++)
                {
                    ImprimirPeca(tab.Peca(l, c));
                }
                Console.WriteLine();
            }

            Console.BackgroundColor = fundoMarcacoes;
            Console.WriteLine("  A B C D E F G H");
            Console.BackgroundColor = fundoOriginal;
        }

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkCyan;
            ConsoleColor fundoMarcacoes = ConsoleColor.Blue;

            for (int l = 0; l < tab.Linhas; l++)
            {
                Console.BackgroundColor = fundoMarcacoes;
                Console.Write($"{8 - l} ");
                Console.BackgroundColor = fundoOriginal;

                for (int c = 0; c < tab.Colunas; c++)
                {
                    if (posicoesPossiveis[l, c]) Console.BackgroundColor = fundoAlterado;
                    else Console.BackgroundColor = fundoOriginal;

                    ImprimirPeca(tab.Peca(l, c));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = fundoMarcacoes;
            Console.WriteLine("  A B C D E F G H");
            Console.BackgroundColor = fundoOriginal;
        }

        public static void ImprimirPeca(Peca peca)
        {
            if (peca == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (peca.Cor == Cor.Branca)
                {
                    Console.Write($"{peca} ");
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{peca} ");
                    Console.ForegroundColor = aux;
                }
            }
        }

        public static PosicaoXadrez LerPosicaoXadrez()
        {
            string s = Console.ReadLine().ToLower();
            char coluna = s[0];
            int linha = int.Parse($"{s[1]}");

            return new PosicaoXadrez(coluna, linha);
        }
    }
}
