using System.Text;

namespace Compilador.Transversal
{
    public class ComponenteLexico
    {
        public string Lexema { get; set; }
        public Categoria Categoria { get; set; }
        public int NumeroLinea { get; set; }
        public int PosicionInicial { get; set; }
        public int PosicionFinal { get; set; }
        public TipoComponente Tipo { get; set; }

        public override string ToString()
        {
            StringBuilder retorno = new StringBuilder();

            retorno.Append("Tipo componente: " + Tipo.ToString() + "\n");
            retorno.Append("Categoria: " + Categoria + "\n");
            retorno.Append("Lexema: " + Lexema + "\n");
            retorno.Append("Numero de Linea: " + NumeroLinea + "\n");
            retorno.Append("Posicion Inicial: " + PosicionInicial + "\n");
            retorno.Append("Poscicion Final: " + PosicionFinal + "\n");

            return retorno.ToString();
        }
    }
}
