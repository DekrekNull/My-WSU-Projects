// <copyright file="ExpressionTreeDemo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace CptS321
{
    using System;

    /// <summary>
    /// Class that runs a demo of the ExpressionTree class to the console.
    /// </summary>
    public class ExpressionTreeDemo
    {
        /// <summary>
        /// Current expression 
        /// </summary>
        private static string expression;

        /// <summary>
        /// Current expression tree
        /// </summary>
        private static ExpressionTree tree;

        /// <summary>
        /// Main entry point for the console program.
        /// </summary>
        /// <param name="args"> Arguments for the program. </param>
        public static void Main(string[] args)
        {
            expression = "A1-B1-C1";
            tree = new ExpressionTree(expression);
            RunDemo();
        }

        /// <summary>
        /// Prints the menu of choices to the console.
        /// </summary>
        public static void PrintMenu()
        {
            Console.WriteLine("Menu (current expression=\"" + expression + "\")");
            Console.WriteLine("  1 = Enter a new expression");
            Console.WriteLine("  2 = Set a variable value");
            Console.WriteLine("  3 = Evaluate tree");
            Console.WriteLine("  4 = Quit");
        }

        /// <summary>
        /// Runs the demo for the user to try the different features of the ExpressionTree class.
        /// </summary>
        public static void RunDemo()
        {
            bool cont = true;
            while (cont)
            {
                cont = RunTask();
            }
        }

        /// <summary>
        /// Prints menu and prompts user for a choice, then runs the task for their choice.
        /// </summary>
        /// <returns> True unless user chooses quit, then returns false instead. </returns>
        public static bool RunTask()
        {
            int choice = 0;
            PrintMenu();
            try
            {
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            switch (choice)
            {
                case 1:
                    ChangeExpression();
                    break;
                case 2:
                    ChangeVariable();
                    break;
                case 3:
                    EvaluateTree();
                    break;
                case 4:
                    return false;
                default:
                    break;
            }

            return true;
        }

        /// <summary>
        /// Prompts the user to enter a new expression,
        /// if valid expression is given create a new expression tree for it.
        /// </summary>
        private static void ChangeExpression()
        {
            ExpressionTree newTree;
            Console.Write("Enter new expression: ");
            string newExpression = Console.ReadLine();
            try
            {
                newTree = new ExpressionTree(newExpression);
                expression = newExpression;
                tree = newTree;
            }
            catch (Exception e) when (e is NotSupportedException || e is ArgumentException)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        /// <summary>
        /// Prompts the user for a variable to update.
        /// </summary>
        private static void ChangeVariable()
        {
            Console.Write("Enter variable name: ");
            string variableName = Console.ReadLine();

            // Check that input can be parsed into double, because a double must be passed.
            Console.Write("Enter variable value: ");
            if (double.TryParse(Console.ReadLine(), out double variableValue))
            {
                try
                {
                    tree.SetVariable(variableName, variableValue);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Variable needs to a number value.");
            }
        }

        /// <summary>
        /// Evaluates the current ExpressionTree and prints it to the user.
        /// </summary>
        private static void EvaluateTree()
        {
            Console.WriteLine(string.Format("Tree value: {0}", tree.Evaluate()));
        }
    }
}
