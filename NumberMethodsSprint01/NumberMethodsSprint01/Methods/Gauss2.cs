﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberMethodsSprint01.Methods
{
    public static class GaussJordanElimination
    {
        private static int RowCount { get; set; }

        /// <summary>
        ///  Method to find a linear equation system using gaussian elimination.
        /// </summary>
        /// <param name="matrix">The key matrix to solve via algorithm.</param>
        /// <returns>whether the input matrix has a unique solution or not.
        /// and solves on the given matrix. </returns>
        public static bool Solve(float[,] matrix, ref float[] roots)
        {
            RowCount = matrix.GetUpperBound(0) + 1;

            if (!CanMatrixBeUsed(matrix))
            {
                throw new ArgumentException("Please use a n*(n+1) matrix with Length > 0.");
            }

            var pivot = PivotMatrix(ref matrix);
            if (!pivot)
            {
                return false;
            }

            Elimination(ref matrix);

            bool result = ElementaryReduction(ref matrix);
            if (result)
            {
                for (int i = RowCount - 1; i >= 0; i--)
                {
                    float sum = 0;
                    for (int j = RowCount - 1; j > i; j--)
                        sum += matrix[i, j] * roots[i + 1];
                    roots[i] = (matrix[i, RowCount] - sum) / matrix[i, i];
                }
            }
            return result;
        }

        /// <summary>
        /// To make simple validation of the matrix to be used.
        /// </summary>
        /// 
        /// <param 
        /// name="matrix">Multidimensional array matrix.
        /// </param>
        private static bool CanMatrixBeUsed(float[,] matrix) => 
            matrix?.Length == RowCount * (RowCount + 1) && RowCount > 1;

        /// <summary>
        /// To prepare given matrix by pivoting rows.
        /// </summary>
        /// <param 
        /// name="matrix">Input matrix.
        /// </param>
        /// 
        /// <returns>Matrix.</returns>
        private static bool PivotMatrix(ref float[,] matrix)
        {
            for (var col = 0; col + 1 < RowCount; col++)
            {
                if (matrix[col, col] == 0)
                {
                    // To find a non-zero coefficient
                    var rowToSwap = FindNonZeroCoefficient(ref matrix, col);

                    if (matrix[rowToSwap, col] != 0)
                    {
                        var tmp = new float[RowCount + 1];
                        for (var i = 0; i < RowCount + 1; i++)
                        {
                            // To make the swap with the element above.
                            tmp[i] = matrix[rowToSwap, i];
                            matrix[rowToSwap, i] = matrix[col, i];
                            matrix[col, i] = tmp[i];
                        }
                    }
                    else
                    {
                        // To return that the matrix doesn't have a unique solution.
                        return false;
                    }
                }
            }

            return true;
        }

        private static int FindNonZeroCoefficient(ref float[,] matrix, int col)
        {
            var rowToSwap = col + 1;

            // To find a non-zero coefficient
            for (; rowToSwap < RowCount; rowToSwap++)
            {
                if (matrix[rowToSwap, col] != 0)
                {
                    return rowToSwap;
                }
            }

            return col + 1;
        }

        /// <summary>
        /// Applies REF.
        /// </summary>
        /// <param name="matrix">Input matrix.</param>
        private static void Elimination(ref float[,] matrix)
        {
            for (var srcRow = 0; srcRow + 1 < RowCount; srcRow++)
            {
                for (var destRow = srcRow + 1; destRow < RowCount; destRow++)
                {
                    var df = matrix[srcRow, srcRow];
                    var sf = matrix[destRow, srcRow];

                    for (var i = 0; i < RowCount + 1; i++)
                    {
                        matrix[destRow, i] = matrix[destRow, i] * df - matrix[srcRow, i] * sf;
                    }
                }
            }
        }

        /// <summary>
        /// To continue reducing the matrix using RREF.
        /// </summary>
        /// <param name="matrix">Input matrix.</param>
        /// <returns>True if it has a unique solution; false otherwise.</returns>
        private static bool ElementaryReduction(ref float[,] matrix)
        {
            for (var row = RowCount - 1; row >= 0; row--)
            {
                var element = matrix[row, row];
                if (element == 0)
                {
                    return false;
                }

                for (var i = 0; i < RowCount + 1; i++)
                {
                    matrix[row, i] /= element;
                }

                for (var destRow = 0; destRow < row; destRow++)
                {
                    matrix[destRow, RowCount] -= matrix[destRow, row] * matrix[row, RowCount];
                    matrix[destRow, row] = 0;
                }
            }

            return true;
        }
    }
}