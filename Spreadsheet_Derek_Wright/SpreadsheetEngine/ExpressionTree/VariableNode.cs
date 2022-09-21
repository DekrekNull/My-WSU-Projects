// <copyright file="VariableNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine
{
     /// <summary>
    /// Inherits the Node class and adds an attribute for a variable name to be stored.
    /// </summary>
    internal class VariableNode : OperandNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="name"> Name of the variable for this VariableNode. </param>
        public VariableNode(string name)
        {
            if (CheckVariableFormat(name))
            {
                // All variables initialize with a value of 0
                this.Value = double.NaN;
                this.Name = name;
            }
            else
            {
                // Variable is of an incorrect format
                throw new System.NotSupportedException(
                    "The variable \"" + name + "\" is of the wrong format");
            }
        }

        /// <summary>
        /// Gets or sets the name of the variable this Node represents.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sets the value of the private value member.
        /// </summary>
        /// <param name="newValue"> New value to update to. </param>
        public void UpdateVariable(double newValue) => this.Value = newValue;

        /// <summary>
        /// Overrides the Evaluate method to add Exception handling specific to VariableNodes.
        /// </summary>
        /// <returns> Evaluation of the VariableNode as a double. </returns>
        public override double Evaluate()
        {
            if (double.IsNaN(this.Value))
            {
                throw new System.ArgumentException(
                    "The variable \"" + this.Name + "\" is not instantiated.");
            }

            return base.Evaluate();
        }

        /// <summary>
        /// Takes a variable name and verifies that it is of the proper format.
        /// </summary>
        /// <param name="name"> Variable name. </param>
        /// <returns> True if variable name is a valid format, otherwise false. </returns>
        private static bool CheckVariableFormat(string name)
        {
            // First character must be a letter.
            if (char.IsLetter(name[0]))
            {
                // all successive characters must a letter or digit.
                for (int i = 0; i < name.Length; i++)
                {
                    if (!char.IsLetter(name[i]) && !char.IsDigit(name[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
