// <copyright file="UndoRedoColorCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine.UndoRedo
{
    using System.Collections.Generic;
    using CptS321;

    /// <summary>
    /// Stores all of the necessary information to Change the color for a given cell at a later time.
    /// </summary>
    public class UndoRedoColorCommand : UndoRedoCollection
    {
        /// <summary>
        /// Stores the spreadsheet that the cell belongs to.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// Stores the cell for which the text should be changed in.
        /// </summary>
        private List<SpreadsheetCell> cells;

        /// <summary>
        /// Stores the text for which the cell should be changed to.
        /// </summary>
        private uint color;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoColorCommand"/> class.
        /// </summary>
        /// <param name="spreadsheet"> Spreadsheet the cell belongs to. </param>
        /// <param name="cells"> Cell that has the text change. </param>
        /// <param name="color"> Text to change the cell to. </param>
        public UndoRedoColorCommand(Spreadsheet spreadsheet, List<SpreadsheetCell> cells, uint color)
        {
            this.spreadsheet = spreadsheet;
            this.cells = cells;
            this.color = color;
        }

        /// <summary>
        /// Gets an object of the same type but with the previous value.
        /// Used to get the redo of an undo and vice versa.
        /// </summary>
        /// <returns> An undo for a redo or a redo for an undo. </returns>
        public override UndoRedoCollection GetOpposite()
        {
            return new UndoRedoColorCommand(this.spreadsheet, this.cells, this.cells[0].BGColor);
        }

        /// <summary>
        /// Executes the ChangeCellColor method for the given cells,
        /// in the given spreadsheet, with the given color.
        /// </summary>
        public override void Execute()
        {
            this.spreadsheet.ChangeCellColor(this.cells, this.color);
        }
    }
}
