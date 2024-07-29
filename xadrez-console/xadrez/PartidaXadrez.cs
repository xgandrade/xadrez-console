using System.Runtime.ConstrainedExecution;
using tabuleiro.Entities;
using tabuleiro.Entities.Exceptions;
using tabuleiro.Enums;
using xadrez.Entities;

namespace xadrez
{
    class PartidaXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        public HashSet<Peca> Pecas;
        public HashSet<Peca> Capturadas;
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }

        public PartidaXadrez()
        {
            this.Tab = new Tabuleiro(8, 8);
            this.Turno = 1;
            this.JogadorAtual = Cor.Branca;
            this.Pecas = new();
            this.Capturadas = new();
            this.VulneravelEnPassant = null;
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);

            if (pecaCapturada != null) Capturadas.Add(pecaCapturada);

            // #JogadaEspecial - Roque
            bool roquePequeno = destino.Coluna == origem.Coluna + 2;
            bool roqueGrande = destino.Coluna == origem.Coluna - 2;

            if (p is Rei && (roquePequeno || roqueGrande))
            {
                Posicao origemTorre = roquePequeno ?
                    new Posicao(origem.Linha, origem.Coluna + 3) :
                    new Posicao(origem.Linha, origem.Coluna - 4);

                Posicao destinoTorre = roquePequeno ?
                    new Posicao(origem.Linha, origem.Coluna + 1) :
                    new Posicao(origem.Linha, origem.Coluna - 1);

                Peca T = Tab.RetirarPeca(destinoTorre);
                T.IncrementarQtdMovimentos();
                Tab.ColocarPeca(T, destinoTorre);
            }

            // #JogadaEspecial - En Passant
            if (p is Peao && origem.Coluna != destino.Coluna && pecaCapturada == null)
            {
                Posicao posicaoPeao;
                if (p.Cor == Cor.Branca)
                    posicaoPeao = new Posicao(destino.Linha + 1, destino.Coluna);
                else
                    posicaoPeao = new Posicao(destino.Linha - 1, destino.Coluna);

                pecaCapturada = Tab.RetirarPeca(posicaoPeao);
                Capturadas.Add(pecaCapturada);
            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQtdMovimentos();

            if (pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }

            Tab.ColocarPeca(p, origem);

            bool roquePequeno = destino.Coluna == origem.Coluna + 2;
            bool roqueGrande = destino.Coluna == origem.Coluna - 2;

            // #JogadaEspecial - Roque
            if (p is Rei && (roquePequeno || roqueGrande))
            {
                Posicao origemTorre = roquePequeno ? 
                    new Posicao(origem.Linha, origem.Coluna + 3) : 
                    new Posicao(origem.Linha, origem.Coluna - 4);

                Posicao destinoTorre = roquePequeno ?
                    new Posicao(origem.Linha, origem.Coluna + 1) :
                    new Posicao(origem.Linha, origem.Coluna - 1);

                Peca T = Tab.RetirarPeca(destinoTorre);
                T.DecrementarQtdMovimentos();
                Tab.ColocarPeca(T, origemTorre);
            }

            // #JogadaEspecial - En Passant
            if (p is Peao && origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
            {
                Peca peao = Tab.RetirarPeca(destino);
                Posicao posicaoPeao;
                if (p.Cor == Cor.Branca)
                    posicaoPeao = new Posicao(3, destino.Coluna);
                else
                    posicaoPeao = new Posicao(4, destino.Coluna);

                Tab.ColocarPeca(peao, posicaoPeao);
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            Peca p = Tab.Peca(destino);

            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em Xeque!");
            }

            // #Jogada Especial - Promoção
            if (p is Peao && (p.Cor == Cor.Branca && destino.Linha == 0 || p.Cor == Cor.Preta && destino.Linha == 7))
            {
                p = Tab.RetirarPeca(destino);
                Pecas.Remove(p);
                Peca dama = new Dama(Tab, p.Cor);
                Tab.ColocarPeca(dama, destino);
                Pecas.Add(dama);
            }

            Xeque = EstaEmXeque(Adversaria(JogadorAtual));

            if (XequeMate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }

            // #JogadaEspecial - En Passant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
                VulneravelEnPassant = p;
        }

        public void ValidaPosicaoDeOrigem(Posicao pos)
        {
            if (Tab.Peca(pos) == null) 
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");

            if (JogadorAtual != Tab.Peca(pos).Cor)
                throw new TabuleiroException("A peça de origem escolhida não é sua!");

            if (!Tab.Peca(pos).ExisteMovimentosPossiveis())
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");

        }

        public void ValidaPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.Peca(origem).MovimentoPossivel(destino))
                throw new TabuleiroException("Posição de destino invalida!");
        }

        private void MudaJogador() => JogadorAtual = JogadorAtual == Cor.Branca ? Cor.Preta : Cor.Branca;

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new();

            foreach (Peca p in Capturadas)
                if (p.Cor == cor) aux.Add(p);

            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new();

            foreach (Peca p in Pecas)
                if (p.Cor == cor) aux.Add(p);

            aux.ExceptWith(PecasCapturadas(cor));

            return aux;
        }

        private Cor Adversaria(Cor cor) => cor == Cor.Branca ? Cor.Preta : Cor.Branca;

        private Peca Rei(Cor cor)
        {
            foreach (Peca peca in PecasEmJogo(cor))
                if (peca is Rei) return peca;

            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca R = Rei(cor);
            if (R == null)
                throw new TabuleiroException($"Não tem Rei da cor {cor} no tabuleiro!");

            foreach (Peca peca in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = peca.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna]) return true;
            }

            return false;
        }

        public bool XequeMate(Cor cor)
        {
            if (!EstaEmXeque(cor)) 
                return false;

            foreach (Peca p in PecasEmJogo(cor))
            {
                bool[,] mat = p.MovimentosPossiveis();
                for (int l = 0; l < Tab.Linhas; l++)
                {
                    for (int c = 0; c < Tab.Colunas; c++)
                    {
                        if (mat[l, c])
                        {
                            Posicao origem = p.Posicao;
                            Posicao destino = new(l, c);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool xeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);

                            if (!xeque) 
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        /// <summary>
        /// Start inicial com todas as peças
        /// </summary>
        private void ColocarPecas()
        {
            // Peças brancas
            ColocarNovaPeca('a', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(Tab, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(Tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(Tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(Tab, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(Tab, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(Tab, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('a', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('b', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('c', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('d', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('e', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('f', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('g', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('h', 2, new Peao(Tab, Cor.Branca, this));

            // Peças Pretas
            ColocarNovaPeca('a', 8, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(Tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(Tab, Cor.Preta));
            ColocarNovaPeca('d', 8, new Dama(Tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(Tab, Cor.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(Tab, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(Tab, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('a', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('b', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('c', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('d', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('e', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('f', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('g', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('h', 7, new Peao(Tab, Cor.Preta, this));
        }
    }
}
