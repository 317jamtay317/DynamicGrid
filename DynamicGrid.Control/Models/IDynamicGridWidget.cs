namespace DynamicGrid.Control.Models;

public interface IDynamicGridWidget
{
    /// <summary>
    /// The name that you'd like to display when in
    /// configure mode
    /// </summary>
    string DisplayName { get; }
    
    /// <summary>
    /// the minimum amount of rows that this takes
    /// </summary>
    int MinHeight { get; }
    
    /// <summary>
    /// the minimum amount of columns that this takes
    /// </summary>
    int MinWidth { get; }
}