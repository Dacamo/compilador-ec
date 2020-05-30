using Compilador.ManejadorErrores;
using Compilador.Transversal;
using System;
using System.Windows.Forms;

namespace Compilador
{
    public class AnalizadorSintactico
    {
       
        private AnalizadorLexico anaLex = new AnalizadorLexico();
        private ComponenteLexico componente = null;
        private string traza = "";
        private bool mostrarTraza = false;
        
  
        public void Analizar()
        {
            traza = "";
            LeerSiguienteComponente();
            Sensor("--");

            if (GestorErrores.HayErrores())
            {
                MessageBox.Show("El programa contiene errores. Por favor verifique la consola respectiva...");
            }
            else if (Categoria.EOF.Equals(componente.Categoria))
            {              
                MessageBox.Show("El programa se encuentra bien escrito...");               
            }
            else
            {
                Error error = Error.CrearErrorSintatico(
                    componente.Lexema,
                    componente.Categoria,
                    componente.NumeroLinea,
                    componente.PosicionInicial,
                    componente.PosicionFinal,
                    "",
                    "Estructura inválida",
                    "Revisar las instrucciones o reglas gramaticales"
                   );
                GestorErrores.Reportar(error);

                MessageBox.Show("Esctructura inválida");
            }
            
            MessageBox.Show("Ruta de evalucion de la gramatica: \n" + traza);
        }

        //<SENSOR>:=  IN<SEPARADOR><INSTRUCCIONES>|OUT<SEPARADOR><DATOS>
        public void Sensor(string posicion)
        {
            posicion = posicion + "----";
            FormarTrazaEntrada(posicion, "SENSOR");
            //IN<SEPARADOR><INSTRUCCIONES>
            if (Categoria.PALABRA_RESERVADA_IN.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();  
                Separador(posicion);
                Instrucciones(posicion);
            }
            //OUT<SEPARADOR><DATOS>
            else if (Categoria.PALABRA_RESERVADA_OUT.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
                Separador(posicion);
                Datos(posicion);
            }
            else
            {
                //Reportar un Error sintáctico.          
                Error error = Error.CrearErrorSintatico(
                    componente.Lexema,
                    componente.Categoria,
                    componente.NumeroLinea,
                    componente.PosicionInicial,
                    componente.PosicionFinal,
                    "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + " y esperaba un IN o un OUT",
                    "asegurese que el caracter que se encuentra en la posicion actual sea IN o OUT");
                GestorErrores.Reportar(error);    
            }

            FormarTrazaSalida(posicion, "SENSOR");
        }

