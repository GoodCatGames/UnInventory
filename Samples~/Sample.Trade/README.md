
# Sample - Trade

 In this example, we will create several inventories for the implementation of trade.

 And namely:

  1. Hero Bag
  2. Hero Table
  3. Trader Table
  4. Trader Bag

 Inventories of the type `Bag` store the Hero's and the Trader's possessions, respectively.
Items for sale are laid out on inventory of the type `Table`.

  The rules are following:

- You can move items between your Bag and Table (for example, items from Hero Bag can be moved to Hero Table, but not to Trader’s inventory).
- Each item has its own cost (including coins, the cost of one is 1).
- In order to complete the Trade action, the cost of items on one table must be equal to the value on another.
- After the Trading action, items from tables are moved to opposite bags (Hero Table => Trader Bag).

## Implementation

1. Using `Inventory Creator`, create an inventories `Bag` and `Table` of the `Grid Support Multislot Entity` type, all components are standard.

2. Save the created inventories to the prefabs.

3. Implement

    ```csharp
    [CreateAssetMenu(fileName = "DataEntity", menuName = "UnInventory/Entity Price")]
    public class DataEntityPrice : DataEntity
    ```

    In this class add data `Price`.
    In editor mode, create some items for load them into inventory.

4. Implement the class `public class EntityCostViewComponent: EntityViewComponent` which respond for displaying the cost of the item.

5. Create your prefab `EntityPrice` based on the standard. Attach an `EntityCostViewComponent` to it.

    *Note: The `EntityViewStandardComponent` remains, not replaced by the `EntityCostViewComponent`! Those as a result, the prefab will have both `EntityViewStandardComponent` and `EntityCostViewComponent` at the same time.*

6. Bind the type to the prefab when overriding the `ContainerDiStandard`:

   ```csharp
    public class ContainerDiTrade : ContainerDiStandard
    {
        [UsedImplicitly]
        [SerializeField] private GameObject _entityPricePrefab;

        protected override void BindDataEntitiesToPrefabs()
        {
            base.BindDataEntitiesToPrefabs();
            BindDataEntityToPrefab<DataEntityPrice>(_entityPricePrefab);
        }
    }
    ```

7. Implement a filter to prohibit movement between the inventory of the Hero and the Trader.

   ```csharp
    public class FilterTrade :
        IFilterMoveInEmptySlots,
        IFilterStack,
        IFilterSwap
    ```

   Please note that this prohibition is only for moving using `Hand` otherwise it can't be able to trade. Simply put, the Hero can't steal a thing from the `Bag` or `Table` of the trader. But when the button `DoTrade` is pressed, if the cost of items on the tables is balanced, these items should fall into the appropriate bags.
    Therefore, this filter will be added like this:

      ```csharp
      InventoryManager.Get().FiltersManager.FiltersForHandOnly.Add(new FilterTrade(new List<IDataInventoryContainer>(){_heroBag, _heroTable}));
      ```

8. Implement class of the subscription on events actions in inventory.
To update the elements UI that display the cost of the all items on the tables, etc.

    `public class UpdateUiViewListener : InventoryListener`

9. Replace `ContainerDiStandard` with `ContainerDiTrade` in the `InventoryManager` on the scene. 

10. Implement and add the class `ApplicationTrade` to the scene.

11. Done!

*For samples, a set of sprites was used:
https://assetstore.unity.com/packages/2d/gui/icons/rpg-inventory-icons-56687*