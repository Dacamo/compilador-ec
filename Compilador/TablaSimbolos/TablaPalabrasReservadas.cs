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
        
        public static void inicializar()
        {
            tabla.Add("ON", ComponenteLexico.crear("ON", Categoria.PALABRA_RESERVADA_ON));
            tabla.Add("OFF", ComponenteLexico.crear("OFF", Categoria.PALABRA_RESERVADA_OFF));
            tabla.Add("SHUTDOWN", ComponenteLexico.crear("SHUTDOWN", Categoria.PALABRA_RESERVADA_SHUTDOWN));
            tabla.Add("IN", ComponenteLexico.crear("IN", Categoria.PALABRA_RESERVADA_IN));
            tabla.Add("R", ComponenteLexico.crear("RANKINE", Categoria.PALABRA_RESERVADA_RANKINE));
            tabla.Add("F", ComponenteLexico.crear("FARENHEIT", Categoria.PALABRA_RESERVADA_FARENHEIT));
            tabla.Add("FAIL", ComponenteLexico.crear("FAIL", Categoria.PALABRA_RESERVADA_FAIL));
            tabla.Add("RESTART", ComponenteLexico.crear("RESTART", Categoria.PALABRA_RESERVADA_RESTART));
            tabla.Add("SUCCESS", ComponenteLexico.crear("SUCCESS", Categoria.PALABRA_RESERVADA_SUCCESS));
            tabla.Add("STATUS", ComponenteLexico.crear("STATUS", Categoria.PALABRA_RESERVADA_STATUS));
            tabla.Add("START", ComponenteLexico.crear("START", Categoria.PALABRA_RESERVADA_START));
            tabla.Add("GET", ComponenteLexico.crear("GET", Categoria.PALABRA_RESERVADA_GET));
            tabla.Add("DOWN", ComponenteLexico.crear("DOWN", Categoria.PALABRA_RESERVADA_DOWN));
            tabla.Add("UP", ComponenteLexico.crear("UP", Categoria.PALABRA_RESERVADA_UP));
            tabla.Add("OUT", ComponenteLexico.crear("OUT", Categoria.PALABRA_RESERVADA_OUT));
            tabla.Add("C", ComponenteLexico.crear("CENTIGRADOS", Categoria.PALABRA_RESERVADA_CENTIGRADOS));
            tabla.Add("K", ComponenteLexico.crear("KELVIN", Categoria.PALABRA_RESERVADA_KELVIN));

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
            if (componente != null && !componente.Lexema.Equals("") && componente.Tipo.Equals(TipoComponente.PALABRA_RESERVADA))
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
