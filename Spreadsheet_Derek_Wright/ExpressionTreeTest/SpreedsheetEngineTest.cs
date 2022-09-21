// <copyright file="SpreadsheetEngineTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngineTest
{
    using System;
    using System.Reflection;
    using CptS321;
    using NUnit.Framework;
    using SpreadsheetEngine;

    /// <summary>
    /// This class tests the functionality of the Spreadsheet class to ensure that
    /// it is working as intended.
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// ExpressionTree for the test cases.
        /// </summary>
        private Spreadsheet testSheet;

        /// <summary>
        /// Performs necessary setup for tests.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.testSheet = new Spreadsheet(50, 26);
        }

        /// <summary>
        /// Tests that the GetCell method properly returns the cell with the given indices or null
        /// if indices out of range.
        /// </summary>
        [Test]
        public void TestGetCell()
        {
            SpreadsheetCell cell = this.testSheet.GetCell(0, 0);

            Assert.AreEqual(
                0,
                cell.RowIndex,
                "GetCell not working: incorrect row index.");
            Assert.AreEqual(
                0,
                cell.ColumnIndex,
                "GetCell not working: incorrect column index.");
            cell = this.testSheet.GetCell(0, 100);
            Assert.AreEqual(
                null,
                cell,
                "GetCell not working: null not returned when column index out of range.");
            cell = this.testSheet.GetCell(100, 0);
            Assert.AreEqual(
                null,
                cell,
                "GetCell not working: null not returned when row index out of range.");
        }

        /// <summary>
        /// Tests that the private method UpdateCell properly updates the value of the cell with
        /// the given expression.
        /// </summary>
        [Test]
        public void TestUpdateCell()
        {
            MethodInfo methodInfo = this.GetMethod("UpdateCell");
            SpreadsheetCell cell = this.testSheet.GetCell(0, 0);
            cell.Text = "11";
            methodInfo.Invoke(
                this.testSheet,
                new object[] { 0, 0 });
            Assert.AreEqual(
                "11",
                cell.Value,
                "UpdateCell not working.");
            cell.Text = "=11+2";
            methodInfo.Invoke(
                this.testSheet,
                new object[] { 0, 0 });
            Assert.AreEqual(
                "13",
                cell.Value,
                "UpdateCell not working: Does not evaluate expression.");
        }

        /// <summary>
        /// Tests that the private method ParseCellIndices returns the correct indices
        /// for a given cell name.
        /// </summary>
        [Test]
        public void TestParseCellIndices()
        {
            MethodInfo methodInfo = this.GetMethod("ParseCellIndices");
            int[] expected = { 19, 3 };
            Assert.AreEqual(
                expected, 
                methodInfo.Invoke(this.testSheet, new object[] { "D20" }), 
                "ParseIndices not working.");
        }

        /// <summary>
        /// Tests that the private method ValidateCellIndices returns the correct boolean
        /// based on the given indices.
        /// </summary>
        [Test]
        public void TestValidateCellIndices()
        {
            MethodInfo methodInfo = this.GetMethod("ValidateCellIndices");
            string error = "ValidateCellIndices not working.";
            Assert.AreEqual(
                true,
                methodInfo.Invoke(this.testSheet, new object[] { 0, 1 }), 
                error);
            error = "ValidateCellIndices not working: Accepts indices out of range.";
            Assert.AreEqual(
                false,
                methodInfo.Invoke(this.testSheet, new object[] { 1,  26 }), 
                error);
        }

        /// <summary>
        /// Tests that the private method GetCellValue returns the correct double value from the given cell
        /// corresponding to the set Text property.
        /// </summary>
        [Test]
        public void TestParseCellValue()
        {
            MethodInfo methodInfo = this.GetMethod("ParseCellValue");
            SpreadsheetCell cell = this.testSheet.GetCell(0, 0);
            cell.Text = "11";
            string error = "GetCellValue not working.";
            Assert.AreEqual(
                11,
                methodInfo.Invoke(this.testSheet, new object[] { cell }), 
                error);
            cell.Text = "=11+2";
            error = "GetCellValue not working: Does not evaluate expression.";
            Assert.AreEqual(
                13,
                methodInfo.Invoke(this.testSheet, new object[] { cell }), 
                error);
        }

        /// <summary>
        /// Tests that cells are properly updated when a cell that is referenced in its expression
        /// is updated.
        /// </summary>
        [Test]
        public void TestOnReferencedValueChanged()
        {
            SpreadsheetCell cell1 = this.testSheet.GetCell(0, 0);
            SpreadsheetCell cell2 = this.testSheet.GetCell(0, 1);
            SpreadsheetCell cell3 = this.testSheet.GetCell(0, 2);
            cell1.Text = "10";
            cell2.Text = "13";
            cell3.Text = "=A1+B1";
            cell1.Text = "4";
            Assert.AreEqual(
                "17",
                cell3.Value,
                "Cells are not updated when a value they refference is changed.");
        }

        /// <summary>
        /// Takes a private method name and retrieves that method from the Spreadsheet class.
        /// </summary>
        /// <param name="methodName"> Name of the private method to be retrieved. </param>
        /// <returns> The private method with the name methodName as a MethodInform object. </returns>
        private MethodInfo GetMethod(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Assert.Fail("methodName cannot be null or whitespace.");
            }

            var method = this.testSheet.GetType().GetMethod(
                methodName,
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

            if (method == null)
            {
                Assert.Fail(string.Format("{0} method not found", methodName));
            }

            return method;
        }
    }
}
