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
        private bool imprimirComponente = false;

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

                        estadoActual = 63;
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
                        estadoActual = 96;
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
                        estadoActual = 96;
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
                    estadoActual = 64;

                }

                else if (estadoActual == 5)
                {
                    DevolverPuntero();
                    continuarAnalisis = false;              
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.VALOR_ENVIO, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);                 
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 6)
                {
                    estadoActual = 65;
                }

                else if (estadoActual == 7)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "N")
                    {
                        estadoActual = 8;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 83;
                    }
                }
                else if (estadoActual == 8)
                {
                    estadoActual = 78;
                }
                else if (estadoActual == 9)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "U")
                    {
                        estadoActual = 10;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "N")
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
                        estadoActual = 63;
                    }
                }
                else if (estadoActual == 10)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "T")
                    {
                        estadoActual = 11;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 85;
                    }
                }
                else if (estadoActual == 11)
                {
                    estadoActual = 66;
                }
                else if (estadoActual == 12)
                {
                    estadoActual = 67;
                }
                else if (estadoActual == 13)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "F")
                    {
                        estadoActual = 14;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 86;
                    }
                }
                else if (estadoActual == 14)
                {
                    estadoActual = 68;
                }
                else if (estadoActual == 15)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "P")
                    {
                        estadoActual = 16;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 88;
                    }
                }
                else if (estadoActual == 16)
                {
                    estadoActual = 69;
                }
                else if (estadoActual == 17)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "O")
                    {
                        estadoActual = 18;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 89;

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
                        estadoActual = 89;
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
                        estadoActual = 89;
                    }
                }
                else if (estadoActual == 20)
                {
                    estadoActual = 70;
                }
                else if (estadoActual == 21)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "E")
                    {
                        estadoActual = 22;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 90;
                    }
                }
                else if (estadoActual == 22)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "T")
                    {
                        estadoActual = 23;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 90;
                    }
                }
                else if (estadoActual == 23)
                {
                    estadoActual = 71;
                }
                else if (estadoActual == 24)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "T")
                    {
                        estadoActual = 25;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "H")
                    {
                        estadoActual = 32;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "U")
                    {
                        estadoActual = 40;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 63;
                    }
                }
                else if (estadoActual == 25)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "A")
                    {
                        estadoActual = 26;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 63;
                    }
                }
                else if (estadoActual == 26)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "R")
                    {
                        estadoActual = 27;
                        concatenarLexema();
                    }
                    else if (CaracterActual == "T")
                    {
                        estadoActual = 29;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 63;
                    }

                }
                else if (estadoActual == 27)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "T")
                    {
                        estadoActual = 28;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 92;
                    }
                }
                else if (estadoActual == 28)
                {
                    estadoActual = 79;
                }
                else if (estadoActual == 29)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "U")
                    {
                        estadoActual = 30;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 93;
                    }
                }
                else if (estadoActual == 30)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "S")
                    {
                        estadoActual = 31;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 93;
                    }
                }
                else if (estadoActual == 31)
                {
                    estadoActual = 72;
                }
                else if (estadoActual == 32)
                {
                    LeerSiguienteCaracter();
                    if (CaracterActual == "U")
                    {
                        estadoActual = 33;
                        concatenarLexema();
                    }
                    else
                    {
                        estadoActual = 94;
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
                        estadoActual = 94;
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
                        estadoActual = 94;
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
                        estadoActual = 94;
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
                        estadoActual = 94;
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
                        estadoActual = 94;
                    }
                }
                else if (estadoActual == 38)
                {
                    estadoActual = 80;
                }
              
                else if (estadoActual == 39)
                {
                    DevolverPuntero();
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.FARENHEIT, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);

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
                        estadoActual = 95;
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
                        estadoActual = 95;
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
                        estadoActual = 95;
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
                        estadoActual = 95;
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
                        estadoActual = 95;
                    }
                }
                else if (estadoActual == 45)
                {
                    estadoActual = 73;
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
                        estadoActual = 87;
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
                        estadoActual = 87;
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
                        estadoActual = 87;
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
                        estadoActual = 87;
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
                        estadoActual = 87;
                    }
                }
                else if (estadoActual == 52)
                {
                    estadoActual = 74;
                }
                else if (estadoActual == 53)
                {
                    DevolverPuntero();
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.RANKINE, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
              
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
                        estadoActual = 91;
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
                        estadoActual = 91;
                    }
                }
                else if (estadoActual == 57)
                {
                    estadoActual = 75;
                }
                else if (estadoActual == 58)
                {
                    estadoActual = 76;
                }
                else if (estadoActual == 59)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.KELVIN, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
            
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 60)
                {
                    estadoActual = 59;
                }
                else if (estadoActual == 61)
                {
                    CargarNuevaLinea();
                    limpiarLexema();
                    estadoActual = 0;
                }
                else if (estadoActual == 62)
                {
                    estadoActual = 77;
                }
                else if (estadoActual == 64)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.VALOR_RETORNO, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                   
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 65)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.SEPARADOR, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 66)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.OUT, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                   
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 72)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.STATUS, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 73)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.SUCCESS, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 74)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.RESTART, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                //eof
                else if (estadoActual == 77)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.EOF, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    limpiarLexema();
                }
                else if (estadoActual == 78)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.IN, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 67)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.ON, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                   
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 68)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.OFF, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 69)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.UP, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 70)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.DOWN, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 71)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.GET, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                   
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 75)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.FAIL, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();

                }
                else if (estadoActual == 76)
                {
                    continuarAnalisis = false;;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.CENTIGRADOS, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 79)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.START, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 80)
                {
                    continuarAnalisis = false;
                    componenteLexico = ComponenteLexico.crear(lexema, Categoria.SHUTDOWN, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);                
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }

                //stopper
                else if (estadoActual == 63)
                {
                    Error error = Error.CrearErrorLexico(
                        CaracterActual,
                        Categoria.CARACTER_NO_VALIDO, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "Caracter no reconocido", "Leí " + CaracterActual,
                        "Asegurese que el caracter sera valido");

                    GestorErrores.Reportar(error);

                    throw new Exception("Se ha presentado un error de tipo STOPPER durante el análisis lexico. Por favor verifique la consola de errores");
                }
                else if (estadoActual == 83)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.IN, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "IN no válido",
                        "Leí " + lexema + " y se esperaba un IN",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);
                    componenteLexico = ComponenteLexico.crear("IN", Categoria.IN, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 85)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.OUT, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "OUT no válido",
                        "Leí " + lexema + " y se esperaba un OUT",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);
                    componenteLexico = ComponenteLexico.crear("OUT", Categoria.OUT, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                   
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 86)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.OFF, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "OFF no válido",
                        "Leí " + lexema + " y se esperaba un OFF",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);
                    componenteLexico = ComponenteLexico.crear("OFF", Categoria.OFF, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                   
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 88)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.UP, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "UP no válido",
                        "Leí " + lexema + " y se esperaba un UP",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);
                    componenteLexico = ComponenteLexico.crear("UP", Categoria.UP, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                   
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 89)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.DOWN, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "DOWN no válido",
                        "Leí " + lexema + " y se esperaba un DOWN",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);
                    componenteLexico = ComponenteLexico.crear("DOWN", Categoria.DOWN, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 90)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.GET, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "GET no válido",
                        "Leí " + lexema + " y se esperaba un GET",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);
                    componenteLexico = ComponenteLexico.crear("GET", Categoria.GET, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                   
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 92)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.START, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "START no válido",
                        "Leí " + lexema + " y se esperaba un START",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);
                    componenteLexico = ComponenteLexico.crear("START", Categoria.START, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 93)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.STATUS, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "STATUS no válido",
                        "Leí " + lexema + " y se esperaba un STATUS",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);
                    componenteLexico = ComponenteLexico.crear("STATUS", Categoria.STATUS, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                   
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 94)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.SHUTDOWN, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "SHUTDOWN no válido",
                        "Leí " + lexema + " y se esperaba un SHUTDOWN",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);                 
                    componenteLexico = ComponenteLexico.crear("SHUTDOWN", Categoria.SHUTDOWN, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                  
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 95)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.SUCCESS, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "SUCCESS no válido",
                        "Leí " + lexema + " y se esperaba un SUCCESS",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);                
                    componenteLexico = ComponenteLexico.crear("SUCCESS", Categoria.SUCCESS, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                  
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 87)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.RESTART, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "RESTART no válido",
                        "Leí " + lexema + " y se esperaba un RESTART",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);                 
                    componenteLexico = ComponenteLexico.crear("RESTART", Categoria.RESTART, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                    
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 91)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.FAIL, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "FAIL no válido",
                        "Leí " + lexema + " y se esperaba un FAIL",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);                  
                    componenteLexico = ComponenteLexico.crear("FAIL", Categoria.FAIL, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;                   
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }
                else if (estadoActual == 96)
                {
                    continuarAnalisis = false;
                    DevolverPuntero();

                    Error error = Error.CrearErrorLexico(
                        lexema,
                        Categoria.VALOR_ENVIO, NumeroLineaActual,
                        Puntero - lexema.Length, Puntero - 1,
                        "VALOR_ENVIO no válido",
                        "Leí " + lexema + " y se esperaba un VALOR_ENVIO",
                        "asegurese de que la instrucción esté escrita correctamente");

                    GestorErrores.Reportar(error);
                    componenteLexico = ComponenteLexico.crear("000", Categoria.VALOR_ENVIO, NumeroLineaActual, Puntero - lexema.Length, Puntero - 1);
                    componenteLexico.Tipo = TipoComponente.DUMMY;
                    TablaMaestra.SincronizarSimbolo(componenteLexico);
                    limpiarLexema();
                }

            }

                return componenteLexico;
        }

        private void MensajeRetorno(ComponenteLexico componente)
        {
            if (imprimirComponente)
            {
                MessageBox.Show(componente.ToString());
            }
           
        }

    }
}
