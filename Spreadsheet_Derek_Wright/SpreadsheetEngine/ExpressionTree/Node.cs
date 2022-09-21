// <copyright file="Node.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine
{
    /// <summary>
    /// Abstract base class for the different Node types for the ExpressionTree class.
    /// </summary>
    internal abstract class Node
    {
        /// <summary>
        /// Evaluates the value of the Node.
        /// </summary>
        /// <returns> The calculated value of the Node. </returns>
        public abstract double Evaluate();
    }
}
