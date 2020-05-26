using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Class1
    {
        double[,] points;
        const int cluster = 2; // the number of cluster
        const int dimension = 2; // the number of dimention
        int data; // the number of data
        double[,] center = null; // current middle value
        double[,] old_center = null; // historical middle value
        double[,] dis = null; // distance
        int[,] result = null; // result : (0, 1) -> membership degree

        int[] c_number = null; // the number of data in cluster
        Boolean loop = false; // repetitive

        Random rand = new Random();

        // 이쿠죠!
        public void Start()
        {
            Init();
        }

        // 초기화
        private void Init()
        {
            // To sets points
            points = new double[7, dimension] { { 1, 1 }, { 6, 6 },
                                          { 7, 7 }, { 3, 2 },
                                          { 6, 8 }, { 2, 1 }, { 2, 3 } };

            // Initialize
            data = points.Length / dimension;
            center = new double[cluster, data];
            old_center = new double[cluster, data];
            dis = new double[cluster, data];
            result = new int[cluster, data];

            c_number = new int[cluster];

            Run();
        }

        private void Run()
        {
            // 1. 소속도 랜덤으로 결정
            for (int d = 0; d < data; d++)
            {
                for (int c = 0; c < cluster; c++)
                {
                    result[c, d] = 0; // 소속도 0으로 초기화
                }

                result[rand.Next(0, cluster), d] = 1; // 랜덤으로 1로 초기화 
            }

            Print();
            Check(); // 클러스터에 하나도 들어가지 않는 것을 검사

            // 중심값 계산
            Center_cal();
            // 거리 계산
            Dis_cal();
            // 확인
            Print();

            //거리 비교 -> 소속도 변경
            Dis_comp();
            Print();

            // 중심값 옮기기(저장)
            Center_move();
            // 중심값 계산하기
            Center_cal();
            Print();

            //반복
            while (!loop)
            {
                // 비교하기
                if (Center_com()) break;

                Console.WriteLine("\n********** 반복!! **********");
                // 중심값-데이터간의 거리 계산
                Dis_cal();
                Console.WriteLine("거리 계산");
                Print();
                // 거리 구하기(소속도 정하기)
                Dis_comp();
                Console.WriteLine("소속도 정하기");
                Print();

                // 중심값 옮기기(저장)
                Center_move();
                // 중심값 계산하기
                Center_cal();
                Console.WriteLine("중심값 계산하기");
                Print();
            }
        }

        private void Check()
        {
            for (int c = 0; c < cluster; c++)
            {
                c_number[c] = 0;
                for (int d = 0; d < data; d++)
                {
                    if (result[c, d] == 1) c_number[c]++;
                }
                if (c_number[c] == 0)
                {
                    Run();
                }
            }
        }

        // 중심값 구하기
        private void Center_cal()
        {
            double x_sum, y_sum;
            int count;
            for (int c = 0; c < cluster; c++)
            {
                x_sum = 0; y_sum = 0;
                count = 0;
                for (int d = 0; d < data; d++)
                {
                    if (result[c, d] == 1)
                    {
                        x_sum += points[d, 0];
                        y_sum += points[d, 1];
                        count++;
                    }
                }

                if (x_sum != 0 || y_sum != 0)
                {
                    center[c, 0] = (double)x_sum / count;
                    center[c, 1] = (double)y_sum / count;
                }
            }
        }

        // 중심-데이터간의 거리구하기
        private void Dis_cal()
        {
            for (int c = 0; c < cluster; c++)
            {
                for (int d = 0; d < data; d++)
                {
                    dis[c, d] = Math.Sqrt(Math.Pow(points[d, 0] - center[c, 0], 2) + Math.Pow(points[d, 1] - center[c, 1], 2));
                }
            }
        }

        //확인
        private void Print()
        {
            Console.WriteLine("중심값");
            for (int c = 0; c < cluster; c++)
            {
                for (int s = 0; s < dimension; s++)
                {
                    Console.Write(center[c, s] + " ");
                }
                Console.WriteLine();
            }

            // 소속도 확인
            Console.Write("소속도 확인 \n");
            for (int c = 0; c < cluster; c++)
            {
                for (int d = 0; d < data; d++)
                {
                    Console.Write(result[c, d]);
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n거리 확인");
            for (int c = 0; c < cluster; c++)
            {
                for (int d = 0; d < data; d++)
                {
                    Console.Write(dis[c, d] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // 거리 비교 및 소속도 변경
        private void Dis_comp()
        {
            for (int d = 0; d < data; d++)
            {
                double min_value = Int32.MaxValue;
                int min_index = 0;
                for (int c = 0; c < cluster; c++)
                {
                    result[c, d] = 0;
                    if (dis[c, d] < min_value)
                    {
                        min_value = dis[c, d];
                        min_index = c;
                    }
                }

                result[min_index, d] = 1;
            }
        }

        // 현재의 중심값을 과거의 중심값으로 옮기기
        private void Center_move()
        {
            for (int c = 0; c < cluster; c++)
            {
                for (int s = 0; s < dimension; s++)
                {
                    old_center[c, s] = center[c, s];
                }
            }
        }

        // 중심값 비교
        private Boolean Center_com()
        {
            for (int c = 0; c < cluster; c++)
            {
                for (int s = 0; s < dimension; s++)
                {
                    if (old_center[c, s] != center[c, s]) return false;
                }
            }

            return true;
        }
    }
}
