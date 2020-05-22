# Sample - Hero

This sample demonstrates the following capabilities for creating and customizing your inventory types:

1) Requirements for the characteristics thing for dressing
2) Changing the characteristics of the hero when changing equipment
3) Damned thing - can't be removed
4) Two-handed weapon - the second slot is blocked
5) Conclusion of a message about the reason why it is impossible to equip a thing
6) Using Hp and Mana potions from inventory using hot keys and hotbar

Including next inventories:

1. **Bag** - hero inventory
2. **Dummy** - hero dummy
3. **HotBar** - panel for using items

## Bag

Inventory storing the hero's possessions.

### Implementation

1. Using `Inventory Creator`, create an inventory of the `Grid Support Multislot Entity` type, all components are standard.
2. Save the created inventory to the prefab.

## Dummy

Inventory allows you to put things on the hero on the relevant parts of the body.
 Things have its own characteristics requirements.

### In this sample

1) The sword can't be taken in hand - it requires more Strength than it is.
2) If you wear a ring - Strength will increase (you can take a sword in your hand).
3) Damned helmet - if you put it on, you can no longer take it off.
4) Bow takes two hands - you can't equip anything in the second hand.
And you can’t equip it if the other hand is busy.

### Implementation

1. Implement the `class DataSlotDummy: DataSlot` where you add the `enum BodyPart`.
2. Implement `public class SlotDummyComponent : SlotRootComponent<DataSlotDummy> { }`
3. Using `Inventory Creator`, create an inventory of type `FreeSlots`, select SlotDummyComponent at creation.
4. Edit the sizes of the created slots, set the value to `BodyPart`.
5. Save the created inventory to the prefab.
6. Implement

    ```csharp
    [CreateAssetMenu(fileName = "DataEntity", menuName = "UnInventory/Entity Equipment")]`
    public class DataEntityEquipment : DataEntity
    ```

    Add data Equipment in this class:

    - BodyPart
    - Hero Required Characteristics for donning on an item
    - Changing the characteristics of the hero when donning (buff / debuff)
    - etc

    In editor mode, create some items and load them into inventory.

7. Bind the type to the prefab when overriding the `ContainerDiStandard`:

   ```csharp
    public class ContainerDiHero : ContainerDiStandard
    {
        [UsedImplicitly]
        [SerializeField] private GameObject _equipmentPrefab;

        protected override void BindDataEntitiesToPrefabs()
        {
            base.BindDataEntitiesToPrefabs();
            BindDataEntityToPrefab<DataEntityEquipment>(_equipmentPrefab);
        }
    }
    ```

8. Implement classes of the filters to prohibit certain kind of movings. For example:

   ```csharp
    public class FilterDummyBodyPart :
        IFilterCreate,
        IFilterMoveInEmptySlots,
        IFilterStack,
        IFilterSwap
    ```

9. Implement classes of the subscription on events actions in inventory.
For example, for implementation buffs / debuffs when putting on a thing.

    `public class BuffDebuffListener : InventoryListener`

10. Implement reaction filter failure classes for the output of the corresponding messages. For example:

    ```csharp
        public class NoFilterValidReactBodyPart :
        IFilterResponseReactConcrete<FilterDummyBodyPart, DataMoveToEmptySlotBefore>,
        IFilterResponseReactConcrete<FilterDummyBodyPart, DataMoveSwapBefore>
    ```

11. Done!

## HotBar

The panel allows you to assign and use health and mana potions through hot keys.
To use, drag them out of Bag.

### Implementation:

1. Create your version of the `HotBar` prefab to change the look (View) - add the output of the column number of the slot. Implement:

    ```csharp
    [RequireComponent(typeof(ISlotRootComponent))]
        public class SlotViewHotBarComponent : MonoBehaviour
    ```

    Attach to the prefab.

2. Implement:

    ```csharp
    [RequireComponent(typeof(ISlotRootComponent))]
        public class SlotHotBarInputComponent : SlotInputComponent, IPointerDownHandler, IPointerUpHandler, IDragHandler
    ```

    Here change the `SlotStandardInputComponent` slightly so that:
     - Always took the whole stack in `Hand`
     - when transferring to a non-HotBar slot, Entity was deleted.
  
3. Using `Inventory Creator`, create an inventory of type `Grid`, select the `SlotHotBar` prefab and the `SlotHotBarInputComponent` component when creating it.

4. Place the created inventory on the scene.

5. Implement classes **filters** to ban certain kind of movings. Namely:
   - allow moving to inventory only `DataEntityEquipment.IsHotBarItem == true`
   - disable stacks From/To inventory HotBar
   - prohibit all transfers From inventory to another inventory
   - prohibit moving To inventory from others, if Entity with such Id is already there.

6. Implement classes of the **subscription** on events actions in inventory. In it, subscribe to the move event To inventory and implement the following actions:
   1. when moving consider the entire amount of Entity in the original inventory
   2. create an Entity with this quantity To inventory HotBar
   3. return taken Entity to original inventory.

   *Example: There are 4 HpPotion in the bag, grab 1 and drag it into HotBar. 4 potions appeared in HotBar, everything remained the same in Bag.*

7. Implement:
   `public class RemoveById : CommandComposite<RemoveByIdInputData>`
   To remove a given amount of Entity from the target inventory.

8. Implement: `void UseHotBar(int numberSlotColumn)`. In which write down the usage code of the placed Entity when pressing the hot key.
In this code use the `RemoveById` command to remove the used count from inventories HotBar and Bag.

9. Done!

*For samples, a set of sprites was used:
https://assetstore.unity.com/packages/2d/gui/icons/rpg-inventory-icons-56687*