using Compilador.Transversal;

namespace Compilador.ManejadorErrores
{
    public class Error : ComponenteLexico
    {
        public string Falla { get; set; }
        public string Causa { get; set; }
        public string Solucion { get; set; }
        public TipoError Tipo { get; set; }

        private Error(string lexema, Categoria categoria, int numeroLinea, int posicionInicial, int posicionFinal, string falla, string causa, string solucion, TipoError tipoError)
        {
            Categoria = categoria;
            Lexema = lexema;
            PosicionFinal = posicionFinal;
            PosicionInicial = posicionFinal;
            NumeroLinea = numeroLinea;
            Falla = falla;
            Causa = causa;
            Solucion = solucion;
            Tipo = tipoError;
        }

        public static Error CrearErrorLexico(string lexema, Categoria categoria, int numeroLinea, int posicionInicial, int posicionFinal, string falla, string causa, string solucion)
        {
            return new Error(lexema, categoria, numeroLinea, posicionInicial, posicionFinal, falla, causa, solucion, TipoError.LEXICO);
        }

        public static Error CrearErrorSintatico(string lexema, Categoria categoria, int numeroLinea, int posicionInicial, int posicionFinal, string falla, string causa, string solucion)
        {
            return new Error(lexema, categoria, numeroLinea, posicionInicial, posicionFinal, falla, causa, solucion, TipoError.SINTATICO);
        }

        public static Error CrearErrorSemantico(string lexema, Categoria categoria, int numeroLinea, int posicionInicial, int posicionFinal, string falla, string causa, string solucion)
        {
            return new Error(lexema, categoria, numeroLinea, posicionInicial, posicionFinal, falla, causa, solucion, TipoError.SEMANTICO);
        }

    }
}
