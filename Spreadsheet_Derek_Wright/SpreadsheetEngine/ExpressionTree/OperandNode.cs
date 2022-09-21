// <copyright file="OperandNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine
{
    /// <summary>
    /// Parent class for Nodes that store an Operand.
    /// </summary>
    internal abstract class OperandNode : Node
    {
        /// <summary>
        /// Value for the operand.
        /// </summary>
        private double value;

        /// <summary>
        /// Gets or sets the value of the operand.
        /// </summary>
        protected double Value { get => this.value; set => this.value = value; }

        /// <summary>
        /// Returns the value of the OperandNode as its evaluation.
        /// </summary>
        /// <returns> Value of the OperandNode. </returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
