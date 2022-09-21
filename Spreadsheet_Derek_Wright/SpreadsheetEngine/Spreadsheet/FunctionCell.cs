// <copyright file="FunctionCell.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// </summary>
namespace SpreadsheetEngine
{
    using System.ComponentModel;
    using CptS321;

    /// <summary>
    /// Inherits the SpreadsheetCell class to give the Spreadsheet class access
    /// to the protected value attribute.
    /// </summary>
    internal class FunctionCell : SpreadsheetCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref= "FunctionCell"/> class with given row and column indices.
        /// </summary>
        /// <param name="row"> row index for the new cell. </param>
        /// <param name="col"> column index for the new cell. </param>
        internal FunctionCell(int row, int col)
            : base(row, col)
        {
        }

        /// <summary>
        /// Override of the event handler for when the Text property of a cell is changed.
        /// </summary>
        public override event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Override of the event handler for when a cell referenced by this one is changed.
        /// </summary>
        public override event PropertyChangedEventHandler ReferencedValueChanged;

        /// <summary>
        /// Clears the subscribers of the ReferencedValueChanged event.
        /// </summary>
        public void ClearReferenceSubscribers() => this.ReferencedValueChanged += null;

        /// <summary>
        /// Triggers when a cell that is referenced by this cell is changed to that this cell can
        /// also be updated.
        /// </summary>
        /// <param name="sender"> Object that triggered the event. </param>
        /// <param name="e"> Event arguments. </param>
        public override void OnReferenceValueChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ReferencedValueChanged?.Invoke(this, e);
        }

        /// <summary>
        /// updates the value of the cell, only accessible through the spreadsheet class.
        /// </summary>
        /// <param name="value"> new value for the cell. </param>
        internal void UpdateValue(string value)
        {
            this.value = value;
            this.OnPropertyChanged(nameof(this.Value));
        }

        /// <summary>
        /// Method to handle the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName"> Name of the changed property, in this case "Text". </param>
        protected override void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
