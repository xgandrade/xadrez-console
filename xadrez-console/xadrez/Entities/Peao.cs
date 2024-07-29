using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro.Entities;
using tabuleiro.Enums;

namespace xadrez.Entities
{
    class Peao : Peca
    {
        private PartidaXadrez Partida;
        public Peao(Tabuleiro tab, Cor cor, PartidaXadrez partida) : base(tab, cor)
        {
            this.Partida = partida;
        }

        public override string ToString() => "P";

        private bool PecaAdversaria(Posicao pos)
        {
            Peca p = Tabuleiro.Peca(pos);
            return p != null && p.Cor != this.Cor;
        }

        private bool Livre(Posicao pos) => Tabuleiro.Peca(pos) == null;

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];
            Posicao pos = new Posicao(0, 0);

            switch (Cor)
            {
                case Cor.Branca:
                    pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
                    if (Tabuleiro.PosicaoValida(pos) && Livre(pos))
                        mat[pos.Linha, pos.Coluna] = true;

                    pos.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
                    if (Tabuleiro.PosicaoValida(pos) && Livre(pos) && QtdMovimentos == 0)
                        mat[pos.Linha, pos.Coluna] = true;

                    pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
                    if (Tabuleiro.PosicaoValida(pos) && PecaAdversaria(pos))
                        mat[pos.Linha, pos.Coluna] = true;

                    pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
                    if (Tabuleiro.PosicaoValida(pos) && PecaAdversaria(pos))
                        mat[pos.Linha, pos.Coluna] = true;

                    //#Jogada Especial - En Passant
                    if (Posicao.Linha == 3)
                    {
                        Posicao esquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                        if (Tabuleiro.PosicaoValida(esquerda) && PecaAdversaria(esquerda) && Tabuleiro.Peca(esquerda) == Partida.VulneravelEnPassant)
                            mat[esquerda.Linha - 1, esquerda.Coluna] = true;

                        Posicao direita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                        if (Tabuleiro.PosicaoValida(direita) && PecaAdversaria(direita) && Tabuleiro.Peca(direita) == Partida.VulneravelEnPassant)
                            mat[direita.Linha - 1, direita.Coluna] = true;
                    }
                    break;
                case Cor.Preta:
                    pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
                    if (Tabuleiro.PosicaoValida(pos) && Livre(pos))
                        mat[pos.Linha, pos.Coluna] = true;

                    pos.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
                    if (Tabuleiro.PosicaoValida(pos) && Livre(pos) && QtdMovimentos == 0)
                        mat[pos.Linha, pos.Coluna] = true;

                    pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
                    if (Tabuleiro.PosicaoValida(pos) && PecaAdversaria(pos))
                        mat[pos.Linha, pos.Coluna] = true;

                    pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
                    if (Tabuleiro.PosicaoValida(pos) && PecaAdversaria(pos))
                        mat[pos.Linha, pos.Coluna] = true;

                    //#Jogada Especial - En Passant
                    if (Posicao.Linha == 4)
                    {
                        Posicao esquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                        if (Tabuleiro.PosicaoValida(esquerda) && PecaAdversaria(esquerda) && Tabuleiro.Peca(esquerda) == Partida.VulneravelEnPassant)
                            mat[esquerda.Linha + 1, esquerda.Coluna] = true;

                        Posicao direita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                        if (Tabuleiro.PosicaoValida(direita) && PecaAdversaria(direita) && Tabuleiro.Peca(direita) == Partida.VulneravelEnPassant)
                            mat[direita.Linha + 1, direita.Coluna] = true;
                    }
                    break;
                default:
                    break;
            }

            return mat;
        }
    }
}
