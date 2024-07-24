using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro.Enums;

namespace tabuleiro.Entities
{
    abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QtdMovimentos { get; protected set; }
        public Tabuleiro Tabuleiro { get; protected set; }

        public Peca(Tabuleiro tabuleiro, Cor cor)
        {
            this.QtdMovimentos = 0;
            this.Posicao = null;
            this.Tabuleiro = tabuleiro;
            this.Cor = cor;
        }

        public void IncrementarQtdMovimentos() => QtdMovimentos++;

        public void DecrementarQtdMovimentos() => QtdMovimentos--;

        public abstract bool[,] MovimentosPossiveis();

        public bool PodeMoverPara(Posicao pos) => MovimentosPossiveis()[pos.Linha, pos.Coluna];

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();

            for (int l = 0; l < Tabuleiro.Linhas; l++)
                for (int c = 0; c < Tabuleiro.Colunas; c++)
                    if (mat[l, c]) return true;

            return false;
        }
    }
}
