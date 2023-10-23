using System.Collections.Generic;
using System.Windows.Controls;

namespace DynamicGrid.Control.Behaviors;

public class GridColumnBehavior : GridBehaviorBase<ColumnDefinition>
{
    public GridColumnBehavior()
    {
        _getDefCollection = GetColumns;
    }
    private IList<ColumnDefinition>? GetColumns() => AssociatedObject?.ColumnDefinitions;
}