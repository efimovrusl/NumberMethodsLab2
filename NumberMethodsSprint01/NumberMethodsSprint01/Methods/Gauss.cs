﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberMethodsSprint01.Methods
{
    public static class Gauss
    {
        public static bool Solve(float[,] M, ref float[] roots)
        {
            // input checks
            if (roots == null || M == null) return false;
            // Matrix checks
            int rowCount = M.GetUpperBound(0) + 1;
            if (M == null || M.Length != rowCount * (rowCount + 1))
                throw new ArgumentException("The algorithm must be provided with a (n x n+1) matrix.");
            if (rowCount < 1)
                throw new ArgumentException("The matrix must at least have one row.");
            // pivoting
            for (int col = 0; col + 1 < rowCount; col++) 
                if (M[col, col] == 0)
                { // check for zero coefficients
                    // find non-zero coefficient
                    int swapRow = col + 1;
                    for (; swapRow < rowCount; swapRow++) 
                        if (M[swapRow, col] != 0) break; // crash

                    if (swapRow < rowCount && M[swapRow, col] != 0)
                    { // found a non-zero coefficient?
                        // yes, then swap it with the above
                        float[] tmp = new float[rowCount + 1];
                        for (int i = 0; i < rowCount + 1; i++)
                        {

                            tmp[i] = M[swapRow, i]; 
                            M[swapRow, i] = M[col, i]; 
                            M[col, i] = tmp[i]; 
                        }
                    }
                    else return false; // no, then the matrix has no unique solution
                }

            // elimination
            for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++)
            {
                for (int destRow = sourceRow + 1; destRow < rowCount; destRow++)
                {
                    float df = M[sourceRow, sourceRow];
                    float sf = M[destRow, sourceRow];
                    for (int i = 0; i < rowCount + 1; i++)
                        M[destRow, i] = M[destRow, i] * df - M[sourceRow, i] * sf;
                }
            }
            // back-insertion
            for (int row = rowCount - 1; row >= 0; row--)
            {
                float f = M[row, row];
                if (f == 0) return false;

                for (int i = 0; i < rowCount + 1; i++) M[row, i] /= f;
                for (int destRow = 0; destRow < row; destRow++)
                { 
                    M[destRow, rowCount] -= M[destRow, row] * M[row, rowCount]; 
                    M[destRow, row] = 0; 
                }
            }
            // roots finding
            for (int i = rowCount - 1; i >= 0; i--)
            {
                float sum = 0;
                for (int j = rowCount - 1; j > i; j--)
                    sum += M[i, j] * roots[i + 1];
                roots[i] = (M[i, rowCount] - sum) / M[i, i];
            }
            return true;
        }

    }
}
