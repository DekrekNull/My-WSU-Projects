// <copyright file="OperatorFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Factory for OperatorNodes.
    /// </summary>
    internal class OperatorFactory : OperatorNode
    {
        /// <summary>
        /// Stores the pairs of valid operators as keys, with the respective OperatorNode subclass
        /// type as the value.
        /// </summary>
        private Dictionary<char, Type> operators = new Dictionary<char, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorFactory"/> class.
        /// </summary>
        internal OperatorFactory()
        {
            this.TraverseAvailableOperators((op, type) => this.operators.Add(op, type));
        }

        /// <summary>
        /// Delegate for the TraverseAvailableOperators method.
        /// </summary>
        /// <param name="op"> Operator value of the OperatorNode as a char. </param>
        /// <param name="type"> Subclass type for the OperatorNode. </param>
        private delegate void OnOperator(char op, Type type);

        /// <summary>
        /// Compares the precedence of two operators to check if they are the same.
        /// </summary>
        /// <param name="operator1"> First operator to compare. </param>
        /// <param name="operator2"> Second operator to compare. </param>
        /// <returns> True if precedence of both operators is equal. </returns>
        public bool IsSamePrecedence(char operator1, char operator2)
        {
            if (this.operators.ContainsKey(operator1) && this.operators.ContainsKey(operator2))
            {
                Type type1 = this.operators[operator1];
                Type type2 = this.operators[operator2];
                PropertyInfo operator1Field = type1.GetProperty("Precedence");
                PropertyInfo operator2Field = type2.GetProperty("Precedence");

                if (operator1Field != null && operator2Field != null)
                {
                    object value1 = operator1Field.GetValue(type1);
                    object value2 = operator2Field.GetValue(type2);

                    if (value1 is int precedence1 && value2 is int precedence2)
                    {
                        return precedence1 == precedence2;
                    }
                }
            }

            throw new Exception(string.Format("No precedence associated with one or both operators: {0}, {1}", operator1, operator2));
        }

        /// <summary>
        /// Determines if the first operator has a higher precedence than the second.
        /// </summary>
        /// <param name="operator1"> First operator to compare. </param>
        /// <param name="operator2"> Second operator to compare. </param>
        /// <returns> True if operator1 has higher precedence,
        /// false if not (including same precedence). </returns>
        public bool IsHigherPrecedence(char operator1, char operator2)
        {
            if (this.operators.ContainsKey(operator1) && this.operators.ContainsKey(operator2))
            {
                Type type1 = this.operators[operator1];
                Type type2 = this.operators[operator2];
                PropertyInfo operator1Field = type1.GetProperty("Precedence");
                PropertyInfo operator2Field = type2.GetProperty("Precedence");

                if (operator1Field != null && operator2Field != null)
                {
                    object value1 = operator1Field.GetValue(type1);
                    object value2 = operator2Field.GetValue(type2);

                    if (value1 is int precedence1 && value2 is int precedence2)
                    {
                        return precedence1 > precedence2;
                    }
                }
            }

            throw new Exception(string.Format("No precedence associated with one or both operators: {0}, {1}", operator1, operator2));
        }

        /// <summary>
        /// Determines whether or not the given operator is left associative.
        /// </summary>
        /// <param name="op"> Operator to determine associativity. </param>
        /// <returns> True if the operator is left associative, otherwise false. </returns>
        public bool IsLeftAssociative(char op)
        {
            if (this.operators.ContainsKey(op))
            {
                Type type = this.operators[op];
                PropertyInfo operatorField = type.GetProperty("Precedence");

                if (operatorField != null)
                {
                    object value = operatorField.GetValue(type);

                    if (value is string associativity)
                    {
                        return associativity == "left";
                    }
                }
            }

            throw new Exception("No associativity");
        }

        /// <summary>
        /// Determines if the given character is a supported operator.
        /// </summary>
        /// <param name="op"> Potential operator. </param>
        /// <returns> True if op is an operator, otherwise false. </returns>
        public bool IsOperator(char op)
        {
            return this.operators.ContainsKey(op);
        }

        /// <summary>
        /// Creates and returns the appropriate OperatorNode from the given operator.
        /// </summary>
        /// <param name="op"> Operator for the node to be made. </param>
        /// <returns> OperatorNode of the given operator type. </returns>
        public OperatorNode CreateOperatorNode(char op)
        {
            if (this.operators.ContainsKey(op))
            {
                object operatorNodeObject = System.Activator.CreateInstance(this.operators[op]);
                if (operatorNodeObject is OperatorNode)
                {
                    return (OperatorNode)operatorNodeObject;
                }
            }

            throw new Exception("Unhandled operator");
        }

        /// <summary>
        /// Looks through the project and finds all classes that inherit from OperatorNode
        /// and adds them to the operator dictionary.
        /// </summary>
        /// <param name="onOperator"> Delegate for the (op, type) pair. </param>
        private void TraverseAvailableOperators(OnOperator onOperator)
        {
            // get the type declaration of OperatorNode
            Type operatorNodeType = typeof(OperatorNode);

            // Iterate over all loaded assemblies:
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Get all types that inherit from our OperatorNode class using LINQ
                IEnumerable<Type> operatorTypes =
                    assembly.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));

                // Iterate over those subclasses of OperatorNode
                foreach (var type in operatorTypes)
                {
                    // for each subclass, retrieve the Operator property
                    PropertyInfo operatorField = type.GetProperty("Operator");
                    if (operatorField != null)
                    {
                        // Get the character of the Operator
                        object value = operatorField.GetValue(type);

                        if (value is char operatorSymbol)
                        {
                            // And invoke the function passed as parameter
                            // with the operator symbol and the operator class
                            onOperator(operatorSymbol, type);
                        }
                    }
                }
            }
        }
    }
}
