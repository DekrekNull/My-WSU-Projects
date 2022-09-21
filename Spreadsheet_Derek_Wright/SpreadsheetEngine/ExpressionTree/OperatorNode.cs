// <copyright file="OperatorNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine
{
    using System;

    /// <summary>
    /// Inherits the Node class and add attributes for children and an operator value to be stored in.
    /// </summary>
    internal abstract class OperatorNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNode"/> class with a given operator value.
        /// </summary>
        public OperatorNode()
        {
            this.Left = this.Right = null;
        }

        /// <summary>
        /// Gets the associativity of this Operator, either left or right.
        /// </summary>
        public static string Associativity { get; }

        /// <summary>
        /// Gets the precedence of this Operator as an integer, from 1 to 15
        /// based on the operator precedence of all C# operators.
        /// </summary>
        public static int Precedence { get; }

        /// <summary>
        /// Gets or sets the left child node of this OperatorNode instance.
        /// </summary>
        internal Node Left { get; set; }

        /// <summary>
        /// Gets or sets the right child node of this OperatorNode instance.
        /// </summary>
        internal Node Right { get; set; }

        /// <summary>
        /// Calculates the value of the sub-tree of this OperatorNode and returns it.
        /// </summary>
        /// <returns> The calculated value of this node. </returns>
        public override double Evaluate()
        {
            throw new NotImplementedException();
        }
    }
}
