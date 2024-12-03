using System;

class MathUP_Luzin_321
{
    public class TransportationProblem
    {
        private int n;
        private int m;

        private double[,] costs; 

        private double[] supply; 
        private double[] demand;

        private double[] resupply;
        private double[] redemand;

        public TransportationProblem(int n, int m)
        {
            this.n = n;
            this.m = m;
            this.costs = new double[n, m];
            this.supply = new double[n];
            this.demand = new double[m];
        }

        //Вносим данные в матрицу
        public void InputData()
        {
            Console.WriteLine("Введите матрицу стоимости перевозок: \n");
            string inputString = "";
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Введите стоимости для поставщика {i+1}: ");
                inputString = Console.ReadLine();

                string[] numbers = inputString.Split(' '); 
                if (numbers.Length != m)
                {
                    Console.WriteLine("Количество ресурсов не соответствует количеству потребителей");
                    
                    return;
                }
                else
                {
                    for (int j = 0; j < numbers.Length; j++)
                    {
                        if (int.TryParse(numbers[j], out int num)) 
                        {
                            costs[i, j] = num;
                        }
                    }
                }
            }

            Console.WriteLine("\nВведите ресурсы поставщиков: ");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Ресурс поставщика {i + 1}: ");
                supply[i] = double.Parse(Console.ReadLine());
            }

            resupply = supply;

            Console.WriteLine("\nВведите потребности потребителей:");
            for (int j = 0; j < m; j++)
            {
                Console.Write($"Потребность потребителя {j + 1}: ");
                demand[j] = double.Parse(Console.ReadLine());
            }

            redemand = demand;
            PrintMatrix(costs);

        }
        
        //Симпекс метод
        public void SolveSimpleIterativeMethod()
        {
            double[,] transportationPlan = new double[n, m];

            //SolveNorthWestCorner(transportationPlan);


            double totalCost = CalculateTotalCost(transportationPlan);
            Console.WriteLine($"\nНачальный план перевозок (метод северо-западного угла):");
            PrintMatrix(transportationPlan);
            Console.WriteLine($"\nОбщая стоимость: {totalCost}");


            bool improved = true;
            while (improved)
            {
                improved = false;
                double[,] deltaCosts = new double[n, m];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        deltaCosts[i, j] = costs[i, j]; 
                    }
                }


                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (transportationPlan[i, j] > 0) 
                        {
                            //очень упрощенное улучшение плана
                            for (int k = 0; k < n; k++)
                            {
                                for (int l = 0; l < m; l++)
                                {
                                    if (transportationPlan[k, l] == 0 && deltaCosts[k, l] < deltaCosts[i, j])
                                    {
                                        deltaCosts[k, l] = deltaCosts[i, j];
                                    }
                                }
                            }
                        }
                    }
                }

                
                int bestI = -1, bestJ = -1;
                double minDeltaCost = double.MaxValue;

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (transportationPlan[i, j] == 0 && deltaCosts[i, j] < minDeltaCost)
                        {
                            minDeltaCost = deltaCosts[i, j];
                            bestI = i;
                            bestJ = j;
                        }
                    }
                }
                if (bestI != -1)
                {
                    improved = true;

                    
                    //Это очень упрощенный пример и не всегда работает правильно!
                    transportationPlan[bestI, bestJ]++;
                    totalCost += minDeltaCost;
                }

            }

            Console.WriteLine("\nОптимальный план перевозок (упрощенный метод):");
            PrintMatrix(transportationPlan);
            Console.WriteLine($"\nОбщая стоимость: {totalCost}");
        }

        //Минимальный элемет
        public (double minVal, int minRow, int minCol) FindMinElement()
        {
            if (costs == null || costs.GetLength(0) == 0 || costs.GetLength(1) == 0)
            {
                return (double.MaxValue, -1, -1);
            }

            double minVal = costs[0, 0];
            int minRow = 0;
            int minCol = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (costs[i, j] < minVal)
                    {
                        minVal = costs[i, j];
                        minRow = i;
                        minCol = j;
                    }
                }
            }

            return (minVal, minRow, minCol);
        }

        //Метод северо-западного угла
        public void SolveNorthWestCorner()
        {
            double[,] transportationPlan = new double[n, m];
            int i = 0, j = 0;

            while (i < n && j < m)
            {
                double minVal = Math.Min(supply[i], demand[j]);
                transportationPlan[i, j] = minVal;
                supply[i] -= minVal;
                demand[j] -= minVal;

                if (supply[i] == 0) i++;
                if (demand[j] == 0) j++;
            }

            Console.WriteLine("\nПлан перевозок (северо-западный угол):");
            PrintMatrix(transportationPlan);

            double totalCost = CalculateTotalCost(transportationPlan);
            Console.WriteLine($"\nОбщая стоимость перевозок: {totalCost}");
        }

        private double CalculateTotalCost(double[,] plan)
        {
            double totalCost = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    totalCost += plan[i, j] * costs[i, j];
                }
            }
            return totalCost;
        }
        
        //Вывод матрицы
        private void RePrint() 
        {
            demand = redemand;
            supply = resupply;
            PrintMatrix(costs);
        }

        private void PrintMatrix(double[,] matrix)
        {
            Console.WriteLine("\nПолучившаяся матрица: ");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"| {matrix[i, j],8} ");
                }

                Console.WriteLine($"|      {supply[i]}");
            }
            Console.WriteLine();
            for (int j = 0; j < demand.Length; j++)
            {
                Console.Write($"| {demand[j],8} ");
            }
            Console.WriteLine("\n");

        }

        public static void Main(string[] args)
        {
            Console.Write("Введите количество поставщиков (n): ");
            int n = int.Parse(Console.ReadLine());
            Console.Write("Введите количество потребителей (m): ");
            int m = int.Parse(Console.ReadLine());
            Console.WriteLine();

            TransportationProblem problem = new TransportationProblem(n, m);
            problem.InputData();

            bool isEnd = true;
            while (isEnd)
            {
                Console.WriteLine("Выберите вариант рещения матрицы: ");
                Console.WriteLine(" |  1. Метод северо-западного угла");
                Console.WriteLine(" |  2. Метод минмимальных элементов");
                Console.WriteLine(" |  3. Симплекс метод (учитывает не все транспортные задачи)");

                Console.Write(" Введите номер: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        problem.SolveNorthWestCorner();
                        break;
                    case 2:
                        (double minVal, int minRow, int minCol) minElement = problem.FindMinElement();
                        Console.WriteLine($"\nМинимальный элемент: {minElement.minVal} (строка {minElement.minRow + 1}, столбец {minElement.minCol + 1})");
                        break;
                    case 3:
                        problem.SolveSimpleIterativeMethod();
                        break;
                    default:

                        break;
                }

                Console.WriteLine("Посмотреть другие варианты решения?: ");
                Console.WriteLine(" |  1. Да");
                Console.WriteLine(" |  2. Нет");

                Console.Write(" Введите номер: ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        isEnd = true;
                        Console.WriteLine();
                        problem.RePrint();
                        break;
                    case 2:
                        isEnd = false;
                        break;
                }

            }
        }


    }
}