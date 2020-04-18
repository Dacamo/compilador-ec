using System.Collections.Generic;

namespace Compilador.Clases
{
    public static class Entrada
    {
        private readonly static List<Linea> Lineas = new List<Linea>();
        public static string Tipo { get; set; }

        public static void AgregarLinea(string contenido)
        {
            if (contenido != null)
            {
                Linea linea = new Linea
                {
                    Contenido = contenido
                };

                if (Lineas.Count == 0)
                {

                    linea.Numero = 1;

                }
                else
                {
                    linea.Numero = Lineas.Count + 1;
                }

                Lineas.Add(linea);
            }
        }

        public static Linea ObtenerLinea(int numero) 
        {
            Linea lineaRetorno;

            if (ExisteLinea(numero))
            {
                lineaRetorno = Lineas[numero - 1];

            }
            else
            {
                lineaRetorno = new Linea();
                lineaRetorno.Contenido = "@EOF@";
                lineaRetorno.Numero = Lineas.Count + 1;
            }

            return lineaRetorno;
        }

        public static void LimpiarLineas()
        {
            Lineas.Clear();
        }

        public static bool ExisteLinea(int numero)
        {
            return Lineas.Count >= numero && numero > 0;
        }
    }
}
