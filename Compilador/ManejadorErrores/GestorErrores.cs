using System.Collections.Generic;
using System.Linq;

namespace Compilador.ManejadorErrores
{
    public static class GestorErrores
    {
        private static Dictionary<TipoError, List<Error>> errores = new Dictionary<TipoError, List<Error>>();

        public static List<Error> ObtenerErrores(TipoError tipoError)
        {
            if (!errores.ContainsKey(tipoError))
            {
                errores.Add(tipoError, new List<Error>());
            }

            return errores[tipoError];
        }

        public static void Reportar(Error error)
        {
            if (error != null)
            {
                ObtenerErrores(error.Tipo).Add(error);
            }
        }

        public static bool HayErroresLexicos()
        {
            return ObtenerErrores(TipoError.LEXICO).Count > 0;
        }

        public static bool HayErroresSintaticos()
        {
            return ObtenerErrores(TipoError.SINTATICO).Count > 0;
        }

        public static bool HayErroresSemanticos()
        {
            return ObtenerErrores(TipoError.SEMANTICO).Count > 0;
        }

        public static List<Error> ObtenerTodosErrores()
        {
            return errores.Values.SelectMany(error => error).ToList();
        }

        public static bool HayErrores()
        {
            return ObtenerTodosErrores().Count > 0;
        }
    }
}
