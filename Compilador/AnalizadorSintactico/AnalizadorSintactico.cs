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
        
  
        public void Analizar()
        {
            LeerSiguienteComponente();
            Sensor();

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

                MessageBox.Show("NI POR EL HPTA");
            }
        }

        //<SENSOR>:=  IN<SEPARADOR><INSTRUCCIONES>|OUT<SEPARADOR><DATOS>
        public void Sensor()
        {
            if (Categoria.PALABRA_RESERVADA_IN.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();  
                Separador();
                Instrucciones();

            }
            else if (Categoria.PALABRA_RESERVADA_OUT.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
                Separador();
                Datos();

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
                    "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + "y esperaba un IN o un OUT",
                    "asegurese que el caracter que se encuentra en la posicion actual sea IN o OUT");
                GestorErrores.Reportar(error);

                
            }
        }

        //<DATOS>:= VALOR_RETORNO<SEPARADOR><UNIDADMEDIDA>|<RESPUESTAS><SEPARADOR>ON|OFF
        public void Datos()
        {
            if (Categoria.VALOR_RETORNO.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
                Separador();
                UnidadMedida();
                
            }
            
            else if (Categoria.PALABRA_RESERVADA_OFF.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            
            else if(Categoria.PALABRA_RESERVADA_SUCCESS.Equals(componente.Categoria)|| Categoria.PALABRA_RESERVADA_FAIL.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
                Separador();
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
                        "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + "y esperaba un ON",
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
                        "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + "y esperaba un VALOR_RETORNO",
                        "asegurese que el caracter que se encuentra en la posicion actual sea VALOR_RETORNO");
                GestorErrores.Reportar(error);
            }
            
        }

        //<INSTRUCCIONES>:= VALOR_ENVIO<SEPARADOR><UNIDADMEDIDA><SEPARADOR><CAMBIARTEMPERATURA>|<UNIDADMEDIDA><SEPARADOR>GET|<SOLICITUDES>

        public void Instrucciones()
        {
            if (Categoria.VALOR_ENVIO.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
                Separador();    
                UnidadMedida();
                Separador();
                CambiarTemperatura();
   
            }
            else if (Categoria.PALABRA_RESERVADA_KELVIN.Equals(componente.Categoria)|| Categoria.PALABRA_RESERVADA_CENTIGRADOS.Equals(componente.Categoria) || Categoria.PALABRA_RESERVADA_FARENHEIT.Equals(componente.Categoria)|| Categoria.PALABRA_RESERVADA_RANKINE.Equals(componente.Categoria) )
            {
                LeerSiguienteComponente();
                Separador();
                  
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
                        "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + "y esperaba un GET",
                        "asegurese que el caracter que se encuentra en la posicion actual sea GET");
                    GestorErrores.Reportar(error);
                }
                                  
            }
            else
            {
                Solicitudes();
            } 
            
        }
        //<UNIDADMEDIDA>:= C|F|K|R
        public void UnidadMedida() 
        {
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
                       "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + "y esperaba una temperatura C, F, K ó R",
                       "asegurese que el caracter que se encuentra en la posicion actual sea C, F, K, R");
                GestorErrores.Reportar(error);

            }
        }



        //<RESPUESTAS>:= SUCCESS|FAIL
        public void Respuestas()
        {
            if (Categoria.PALABRA_RESERVADA_SUCCESS.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else if (Categoria.PALABRA_RESERVADA_FAIL.Equals(componente.Categoria))
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
                       "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + "y esperaba un SUCCESS o FAIL",
                       "asegurese que el caracter que se encuentra en la posicion actual sea SUCCESS o FAIL");
                GestorErrores.Reportar(error);

            }
        }
        //<SOLICITUDES>:= START|STATUS|SHUTDOWN|RESTART
        public void Solicitudes()
        {
            if (Categoria.PALABRA_RESERVADA_START.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else if (Categoria.PALABRA_RESERVADA_STATUS.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else if (Categoria.PALABRA_RESERVADA_SHUTDOWN.Equals(componente.Categoria))
            {
                LeerSiguienteComponente();
            }
            else if (Categoria.PALABRA_RESERVADA_RESTART.Equals(componente.Categoria))
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
                       "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + "y esperaba un START, SHUTDOWN; RESTART, STATUS",
                       "asegurese que el caracter que se encuentra en la posicion actual sea START, SHUTDOWN; RESTART, STATUS");
                GestorErrores.Reportar(error);

            }


        }
        //<CAMBIARTEMPERATURA>:= UP | DOWN
        public void CambiarTemperatura()
        {
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
                       "Componente no válido en la ubicación actual", "Leí " + componente.Categoria + ":" + componente.Lexema + "y esperaba un UP o DOWN",
                       "asegurese que el caracter que se encuentra en la posicion actual sea UP o DOWN");
                GestorErrores.Reportar(error);

            }
        }


        //<SEPARADOR>:= #
        public void Separador()
        {
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
                   "Componente no válido en la ubicación actual", "Leí: " + componente.Categoria + ":" + componente.Lexema + "y esperaba un # SEPARADOR",
                   "asegurese que el caracter que se encuentra en la posicion actual sea #");
                GestorErrores.Reportar(error);

            }
        }
        private void LeerSiguienteComponente()
        {
            componente = anaLex.Analizar();
        }
    }
}
