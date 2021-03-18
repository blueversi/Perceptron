using System;
using System.Collections.Generic;
using System.Text;

namespace nai_p2
{
    class Perceptron
    {
        public static readonly string NIE_SETOSA = "nie setosa";
        public static readonly string IRIS_SETOSA = "Iris-setosa";
        public static readonly string IRIS_VERSICOLOR = "Iris-versicolor";
        public static readonly string IRIS_VIRGINICA = "Iris-virginica";
        public double A { get; set; } //wspolczynnik dokladosci
        public double B { get; set; } //prog
        public List<double> Wektory { get; set; }
        public List<Iris> DaneTreningowe { get; set; }


        public Perceptron(double a, List<Iris> dane)
        {
            A = a;
            DaneTreningowe = dane;
            Wektory = generujWektory(dane[0]);
        }


        private List<double> generujWektory(Iris iris)
        {
            List<double> tmpWektory = new List<double>();
            int liczba = iris.Atrybuty.Count;

            Random los = new Random();

            for (int i = 0; i < liczba; i++)
            {
                tmpWektory.Add(los.NextDouble());
            }

            return tmpWektory;

        }

        public List<Double> Trenuj(string gatunek)
        {
            List<double> tmpWektory = new List<double>();

            double tmpB = 0;
            int y = 0, d = 0;
            Shuffle(DaneTreningowe);

            foreach (var item in DaneTreningowe)
            {
                double tmpSuma = Iloczyn(Wektory, item.Atrybuty);
                bool czySzukanyIris = String.Equals(item.Nazwa, gatunek);
                if (czySzukanyIris && tmpSuma < tmpB)
                {
                    y = 0;
                    d = 1;
                } else if (!czySzukanyIris && tmpSuma < tmpB)
                {
                    y = 0;
                    d = 0;
                }

                if (czySzukanyIris && tmpSuma >= tmpB)
                {
                    y = 1;
                    d = 1;
                }
                else if (!czySzukanyIris && tmpSuma >= tmpB)
                {
                    y = 1;
                    d = 0;
                }

                Wektory = KorektaWektoru(Wektory, item.Atrybuty, d, y);
                tmpB += (d-y) * A * (-1);
            }

            B = tmpB;
            
            return tmpWektory;
        }

        public void OkreslGatunekIrisaSprwadzajacPoprawnosc(List<Iris> daneTestowe)
        {
            int licznikPoprawnychKwalifikacji = 0;
            double dokladnoscEksperymentu = 0;
           // while (dokladnoscEksperymentu >= 100)
           // {
                foreach (var item in daneTestowe)

                {
                bool test = czySetosa(item);
                if (test && String.Equals(item.Nazwa, IRIS_SETOSA))
                {
                    Console.WriteLine("Uzyskano poprawna kwalifikacje!");
                    Console.WriteLine("Gatunek wg Perceptronu to:  " + IRIS_SETOSA);
                    Console.WriteLine("Gatunek wg Pliku to:  " + item.Nazwa);
                    licznikPoprawnychKwalifikacji++;
                }
                else if (!test && (String.Equals(item.Nazwa, IRIS_VERSICOLOR) || String.Equals(item.Nazwa, IRIS_VIRGINICA)))
                {
                    Console.WriteLine("Uzyskano poprawna kwalifikacje! ");
                    Console.WriteLine("Gatunek wg Perceptronu to:  " + NIE_SETOSA);
                    Console.WriteLine("Gatunek wg Pliku to:  " + item.Nazwa);
                    licznikPoprawnychKwalifikacji++;
                }

                else
                {
                    Console.WriteLine("Nie uzyskano poprawnej kwalifikacji :(");
                    Console.WriteLine("Gatunek wg Perceptronu to:  " + (test ? IRIS_SETOSA : NIE_SETOSA));
                    Console.WriteLine("Gatunek wg Pliku to:  " + item.Nazwa);

                }

                    Console.WriteLine(" ");
                    Console.WriteLine("****************************");
                    Console.WriteLine("****************************");
                    Console.WriteLine("****************************");
                    Console.WriteLine(" ");
                }
            


            dokladnoscEksperymentu = ((double)licznikPoprawnychKwalifikacji / (double)daneTestowe.Count) * 100; 
            Console.WriteLine("------------------------------ ");
            Console.WriteLine(" ");
            Console.WriteLine("DOKŁADNOŚĆ EKSPERYMENTU ");
            Console.WriteLine(dokladnoscEksperymentu + "%");
            Console.WriteLine(" ");
            Console.WriteLine("------------------------------");
                if (dokladnoscEksperymentu < 99)
               {
                    Trenuj(IRIS_SETOSA);
                    OkreslGatunekIrisaSprwadzajacPoprawnosc(daneTestowe);
               }

           // }
        }


        public List<double> KorektaWektoru(List<double> wektory, List<double> atrybuty, int d, int y)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < wektory.Count; i++)
            {             
                result.Add(atrybuty[i] + (d-y) * wektory[i]);
            }
            return result;
        }
        public double Iloczyn(List<double> wektory, List<double> atrybuty)
        {
            double result = 0.0;
            for(int i = 0; i< wektory.Count; i++)
            {
                result += (wektory[i] * atrybuty[i]);
            }
            return result;
        }
        public bool czySetosa(Iris obj)
        {
            Console.WriteLine("CZY SETOSA THETA ROWNA SIE " + B);
            double tmp = Iloczyn(Wektory, obj.Atrybuty);
            Console.WriteLine("Iloczyn = " + tmp);
            if (tmp < B)
            {

                return true;
            }
            else
            {
                return false;
            }

        }

        public void Shuffle<T>(List<T> list)
        {
            Random r = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
