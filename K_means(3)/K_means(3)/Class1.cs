using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_means_3_
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
        //Boolean loop = false; // repetitive

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
            points = new double[,] {{119,168}, {131,168},{129,161},{127,168},{127,172},{128,172},{133,172},{138,183},{140,169},{139,159},{137,159},{150,157},{157,162},{152,173},
{148,179},{143,177},{144,168},{158,159},{160,154},{168,154},{180,161},{180,175},{167,181},{152,181},{161,165},{182,158}};

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
            // 1. 중심값 랜덤으로 결정
            Random rand = new Random();

            int[] n = new int[cluster]; // 각 클러스터의 임시 중심값(데이터의 인덱스를 저장)
            for (int c = 0; c < cluster; c++)
            {
                Boolean check = true; // 중복 확인용
                while (check) {
                    n[c] = rand.Next(1, data);
                    check = false;
                    for (int i = 0; i < c; i++)
                    {
                        if (n[i] == n[c]) check = true; // 랜덤수 중복 확인
                    }
                }
                
                for(int s = 0; s < dimension; s++)
                {
                    result[c, n[c]] = 1; // 저장
                }
            }

            // 소속도 모두 채워주기
            Console.WriteLine("\n********** 반복!! **********");
            Center_cal();
            Console.WriteLine("거리 계산");
            Dis_cal();
            Print();

            Console.WriteLine("소속도 정하기");
            Dis_comp();
            Print();

            Console.WriteLine("중심값 계산하기");
            Center_move();
            Center_cal();
            Print();

            do
            {
                Console.WriteLine("\n********** 반복!! **********");
                Center_cal();
                Console.WriteLine("거리 계산");
                Dis_cal();
                Print();

                Console.WriteLine("소속도 정하기");
                Dis_comp();
                Print();

                Console.WriteLine("중심값 계산하기");
                Center_move();
                Center_cal();
                Print();

                if (Center_com()) break;
            } while (true);

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
