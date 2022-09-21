// <copyright file="OperandFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine
{
    using System;

    /// <summary>
    /// Factory class in charge of creating OperandNodes.
    /// </summary>
    internal abstract class OperandFactory : OperandNode
    {
        /// <summary>
        /// Factory method for creating the OperandNodes.
        /// </summary>
        /// <param name="value"> Value for the OperandNode that will be created. </param>
        /// <returns> OperandNode made from the given value. </returns>
        public static OperandNode CreateOperandNode(string value)
        {
            if (double.TryParse(value, out double number))
            {
                // If a number, make a ConstantNode
                return new ConstantNode(number);
            }
            else
            {
                // Otherwise try to make a VariableNode
                try
                {
                    return new VariableNode(value);
                }
                catch (NotSupportedException e)
                {
                    // The expression is not a constant or a valid variable name
                    throw e;
                }
            }
        }
    }
}
