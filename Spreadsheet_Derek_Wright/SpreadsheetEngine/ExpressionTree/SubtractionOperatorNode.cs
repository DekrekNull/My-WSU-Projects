// <copyright file="SubtractionOperatorNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine
{
    /// <summary>
    /// Inherits the OperatorNode class and specifically represents
    /// the addition operator for the expression tree.
    /// </summary>
    internal class SubtractionOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractionOperatorNode"/> class.
        /// </summary>
        public SubtractionOperatorNode()
            : base()
        {
        }

        /// <summary>
        /// Gets the operator value of SubtractionNode.
        /// </summary>
        public static char Operator { get; } = '-';

        /// <summary>
        /// Gets the associativity of this Operator, either left or right.
        /// </summary>
        public static new string Associativity { get; } = "left";

        /// <summary>
        /// Gets the precedence of this Operator as an integer, from 1 to 15
        /// based on the operator precedence of all C# operators.
        /// </summary>
        public static new int Precedence { get; } = 12;

        /// <summary>
        /// Overrides the Evaluate() method and subtracts the right child from the
        /// left child of this node and returns the result.
        /// </summary>
        /// <returns> Difference of the two child nodes. </returns>
        public override double Evaluate()
        {
            try
            {
                return this.Left.Evaluate() - this.Right.Evaluate();
            }
            catch (System.ArgumentException e)
            {
                throw e;
            }
        }
    }
}
