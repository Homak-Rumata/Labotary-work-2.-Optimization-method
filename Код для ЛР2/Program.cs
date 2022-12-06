using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Код_для_ЛР2
{
    internal class Program
    {

        static void Main(string[] args)
        {
            IMethod polygon = new Polygon(2, 100, FunctionConstract());
            IMethod gelfanmethod = new GelfandMethod(GradientConstract(), FunctionConstract(), 0.005, 0.5, 5.1, 50, 50, 5, new double[2] { 5, 5 });
            IMethod gradient = new Gradient(GradientConstract(), FunctionConstract(), new double[] { 0, 0 });
            IMethod NewGradient = new ExperemetalMethod(GradientConstract(), FunctionConstract(), new double[] { 0,0 });

            double a;
            double[] b;

            (a, b) = polygon.FindMin();
           Console.WriteLine($"Методом многоугольников: x1 = {b[0]}, x2 = {b[1]}, F(x) = {a}");

            (a, b) = gelfanmethod.FindMin();
            Console.WriteLine($"Методом Гельфанда-Цейтлина : x1 = {b[0]}, x2 = {b[1]}, F(x) = {a}");

            (a, b) = gradient.FindMin();
            Console.WriteLine($"Методом градиента: x1 = {b[0]}, x2 = {b[1]}, F(x) = {a}");

            (a, b) = NewGradient.FindMin();
            Console.WriteLine($"Методом скачка: x1 = {b[0]}, x2 = {b[1]}, F(x) = {a}");

            Console.WriteLine("\n\t\t\tДля второй функции\n");

            polygon = new Polygon(2, 100, SecondFunctionConstract());
            gelfanmethod = new GelfandMethod(SecondGradientConstract(), SecondFunctionConstract(), 0.005, 0.5, 5.1, 50, 50, 5, new double[2] { 5, 5 });
            gradient = new Gradient(SecondGradientConstract(), SecondFunctionConstract(), new double[] { 0.786885245901639, 0.786885245901639 });
            NewGradient = new ExperemetalMethod(SecondGradientConstract(), SecondFunctionConstract(), new double[] { 0.786885245901639, 0.786885245901639 });


            

            (a, b) = polygon.FindMin();
            Console.WriteLine($"Методом многоугольников: x1 = {b[0]}, x2 = {b[1]}, F(x) = {a}");

            (a, b) = gelfanmethod.FindMin();
            Console.WriteLine($"Методом Гельфанда-Цейтлина : x1 = {b[0]}, x2 = {b[1]}, F(x) = {a}");

            (a, b) = gradient.FindMin();
            Console.WriteLine($"Методом градиента: x1 = {b[0]}, x2 = {b[1]}, F(x) = {a}");

            (a, b) = NewGradient.FindMin();
            Console.WriteLine($"Методом скачка: x1 = {b[0]}, x2 = {b[1]}, F(x) = {a}");
        }

        public static OperationArray FunctionConstract ()
        {
            double Function (double[] x) 
            {
                return ((4*(x[0]-4) * (x[0]-4))) + (3*(x[1]-1) * (x[1]-1));
            }
            return Function;
        }

        public static OperationArray SecondFunctionConstract ()
        {
            double Function(double[] x)
            {
                return Math.Pow(x[0] - 4, 2) + Math.Pow(x[1] - 4, 2) + (30 * Math.Pow(x[1] + x[0] - 6, 2)) + 1.4;
            }
            return Function;
        }

        public static Antigradient GradientConstract ()
        {
            OperationArray[] Grad()
            {
                OperationArray[] FunctionPull = new OperationArray[2]{ (double[] x) => 8 * x[0] - 32, (double[] x) => 6 * x[1] - 6 };
                return FunctionPull;
            }
            return Grad;

        }

        public static Antigradient SecondGradientConstract()
        {
            OperationArray[] Grad()
            {
                OperationArray[] FunctionPull = new OperationArray[2] { (double[] x) => (62 * x[0]) + (60 * x[1]) - (60*6) - 1.4-8, (double[] x) => (62 * x[0]) + (60 * x[1]) - (60 *6) - 1.4-8 };
                return FunctionPull;
            }
            return Grad;

        }

        interface IMethod
        {
            public (double, double[]) FindMin();
        }

        class Polygon:IMethod
        {
            Node[] x;
            private double alph = 1, gamma = 2, betta = 0.5, epsilon = 0.000001;
            private Node LightPound;
            private Node HeavyPound;
            private Node MassCentre;
            private Node ReflectionMassCentre;
            private Node StretchingPound;
            private Node CompressionPound;
            private OperationArray function;
            private int n;
            public Polygon (int n, int t, OperationArray function)
            {
                this.n = n;
                this.function = function;
                this.x = new Node[n+1];
                x[0] = new Node(n, t);
                x[0].ID = 0;
                for (int i = 1; i < n + 1; i++)
                {
                    x[i] = new Node(i);
                    x[i].ID = i;
                }

                MassCentre = new Node();
                MassCentre.ID = 3;
                ReflectionMassCentre = new Node();
                ReflectionMassCentre.ID = 4;
                StretchingPound = new Node();
                StretchingPound.ID = 5;
                CompressionPound = new Node();
                CompressionPound.ID = 6;

        }

            public (double, double[]) FindMin() //Инициализирующая функция
            {
                double counter;
                do
                {
                    Search(false);
                    counter = 0;
                    for (int i = 0; i<=n; i++)
                    {
                        Console.WriteLine(function(x[i].GetCoords) + " " + x[i].GetCoords[0] + " " + x[i].GetCoords[1]);
//                        Console.WriteLine (function(MassCentre.GetCoords) +" " + MassCentre.GetCoords[0] + " " + MassCentre.GetCoords[1]);
                        counter += Math.Pow(function(x[i].GetCoords) - function(MassCentre.GetCoords), 2);
//                       Console.WriteLine($"{i}: x1 = {x[i].GetCoord(0)}; x2 = {x[i].GetCoord(1)}");
                        
                    }
//                    Console.WriteLine($"{ReflectionMassCentre.ID} {ReflectionMassCentre.GetCoord(0)} {ReflectionMassCentre.GetCoord(1)} {HeavyPound.ID} {HeavyPound.GetCoord(0)} {HeavyPound.GetCoord(1)} {x[0].ID} {x[0].GetCoord(0)} {x[0].GetCoord(1)}");
                    counter = Math.Sqrt(counter);
                    counter /= (n + 1);
                    NextStep();
                }
                while (counter > epsilon);
                return (function(MassCentre.GetCoords), MassCentre.GetCoords);    
            }

            private void NextStep() //Проверка на следующие действия
            {

//                Console.WriteLine($"function(ReflectionMassCentre.GetCoords = {function(ReflectionMassCentre.GetCoords)} \n function(LightPound.GetCoords) = {function(LightPound.GetCoords)}");
                if (function(ReflectionMassCentre.GetCoords)< function(LightPound.GetCoords))
                {
                    stretching();
                    if (function(StretchingPound.GetCoords) < function(LightPound.GetCoords))
                    {
                        HeavyPound.SetCoords =StretchingPound.GetCoords;
                        
                    }
                    else
                    {
                        HeavyPound.SetCoords = ReflectionMassCentre.GetCoords;
                        
                    }
                }
                else
                {
                    bool flag = true;
                    for (int i = 0; i < n+1; i++) 
                    {
                        if (function(ReflectionMassCentre.GetCoords) <= function(x[i].GetCoords))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        if (function(ReflectionMassCentre.GetCoords) > function(HeavyPound.GetCoords))
                        {
                            redyce();
                        }
                        else
                        {
                            compression();
                            if (function(CompressionPound.GetCoords) > function(HeavyPound.GetCoords))
                            {
                                redyce();
                            }
                            else
                            {
                                HeavyPound.SetCoords = CompressionPound.GetCoords;
                            }
                        }
                    }
                    else
                    {
                        HeavyPound.SetCoords = ReflectionMassCentre.GetCoords;
                        
                    }
                        
                }
            }



            private void Search (bool D) //Нахождение нового центра массы и нового отражённого центра массы
            {
                int a = 5;
                int c;
                if (!D)
                {
                    HeavyPound = x[0];
                }
                LightPound = x[0];
                foreach (Node selested in x)
                {
                    if (!D)
                    {
                        HeavyPound = function(selested.GetCoords) > function(HeavyPound.GetCoords) ? selested : HeavyPound;
                    }
                    LightPound = function(selested.GetCoords) < function(LightPound.GetCoords) ? selested : LightPound;
                }

                double[] counter = new double[n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j <= n; j++)
                    {
                        counter[i] = counter[i] + x[j].GetCoord(i);
                    }
                    counter[i] -= HeavyPound.GetCoord(i);
                    counter[i] /= this.n;
                }
                MassCentre.SetCoords = (counter);


                for (int i = 0; i < n; i++)
                {
                    ReflectionMassCentre.SetCoord(i, (MassCentre.GetCoord(i)+alph*(MassCentre.GetCoord(i)-HeavyPound.GetCoord(i))));
                }
            }

            private void redyce () //Редукция
            {
                for (int i = 0; i <= n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        x[i].SetCoord(j, LightPound.GetCoord(j) + 0.5 * (x[i].GetCoord(j)- LightPound.GetCoord(j)));
                    }
                }
            }

            private void stretching () //Растяжение
            {
                for (int i = 0; i < n; i++)
                {
                    StretchingPound.SetCoord(i, (MassCentre.GetCoord(i) + gamma * (ReflectionMassCentre.GetCoord(i) - MassCentre.GetCoord(i))));
                }
            }

            private void compression () //Сжатие
            {
                for (int i = 0; i < n; i++)
                {
                    CompressionPound.SetCoord(i,(MassCentre.GetCoord(i) + betta * (MassCentre.GetCoord(i) - LightPound.GetCoord(i))));
                }
            }





            private class Node //Класс точки
            {

                public int ID;

                private static PolynomConstation LocalCostation;

                static Node()
                {
                    Console.WriteLine("Стройка завершенна");
                }

                public Node() //Главный конструктор
                {
                    Coord = new double[Polygon.PolynomConstation.dimensions];
                }

                public Node(int n, double t) //Конструктор инициализации первого экземпляра класса
                {
                    LocalCostation = new PolynomConstation(n, t);
                    Coord = new double[Polygon.PolynomConstation.dimensions];
                    SetZero();
                }

                public Node(int n) : this() //Конструктор, устанавливает значения d1, d2 по ячейкам
                {
                    n -= 1;
                    for (int i = 0; i < Polygon.PolynomConstation.dimensions; Coord[i] = Polygon.PolynomConstation.b2, i++) ;
                    Coord[n] = Polygon.PolynomConstation.b1;
                }

                public void SetZero() //Установить нулевые значения, бесполезен
                {
                    for (int i = 0; i < Polygon.PolynomConstation.dimensions - 1; i++, Coord[i] = 0) ;
                }



                private double[] Coord;

                public double GetCoord(int n)
                {
                    return Coord[n];
                }

                public void SetCoord(int n, double x)
                {
                    Coord[n] = x;
                }

                public double[] GetCoords
                {
                    get { return Coord; }
                }

                public double[] SetCoords //Методот установки значения координат, нельзя просто отправлять ссылку на 
                                          //Обязателен построчный перебор
                {
                        set
                    {
                        
                            for (int i = 0; i<value.Length; i++)
                        {
                            Coord[i] = value[i];
                        }
                        
                    }

                }


            }

            readonly private struct PolynomConstation
            {
                public PolynomConstation(int n, double f)
                {
                    t = f;
                    b2 = (t / (Math.Sqrt(n + 1) - 1)) / (Math.Sqrt(2) * n);
                    b1 = (t / (Math.Sqrt(n + 1) - 1 + n)) / (Math.Sqrt(2) * n);
                    dimensions = n;

                }
                public static int dimensions;
                public static double b1, b2, t;
            } //Класс - хранилище констант, устанавливаются при первом
                                                         //Конструировании класса Node



        }

        class Gradient:IMethod
        {
            private protected double[] x;
            private int n;
            private protected Antigradient gradient;
            private protected OperationArray function;
            public Gradient (Antigradient grad, OperationArray function, double[] x) 
            {
                gradient = grad;
                this.function = function;
                this.x = x;
                n = x.Length;
            }

            private OperationSingle SetFunction (int i, double[] arg) //Замыкающая функция, создают промежуточные функции для поиска Т
            {
                double F (double z)
                {
                    return arg[i] - z * (gradient()[i](arg));
                }
                return F;
            }

            private double GetT(double[] x) //Поиск Т
            {
                OperationSingle[] FunctionPull = new OperationSingle[gradient().Length];
                for (int i = 0; i<FunctionPull.Length; i++)
                {
                    FunctionPull[i] = SetFunction(i, x);
                }
                OperationSingle TimeFunction = Function(FunctionPull); //Собираем из промежуточных функций финальную F(t)
                bool flag = true;
                double key = 0;
                double output = 0;
                int a = 0, b = 1;
                
                    (key, output) = FibonachiMethod(TimeFunction, a, b);
                
//                Console.WriteLine("key = " + key + " out = " + output);
                return (key);
            }

            private double[] NextStep(double[] x) //Новая итерация
            {
                double t = GetT(x);
//                Console.WriteLine($" old x1 = {x[0]}, x2 = {x[1]}, t = {t}");
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = x[i] - t * (gradient()[i](x));
//                    Console.WriteLine($" gr = {(gradient()[i](x[i]))} ");
                }
//                Console.WriteLine($"x1 = {x[0]}, x2 = {x[1]}, ");
                return x;
            }

            public virtual (double, double[]) FindMin() //Функция инициализатор
            {
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    for (int i = 0; i< this.x.Length; i++)
                    {
                        if (Math.Abs(gradient()[i](this.x)) > 0.001)
                        {
                            x = NextStep(x);
                            flag = true;
                            break;
                        }
                        
                    }
                }
                double[] counter = new double[n];
                
                return (function (x), x);
            }


            private (double, double) GoldSek(OperationSingle Function, double a, double b) //Резервный Метод поиска минимума
            {
                double XB, XA;
                const double Alph = 0.618;
                const double Bet = 0.382;
                double jk = Function(0);
                while (Math.Abs(a - b) > 0.0000001)
                {
                    XB = a + Bet * Math.Abs(a - b);
                    XA = a + Alph * Math.Abs(a - b);
                    if (Function(XB) > Function(XA))
                    {
                        a = XB;
                    }
                    else
                    {
                        b = XA;
                    }
                }

                if (Function(a) < Function(b))
                {
                    return (a, Function(a));
                }
                else
                {
                    return (b, Function(b));
                }
            }

            private protected (double, double) FibonachiMethod(OperationSingle Function, double a, double b) //Главный способ поиска минимума F(t)
            {
                double X1, X2;
                int k, n = 0;
                k = 1;
                long usl = Convert.ToInt64(NumberFib(1));

                for (int i = 1; usl < (Math.Abs(a - b) / 0.0001); i++, n = i - 1)
                    usl = Convert.ToInt64(NumberFib(i));
                while (Math.Abs(a - b) >= 0.0001)
                {
                    X1 = a + (NumberFib(n - k + 1) / NumberFib(n - k + 3) * (b - a));
                    X2 = a + (NumberFib(n - k + 2) / NumberFib(n - k + 3) * (b - a));
                    if (Function(X1) >= Function(X2))
                    {
                        a = X1;
                        k++;
                    }
                    else
                    {
                        b = X2;
                        k++;
                    }
                }
                return ((a + b) / 2, Function((a + b) / 2));
            }

            private double NumberFib(int num) //Вспомогательная функция метода поиска минимума
            {
                return Convert.ToInt64((Math.Pow((1 + Math.Sqrt(5)) / 2, num) - Math.Pow((1 - Math.Sqrt(5)) / 2, num)) / Math.Sqrt(5));
            }

            public OperationSingle Function(OperationSingle[] functions) ////Собираем из промежуточных функций финальную F(t)
            {
                double Funct (double arg)
                {
                    double[] timearray = new double[functions.Length];
                    for (int i = 0; i<x.Length; i++)
                    {
                        timearray[i] = functions[i](arg);
                    }
                    return (this.function(timearray));
                }
                return Funct;
            }

            
        }

        class GelfandMethod:IMethod
        {
            Antigradient grad;
            OperationArray functions;
            readonly double t1, t2, E1, E2;
            readonly int N1, N2;
            double[] x;
            public GelfandMethod(Antigradient grad, OperationArray Functia, double t1, double t2, double E1, double E2, int N1, int N2, double[] StartX)
            {
                this.grad = grad;
                this.functions = Functia;
                this.t1 = t1;
                this.t2 = t2;
                this.E1 = E1;
                this.E2 = E2;
                this.N1 = N1;
                this.N2 = N2;
                x = StartX;
                G = new Gf();
                
                    G.g = new double[x.Length];
                    for (int j = 0; j < x.Length; j++)
                    {
                    for (int i = 0; i < x.Length; i++)
                    {
                        if (E1 > Math.Abs(grad()[i](x)))
                        {
                            
                            G.g[i] = 0;
                        }
                        else
                        {
                            G.g[i] = grad()[i](x);
                        }
                    }
                }
                

            }

            Gf G;

            public (double, double[]) FindMin () //Головной метод
            {
                bool flag = true;
                int LocalCounter = 0;
                while (flag && LocalCounter <= N1) 
                {
                    G.g = Sets(ref flag, false, E1);
                    if (!flag)
                    {
                        break;
                    }
                    LocalCounter++;
                    for (int i = 0; i < x.Length; i++)
                    {
                        x[i] = x[i] - t1 * (G.g[i]);
                    }

                }


                LocalCounter = 0;

                flag = true;

                while (flag && LocalCounter <= N2) 
                {
                    G.g = Sets(ref flag, true, E2); 
                    LocalCounter++;
                    for (int i = 0; i < x.Length; i++)
                    {
                        x[i] = x[i] - t2 * (G.g[i]);
//                        Console.WriteLine($"x[{i}] = {grad()[i](x)}");
                    }

//                    Console.WriteLine($"x1 = {x[0]}, x2 = {x[1]}");
                    
                }
                if (flag == true)
                {
                 //   Console.WriteLine("Yes");
                    return (FindMin());
                }
                else
                {

                    return (functions(x), x);
                }


            }

            private double[] Sets(ref bool flag, bool SetsNumber, double E ) //Метод этапа
            {
                double[] TimeArray = new double[x.Length];
                int counter = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    if ((E > Math.Abs(grad()[i](x))) ^ (SetsNumber))
                    {
                        counter++;
                        TimeArray[i] = 0;
                    }
                    else
                    {
                        TimeArray[i] = grad()[i](x);
                    }
                }
                if (counter == x.Length)
                {
                    flag = false;
                    return TimeArray;
                }
                else
                {
                    return TimeArray;
                }
            }

            private struct Gf
            {
                public double[] g;
            }
        }

        sealed class ExperemetalMethod:Gradient,IMethod
        {



            private double[] FirstPoint;
            private double[] SecondPoint;
            public ExperemetalMethod (Antigradient gradient, OperationArray function, double[] x)
                :base (gradient, function, x)
            {
                (_, FirstPoint) = base.FindMin();
                SecondPoint = new double[FirstPoint.Length];
                for (int i = 0; i<x.Length; i++)
                {
                    SecondPoint[i] = x[i]-10;
                }
            }

            private OperationSingle SetFunction(int i, double[] FirstPoint, double[] SecondPoint, double multiplier) //Сборка промежуточных функций
            {
                double F(double z)
                {
                    
                    return FirstPoint[i] - multiplier * z * (SecondPoint[i] - FirstPoint[i]);;
                }
                return F;
            }

            

            private void Iteration () //Новая итерация
            {
                base.x = FirstPoint;
                base.FindMin();
                for (int i = 0; i<FirstPoint.Length; i++)
                {
                    SecondPoint[i] = FirstPoint[i]+10;
                }
                base.x = SecondPoint;
                base.FindMin();
                double multiplier = 0;
                for (int i = 0; i < FirstPoint.Length;i++)
                {
                    multiplier += Math.Pow(SecondPoint[i] - FirstPoint[i],2);
                }
                multiplier = (function(SecondPoint) - function(FirstPoint))/ Math.Sqrt(multiplier);

                OperationSingle[] Operate;

                double T = 2;

                for (int i = 0; i< FirstPoint.Length;i++)
                {

                    FirstPoint[i] = FirstPoint[i] - multiplier * (SecondPoint[i] - FirstPoint[i])*T;
                }
            }



            private int counter = 0;

            public override (double, double[]) FindMin () //Главная функция
            {
                double[] OnTimeFirstPoint, OnTimeSecondPoint;
                OnTimeFirstPoint= new double[FirstPoint.Length];
                OnTimeSecondPoint= new double[SecondPoint.Length];

                for (int i = 0; i < FirstPoint.Length;i++)
                {
                    OnTimeFirstPoint[i] = FirstPoint[i];
                    OnTimeSecondPoint[i] = SecondPoint[i];
                }

                do
                {
//                    Console.WriteLine($"х[k-1] = {function(OnTimeFirstPoint)}, x[k] {function(FirstPoint)}");

                    for (int i = 0; i < FirstPoint.Length; i++)
                    {
                        OnTimeFirstPoint[i] = FirstPoint[i];
                        OnTimeSecondPoint[i] = SecondPoint[i];
                    }



                    Iteration();


//                    Console.WriteLine($"{function(OnTimeFirstPoint)}, {function(FirstPoint)}, {function(OnTimeSecondPoint)}, {function(SecondPoint)}");

                }
                while ((function(OnTimeFirstPoint) >= function(FirstPoint)));
               
                return (function(OnTimeFirstPoint), OnTimeFirstPoint);
                
            }



        }
        

        public delegate double OperationArray (double[] x);
        public delegate double OperationSingle (double x);
        
        public delegate OperationArray[] Antigradient();
    }


}