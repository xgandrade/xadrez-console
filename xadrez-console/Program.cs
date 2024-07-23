using System.Diagnostics;
using tabuleiro.Entities;
using tabuleiro.Entities.Exceptions;
using tabuleiro.Enums;
using xadrez.Entities;

namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Tabuleiro tab = new Tabuleiro(8, 8);

                //tab.ColocarPeca(new Torre(tab, Cor.Preta), new Posicao(0, 0));
                //tab.ColocarPeca(new Torre(tab, Cor.Preta), new Posicao(1, 3));
                //tab.ColocarPeca(new Rei(tab, Cor.Preta), new Posicao(0, 9));

                //Tela.ImprimirTabuleiro(tab);

                PosicaoXadrez pos = new PosicaoXadrez('a', 1);

                Console.WriteLine(pos);
                Console.WriteLine(pos.ToPosicao());
            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}