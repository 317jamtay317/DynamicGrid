using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DynamicGrid.Control.Behaviors;

public class GridRowBehavior : GridBehaviorBase<RowDefinition>
{
    public GridRowBehavior()
    {
        _getDefCollection = GetRows;
    }

    private IList<RowDefinition>? GetRows() => AssociatedObject?.RowDefinitions;
}