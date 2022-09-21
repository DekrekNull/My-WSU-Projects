// <copyright file="UndoRedoCollection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine.UndoRedo
{
    /// <summary>
    /// Represents a command for the spreadsheet engine that supports Undo and Redo.
    /// </summary>
    public abstract class UndoRedoCollection
    {
        /// <summary>
        /// Executes the command that this object represents.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Gets an object of the same type but with the previous value.
        /// Used to get the redo of an undo and vice versa.
        /// </summary>
        /// <returns> An undo for a redo or a redo for an undo. </returns>
        public abstract UndoRedoCollection GetOpposite();
    }
}