        //<DATOS>:= VALOR_RETORNO<SEPARADOR><UNIDADMEDIDA>|<RESPUESTAS><SEPARADOR>ON|OFF
        public void Datos(string posicion)
        {
            posicion = posicion + "----";
            FormarTrazaEntrada(posicion, "DATOS");
            //VALOR_RETORNO<SEPARADOR><UNIDADMEDIDA>
            if (Categoria.VALOR_RETORNO.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
                Separador(posicion);
                UnidadMedida(posicion);               
            }
            //OFF
            else if (Categoria.PALABRA_RESERVADA_OFF.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            //<RESPUESTAS><SEPARADOR>ON
            else if(Categoria.PALABRA_RESERVADA_SUCCESS.Equals(componente.Categoria)|| Categoria.PALABRA_RESERVADA_FAIL.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
                Separador(posicion);
                if (Categoria.PALABRA_RESERVADA_ON.Equals(componente.Categoria))
                {
                    LeerSiguienteComponente();
                }
                else
                {
                    Error error = Error.CrearErrorSintatico(
                        componente.Lexema,
                        componente.Categoria,
                        componente.NumeroLinea,
                        componente.PosicionInicial,
                        componente.PosicionFinal,
                        "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + " y esperaba un ON",
                        "asegurese que el caracter que se encuentra en la posicion actual sea ON");
                    GestorErrores.Reportar(error);
                }
            }
            else
            {
                Error error = Error.CrearErrorSintatico(
                        componente.Lexema,
                        componente.Categoria,
                        componente.NumeroLinea,
                        componente.PosicionInicial,
                        componente.PosicionFinal,
                        "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + " y esperaba un VALOR_RETORNO",
                        "asegurese que el caracter que se encuentra en la posicion actual sea VALOR_RETORNO");
                GestorErrores.Reportar(error);
            }

            FormarTrazaSalida(posicion, "DATOS");            
        }

        //<INSTRUCCIONES>:= VALOR_ENVIO<SEPARADOR><UNIDADMEDIDA><SEPARADOR><CAMBIARTEMPERATURA>|<UNIDADMEDIDA><SEPARADOR>GET|<SOLICITUDES>
        public void Instrucciones(string posicion)
        {
            posicion = posicion + "----";
            FormarTrazaEntrada(posicion, "INSTRUCCIONES");
            //VALOR_ENVIO<SEPARADOR><UNIDADMEDIDA><SEPARADOR><CAMBIARTEMPERATURA>
            if (Categoria.VALOR_ENVIO.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
                Separador(posicion);    
                UnidadMedida(posicion);
                Separador(posicion);
                CambiarTemperatura(posicion);
   
            }
            //<UNIDADMEDIDA><SEPARADOR>GET
            else if (Categoria.PALABRA_RESERVADA_KELVIN.Equals(componente.Categoria)|| Categoria.PALABRA_RESERVADA_CENTIGRADOS.Equals(componente.Categoria) || Categoria.PALABRA_RESERVADA_FARENHEIT.Equals(componente.Categoria)|| Categoria.PALABRA_RESERVADA_RANKINE.Equals(componente.Categoria) )
            {
                LeerSiguienteComponente();
                Separador(posicion);
                  
                if (Categoria.PALABRA_RESERVADA_GET.Equals(componente.Categoria))
                {
                    LeerSiguienteComponente();
                }
                else
                {
                    //Reportar Error sintáctico
                    Error error = Error.CrearErrorSintatico(
                        componente.Lexema,
                        componente.Categoria,
                        componente.NumeroLinea,
                        componente.PosicionInicial,
                        componente.PosicionFinal,
                        "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + " y esperaba un GET",
                        "asegurese que el caracter que se encuentra en la posicion actual sea GET");
                    GestorErrores.Reportar(error);
                }
                                  
            }
            //<SOLICITUDES>
            else if (Categoria.PALABRA_RESERVADA_START.Equals(componente.Categoria) || Categoria.PALABRA_RESERVADA_SHUTDOWN.Equals(componente.Categoria) || Categoria.PALABRA_RESERVADA_RESTART.Equals(componente.Categoria) || Categoria.PALABRA_RESERVADA_STATUS.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else
            {
                //Reportar Error sintáctico
                Error error = Error.CrearErrorSintatico(
                    componente.Lexema,
                    componente.Categoria,
                    componente.NumeroLinea,
                    componente.PosicionInicial,
                    componente.PosicionFinal,
                    "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + " y esperaba un VALOR_ENVIO, <UNIDADMEDIDA> ó <SOLICITUDES>",
                    "asegurese que el caracter que se encuentra en la posicion actual sea VALOR_ENVIO, <UNIDADMEDIDA> ó <SOLICITUDES>");
                GestorErrores.Reportar(error);
            }

            FormarTrazaSalida(posicion, "INSTRUCCIONES");
            
        }

        //<UNIDADMEDIDA>:= C|F|K|R
        public void UnidadMedida(string posicion) 
        {
            posicion = posicion + "----";
            FormarTrazaEntrada(posicion, "UNIDADMEDIDA");

            if (Categoria.PALABRA_RESERVADA_CENTIGRADOS.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else if (Categoria.PALABRA_RESERVADA_FARENHEIT.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else if (Categoria.PALABRA_RESERVADA_KELVIN.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else if (Categoria.PALABRA_RESERVADA_RANKINE.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else
            {
                //Reportar Error sintáctico
                Error error = Error.CrearErrorSintatico(
                       componente.Lexema,
                       componente.Categoria,
                       componente.NumeroLinea,
                       componente.PosicionInicial,
                       componente.PosicionFinal,
                       "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + " y esperaba una temperatura C, F, K ó R",
                       "asegurese que el caracter que se encuentra en la posicion actual sea C, F, K, R");
                GestorErrores.Reportar(error);

            }

            FormarTrazaSalida(posicion, "UNIDADMEDIDA");
        }

 
       
        //<CAMBIARTEMPERATURA>:= UP|DOWN
        public void CambiarTemperatura(string posicion)
        {
            posicion = posicion + "----";
            FormarTrazaEntrada(posicion, "CAMBIARTEMPERATURA");
            if (Categoria.PALABRA_RESERVADA_UP.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();

            }
            else if (Categoria.PALABRA_RESERVADA_DOWN.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else
            {
                //Reportar Error sintáctico
                Error error = Error.CrearErrorSintatico(
                       componente.Lexema,
                       componente.Categoria,
                       componente.NumeroLinea,
                       componente.PosicionInicial,
                       componente.PosicionFinal,
                       "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + " y esperaba un UP o DOWN",
                       "asegurese que el caracter que se encuentra en la posicion actual sea UP o DOWN");
                GestorErrores.Reportar(error);
            }

            FormarTrazaSalida(posicion, "CAMBIARTEMPERATURA");
        }


        //<SEPARADOR>:= #
        public void Separador(string posicion)
        {
            posicion = posicion + "----";
            FormarTrazaEntrada(posicion, "SEPARADOR");
            if (Categoria.SEPARADOR.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else
            {
                //Reportar Error
                Error error = Error.CrearErrorSintatico(
                   componente.Lexema,
                   componente.Categoria,
                   componente.NumeroLinea,
                   componente.PosicionInicial,
                   componente.PosicionFinal,
                   "Componente no válido en la ubicación actual", "Leí: " + componente.Categoria + ":" + componente.Lexema + " y esperaba un # SEPARADOR",
                   "asegurese que el caracter que se encuentra en la posicion actual sea #");
                GestorErrores.Reportar(error);

            }
            FormarTrazaSalida(posicion, "SEPARADOR");
        }

        private void FormarTrazaEntrada(string posicion, string nombreRegla)
        {
            traza = traza + posicion + "Entrada Regla:" + nombreRegla + "Categoria:" + componente.Categoria + ", lexema: " + componente.Lexema + "\n";
            ImprimirTraza();
        }

        private void FormarTrazaSalida(string posicion, string nombreRegla)
        {
            traza = traza + posicion + "Salida Regla:" + nombreRegla + "\n";
            ImprimirTraza();
        }

        private void ImprimirTraza()
        {
            if (mostrarTraza)
            {
                MessageBox.Show(traza);
            }
        }

        private void LeerSiguienteComponente()
        {
            componente = anaLex.Analizar();
        }
    }
}
