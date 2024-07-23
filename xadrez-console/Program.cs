using System.Diagnostics;
using tabuleiro.Entities;
using tabuleiro.Entities.Exceptions;
using tabuleiro.Enums;
using xadrez;
using xadrez.Entities;

namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PartidaXadrez partida = new PartidaXadrez();

                while (!partida.Terminada)
                {
                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.Tab);

                    Console.Write("\nOrigem: ");
                    Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                    Console.Write("\nDestino: ");
                    Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();

                    partida.ExecutaMovimento(origem, destino);
                }
            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}