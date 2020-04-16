using Compilador.Clases;
using Compilador.ManejadorErrores;
using Compilador.TablaSimbolos;
using Compilador.Transversal;
using System;
using System.Windows.Forms;

namespace Compilador
{
    public class AnalizadorLexico
    {
        private int NumeroLineaActual;
        private int Puntero;
        private string CaracterActual;
        private Linea lineaActual;
        string lexema;


        public AnalizadorLexico()
        {
            CargarNuevaLinea();
        }

        private void CargarNuevaLinea()
        {
            NumeroLineaActual++;
            lineaActual = Entrada.ObtenerLinea(NumeroLineaActual);
            if (lineaActual.Contenido.Equals("@EOF@"))
            {
                NumeroLineaActual = lineaActual.Numero;
            }

            Puntero = 1;
        }

        private void DevolverPuntero()
        {
            Puntero -= 1;
        }

        public void LeerSiguienteCaracter()
        {
            if (lineaActual.Contenido.Equals("@EOF@"))
            {
                CaracterActual = lineaActual.Contenido;
            }
            else if (Puntero > lineaActual.Contenido.Length)
            {
                CaracterActual = "@FL@";
                Puntero++;
            }
            else
            {
                CaracterActual = lineaActual.Contenido.Substring(Puntero -1, 1);
                Puntero++;
            }
        }

        private void concatenarLexema()
        {
            lexema = lexema + CaracterActual;
        }

        private void limpiarLexema()
        {
            lexema = "";
        }

        private void DevorarEspaciosBlanco()
        {
            while (CaracterActual.Equals(" "))
            {
                LeerSiguienteCaracter();
            }
        }

        public bool EsLetra(string simbolo)
        {

            return Char.IsLetter(simbolo, 0);
        }

        public bool EsDigito(string simbolo)
        {

            return Char.IsDigit(simbolo, 0);
        }

        public bool EsLetraODigito(string simbolo)
        {

            return EsLetra(simbolo) || EsDigito(simbolo);
        }

