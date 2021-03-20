using System;
using System.Linq;

namespace tsv
{
    class Program
    {
        public static Random random = new Random();
        static void Main(string[] args)
        {
            int N = 200;
            int M = (int)Math.Floor(Math.Log(N));
            int N_M = (int)Math.Ceiling((decimal)N / M);

            int[][] A = GeneratMatrix(N, N);
            int[][] B = GeneratMatrix(N, N);

            int[][] result = FourRussians(A, B, N_M);
        }

        public static int[][] FourRussians(int[][] A, int[][] B, int N_M)
        {
            int n = A.Length / N_M;
            int[][][] Cmatrices = new int[N_M][][];

            for (int i = 0; i < N_M; i++)
            {
                Cmatrices[i] = new int[A.Length][];

                int[][] newA = MatrixSplit(A, 0, A.Length, i * n, (i + 1) * n);
                int[][] newB = NewBMatrix(MatrixSplit(B, i * n, (i + 1) * n, 0, B.Length), n);

                for (int j = 0; j < A.Length; j++)
                    Cmatrices[i][j] = newB[newA[j].Sum()];
            }

            int[][] C = GeneratMatrix(A.Length, A[0].Length, true);
            for (int i = 0; i < N_M; i++)
                for (int x = 0; x < C.Length; x++)
                    for (int j = 0; j < C[0].Length; j++)
                        C[x][j] = Math.Max(C[x][j], Cmatrices[i][x][j]);
            return C;
        }

        public static int[][] GeneratMatrix(int a, int b, bool nulls = false)
        {
            int[][] arr = new int[a][];

            for (int i = 0; i < a; i++)
            {
                arr[i] = new int[b];
                if (nulls == false)
                    for (int j = 0; j < b; j++)
                        arr[i][j] = random.Next(2);
            }
            return arr;
        }

        public static int[][] MatrixSplit(int[][] arr, int i_start, int i_end, int j_start, int j_end)
        {
            int[][] new_matrix = new int[i_end - i_start][];

            for (int i = 0; i < new_matrix.Length; i++)
            {
                new_matrix[i] = new int[j_end - j_start];
                for (int j = 0; j < j_end - j_start; ++j)
                    new_matrix[i][j] = arr[i + i_start][j + j_start];
            }
            return new_matrix;
        }

        public static int[][] NewBMatrix(int[][] B, int N_M)
        {
            int size = (int)Math.Pow(2, N_M);

            int[][] newB = GeneratMatrix(size, B[0].Length, true);
            string[] newBstr = new string[size];
            newBstr[0] = new string('0', size);

            for (int i = 1; i < Math.Pow(2, N_M); i++)
            {
                newBstr[i] = Convert.ToString(i, 2);
                while (newBstr[i].Length < N_M)
                    newBstr[i] = '0' + newBstr[i];

                int x = 0;
                int y = 0;
                string newBstrPrev = newBstr[i - 1];

                for (int j = 0; j < newBstr[i].Length; j++)
                {
                    if (newBstr[i][newBstr[i].Length - 1 - j] > newBstrPrev[newBstr[i].Length - 1 - j])
                    {
                        x = (int)Math.Pow(2, newBstr[i].Length - ++j);
                        y = 0;
                        while (j < newBstr[i].Length)
                        {
                            int i_len = newBstr[i].Length - ++j;
                            y += (int)(Math.Pow(2, i_len) * Convert.ToInt32(newBstr[i].Substring(i_len, 1)));
                        }
                        break;
                    }
                }

                if (y == 0)
                    newB[i] = B[(int)Math.Log2(x)];
                else
                    for (int z = 0; z < newB[x].Length; z++)
                        newB[i][z] = Math.Max(newB[x][z], newB[y][z]);
            }
            return newB;
        }
    }
}
