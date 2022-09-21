// <copyright file="Form1.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using SpreadsheetEngine;
    using SpreadsheetEngine.UndoRedo;

    /// <summary>
    /// Inherits the Form class and give the functionality for the Spreadsheet class UI.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Spreadsheet object to be tied to the DataGridView object.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// Initializes a new instance of the <see cref= "Form1"/> class and all of its components.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.SetupLayout(26, 50);
        }

        /// <summary>
        /// Sets up the DataGridView with the given number of rows and columns.
        /// </summary>
        /// <param name="columnsNum"> Number of columns for the grid. </param>
        /// <param name="rowsNum"> Number of rows for the grid.</param>
        private void SetupLayout(int columnsNum, int rowsNum)
        {
            int asciiA = 65;
            this.dataGridView1.ColumnCount = columnsNum;
            for (int i = 0; i < this.dataGridView1.ColumnCount; i++)
            {
                char columnHead = (char)(asciiA + i);
                this.dataGridView1.Columns[i].Name = columnHead.ToString();
            }

            this.dataGridView1.Rows.Add(rowsNum);
            for (int i = 0; i < rowsNum; i++)
            {
                int rowHead = i + 1;
                this.dataGridView1.Rows[i].HeaderCell.Value = rowHead.ToString();
            }

            this.dataGridView1.CellBeginEdit += this.DataGridView1_CellBeginEdit;
            this.dataGridView1.CellEndEdit += this.DataGridView1_CellEndEdit;
            this.DemoBttn.Click += this.DemoBttn_Click;
            this.undoToolStripMenuItem.Click += this.Undo_Click;
            this.redoToolStripMenuItem.Click += this.Redo_Click;
            this.redoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Enabled = false;
            this.spreadsheet = new Spreadsheet(rowsNum, columnsNum);
            this.spreadsheet.CellPropertyChanged += new PropertyChangedEventHandler(this.OnCellPropertyChanged);
            this.changeBackgorundColorToolStripMenuItem.Click += this.ChangeColor_Click;
        }

        /// <summary>
        /// Redoes the last undone task.
        /// </summary>
        /// <param name="sender"> Redo button object. </param>
        /// <param name="e"> Event arguments. </param>
        private void Redo_Click(object sender, EventArgs e)
        {
            if (this.spreadsheet.Redo() == 0)
            {
                this.redoToolStripMenuItem.Enabled = false;
            }

            // Enables the undo button if not already since an undo is pushed onto stack
            if (this.undoToolStripMenuItem.Enabled == false)
            {
                this.undoToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// Undoes the last performed task.
        /// </summary>
        /// <param name="sender"> Undo button. </param>
        /// <param name="e"> Event arguments. </param>
        private void Undo_Click(object sender, EventArgs e)
        {
            if (this.spreadsheet.Undo() == 0)
            {
                this.undoToolStripMenuItem.Enabled = false;
            }

            // Enables the redo button if not already since a redo is pushed onto stack
            if (this.redoToolStripMenuItem.Enabled == false)
            {
                this.redoToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// Event handler for when the Change Background Color button is clicked.
        /// Pops a color dialog up for the user to select a background color for all of
        /// the currently selected cells.
        /// </summary>
        /// <param name="sender"> The Change Background Color button. </param>
        /// <param name="e"> The event arguments. </param>
        private void ChangeColor_Click(object sender, EventArgs e)
        {
            ColorDialog myDialog = new ColorDialog
            {
                // Keeps the user from selecting a custom color.
                AllowFullOpen = false,

                // Allows the user to get help. (The default is false.)
                ShowHelp = true,

                // Sets the initial color select to the current text color.
                Color = this.dataGridView1.SelectedCells[0].Style.BackColor,
            };

            // Update the text box color if the user clicks OK
            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                uint color = (uint)myDialog.Color.ToArgb();
                List<SpreadsheetCell> cells = new List<SpreadsheetCell>();
                foreach (DataGridViewCell cell in this.dataGridView1.SelectedCells)
                {
                    cells.Add(this.spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex));
                }

                uint oldColor = cells[0].BGColor;
                this.spreadsheet.AddUndo(new UndoRedoColorCommand(
                    this.spreadsheet,
                    cells,
                    oldColor));
                if (this.undoToolStripMenuItem.Enabled == false)
                {
                    this.undoToolStripMenuItem.Enabled = true;
                }

                this.spreadsheet.ChangeCellColor(cells, color);
            }
        }

        /// <summary>
        /// Updates the cell value in the dataGridView to match what it is updated to in the spreadsheet engine.
        /// </summary>
        /// <param name="sender"> SpreadsheetCell object that was updated. </param>
        /// <param name="e"> Arguments for property change, in this case it will be "Text". </param>
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                SpreadsheetCell cell = (SpreadsheetCell)sender;
                this.dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
            }
            else if (e.PropertyName == "BGColor")
            {
                SpreadsheetCell cell = (SpreadsheetCell)sender;
                this.dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Style.BackColor = System.Drawing.Color.FromArgb((int)cell.BGColor);
            }
        }

        /// <summary>
        /// Event to trigger the demo when the button is clicked.
        /// </summary>
        /// <param name="sender"> Button object. </param>
        /// <param name="e"> arguments for click event. </param>
        private void DemoBttn_Click(object sender, EventArgs e)
        {
            this.spreadsheet.RunDemo();
        }

        /// <summary>
        /// Updates the DataGridView Cell that is currently being edited to the Text value of the
        /// corresponding SpreadsheetCell.
        /// </summary>
        /// <param name="sender"> The object that triggered the event, in this case dataGridView1. </param>
        /// <param name="e"> The arguments for the event, in this case the cell being edited. </param>
        private void DataGridView1_CellBeginEdit(
            object sender,
            DataGridViewCellCancelEventArgs e)
        {
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }

        /// <summary>
        /// Updates the DataGridView Cell that was just edited to the Value value of the
        /// corresponding SpreadsheetCell.
        /// </summary>
        /// <param name="sender"> The object that triggered the event, in this case dataGridView1. </param>
        /// <param name="e"> The arguments for the event, in this case the cell that was just edited. </param>
        private void DataGridView1_CellEndEdit(
            object sender,
            DataGridViewCellEventArgs e)
        {
            SpreadsheetCell cell = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex);
            try
            {
                var cellText = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (cellText != null)
                {
                    if (cellText.ToString() != cell.Text)
                    {
                        this.spreadsheet.AddUndo(new UndoRedoTextCommand(this.spreadsheet, cell, cell.Text));
                        if (this.undoToolStripMenuItem.Enabled == false)
                        {
                            this.undoToolStripMenuItem.Enabled = true;
                        }

                        cell.Text = cellText.ToString();
                    }
                    else
                    {
                        this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cell.Value;
                    }
                }
            }
            catch (ArgumentException exception)
            {
                this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = exception.Message;
            }
        }
    }
}
