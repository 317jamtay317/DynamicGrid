using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DynamicGrid.Control.Behaviors;

public abstract class GridBehaviorBase<T> : GridBehavior
    where T : DefinitionBase, new()
{
    protected Func<IList<T>?> _getDefCollection { get; init; }
    private IList<T> _definitionCollection;
    private readonly Func<T> _createDefinition;

    #region Properties

    public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
        nameof(Count),
        typeof(int), 
        typeof(GridBehaviorBase<T>),
        new PropertyMetadata(1, CountChanged));

    /// <summary>
    /// The amount of grid items that are being displayed, by default there are 1
    /// </summary>
    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    #endregion

    private static void CountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GridBehaviorBase<T> behaviorBase)
        {
            var action = (int)e.NewValue > (int)e.OldValue ? UpdateItemsAction.Add : UpdateItemsAction.Remove;
            var collection = behaviorBase._getDefCollection();
            if (collection is null) return;
            
            Func<bool> continueUpdating = () => collection.Count < behaviorBase.Count;
            if (action == UpdateItemsAction.Remove)
            {
                continueUpdating = () => collection.Count > behaviorBase.Count;
            }
            while (continueUpdating())
            {
                behaviorBase.UpdateItems(action);
            }
        }
    }

    protected enum UpdateItemsAction
    {
        Add,
        Remove
    }

    /// <summary>
    /// updates the amount if items that are in the collection
    /// </summary>
    /// <param name="updateItemsAction"></param>
    protected virtual void UpdateItems(UpdateItemsAction action)
    {
        if (action == UpdateItemsAction.Add)
        {
            AddItem();
        }
        else
        {
            RemoveLastItem();
        }
    }

    /// <summary>
    /// Removes the last item from the definition Collection
    /// </summary>
    protected virtual void RemoveLastItem()
    {
        var lastItem = _definitionCollection.Cast<T>().LastOrDefault();
        _definitionCollection.Remove(lastItem);
    }

    /// <summary>
    /// Adds an item to the definition Collection
    /// </summary>
    protected virtual void AddItem()
    {
        var collection = _getDefCollection();
        if (collection is null) return;
        var definition = new T();
        _definitionCollection.Add(definition);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        _definitionCollection = _getDefCollection();
        var collection = _getDefCollection();
        while (collection?.Count < Count)
        {
            UpdateItems(UpdateItemsAction.Add);
        }
    }
}