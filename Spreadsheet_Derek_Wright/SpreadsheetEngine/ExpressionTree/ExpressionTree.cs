// <copyright file="ExpressionTree.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
///  Derek Wright 11766151
/// </summary>
namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SpreadsheetEngine;

    /// <summary>
    /// Expression tree object for the supported expressions of the Spreadsheet cells.
    /// </summary>
    public class ExpressionTree
    {
        /// <summary>
        /// Starting node of the ExpressionTree.
        /// </summary>
        private readonly Node root;

        /// <summary>
        /// Instance of operatorFactory to create OperatorNodes and access methods.
        /// </summary>
        private OperatorFactory operatorFactory;

        /// <summary>
        /// Stores all of the existing variables of the current Expression.
        /// </summary>
        private Dictionary<string, double> variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class using an expression string.
        /// </summary>
        /// <param name="expression"> an expression in string form. </param>
        public ExpressionTree(string expression)
        {
            this.variables = new Dictionary<string, double>();
            this.operatorFactory = new OperatorFactory();

            if (!this.ContainsOperator(expression))
            {
                // Before attemting to construct the tree, confirm that at least one operator exists
                throw new ArgumentException(
                    "The expression \"" + expression + "\" does not contain a valid operator.");
            }

            try
            {
                this.root = this.ParseExpression(expression);
            }
            catch (Exception e) when (e is NotSupportedException || e is ArgumentException)
            {
                throw e;
            }
        }

        /// <summary>
        /// Sets the specified variable within the ExpressionTree variables dictionary and all
        /// corresponding VariableNodes in the ExpressionTree.
        /// </summary>
        /// <param name="variableName"> Name of the variable to set in variables dictionary. </param>
        /// <param name="variableValue"> Value to set the variable to in the variables dictionary. </param>
        public void SetVariable(string variableName, double variableValue)
        {
            if (this.variables.ContainsKey(variableName))
            {
                this.variables[variableName] = variableValue;
                UpdateVariableNode(this.root, variableName, variableValue);
            }
            else
            {
                throw new ArgumentException(
                    "The variable \"" + variableName + "\" does not exist in the current exrpession.");
            }
        }

        /// <summary>
        /// Evaluates the ExpressionTree and returns the result as a double.
        /// </summary>
        /// <returns> The double value of the evaluated expression tree. </returns>
        public double Evaluate()
        {
            if (this.root == null)
            {
                throw new NullReferenceException(
                    "Can't evaluate a tree when root is Null.");
            }

            try
            {
                return this.root.Evaluate();
            }
            catch (ArgumentNullException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets the variables that are present in the expression.
        /// </summary>
        /// <returns> String array of all variable names in the expression. </returns>
        public string[] GetVariables()
        {
            return this.variables.Keys.ToArray();
        }

        /// <summary>
        /// Takes a variable name and finds all VariableNodes in the ExpressionTree
        /// and updates them to the new value.
        /// </summary>
        /// <param name="current"> Current node to be checked and updated if it matches the given variable. </param>
        /// <param name="name"> Name of the variable to update. </param>
        /// <param name="value"> Value for the variable to be updated to. </param>
        private static void UpdateVariableNode(Node current, string name, double value)
        {
            if (current is VariableNode variableNode)
            {
                // if current node is a variable node, just check and update value
                if (variableNode.Name == name)
                {
                    // if variable name matches, update value
                    variableNode.UpdateVariable(value);
                }
            }
            else if (current is OperatorNode operatorNode)
            {
                // if current node is an operator, check both children nodes
                UpdateVariableNode(operatorNode.Left, name, value);
                UpdateVariableNode(operatorNode.Right, name, value);
            }
        }

        /// <summary>
        /// Determines whether or not the expression contains a valid operator.
        /// </summary>
        /// <param name="expression"> Expression to check for operator. </param>
        /// <returns> True if operator is found in expression, false otherwise. </returns>
        private bool ContainsOperator(string expression)
        {
            foreach (char c in expression)
            {
                if (this.operatorFactory.IsOperator(c))
                {
                    // At least one operator found in the expression string
                    return true;
                }
            }

            // No operator found in expression string
            return false;
        }

        /// <summary>
        /// Takes an expression in infix order and changes it to postfix order
        /// using Dijkstra's Shunting Yard Algorithm.
        /// </summary>
        /// <param name="expression"> Expression to be changed. </param>
        /// <returns> The postfix order of the given expression. </returns>
        private List<Node> InfixToPostfix(string expression)
        {
            Stack<char> stack = new Stack<char>();
            List<Node> postfix = new List<Node>();
            char popped;
            string operand = string.Empty;

            foreach (char c in expression)
            {
                if (!this.operatorFactory.IsOperator(c))
                {
                    if (c == '(')
                    {
                        stack.Push(c);
                    }
                    else if (c == ')')
                    {
                        if (operand.Length > 0)
                        {
                            postfix.Add(OperandFactory.CreateOperandNode(operand));
                            if (char.IsLetter(operand[0]) && !this.variables.ContainsKey(operand))
                            {
                                this.variables.Add(operand, 0);
                            }

                            operand = string.Empty;
                        }

                        // if stack is empty and a ')' is read, parenthesis are imbalanced
                        if (stack.Count == 0)
                        {
                            throw new ArgumentException(
                                    "The given expression \"" + expression + "\" has more \')' than '(' and is therefore invalid.");
                        }

                        while (true)
                        {
                            popped = stack.Pop();
                            if (stack.Count == 0 && popped != '(')
                            {
                                // if a '(' is not found in stack, parenthesis are imbalanced
                                throw new ArgumentException(
                                    "The given expression \"" + expression + "\" has more \')' than '(' and is therefore invalid.");
                            }
                            else if (popped == '(')
                            {
                                // Closing parenthesis found, can continue reading expression
                                break;
                            }

                            postfix.Add(this.operatorFactory.CreateOperatorNode(popped));
                        }
                    }
                    else
                    {
                        // current char of expression is an operand
                        operand += c;
                    }
                }
                else
                {
                    if (operand.Length > 0)
                    {
                        postfix.Add(OperandFactory.CreateOperandNode(operand));
                        if (char.IsLetter(operand[0]) && !this.variables.ContainsKey(operand))
                        {
                            this.variables.Add(operand, 0);
                        }

                        operand = string.Empty;
                    }

                    // operator found
                    while ((stack.Count != 0 && stack.Peek() != '(') && (!this.operatorFactory.IsHigherPrecedence(c, stack.Peek())
                        || (this.operatorFactory.IsSamePrecedence(c, stack.Peek()) && this.operatorFactory.IsLeftAssociative(c))))
                    {
                        // keeps poping stack until the top operator is lower precedence or same precedence and leftAssociative
                        postfix.Add(this.operatorFactory.CreateOperatorNode(stack.Pop()));
                    }

                    // Once the above is done, all other cases push operator to stack
                    stack.Push(c);
                }
            }

            if (operand.Length > 0)
            {
                postfix.Add(OperandFactory.CreateOperandNode(operand));
                if (char.IsLetter(operand[0]) && !this.variables.ContainsKey(operand))
                {
                    this.variables.Add(operand, 0);
                }

                operand = string.Empty;
            }

            // Once expression is fully traversed, check remaining stack
            while (stack.Count != 0)
            {
                popped = stack.Pop();
                if (popped == '(')
                {
                    // If any parenthesis remain, there is a parenthesis imbalance
                    throw new ArgumentException(
                        "The given expression \"" + expression + "\" has more \'(' than ')' and is therefore invalid.");
                }

                // add all remaining operators to end of the string
                postfix.Add(this.operatorFactory.CreateOperatorNode(popped));
            }

            return postfix;
        }

        /// <summary>
        /// Parses the expression into an ExpressionTree from an expression
        /// that is in infix order initially.
        /// </summary>
        /// <param name="expression"> Expression to be made into and ExpressionTree. </param>
        /// <returns> Root node of the built ExpressionTree. </returns>
        private Node ParseExpression(string expression)
        {
            List<Node> postfixList;
            Stack<Node> stack = new Stack<Node>();

            // Convert infix expression to postfix expression
            try
            {
                postfixList = this.InfixToPostfix(expression);
            }
            catch (ArgumentException e)
            {
                throw e;
            }

            foreach (Node symbol in postfixList)
            {
                if (symbol is OperatorNode operatorNode)
                {
                    // if the current symbol is an operator
                    operatorNode.Right = stack.Pop();
                    operatorNode.Left = stack.Pop();
                    stack.Push(operatorNode);
                }
                else
                {
                    // if not operator, it must be operand node so push to stack
                    stack.Push(symbol);
                }
            }

            // Final node on stack should be the root to the expression tree
            return stack.Pop();
        }
    }
}
