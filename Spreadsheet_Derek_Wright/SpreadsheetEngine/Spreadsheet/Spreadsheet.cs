// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using CptS321;
    using SpreadsheetEngine.UndoRedo;

    /// <summary>
    /// Class to store the array of Cells and handle the changes to the text and value of each cell.
    /// </summary>
    public class Spreadsheet
    {
        /// <summary>
        /// Queue of Undo commands.
        /// </summary>
        private Stack<UndoRedoCollection> undos;

        /// <summary>
        /// Queue of Redo commands.
        /// </summary>
        private Stack<UndoRedoCollection> redos;

        /// <summary>
        /// Initializes a new instance of the <see cref= "Spreadsheet"/> class with a given row and column value.
        /// </summary>
        /// <param name="rowCount"> the number of rows for the spreadsheet. </param>
        /// <param name="columnCount"> the number of columns for the spreadsheet. </param>
        public Spreadsheet(int rowCount, int columnCount)
        {
            this.RowCount = rowCount;
            this.ColumnCount = columnCount;
            this.Cells = new FunctionCell[rowCount, columnCount];
            this.undos = new Stack<UndoRedoCollection>();
            this.redos = new Stack<UndoRedoCollection>();

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    FunctionCell cell = new FunctionCell(row, col);
                    cell.PropertyChanged += new PropertyChangedEventHandler(this.OnCellPropertyChanged);
                    cell.ReferencedValueChanged += new PropertyChangedEventHandler(this.OnReferencedValueChanged);
                    this.Cells[row, col] = cell;
                }
            }
        }

        /// <summary>
        /// Event handler for when text in a cell changes.
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged;

        /// <summary>
        /// Gets the number of Columns in the Spreadsheet object.
        /// </summary>
        private int ColumnCount { get; }

        /// <summary>
        /// Gets the number of Rows in the Spreadsheet object.
        /// </summary>
        private int RowCount { get; }

        /// <summary>
        /// Gets the 2D array of the SpreadsheetCell objects associated with the Spreadsheet object.
        /// </summary>
        private FunctionCell[,] Cells { get; }

        /// <summary>
        /// Changes the color of the given cells to the given color.
        /// </summary>
        /// <param name="cells"> Cells to update. </param>
        /// <param name="color"> Next color for the cell. </param>
        public void ChangeCellColor(List<SpreadsheetCell> cells, uint color)
        {
            foreach (SpreadsheetCell cell in cells)
            {
                cell.BGColor = color;
            }
        }

        /// <summary>
        /// Pushes an undo command onto the undo stack.
        /// </summary>
        /// <param name="undo"> Undo command to add to stack. </param>
        public void AddUndo(UndoRedoCollection undo)
        {
            this.undos.Push(undo);
        }

        /// <summary>
        /// Pushes an redo command onto the redo stack.
        /// </summary>
        /// <param name="redo"> Redo command to add to stack. </param>
        public void AddRedo(UndoRedoCollection redo)
        {
            this.redos.Push(redo);
        }

        /// <summary>
        /// Pops and executes the first undo command in the undo stack and
        /// pushes a respective redo command onto the redo stack.
        /// </summary>
        /// <returns> Count of undo stack. </returns>
        public int Undo()
        {
            if (this.undos.Count > 0)
            {
                UndoRedoCollection undo = this.undos.Pop();
                this.redos.Push(undo.GetOpposite());
                undo.Execute();
            }

            return this.undos.Count;
        }

        /// <summary>
        /// Pops and executes the first redo command in the redo stack and
        /// pushes a respective undo command onto the undo stack.
        /// </summary>
        /// <returns> Count of redo stack. </returns>
        public int Redo()
        {
            if (this.redos.Count > 0)
            {
                UndoRedoCollection redo = this.redos.Pop();
                this.undos.Push(redo.GetOpposite());
                redo.Execute();
            }

            return this.redos.Count;
        }

        /// <summary>
        /// Retrieves the SpreadsheetCell object at the given row and column position.
        /// </summary>
        /// <param name="rowIndex"> Row index of the desired cell. </param>
        /// <param name="columnIndex"> Column index of the desired cell. </param>
        /// <returns> The SpreadsheetCell object at the given row and column position, null if one or both
        /// given indices are invalid. </returns>
        public SpreadsheetCell GetCell(int rowIndex, int columnIndex)
        {
            if (rowIndex >= this.RowCount || columnIndex >= this.ColumnCount)
            {
                return null;
            }

            return this.Cells[rowIndex, columnIndex];
        }

        /// <summary>
        /// When a cell's text changes this is triggered and updates the value, or when a cells color is updated.
        /// </summary>
        /// <param name="sender"> the SpreadsheetCell object that had its text changed. </param>
        /// <param name="e"> the arguments for the event, in this case it is "Text" or "BGColor". </param>
        public void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                FunctionCell cell = (FunctionCell)sender;
                this.UpdateCell(cell.RowIndex, cell.ColumnIndex);
                this.CellPropertyChanged?.Invoke(sender, e);
            }
            else if (e.PropertyName == "BGColor")
            {
                FunctionCell cell = (FunctionCell)sender;
                this.CellPropertyChanged?.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Triggered when a cell referenced by another cell is updated. Updates the cell
        /// that references the updated cell.
        /// </summary>
        /// <param name="sender"> Cell that needs to be updated. </param>
        /// <param name="e"> Event arguments. Should be "Value". </param>
        public void OnReferencedValueChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                FunctionCell cell = (FunctionCell)sender;
                this.UpdateCell(cell.RowIndex, cell.ColumnIndex);
                this.CellPropertyChanged?.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Performs the demo to show that the cell updating functions as it should.
        /// </summary>
        public void RunDemo()
        {
            Random rd = new Random();
            for (int i = 0; i < 50; i++)
            {
                this.Cells[rd.Next(0, this.RowCount - 1), rd.Next(0, this.ColumnCount - 1)].Text = "Hello World!";
            }

            for (int i = 0; i < this.RowCount; i++)
            {
                this.Cells[i, 1].Text = string.Format("This is cell B{0}", i + 1);
                this.Cells[i, 0].Text = string.Format("=B{0}", i + 1);
            }
        }

        /// <summary>
        /// Takes a cell name (e.g. A1) and returns the indices values.
        /// </summary>
        /// <param name="name"> Cell name. </param>
        /// <returns> Array of indices for the cell in the form [ row, col ]. </returns>
        private static int[] ParseCellIndices(string name)
        {
            int[] indices = new int[2];
            indices[1] = (int)name[0] - 65;
            indices[0] = int.Parse(name.Substring(1)) - 1;
            return indices;
        }

        /// <summary>
        /// Determines if the given indices correspond to a cell in the spreadsheet.
        /// </summary>
        /// <param name="row"> Row index. </param>
        /// <param name="col"> Column index. </param>
        /// <returns> True if a cell exists in the Spreadsheet instance with at the given indices. </returns>
        private bool ValidateCellIndices(int row, int col)
        {
            if ((col >= this.ColumnCount) || (row >= this.RowCount))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the value associated to the given cell name.
        /// </summary>
        /// <param name="cell"> Cell to get value from. </param>
        /// <returns> Value of the given cell as a double. If the value of the cell cannot parse to double, return 0. </returns>
        private double ParseCellValue(SpreadsheetCell cell)
        {
            if (double.TryParse(cell.Value, out double value))
            {
                return value;
            }
            else if (cell.Value == null)
            {
                return double.NaN;
            }

            return 0;
        }

        /// <summary>
        /// Creates an expression tree for the given cell based on the given text expression.
        /// </summary>
        /// <param name="text"> The expression to make the tree from. </param>
        /// <param name="cell"> The Cell object to make the tree for. </param>
        /// <param name="newVariables"> Boolean for if the expression contains new values compared to the previous expression. </param>
        /// <returns> An ExpressionTree object representing the given expression. </returns>
        private ExpressionTree CreateExpressionTree(string text, SpreadsheetCell cell, bool newVariables)
        {
            ExpressionTree expression = new ExpressionTree(text);
            string[] variableNames = expression.GetVariables();
            foreach (string name in variableNames)
            {
                try
                {
                    int[] indices = ParseCellIndices(name);
                    if (!this.ValidateCellIndices(indices[0], indices[1]))
                    {
                        throw new ArgumentException(
                            "Cell " + name + " DNE.");
                    }

                    SpreadsheetCell cell2 = this.GetCell(indices[0], indices[1]);
                    expression.SetVariable(name, this.ParseCellValue(cell2));
                    if (newVariables)
                    {
                        cell2.PropertyChanged += new PropertyChangedEventHandler(cell.OnReferenceValueChanged);
                    }
                }
                catch (ArgumentException e)
                {
                    throw e;
                }
            }

            return expression;
        }

        /// <summary>
        /// Updates the Value of a cell based on the given indices and the value of its Text member.
        /// </summary>
        /// <param name="rowA"> row index of the cell. </param>
        /// <param name="colA"> column index of the cell. </param>
        private void UpdateCell(int rowA, int colA)
        {
            FunctionCell cell = (FunctionCell)this.Cells[rowA, colA];
            bool newVariables = false;
            string newText = cell.Text;
            if (cell.Text != cell.Value)
            {
                cell.ClearReferenceSubscribers();
                newVariables = true;
            }

            if (newText != null)
            {
                newText = string.Concat(newText.Where(c => !char.IsWhiteSpace(c)));
                if (newText[0] == '=')
                {
                    newText = newText.Substring(1);
                    ExpressionTree expression;
                    try
                    {
                        expression = this.CreateExpressionTree(newText, cell, newVariables);
                        try
                        {
                            newText = expression.Evaluate().ToString();
                        }
                        catch (ArgumentException)
                        {
                            newText = "REF ERRORR";
                        }
                    }
                    catch (ArgumentException)
                    {
                        int[] indices = ParseCellIndices(newText);
                        if (this.ValidateCellIndices(indices[0], indices[1]))
                        {
                            SpreadsheetCell cell2 = this.Cells[indices[0], indices[1]];
                            cell2.PropertyChanged += new PropertyChangedEventHandler(cell.OnReferenceValueChanged);
                            newText = cell2.Value;
                            if (newText == null)
                            {
                                newText = "REF ERROR";
                            }
                        }
                        else
                        {
                            throw new ArgumentException(
                                "The entered function is invalid.");
                        }
                    }
                }
            }

            cell.UpdateValue(newText);
        }
    }
}
