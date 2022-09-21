// <copyright file="UndoRedoTextCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine.UndoRedo
{
    using CptS321;

    /// <summary>
    /// Stores all of the necessary information to Change the text for a given cell at a later time.
    /// </summary>
    public class UndoRedoTextCommand : UndoRedoCollection
    {
        /// <summary>
        /// Stores the spreadsheet that the cell belongs to.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// Stores the cell for which the text should be changed in.
        /// </summary>
        private SpreadsheetCell cell;

        /// <summary>
        /// Stores the text for which the cell should be changed to.
        /// </summary>
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoTextCommand"/> class.
        /// </summary>
        /// <param name="spreadsheet"> Spreadsheet the cell belongs to. </param>
        /// <param name="cell"> Cell that has the text change. </param>
        /// <param name="text"> Text to change the cell to. </param>
        public UndoRedoTextCommand(Spreadsheet spreadsheet, SpreadsheetCell cell, string text)
        {
            this.spreadsheet = spreadsheet;
            this.cell = cell;
            this.text = text;
        }

        /// <summary>
        /// Gets an object of the same type but with the previous value.
        /// Used to get the redo of an undo and vice versa.
        /// </summary>
        /// <returns> An undo for a redo or a redo for an undo. </returns>
        public override UndoRedoCollection GetOpposite()
        {
            return new UndoRedoTextCommand(this.spreadsheet, this.cell, this.cell.Text);
        }

        /// <summary>
        /// Executes the ChangeCellText method in the given spreadsheet,
        /// for the given cell, with the given text.
        /// </summary>
        public override void Execute()
        {
            this.cell.Text = this.text;
        }
    }
}
