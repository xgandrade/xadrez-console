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
        public Peao(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
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
                    break;
                default:
                    break;
            }

            return mat;
        }
    }
}
