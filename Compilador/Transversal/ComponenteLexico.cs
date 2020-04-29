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
        
        public static ComponenteLexico crear(string lexema, Categoria categoria, int numeroLinea, int posicionInicial, int posicionFinal)
        {
            ComponenteLexico retorno = new ComponenteLexico();
            retorno.Lexema = lexema;
            retorno.Categoria = categoria;
            retorno.NumeroLinea = numeroLinea;
            retorno.PosicionFinal = posicionFinal;
            retorno.PosicionInicial = posicionInicial;

            return retorno;
        }

        //para las palabras reservadas
        public static ComponenteLexico crear(string lexema, Categoria categoria)
        {
            ComponenteLexico retorno = new ComponenteLexico();
            retorno.Lexema = lexema;
            retorno.Categoria = categoria;
            retorno.NumeroLinea = 0;
            retorno.PosicionFinal = 0;
            retorno.PosicionInicial = 0;

            return retorno;
        }

        public override string ToString()
        {
            StringBuilder retorno = new StringBuilder();

            retorno.Append("Tipo componente: " + Tipo.ToString() + "\n");
            retorno.Append("Categoria: " + Categoria + "\n");
            retorno.Append("Lexema: " + Lexema + "\n");
            retorno.Append("Numero de Linea: " + NumeroLinea + "\n");
            retorno.Append("Posicion Inicial: " + PosicionInicial + "\n");
            retorno.Append("Posicion Final: " + PosicionFinal + "\n");

            return retorno.ToString();
        }
    }
}
