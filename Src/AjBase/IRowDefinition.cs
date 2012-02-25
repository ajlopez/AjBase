namespace AjBase
{
    using System;

    public interface IRowDefinition
    {
        int ColumnCount { get; }
        Column GetColumn(string name);
        Column GetColumn(int ncol);
        int GetColumnPosition(string name);
    }
}
