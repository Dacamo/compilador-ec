﻿using Compilador.Transversal;

namespace Compilador.TablaSimbolos
{
    public static class TablaMaestra
    {
        public static void SincronizarSimbolo(ComponenteLexico componente)
        {
            if (componente != null)
            {
                var esDummy = false;
               if(componente.Tipo == TipoComponente.DUMMY)
               {
                    TablaDummys.Agregar(componente);
                    esDummy = true;
                    
               }
                
                TablaPalabrasReservadas.ValidarSiEsPalabraReservada(componente);

                switch (componente.Tipo)
                {
                    case TipoComponente.SIMBOLO:
                        
                        //tabla de simbolos completa
                        if ( componente.Categoria.Equals(Categoria.VARIABLE))
                        {
                            TablaSimbolos.Agregar(componente);
                        }
                        // tabla de simbolos resumida
                        else
                        {
                            componente.Tipo = TipoComponente.LITERAL;
                            TablaLiterales.Agregar(componente);
                        }
                        break;

                    case TipoComponente.PALABRA_RESERVADA:
                        if (!esDummy)
                        {
                            TablaPalabrasReservadas.Agregar(componente);
                        }                      
                        break;
                }
            }
        }
    }
}
