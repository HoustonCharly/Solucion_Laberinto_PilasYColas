using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace WFmatGrid
{

    public partial class Form1 : Form
    {
        static int nfilas = 11;
        static int ncol = 11;

        CoordinateAndValue[,] Laberinto = new CoordinateAndValue[nfilas, ncol];


        public Form1()
        {
            InitializeComponent();
        }

        #region GENERAR EL LABERINTO
        private void bttamano_Click(object sender, EventArgs e)
        {

            Random k;
            k = new Random();
            int aleat, valor;
            Graphics g = pB1.CreateGraphics();
            Pen lapiz = new Pen(Color.DarkBlue);
            Size s = new System.Drawing.Size(50, 50);
            //SolidBrush brocha = new SolidBrush(Color.AliceBlue);
            Image pasto = Image.FromFile(@"diamante.bmp");
            //             Bitmap image1 = (Bitmap) Image.FromFile(@"pasto.bmp", true);
            //             TextureBrush pasto = new TextureBrush(image1);
            Image pared = Image.FromFile(@"agua.bmp");

            //             Bitmap image2 = (Bitmap)Image.FromFile(@"pared.bmp", true);
            //            TextureBrush pared = new TextureBrush(image2);
            //             Bitmap image3 = Bitmap(Image.FromFile(@"entrada.bmp", true), s);
            Image entrada = Image.FromFile(@"cisne.bmp");

            //             Bitmap image3 = (Bitmap)Image.FromFile(@"entrada.bmp", true);
            //            TextureBrush entrada = new TextureBrush(image3);

            //TextureBrush textura;
            Image textura;
            Point p = new Point(0, 0);
            Rectangle[,] r;
            r = new Rectangle[nfilas, ncol];
            for (int i = 0; i < nfilas; i++)
            {
                for (int j = 0; j < ncol; j++)
                {
                    if ((i == 0 && j == 0) || (i == nfilas - 1 && j == ncol - 1))
                    {
                        if (i == 0)
                            textura = entrada;
                        else
                            textura = pasto;
                        valor = 0;
                    }
                    else
                    {
                        if (i % 2 == 0 && j % 2 != 0)
                        {
                            textura = pasto;
                            valor = 0;
                        }
                        else
                        {
                            aleat = k.Next(2);
                            if (aleat == 0)
                                textura = pasto;
                            else
                                textura = pared;
                            valor = aleat;
                        }
                    }
                    //  Laberinto[i, j] = valor;
                    Laberinto[i, j] = new CoordinateAndValue(i, j, valor);
                    r[i, j].Location = p;
                    r[i, j].Size = s;
                    //  g.DrawRectangle(lapiz, r[i, j]);
                    //  g.FillRectangle(textura, r[i, j]);
                    g.DrawImage(textura, r[i, j]);
                    p.X += 50;
                }

                p.X = 0;
                p.Y += 50;


            }
        }
 #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            lBpila.Items.Clear();
            Stack<CoordinateAndValue> Pila1 = new Stack<CoordinateAndValue>();
            Stack<CoordinateAndValue> Pila2 = new Stack<CoordinateAndValue>();
            Stack<CoordinateAndValue> aux = new Stack<CoordinateAndValue>();
            Stack<CoordinateAndValue> Resultado = new Stack<CoordinateAndValue>();
            /*Pilas Auxiliares*/
            Stack<CoordinateAndValue> Paux2 = new Stack<CoordinateAndValue>();


            CoordinateAndValue[,] TemporaryPosition = new CoordinateAndValue[1, 1];
            TemporaryPosition[0, 0] = Laberinto[0, 0];
            Pila2.Push(TemporaryPosition[0, 0]);

            while (Pila2.Count > 0)
            {

                if (Laberinto[0, 0].value() == 1 || Laberinto[nfilas - 1, ncol - 1].value() == 1)
                {
                    //  Console.WriteLine("no hay salida");
                    MessageBox.Show("No hay salida");
                    Pila2.Clear();
                }
                else
                {
                    //clone stack
                    Paux2 = new Stack<CoordinateAndValue>(Pila2);

                    // convert Stack in matrix
                    String[] matrix = new String[Paux2.Count];
                    CoordinateAndValue[] array = Paux2.ToArray();
                    for (int i = 0; i < array.Length; i++)
                    {
                        matrix[i] = array[i].posX().ToString() + "," + array[i].posY().ToString();
                    }
                    //print matrix
                    Array.Reverse(matrix);
                    lBpila.Items.Add(String.Join(" | ", matrix));
                    Paux2.Clear();

                    CoordinateAndValue PositionTemporary, next;
                    PositionTemporary = Pila2.Pop();

                    Console.WriteLine($"sacando de de pila 2 {PositionTemporary.posX()},{PositionTemporary.posY()}");

                    if (PositionTemporary.posX() == nfilas - 1 && PositionTemporary.posY() == ncol - 1)
                    {
                        MessageBox.Show("Encontraste la Salida");

                        while (Pila1.Count != 0)
                        {

                            Resultado.Push(Pila1.Pop());

                        }
                        while (Resultado.Count != 0)
                        {
                            CoordinateAndValue aux2;
                            aux2 = Resultado.Pop();
                            lBpila.Items.Add($"{aux2.posX()},{aux2.posY()}");

                        }


                        Pila2.Clear();
                        break;

                    }

                    int ElemtosDePila = Pila2.Count;
                    #region Movimientos
                    next = PositionTemporary.Arriba();
                    if (EstaAdentroDelMaze(next) && MePuedoMover(next))
                    {


                        if (!Pila2.Contains(Laberinto[next.posX(), next.posY()]))
                        {
                            bool estaAdentro = Pila1.Contains(Laberinto[next.posX(), next.posY()]);
                            if (estaAdentro == false)
                            {
                                Pila2.Push(Laberinto[next.posX(), next.posY()]);

                                Console.WriteLine($"Guardando en Pila2 {next.posX()},{next.posY()}\n ");

                            }

                        }

                    }
                    next = PositionTemporary.Abajo();
                    if (EstaAdentroDelMaze(next) && MePuedoMover(next))
                    {

                        if (!Pila2.Contains(Laberinto[next.posX(), next.posY()]))
                        {
                            bool estaAdentro = Pila1.Contains(Laberinto[next.posX(), next.posY()]);
                            if (estaAdentro == false)
                            {
                                Pila2.Push(Laberinto[next.posX(), next.posY()]);
                                Console.WriteLine($"Guardando en Pila2 {next.posX()},{next.posY()}\n ");
                            }

                        }
                    }
                    next = PositionTemporary.Izquierda();
                    if (EstaAdentroDelMaze(next) && MePuedoMover(next))
                    {


                        if (!Pila2.Contains(Laberinto[next.posX(), next.posY()]))
                        {
                            bool estaAdentro = Pila1.Contains(Laberinto[next.posX(), next.posY()]);
                            if (estaAdentro == false)
                            {
                                Pila2.Push(Laberinto[next.posX(), next.posY()]);
                                Console.WriteLine($"Guardando en Pila2 {next.posX()},{next.posY()}\n ");
                            }

                        }
                    }
                    next = PositionTemporary.Derecha();
                    if (EstaAdentroDelMaze(next) && MePuedoMover(next))
                    {



                        if (!Pila2.Contains(Laberinto[next.posX(), next.posY()]))
                        {
                            bool estaAdentro = Pila1.Contains(Laberinto[next.posX(), next.posY()]);
                            if (estaAdentro == false)
                            {
                                Pila2.Push(Laberinto[next.posX(), next.posY()]);
                                Console.WriteLine($"Guardando en Pila2 {next.posX()},{next.posY()}\n ");
                            }

                        }
                    }

                    #endregion Movimientos
                    if (ElemtosDePila == Pila2.Count)
                    {
                        Console.WriteLine("Este camino no tiene Salida");

                    }
                    else
                    {
                        if (!Pila1.Contains(Laberinto[PositionTemporary.posX(), PositionTemporary.posY()]))
                        {

                            Pila1.Push(Laberinto[PositionTemporary.posX(), PositionTemporary.posY()]);
                            Console.WriteLine($"Metiendo a la pila1 nuestro recorrido {PositionTemporary.posX()},{PositionTemporary.posY()}\n ");
                        }
                    }


                    if (Pila2.Count == 0)
                    {
                        Console.WriteLine("No hay Salida :v");
                        MessageBox.Show("No hay Salida :v");

                    }
                }
            }

        }

        private void btcola_Click(object sender, EventArgs e)
        {
            lBcola.Items.Clear();
            Queue<CoordinateAndValue> Fila1 = new Queue<CoordinateAndValue>();
            Queue<CoordinateAndValue> Fila2 = new Queue<CoordinateAndValue>();
            Queue<CoordinateAndValue> aux = new Queue<CoordinateAndValue>();
            Queue<CoordinateAndValue> Resultado = new Queue<CoordinateAndValue>();
            /*Pilas Auxiliares*/
            Queue<CoordinateAndValue> Faux2 = new Queue<CoordinateAndValue>();


            CoordinateAndValue[,] TemporaryPosition = new CoordinateAndValue[1, 1];
            TemporaryPosition[0, 0] = Laberinto[0, 0];
            Fila2.Enqueue(TemporaryPosition[0, 0]);

            while (Fila2.Count > 0)
            {

                if (Laberinto[0, 0].value() == 1 || Laberinto[nfilas - 1, ncol - 1].value() == 1)
                {
                    //  Console.WriteLine("no hay salida");
                    MessageBox.Show("No hay salida ¡Auczilio! :'c");
                    Fila2.Clear();
                }
                else
                {
                    //clone stack
                    Faux2 = new Queue<CoordinateAndValue>(Fila2);

                    // convert Stack in matrix
                    String[] matrix = new String[Faux2.Count];
                    CoordinateAndValue[] array = Faux2.ToArray();
                    for (int i = 0; i < array.Length; i++)
                    {
                        matrix[i] = array[i].posX().ToString() + "," + array[i].posY().ToString();
                    }
                    //print matrix
                    Array.Reverse(matrix);
                    lBcola.Items.Add(String.Join(" | ", matrix));
                    Faux2.Clear();

                    CoordinateAndValue PositionTemporary, next;
                    PositionTemporary = Fila2.Dequeue();

                    Console.WriteLine($"sacando de de Fila 2 {PositionTemporary.posX()},{PositionTemporary.posY()}");

                    if (PositionTemporary.posX() == nfilas - 1 && PositionTemporary.posY() == ncol - 1)
                    {
                        //Console.WriteLine("LLEGAMOS!");
                        MessageBox.Show("Encontraste la Salida");

                        while (Fila1.Count != 0)
                        {

                            Resultado.Enqueue(Fila1.Dequeue());

                        }
                        while (Resultado.Count != 0)
                        {
                            CoordinateAndValue aux2;
                            aux2 = Resultado.Dequeue();
                            lBcola.Items.Add($"{aux2.posX()},{aux2.posY()}");

                        }


                        Fila2.Clear();
                        break;

                    }

                    int ElemtosDePila = Fila2.Count;
                    #region Movimientos
                    next = PositionTemporary.Arriba();
                    if (EstaAdentroDelMaze(next) && MePuedoMover(next))
                    {


                        if (!Fila2.Contains(Laberinto[next.posX(), next.posY()]))
                        {
                            bool estaAdentro = Fila1.Contains(Laberinto[next.posX(), next.posY()]);
                            if (estaAdentro == false)
                            {
                                Fila2.Enqueue(Laberinto[next.posX(), next.posY()]);

                                Console.WriteLine($"Guardando en Pila2 {next.posX()},{next.posY()}\n ");

                            }

                        }

                    }
                    next = PositionTemporary.Abajo();
                    if (EstaAdentroDelMaze(next) && MePuedoMover(next))
                    {

                        if (!Fila2.Contains(Laberinto[next.posX(), next.posY()]))
                        {
                            bool estaAdentro = Fila1.Contains(Laberinto[next.posX(), next.posY()]);
                            if (estaAdentro == false)
                            {
                                Fila2.Enqueue(Laberinto[next.posX(), next.posY()]);
                                Console.WriteLine($"Guardando en Pila2 {next.posX()},{next.posY()}\n ");
                            }

                        }
                    }
                    next = PositionTemporary.Izquierda();
                    if (EstaAdentroDelMaze(next) && MePuedoMover(next))
                    {


                        if (!Fila2.Contains(Laberinto[next.posX(), next.posY()]))
                        {
                            bool estaAdentro = Fila1.Contains(Laberinto[next.posX(), next.posY()]);
                            if (estaAdentro == false)
                            {
                                Fila2.Enqueue(Laberinto[next.posX(), next.posY()]);
                                Console.WriteLine($"Guardando en Pila2 {next.posX()},{next.posY()}\n ");
                            }

                        }
                    }
                    next = PositionTemporary.Derecha();
                    if (EstaAdentroDelMaze(next) && MePuedoMover(next))
                    {



                        if (!Fila2.Contains(Laberinto[next.posX(), next.posY()]))
                        {
                            bool estaAdentro = Fila1.Contains(Laberinto[next.posX(), next.posY()]);
                            if (estaAdentro == false)
                            {
                                Fila2.Enqueue(Laberinto[next.posX(), next.posY()]);
                                Console.WriteLine($"Guardando en Pila2 {next.posX()},{next.posY()}\n ");
                            }

                        }
                    }

                    #endregion Movimientos
                    if (ElemtosDePila == Fila2.Count)
                    {
                        Console.WriteLine("No hay Salida :v");

                    }
                    else
                    {
                        if (!Fila1.Contains(Laberinto[PositionTemporary.posX(), PositionTemporary.posY()]))
                        {

                            Fila1.Enqueue(Laberinto[PositionTemporary.posX(), PositionTemporary.posY()]);
                            Console.WriteLine($"Metiendo a la pila1 nuestro recorrido {PositionTemporary.posX()},{PositionTemporary.posY()}\n ");
                        }
                    }


                    if (Fila2.Count == 0)
                    {
                        Console.WriteLine("No hay Salida :v");
                        MessageBox.Show("No hay Salida :v ¡Auczilio!");

                    }
                }
            }

        }




        public bool EstaAdentroDelMaze(CoordinateAndValue posicion)
        {
            return EstaAdentroDelMaze(posicion.posX(), posicion.posY());
        }
        public bool EstaAdentroDelMaze(int x, int y)
        {
            if (x >= 0 && x < nfilas && y >= 0 && y < ncol)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool MePuedoMover(CoordinateAndValue libre)
        {

            return MePuedoMover(libre.posX(), libre.posY());
        }
        public bool MePuedoMover(int x, int y)
        {

            if (x >= 0 && x < nfilas && y >= 0 && y < ncol)
            {

                if (Laberinto[x, y].value() == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool Contains(CoordinateAndValue c1)
        {
            return c1 == Laberinto[c1.posX(), c1.posY()];
        }





        private void firmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Los Increibles");
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

    }

}
