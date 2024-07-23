using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro.Entities;
using tabuleiro.Enums;
using xadrez.Entities;

namespace xadrez_console
{
    class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int l = 0; l < tab.Linhas; l++)
            {
                Console.Write($"{8-l} ");
                for (int c = 0; c < tab.Colunas; c++)
                {
                    if (tab.Peca(l, c) == null) Console.Write("- ");
                    else ImprimirPeca(tab.Peca(l, c));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H");
        }

        public static void ImprimirPeca(Peca peca)
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

        public static PosicaoXadrez LerPosicaoXadrez()
        {
            string s = Console.ReadLine().ToLower();
            char coluna = s[0];
            int linha = int.Parse($"{s[1]}");

            return new PosicaoXadrez(coluna, linha);
        }
    }
}
