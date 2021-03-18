using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;


/*          
 **********************   Rafał Sadowski   **********************    
 **********************      s18311        **********************  
 **********************     Projek NAI     **********************  
 **********************       Nr 2         **********************              
 **********************     Perceptron     **********************  
 */

namespace nai_p2
{
    class Program
    {
        public static readonly string CLOSE_PROGRAM = "exit";
        public static readonly string NIE_SETOSA = "nie setosa";
        public static readonly string IRIS_SETOSA = "Iris-setosa";
        public static readonly string IRIS_VERSICOLOR = "Iris-versicolor";
        public static readonly string IRIS_VIRGINICA = "Iris-virginica";
        static void Main(string[] args)
        {
            var daneTreningowe = @"iris_training.txt";
            var daneTestowe = @"iris_test.txt";
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory + @"iris_training.txt");
            List<string[]> daneTreningoweStringList = PobierzDane(daneTreningowe);
            List<string[]> daneTestoweStringList = PobierzDane(daneTestowe);
            List<Iris> daneTreningoweRawList = WczytajIrisy(daneTreningoweStringList);
            List<Iris> daneTestoweRawList = WczytajIrisy(daneTestoweStringList);
            Shuffle(daneTreningoweRawList);
            Shuffle(daneTestoweRawList);

            /*          
             **********************                    **********************    
             **********************                    **********************  
             **********************       KONSOLA      **********************  
             **********************                    **********************              
             **********************                    **********************  
             */

            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("Prosze o podanie wartosci  parmetru A dla perceptronu");
            Console.WriteLine(" ");

            double A = double.Parse(Console.ReadLine().Replace('.', ','), CultureInfo.CurrentCulture);


            Perceptron percep = new Perceptron(A, daneTreningoweRawList);
            percep.Trenuj(IRIS_SETOSA);

            Console.WriteLine("Jesli chcesz sprawdzic dane z pliku iris_test.txt wpisz komende: ");
            Console.WriteLine("test");
            Console.WriteLine(" ");
            Console.WriteLine("Jeśli chcesz podać własny wektor wpisz komende: ");
            Console.WriteLine("wektor");
            Console.WriteLine(" ");
            Console.WriteLine("Aby zmienić wartość A wpisz komende ");
            Console.WriteLine("a");
            Console.WriteLine("Aby uczyc dalej wpisz komende ");
            Console.WriteLine("ucz");
            Console.WriteLine("Aby zakonczyc dzialanie programu wpisz komende ");
            Console.WriteLine("exit");
            Console.WriteLine(" ");

            string comand = " ";
            while ((comand = Console.ReadLine()) != CLOSE_PROGRAM)
            {
                if (comand == "a")
                {
                    Console.WriteLine("Prosze o podanie wartosci parmetru A dla perceptronu");
                    Console.WriteLine(" ");

                    A = double.Parse(Console.ReadLine().Replace('.', ','), CultureInfo.CurrentCulture);
                    percep.Trenuj(IRIS_SETOSA);
                }

                if (comand == "ucz")
                {

                    Console.WriteLine("nauka... ");
                    percep.Trenuj(IRIS_SETOSA);
                    Console.WriteLine("skonczona ");
                }

                if (comand == "test")
                {
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    Console.WriteLine("Klasyfikacja danych testowych");
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    percep.OkreslGatunekIrisaSprwadzajacPoprawnosc(daneTestoweRawList);

                    Console.WriteLine("-----");
                    Console.WriteLine(
                    percep.Wektory[0]  + " " +percep.Wektory[1] + " " + percep.Wektory[2] + " " + percep.Wektory[3]);

                }

                if (comand == "wektor")
                {
                    Console.WriteLine("Prosze pamietac ze wektor musi byc podany zgodnie z liczba atrybutow w pliku treningowym");
                    Console.WriteLine("Akceptowalny format:  x,y np 2,5");
                    List<double> tmpWektor = new List<double>();
                    for (int i = 0; i < daneTreningoweRawList[0].Atrybuty.Count; i++)
                    {

                        Console.WriteLine("Podaj atrybut nr: " + (i + 1));
                        string tmpStr = Console.ReadLine();
                        if (tmpStr.Length != 0)
                        {
                            tmpWektor.Add(double.Parse(tmpStr.Replace('.', ','), CultureInfo.CurrentCulture));
                        }
                        else
                        {
                            Console.WriteLine("Nie wprowadzono zadnego atrybutu, zatem jego wartosc zostaje ustawiona na 0 " + i);
                            tmpWektor.Add(0.0);
                        }

                    }

                    Iris kwiatek = new Iris { Atrybuty = tmpWektor };
                    if (percep.czySetosa(kwiatek))
                    {
                        Console.WriteLine("Gatunek to:  " + IRIS_SETOSA);
                    }
                    else
                    {
                        Console.WriteLine("Gatunek to:  " + NIE_SETOSA);
                    }



                    Console.WriteLine("");
                    Console.WriteLine("Aby wpisac nowy wektor wpisz komende wektor");
                }

                Console.WriteLine("Jesli chcesz sprawdzic dane z pliku iris_test.txt wpisz komende: ");
                Console.WriteLine("test");
                Console.WriteLine(" ");
                Console.WriteLine("Jeśli chcesz podać własny wektor wpisz komende: ");
                Console.WriteLine("wektor");
                Console.WriteLine(" ");
                Console.WriteLine("Aby zakonczyc dzialanie programu wpisz komende ");
                Console.WriteLine("exit");
                Console.WriteLine(" ");


            }
        }


            /*          
            **********************                    **********************    
            **********************                    **********************  
            **********************       METODY       **********************  
            **********************                    **********************              
            **********************                    **********************  
            */

            public static List<Iris> WczytajIrisy(List<string[]> dane)
        {
            List<Iris> tmp = new List<Iris>();

            foreach (var item in dane)
            {
                string tmpNazwa = item[item.Length-1].Replace(" ", null);
                List<double> tmpAtrybuty = new List<double>();
                for(int i=0; i<item.Length-1; i++)
                {
                    tmpAtrybuty.Add(double.Parse(item[i].Replace('.', ','), CultureInfo.CurrentCulture));
                }
                tmp.Add(new Iris { Nazwa = tmpNazwa, Atrybuty = tmpAtrybuty });
            }

            return tmp;
        }

        public static List<string[]> PobierzDane(string Path)
        {
            var lines = File.ReadLines(Path);
            List<String[]> tmpList = new List<string[]>();

            foreach (var line in lines)
            {
                string[] tmpArr = line.Split("\t");
                tmpList.Add(tmpArr);
            }

            return tmpList;
        }
        public static void Shuffle<T>(List<T> list)
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
