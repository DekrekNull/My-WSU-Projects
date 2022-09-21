// <copyright file="SpreadsheetEngineTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace ExpressionTreeTest
{
    using CptS321;
    using NUnit.Framework;

    /// <summary>
    /// This class tests the functionality of the ExpressionTree class to ensure that it is working as intended.
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// ExpressionTree for the test cases.
        /// </summary>
        private ExpressionTree testTree;
        
        /// <summary>
        /// Performs necessary setup for tests.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Tests that the tree evaluate functions as expected.
        /// </summary>
        [Test]
        public void TestEvaluate1()
        {
            this.testTree = new ExpressionTree("8-1");
            Assert.AreEqual(this.testTree.Evaluate(), 7.0, "ExpressionTree.Evaluate() is not working.");
        }

        /// <summary>
        /// Tests that the tree evaluate method throws an exception when a variable is not set.
        /// </summary>
        [Test]
        public void TestEvaluate2()
        {
            this.testTree = new ExpressionTree("10+A1");
            var exception = Assert.Throws<System.ArgumentException>(() => this.testTree.Evaluate());
            Assert.AreEqual("The variable \"A1\" is not instantiated.", exception.Message);
        }

        /// <summary>
        /// Tests that the tree evaluate functions as expected.
        /// </summary>
        [Test]
        public void TestEvaluate3()
        {
            this.testTree = new ExpressionTree("8-1-2");
            Assert.AreEqual(this.testTree.Evaluate(), 5.0, "ExpressionTree.Evaluate() is not working - possibly multiple instances of operator error.");
        }

        /// <summary>
        /// Tests that the tree evaluate functions as expected.
        /// </summary>
        [Test]
        public void TestEvaluate4()
        {
            this.testTree = new ExpressionTree("A1+B1+C1");
            var exception = Assert.Throws<System.ArgumentException>(() => this.testTree.Evaluate());
            Assert.AreEqual("The variable \"A1\" is not instantiated.", exception.Message);
        }

        /// <summary>
        /// Tests that the tree evaluate functions as expected.
        /// </summary>
        [Test]
        public void TestEvaluate5()
        {
            this.testTree = new ExpressionTree("(10)-((5))+(((3*6)))");
            Assert.AreEqual(this.testTree.Evaluate(), 23, "ExpressionTree is not handling extra but balanced parenthesis.");
        }

        /// <summary>
        /// Tests that the tree builds and evaluates properly when variables with names containing other variable names exist.
        /// </summary>
        [Test]
        public void TestVariableNames()
        {
            this.testTree = new ExpressionTree("A1+A12");
            this.testTree.SetVariable("A1", 4);
            this.testTree.SetVariable("A12", 10);
            Assert.AreEqual(this.testTree.Evaluate(), 14, "ExpressionTree is not handling variables that contain other variable names.");
        }

        /// <summary>
        /// Tests that the tree SetVariable() method functions as expected.
        /// </summary>
        [Test]
        public void TestSetVariable1()
        {
            this.testTree = new ExpressionTree("10+A1");
            this.testTree.SetVariable("A1", 4);
            Assert.AreEqual(this.testTree.Evaluate(), 14, "ExpressionTree.SetVariable() is not working.");
        }

        /// <summary>
        /// Tests that the tree SetVariable() method functions as expected.
        /// </summary>
        [Test]
        public void TestSetVariable2()
        {
            this.testTree = new ExpressionTree("A1+4");
            this.testTree.SetVariable("A1", 4);
            Assert.AreEqual(this.testTree.Evaluate(), 8, "ExpressionTree.SetVariable() is not working.");
        }

        /// <summary>
        /// Tests that the tree SetVariable() method functions as expected.
        /// </summary>
        [Test]
        public void TestSetVariableMulti()
        {
            this.testTree = new ExpressionTree("A1+A1+A1");
            this.testTree.SetVariable("A1", 4);
            Assert.AreEqual(
                this.testTree.Evaluate(),
                12,
                "ExpressionTree.SetVariable() is not working with multiple instances of a variable");
        }

        /// <summary>
        /// Tests that the proper exception is thrown when an invalid variable name is used.
        /// </summary>
        [Test]
        public void TestVariableFormatException()
        {
            var exception = Assert.Throws<System.NotSupportedException>(() => this.testTree = new ExpressionTree("B1+A~"));
            Assert.AreEqual(exception.Message, "The variable \"A~\" is of the wrong format");
        }

        /// <summary>
        /// Tests that the proper exception is thrown when a non-existing variable is passed to SetVariable().
        /// </summary>
        [Test]
        public void TestSetVariableException()
        {
            this.testTree = new ExpressionTree("B1+B2");
            var exception = Assert.Throws<System.ArgumentException>(() => this.testTree.SetVariable("B3", 4));
            Assert.AreEqual(exception.Message, "The variable \"B3\" does not exist in the current exrpession.");
        }

        /// <summary>
        /// Tests that the expression tree supports different operators together.
        /// </summary>
        [Test]
        public void TestMultiOperator()
        {
            this.testTree = new ExpressionTree("2+10-9");
            Assert.AreEqual(this.testTree.Evaluate(), 3, "ExpressionTree.Evaluate() with multiple operators is not working.");
        }

        /// <summary>
        /// Tests that the order precedence is followed properly for the evaluation of the tree.
        /// </summary>
        [Test]
        public void TestOperatorPrecedence()
        {
            this.testTree = new ExpressionTree("9*10+4/2");
            Assert.AreEqual(this.testTree.Evaluate(), 92, "ExpressionTree.Evaluate() order precedence is not working.");
        }

        /// <summary>
        /// Tests that the order precedence is followed properly with parenthesis for the evaluation of the tree.
        /// </summary>
        [Test]
        public void TestParenthesis()
        {
            this.testTree = new ExpressionTree("9*(10+4)/2");
            Assert.AreEqual(this.testTree.Evaluate(), 63, "ExpressionTree.Evaluate() order precedence with parenthesis is not working.");
        }

        /// <summary>
        /// Tests that the proper exception is thrown when an expression with no operator is given.
        /// </summary>
        [Test]
        public void TestNoOperatorException()
        {
            var exception = Assert.Throws<System.ArgumentException>(() => this.testTree = new ExpressionTree("B1"));
            Assert.AreEqual(exception.Message, "The expression \"B1\" does not contain a valid operator.");
        }

        /// <summary>
        /// Tests that an ArgumentException is thrown when there are 
        /// more '(' than ')' in a given expression.
        /// </summary>
        [Test]
        public void TestImbalancedParenthesisException1()
        {
            var exception = Assert.Throws<System.ArgumentException>(() => this.testTree = new ExpressionTree("((x+1)"));
            Assert.AreEqual(exception.Message, "The given expression \"((x+1)\" has more \'(' than ')' and is therefore invalid.");
        }

        /// <summary>
        /// Tests that an ArgumentException is thrown when there are 
        /// more ')' than '(' in a given expression.
        /// </summary>
        [Test]
        public void TestImbalancedParenthesisException2()
        {
            var exception = Assert.Throws<System.ArgumentException>(() => this.testTree = new ExpressionTree("(x+1))"));
            Assert.AreEqual(exception.Message, "The given expression \"(x+1))\" has more \')' than '(' and is therefore invalid.");
        }
    }
}