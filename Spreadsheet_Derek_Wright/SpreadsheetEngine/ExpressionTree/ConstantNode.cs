// <copyright file="ConstantNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine
{
    /// <summary>
    /// Inherits the Node class and add an attribute for a value to be stored in.
    /// </summary>
    internal class ConstantNode : OperandNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// </summary>
        /// <param name="value"> Value for the ConstantNode. </param>
        public ConstantNode(double value) => this.Value = value;
    }
}
