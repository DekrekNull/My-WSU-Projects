// <copyright file="SpreadsheetCell.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Derek Wright 11766151
/// Do not reference System.Windows.Forms in this Library
/// </summary>
namespace CptS321
{
    using System.ComponentModel;

    /// <summary>
    /// An object representing a cell for the Spreadsheet class with indices and text/value values stored
    /// Inherits the INotifyPropertyChanged interface to notify the Spreadsheet class
    /// that its text attribute is updated.
    /// </summary>
    public abstract class SpreadsheetCell : INotifyPropertyChanged
    {
        /// <summary>
        /// Stores the text that is in the SpreadsheetCell.
        /// </summary>
        protected string text;

        /// <summary>
        /// Stores the Value for the SpreadsheetCell.
        /// </summary>
        protected string value;

        /// <summary>
        /// Stores the background color of the cell as a unsigned integer.
        /// </summary>
        protected uint bgColor;

        /// <summary>
        /// Initializes a new instance of the <see cref= "SpreadsheetCell"/> class with given row and column indices.
        /// </summary>
        /// <param name="row"> row index for the new cell. </param>
        /// <param name="col"> column index for the new cell. </param>
        public SpreadsheetCell(int row, int col)
        {
            this.RowIndex = row;
            this.ColumnIndex = col;
            this.bgColor = 0xFFFFFFFF;
        }

        /// <summary>
        /// Event handler for when the Text of the cell is updated.
        /// </summary>
        public abstract event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event handler for when a cell that is referenced by this cell is updated.
        /// </summary>
        public abstract event PropertyChangedEventHandler ReferencedValueChanged;

        /// <summary>
        /// Gets the row index of the cell in the Spreadsheet.
        /// </summary>
        public int RowIndex { get; }

        /// <summary>
        /// Gets the column index of the cell in the Spreadsheet.
        /// </summary>
        public int ColumnIndex { get; }

        /// <summary>
        /// Gets or sets the integer value of the background color of the cell.
        /// </summary>
        public uint BGColor
        {
            get
            {
                return this.bgColor;
            }

            set
            {
                if (value != this.bgColor)
                {
                    this.bgColor = value;
                    this.OnPropertyChanged(nameof(this.BGColor));
                }
            }
        }

        /// <summary>
        /// Gets or sets the text attribute, triggers event upon successful change.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (value != this.text)
                {
                    this.text = value;
                    this.OnPropertyChanged(nameof(this.Text));
                }
            }
        }

        /// <summary>
        /// Gets the value attribute.
        /// </summary>
        public string Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Triggers when a cell that is referenced by this cell is changed to that this cell can
        /// also be updated.
        /// </summary>
        /// <param name="sender"> Object that triggered the event. </param>
        /// <param name="e"> Event arguments. </param>
        public abstract void OnReferenceValueChanged(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Event for when the Text of the cell is updated.
        /// </summary>
        /// <param name="propertyName"> The calling member's name. </param>
        protected abstract void OnPropertyChanged(string propertyName);
    }
}
