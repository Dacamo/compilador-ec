using Compilador.Clases;
using Compilador.ManejadorErrores;
using Compilador.TablaSimbolos;
using Compilador.Transversal;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Compilador
{
    public partial class Form1 : Form
    {
        //



        public Form1()
        {
            InitializeComponent();
            textBoxRuta.Enabled = false;       
    
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void cargarArchivo_Click(object sender, EventArgs e)
        {
            BorrarPestañas();
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                //Aqui va el código para abrir y leer el archivo
                textBoxRuta.Text = archivo.FileName;
                string[] direccion = archivo.FileName.Split('\\');

                System.IO.File.Copy(archivo.FileName, direccion[direccion.Length - 1], true);
                MessageBox.Show("Archivo creado correctamente");

                using (StreamReader reader = new StreamReader(archivo.FileName))
                {
                    string[] texto = reader.ReadToEnd().Split('\n'); //Salto de linea

                    StringBuilder cadenaConcatenada = new StringBuilder();
                    foreach (var lineaArchivo in texto)
                    {
                        cadenaConcatenada.Append(lineaArchivo);
                    }

                    texto = cadenaConcatenada.ToString().Split('\r'); //Retornar carro
                    int contadorLineas = 1;
                    Entrada.Tipo = "Archivo";
                    StringBuilder lineaInicial = new StringBuilder();
                    foreach (var linea in texto)
                    {
                        Entrada.AgregarLinea(linea);
                        lineaInicial.Append(contadorLineas + "->" + linea + Environment.NewLine);
                        contadorLineas++;
                    }
                    registroCarga.Text = lineaInicial.ToString();
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            consola.Visible = true;
            file.Visible = false;
            registroCarga.Clear();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            file.Visible = true;
            consola.Visible = false;
            registroCarga.Clear();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            consola.Visible = false;
        }

        private void registroCarga_TextChanged(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            BorrarPestañas();
            string[] texto = console.Text.Split('\n'); //Salto de linea
            int contadorLineas = 1;
            Entrada.Tipo = "Consola";
            StringBuilder lineaInicial = new StringBuilder();
            foreach (var linea in texto)
            {
                Entrada.AgregarLinea(linea);
                lineaInicial.Append(contadorLineas + "->" + linea + Environment.NewLine);
                contadorLineas++;
            }
            registroCarga.Clear();
            registroCarga.Text = lineaInicial.ToString();
            console.Clear();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*BorrarPestañas();
            try
            {
                TablaPalabrasReservadas.inicializar();
                AnalizadorLexico anaLex = new AnalizadorLexico();
                anaLex.Analizar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            CrearPestañaDeComponentes();
            CrearPestañaDeErrores();
            CrearPestañaDePalabrasReservadas();
            CrearPestañaDeLiterales();
            Entrada.LimpiarLineas();*/

            /*BorrarPestañas();
            TablaPalabrasReservadas.inicializar();
            AnalizadorSintactico anaSin = new AnalizadorSintactico();
            anaSin.Analizar();
            CrearPestañaDeComponentes();
            CrearPestañaDeErrores();
            CrearPestañaDePalabrasReservadas();
            CrearPestañaDeLiterales();
            Entrada.LimpiarLineas();*/


            /*try
            {
                BorrarPestañas();
                TablaPalabrasReservadas.inicializar();
                AnalizadorLexico anaLex = new AnalizadorLexico();
                ComponenteLexico componente = anaLex.Analizar();

                while (componente.Lexema != "@EOF@")
                {
                    componente = anaLex.Analizar();
                }

                CrearPestañaDeComponentes();
                CrearPestañaDeErrores();
                CrearPestañaDePalabrasReservadas();
                CrearPestañaDeLiterales();
                Entrada.LimpiarLineas();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }*/

            try
            {
                BorrarPestañas();
                TablaPalabrasReservadas.inicializar();
                AnalizadorSintactico anaSin = new AnalizadorSintactico();
                anaSin.Analizar();

                CrearPestañaDeComponentes();
                CrearPestañaDeErrores();
                CrearPestañaDePalabrasReservadas();
                CrearPestañaDeLiterales();
                Entrada.LimpiarLineas();
            }
            catch(Exception ex)
            {
                CrearPestañaDeComponentes();
                CrearPestañaDeErrores();
                CrearPestañaDePalabrasReservadas();
                CrearPestañaDeLiterales();
                Entrada.LimpiarLineas();
                MessageBox.Show(ex.Message);
            }
        }

        private void CrearPestañaDePalabrasReservadas()
        {
            TabPage newPage = new TabPage("Tabla_Palabras_Reservadas");
            tabControl1.TabPages.Add(newPage);

            DataGrid dataGridPalabrasReservadas = new DataGrid()
            {
                DataSource = TablaPalabrasReservadas.ObtenerTodosLosSimbolos(),
                Location = new System.Drawing.Point(16, 78),
                Width = 656,
                Height = 150,
                PreferredColumnWidth = 104
            };
            Label tablaPalabrasReservadas = new Label()
            {
                Text = "Tabla de palabras reservadas",
                Location = new System.Drawing.Point(13, 33)
            };
            newPage.Controls.Add(tablaPalabrasReservadas);
            newPage.Controls.Add(dataGridPalabrasReservadas);
        }

        private void CrearPestañaDeLiterales()
        {
            TabPage newPage = new TabPage("Tabla_Literales");
            tabControl1.TabPages.Add(newPage);

            DataGrid dataGridLiterales = new DataGrid()
            {
                DataSource = TablaLiterales.ObtenerTodosLosSimbolos(),
                Location = new System.Drawing.Point(16, 78),
                Width = 656,
                Height = 150,
                PreferredColumnWidth = 104
            };
            Label tablaPalabrasLiterales = new Label()
            {
                Text = "Tabla de Literales",
                Location = new System.Drawing.Point(13, 33)
            };
            newPage.Controls.Add(tablaPalabrasLiterales);
            newPage.Controls.Add(dataGridLiterales);
        }

        private void Tabla_Componentes_Click(object sender, EventArgs e)
        {
            
        }

        private void Tabla_Errores_Click(object sender, EventArgs e)
        {

        }

        private void CrearPestañaDeComponentes()
        {
            TabPage newPage = new TabPage("Tabla_componentes");
            tabControl1.TabPages.Add(newPage);
            
            DataGrid dataGridSimbolos = new DataGrid()
            {
                DataSource = TablaSimbolos.TablaSimbolos.ObtenerTodosLosSimbolos(),
                Location = new System.Drawing.Point(16, 58),
                Width = 665,
                Height = 145,
                PreferredColumnWidth = 104
            };

            DataGrid dataGridDummys = new DataGrid()
            {
                DataSource = TablaSimbolos.TablaDummys.ObtenerTodosLosSimbolos(),
                Location = new System.Drawing.Point(16, 255),
                Width = 665,
                Height = 151,
                PreferredColumnWidth = 104
            };
            Label tablaSimbolos = new Label()
            {
                Text = "Tabla de simbolos",
                Location = new System.Drawing.Point(13, 33)
            };
            Label tablaDummys = new Label()
            {
                Text = "Tabla de dummys",
                Location = new System.Drawing.Point(13, 219)
            };
            newPage.Controls.Add(tablaSimbolos);
            newPage.Controls.Add(dataGridSimbolos);
            newPage.Controls.Add(tablaDummys);
            newPage.Controls.Add(dataGridDummys);
          
        }

        private void CrearPestañaDeErrores()
        {
            TabPage newPage = new TabPage("Tabla_errores");
            tabControl1.TabPages.Add(newPage);
            DataGrid dataGridErrores= new DataGrid()
            {
                DataSource = ManejadorErrores.GestorErrores.ObtenerTodosErrores(),
                Location = new System.Drawing.Point(16, 78),
                Width = 656,
                Height = 150,
                PreferredColumnWidth = 104
            };
            Label tablaErrores = new Label()
            {
                Text = "Tabla de errores",
                Location = new System.Drawing.Point(13, 33)
            };
            newPage.Controls.Add(tablaErrores);
            newPage.Controls.Add(dataGridErrores);
        }

        private void BorrarPestañas()
        {
            if (tabControl1.TabPages.Count > 1)
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages[1]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[1]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[1]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[1]);
                TablaSimbolos.TablaSimbolos.Limpiar();
                TablaDummys.Limpiar();
                TablaLiterales.Limpiar();
                TablaPalabrasReservadas.Limpiar();
                GestorErrores.BorrarTablaErrores();
            }
        }
        private void Datos_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            BorrarPestañas();
            registroCarga.Clear();
            textBoxRuta.Clear();     
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