        public ComponenteLexico Analizar()
        {
            ComponenteLexico componenteLexico = new ComponenteLexico();
            limpiarLexema();
            int estadoActual = 0;
            bool continuarAnalisis = true;

            while (continuarAnalisis)
            {
               
                if (estadoActual == 0)
                {
                    LeerSiguienteCaracter();
                    DevorarEspaciosBlanco();
                    if (CaracterActual == "0" || CaracterActual == "1")
                    {
                        estadoActual = 1;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "#")
                    {
                        estadoActual = 6;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "K")
                    {
                        estadoActual = 60;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "C")
                    {
                        estadoActual = 58;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "I")
                    {
                        estadoActual = 7;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "O")
                    {
                        estadoActual = 9;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "U")
                    {
                        estadoActual = 15;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "D")
                    {
                        estadoActual = 17;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "G")
                    {
                        estadoActual = 21;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "S")
                    {
                        estadoActual = 24;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "R")
                    {
                        estadoActual = 46;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "F")
                    {
                        estadoActual = 54;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "@FL@")
                    {
                        estadoActual = 61;
                    }
                    else if (CaracterActual == "@EOF@")
                    {
                        estadoActual = 62;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "Otro caracter");
                    }

                }
                else if (estadoActual == 1)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "0" || CaracterActual == "1")
                    {
                        estadoActual = 2;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "1 ó 0");
                    }
                }

                else if (estadoActual == 2)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "0" || CaracterActual == "1")
                    {
                        estadoActual = 3;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "1 ó 0");
                    }
                }
                else if (estadoActual == 3)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "0" || CaracterActual == "1")
                    {
                        estadoActual = 4;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 5;
                    }
                }
                else if (estadoActual == 4)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.VALOR_RETORNO;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 5)
                {
                    DevolverPuntero();
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.VALOR_ENVIO;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 6)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.SEPARADOR;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if(estadoActual == 7)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual =="N")
                    {
                        estadoActual = 8;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "N");
                    }
                }
                else if(estadoActual == 8)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.IN;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if(estadoActual == 9)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual == "U")
                    {
                        estadoActual = 10;
                        concatenarLexema();
                    }
                    else if(CaracterActual =="N")
                    {
                        estadoActual = 12;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "F")
                    {
                        estadoActual = 13;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "F");
                    }
                }
                else if(estadoActual == 10)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "T")
                    {
                        estadoActual = 11;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "T");
                    }
                }
                else if(estadoActual == 11)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.OUT;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if(estadoActual == 12)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.ON;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if(estadoActual == 13)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual== "F")
                    {
                        estadoActual = 14;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "F");
                    }
                }
                else if(estadoActual== 14)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.OFF;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if(estadoActual == 15)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual =="P")
                    {
                        estadoActual = 16;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "P");
                    }
                }
                else if(estadoActual == 16)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.UP;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if(estadoActual == 17)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "O")
                    {
                        estadoActual = 18;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "O");
                      
                    }
                }
                else if (estadoActual == 18)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "W")
                    {
                        estadoActual = 19;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "W");
                    }
                }
                else if (estadoActual == 19)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "N")
                    {
                        estadoActual = 20;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "N");
                    }
                }
                else if (estadoActual == 20)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.DOWN;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if( estadoActual == 21)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual == "E")
                    {
                        estadoActual = 22;
                        concatenarLexema();
                    }
                    else 
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "E");
                    }
                }
                else if( estadoActual== 22)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual =="T")
                    {
                        estadoActual = 23;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "T");
                    }
                }
                else if (estadoActual == 23)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.GET;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if(estadoActual == 24)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual =="T")
                    {
                        estadoActual = 25;
                        concatenarLexema();
                    }
                    else if(CaracterActual =="H")
                    {
                        estadoActual = 32;
                        concatenarLexema();
                    }
                    else if(CaracterActual =="U")
                    {
                        estadoActual = 40;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "U");
                    }
                }
                else if(estadoActual ==25)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual =="A")
                    {
                        estadoActual = 26;
                        concatenarLexema();
                    }
                    else 
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "A");
                    }
                }
                else if(estadoActual ==26)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual =="R")
                    {
                        estadoActual = 27;
                        concatenarLexema();
                    }
                    else if (CaracterActual =="T")
                    {
                        estadoActual = 29;
                        concatenarLexema();
                    }
                    else 
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "T");
                    }

                }
                else if (estadoActual == 27)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual =="T")
                    {
                        estadoActual = 28;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "T");
                    }
                }
                else if(estadoActual == 28)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.START;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if(estadoActual == 29)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual =="U")
                    {
                        estadoActual = 30;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "U");
                    }
                }
                else if(estadoActual ==30)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual =="S")
                    {
                        estadoActual = 31;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "S");
                    }
                }
                else if(estadoActual == 31)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.STATUS;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if(estadoActual == 32)
                {
                    LeerSiguienteCaracter();
                    if(CaracterActual == "U")
                    {
                        estadoActual = 33;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "U");
                    }
                }
                else if (estadoActual == 33)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "T")
                    {
                        estadoActual = 34;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "T");
                    }
                }
                else if (estadoActual == 34)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "D")
                    {
                        estadoActual = 35;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "D");
                    }
                }
                else if (estadoActual == 35)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "O")
                    {
                        estadoActual = 36;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "O");
                    }
                }
                else if (estadoActual == 36)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "W")
                    {
                        estadoActual = 37;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "W");
                    }
                }
                else if (estadoActual == 37)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "N")
                    {
                        estadoActual = 38;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "N");
                    }
                }
                else if (estadoActual == 32)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.SHUTDOWN;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 39)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.FARENHEIT;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 40)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "C")
                    {
                        estadoActual = 41;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "C");
                    }
                }
                else if (estadoActual == 41)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "C")
                    {
                        estadoActual = 42;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "C");
                    }
                }
                else if (estadoActual == 42)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "E")
                    {
                        estadoActual = 43;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "E");
                    }
                }
                else if (estadoActual == 43)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "S")
                    {
                        estadoActual = 44;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "S");
                    }
                }
                else if (estadoActual == 44)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "S")
                    {
                        estadoActual = 45;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "S");
                    }
                }
                else if (estadoActual == 45)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.SUCCESS;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 46)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "E")
                    {
                        estadoActual = 47;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 53;
                    }
                }
                else if (estadoActual == 47)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "S")
                    {
                        estadoActual = 48;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "S");
                    }
                }
                else if (estadoActual == 48)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "T")
                    {
                        estadoActual = 49;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "T");
                    }
                }
                else if (estadoActual == 49)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "A")
                    {
                        estadoActual = 50;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "A");
                    }
                }
                else if (estadoActual == 50)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "R")
                    {
                        estadoActual = 51;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "R");
                    }
                }
                else if (estadoActual == 51)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "T")
                    {
                        estadoActual = 52;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "T");
                    }
                }
                else if (estadoActual == 52)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.RESTART;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 53)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.RANKINE;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 54)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "A")
                    {
                        estadoActual = 55;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 39;
                    }
                }
                else if (estadoActual == 55)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "I")
                    {
                        estadoActual = 56;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "I");
                    }
                }
                else if (estadoActual == 56)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "L")
                    {
                        estadoActual = 57;
                        concatenarLexema();
                    }
                    else
                    {
                        error(CaracterActual, NumeroLineaActual, Puntero, lexema, "L");
                    }
                }
                else if (estadoActual == 57)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.FAIL;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 58)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.CENTIGRADOS;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 60)
                {
                    continuarAnalisis = true;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.KELVIN;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero - lexema.Length;
                    componenteLexico.PosicionFinal = Puntero - 1;
                    estadoActual = 0;
                    MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 61)
                {
                    CargarNuevaLinea();
                    limpiarLexema();
                    estadoActual = 0;
                }
                else if(estadoActual == 62)
                {
                    continuarAnalisis = false;
                    componenteLexico = new ComponenteLexico();
                    componenteLexico.Categoria = Categoria.EOF;
                    componenteLexico.Lexema = lexema;
                    componenteLexico.NumeroLinea = NumeroLineaActual;
                    componenteLexico.PosicionInicial = Puntero;
                    componenteLexico.PosicionFinal = 5;
                    //estadoActual = 0;
                    //MensajeRetorno(componenteLexico);
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }

            }

                return componenteLexico;
        }

        private void MensajeRetorno(ComponenteLexico componente)
        {
            MessageBox.Show(componente.ToString());
        }

        private void error(string caracterActual,int numeroLineaActual,int puntero,string lexema, string siguienteCaracter)
        {
            string mensaje = "leí " + caracterActual + " y esperaba " + siguienteCaracter;
            if (CaracterActual == "@FL@")
            {
                caracterActual = "";
                mensaje = "expresion incompleta";
            }
                Error error1 = Error.CrearErrorLexico(
                lexema + caracterActual,
                Categoria.CARACTER_NO_VALIDO, numeroLineaActual,
                puntero - lexema.Length, puntero - 1,
                "caracter no valido", mensaje,
                "asegurese de que la expresión esté escrita correctamente"); ;

            GestorErrores.Reportar(error1);

            throw new Exception("Se ha producido un error de tipo STOPPER durante el analisis léxico, por favor verifique la consola de errores");

        }


    }
}
