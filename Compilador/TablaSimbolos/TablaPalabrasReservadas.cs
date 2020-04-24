using Compilador.Transversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.TablaSimbolos
{
    public class TablaPalabrasReservadas
    {
        private static Dictionary<string, ComponenteLexico> tabla = new Dictionary<string, ComponenteLexico>();
        private static Dictionary<string, List<ComponenteLexico>> simbolos = new Dictionary<string, List<ComponenteLexico>>();
        private static void inicializar()
        {
            tabla.Add("ON", ComponenteLexico.crear("ON", Categoria.PALABRA_RESERVADA_ON));
            tabla.Add("OFF", ComponenteLexico.crear("OFF", Categoria.PALABRA_RESERVADA_OFF));
            tabla.Add("SHUTDOWN", ComponenteLexico.crear("SHUTODWN", Categoria.PALABRA_RESERVADA_SHUTDOWN));
            //agregar 
        }

        public static void ValidarSiEsPalabraReservada(ComponenteLexico componente)
        {
            if(componente != null && tabla.ContainsKey(componente.Lexema))
            {
                componente.Tipo = TipoComponente.PALABRA_RESERVADA;
                componente.Categoria = tabla[componente.Lexema].Categoria;
            }

        }

        private static List<ComponenteLexico> ObtenerSimbolos(string clave)
        {
            if (!simbolos.ContainsKey(clave))
            {
                simbolos.Add(clave, new List<ComponenteLexico>());
            }

            return simbolos[clave];

        }

        public static void Agregar(ComponenteLexico componente)
        {
            if (componente != null && !componente.Lexema.Equals("") && componente.Tipo.Equals(TipoComponente.SIMBOLO))
            {
                ObtenerSimbolos(componente.Lexema).Add(componente);
            }
        }

        public static List<ComponenteLexico> ObtenerTodosLosSimbolos()
        {
            return simbolos.Values.SelectMany(componente => componente).ToList();
        }

        public static void Limpiar()
        {
            simbolos.Clear();
        }
    }
}
