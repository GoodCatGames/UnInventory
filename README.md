# UnInventory - Inventory framework for Unity

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

The main goals of the project:

1. Easy creation of simple and complex inventories, hotbar, skill trees, etc.
2. Easy customization of inventory system behavior.
3. Easy functionality extension by creating your own plugins.

## Table of Contents

- [Features](#features)
- [Installation](#Installation)  
- [Quick Start](#quick-start---create-your-first-simple-inventory)
- [How to work with UnInventory](#how-to-work-with-uninventory)
- [Main parts of UnInventory](#main-parts-of-uninventory)
  - [Inventory Manager](#inventory-manager)
  - [Inventory](#inventory)
  - [Entity](#entity)
  - [Slot](#slot)
  - [Hand](#hand)
  - [Command](#command)
  - [Filter](#filter)
  - [Listener](#listener)
  - [FilterReaction](#filterreaction)
- [Samples](#samples)

## Features

- Multislot items (Width * Height)
- Stacks and split items
- Swap items
- Drag & drop supported
- Create your Views and Data for items
  - *Add the Price for the View of item, its cost in the Data of item*

- Creating your own Views for slots
  - *Add hotkey's character for the slot in the Quick Hotbar*
  
- Creating your own Datas for slots
  - *Assign to each slot of the Hero's dummy a part of the body - Head, Hand, etc*

- Easy creation of your controllers based on the class `Hand`
  - *Do you want to use your mouse to take the entire stack of an item in one click? If you dragged the taken stack out of inventory, will it be thrown out or come back?*

- Create your actions with inventory
  - *Do you want to move the needed amount of coins between inventories by clicking on the corresponding button? Will be enough to inherit CommandComposite and implement your action!*

- Creating your own Filters - prohibition of certain actions by the condition
  - *You can't put a Helmet on your hand*

- Creating your own FiltersReaction - reactions to denial of action
  - *Displaying a message to the player - "You can't take this Sword in your hand - it required 100 500 Strength!"*

- Subscriptions to actions with inventory
  - *When you put on the Devil's Ring, you add 666 points to all characteristics, when you remove it, you subtract points*

## Installation

### As unity module

This repository can be installed as unity module directly from git url. In this way new line should be added to `Packages/manifest.json`:

`"com.goodcat.un-inventory": "https://github.com/GoodCatGames/UnInventory.git"`

By default last released version will be used. If you need trunk / developing version then develop name of branch should be added after hash:

`"com.goodcat.un-inventory": "https://github.com/GoodCatGames/UnInventory.git#develop"`

*Note: UnInventory comes with some samples.
You can install the samples via the Package Manager. Select the sample you want and click "Import in project". The samples are imported into your Assets folder at /Assets/Samples/UnInventory/[version#]/*

### As source

If you can't/don't want to use unity modules, code can be downloaded as sources archive of required release from Releases page.

*Note: In this case, you can find samples in the "Samples ~" folder*

## Quick Start - Create your first simple inventory

1. Create a `Canvas` on scene
2. Add the `InventoryManager` prefab to the scene `GameObject/UnInventory/Create InventoryManager`
3. Open `GameObject/UnInventory/Inventory Creator` 
4. Select Inventory Type `Grid Support Multislot Entity`
5. Set the dimensions of the Slot (optional)
6. Set the number of columns and rows (optional)
7. Click the `Create Inventory` button
8. Save the created GameObject as a prefab, remove the object from the scene
9. Create folders `"Resources/InventoryFolder`. Create several Entities in the folder `InventoryFolder`
  
      `Click right mouse button-> Create-> UnInventory-> Entity Standard`

10. Add a button to open inventory on the scene
11. Create Empty GameObject on scene. Attach the following script to it

    ```csharp
      using UnityEngine;
      using UnityEngine.UI;
      using UnInventory.Core.Extensions;
      using UnInventory.Standard;

      public class Application : MonoBehaviour
      {
        [SerializeField] private GameObject _prefabInventory = default;
        [SerializeField] private Button _buttonOpenClose = default;

        void Start()
        {
            var entities = ResourcesExt.LoadDataEntities("InventoryFolder");
            var inventory = new InventoryOpenCloseObject(_prefabInventory, entities, "FirstInventory");
            _buttonOpenClose.onClick.AddListener(() => inventory.OpenClose());
        }
      }
    ```

12. Set `_prefabInventory` and `_buttonOpenClose` in Editor mode

13. Launch Unity in Play mode. When you click on the button, Inventory will open with the loaded items.

    Try moving, swapping and stacking the items you created. (The standard input module picks up a few items in the hand while holding the left mouse button. When you click on the right mouse button, the entire stack will be taken)

    Ok. It was a simple example of its working. You can look at more complex examples directly in the supplied package.

## How to work with UnInventory
1. Getting Started
    - create [`Inventory Manager`](#inventory-manager) on the home scene. 
2. Editor - create your own inventory in editor mode
   - [implementation of the required classes of heirs](#entity) (optionally) 
   - implementation of the heir `ContainerDiStandard`, attaching to `Inventory Manager` on the scene (replacing the standard one)
   - creating the necessary prefabs based on standard (optionally)
   - creating inventory using `Inventory Creator` using previously created classes and prefabs.  
3. Runtime - work with inventory during the game
   - Using `IInventoryBinding` or its wrappers, create runtime inventories and work with them.



## Main parts of UnInventory

### Inventory Manager

This is the central object of the system. It must and can exist in a single copy on scene.
Create it using `GameObject/UnInventory/Create InventoryManager`. Before that, you need to create a `Canvas` on the scene.

Inventory Manager must be created on the home scene in the editor mode `GameObject/UnInventory/Create InventoryManager`. It will not be destroyed when the scene changes, but it requires a `Canvas` with attached component `IsRootCanvasOnScene` at each scene.
All of your created inventories, hotbar, skill trees, etc. will be display at this `Canvas`.

When you customize/extend  your inventory system, in most cases you will need to inherit the  `ContainerDiStandard` class. The standard implementation of this class ( `ContainerDiStandard`) will need to be replaced with yours. To do this, simply remove the ContainerDiStandard component attached to the `InventoryManager` instance on the scene and attach your version.

To access the API `UnInventory` from any place of the code simply use it

```csharp
InventoryManager.Get();
```

### Inventory

Inventory is a collection of Slots on which Entity can be placed.
Inventories are created using `Inventory Creator` in Unity editor mode.

Types of inventory:

- Grid - standard inventory - slots are connected in one grid:
  - GridSupportMultislotEntity - multislot entities occupy several slots
    - *Diablo and etc*

  - Grid - multislot entities occupy one slot (migrated from another inventory will be placed in one slot)
    - *Baldur's gate*
    - *HotBar panel (Might in slot for hotkey use)*

- Free Slots - Slots are not interconnected, you can freely move and resize them. The behavior of multislot entities is similar to GridNoSuppArea.
  - *Hero's Dummy*

### Entity

An entity that is located in the Inventory. It can be an Item, Skill, etc.

May stack (set `MaxAmount> 1`)

Entities can occupy several slots in the Inventory (set `Width` and `Height`), hereinafter we will call them a multislot entity.

#### How to work with it

- Create the entities you need in Editor mode `Click right mouse button-> Create-> UnInventory`. And load them into Inventories using `Extensions.Resources.LoadDataEntities (string path)`. Or use it programmatically.
  
- If you want to add your data

  - Inherit the class from `DataEntity`

    ```csharp
      [CreateAssetMenu(fileName = "DataEntity", menuName = "UnInventory/Entity Equipment")]
      public class DataEntityEquipment : DataEntity
    ```
  
  - If, when changing the added data, the appearance should change, you need to call the event `DataChangeEvent.Invoke ()`

    ```csharp
      public int Cost {
            get => _cost;
            set
            {
               var newValue = value < 0 ? 0 : value;
               RxSimple.SetValueInvokeChangeEvent(ref _cost, newValue, DataChangeEvent);
            }
        }
        [SerializeField] private int _cost;
      ```

  - Bind your created class to the prefab.

    ```csharp
    public class ContainerDiHero : ContainerDiStandard
    {
        [SerializeField] private GameObject _equipmentPrefab;

        protected override void BindDataEntitiesToPrefabs()
        {
            base.BindDataEntitiesToPrefabs();
            BindDataEntityToPrefab<DataEntityEquipment>(_equipmentPrefab);
        }
    }
    ```

- If you need to change the view.

  - Implement your prefab `Entity` based on the standard (`Create/UnInventory/Prefabs/Entity Standard"`). The prefab must necessarily contain the components:
`[RequireComponent(typeof(IEntityRootComponent)]`.
  
    *Note: all prefab children must have a `RaycastTarget == false`!*

  - Implement EntityViewComponent, attach it to the prefab
  
      `public class EntityCostViewComponent : EntityViewComponent`
  
    *Note: `EntityViewStandardComponent` should stay!*

### Slot

A cell designed to house Entity. It has the coordinates X (row), Y (column).
These coordinates are unique within each Inventory.

#### How to work with it

- Slots are created automatically when creating an Inventory using the `Inventory Creator`.
- If you want to add your data
  - Inherit class  `[Serializable] class DataSlotDummy : DataSlot`
  - Inherit a class with an empty body
  `public class SlotDummyComponent : SlotRootComponent<DataSlotDummy> { }`
  - Choose your `SlotDummyComponent` in Inventory Creator when creating Inventory.
  
- If you need to change the view. 
  - Implement your prefab `Slot` based on the standard (`Create/UnInventory/Prefabs/Slot Standard"`). The prefab must necessarily contain the components:
  `[RequireComponent(typeof(ISlotRootComponent)]`
  
  - Implement SlotViewComponent, attach it to the prefab
  
       `public class SlotViewHotBarComponent : SlotViewComponent`
  - Select prefab in Inventory Creator when creating Inventory.

- You can add your `InputComponent` (Optional) to implement your own way of interacting with inventory. Use the `Hand` class to do this in its code.

  ```csharp
  [RequireComponent(typeof(ISlotRootComponent))]
    public class SlotHotBarInputComponent : SlotInputComponent,
         IPointerDownHandler, IPointerUpHandler, IDragHandler
    ```

- Choose your `SlotInputComponent` in Inventory Creator when creating Inventory.

### Hand

 Virtual "Hand" with which you drag and drop a certain amount of Entity. It provides visual display too.
 It is a singleton and it exists in the system in a single copy.

 #### How to work with it
Use this in the `SlotInputComponent` and / or other Input modules you implement.

### Command

Implement the heir of this class if you want to create your own action with Inventories. (*For example, move a certain amount of coins between inventories*).

- The class accepts incoming data and answers the question - Is this command possible?

  *Example*: `Commands.Create<RemoveCommand>().EnterData(new RemoveInputData(entityApple, 1))`

  If it is not possible to execute the property of the class instance `IsCanExecute == false`.
  In addition, the collection `IReadOnlyCausesCollection CausesFailure` will contain the causes for the failure.

- To perform an action, use `command.ExecuteTry()`.
  
  *Note:* Please note that after the action is completed, the state of the Inventory will change and repeated execution may not be possible.

  ```csharp
  var command = Commands.Create<RemoveCommand>().EnterData(new RemoveInputData(entity, 1));
  command.ExecuteTry(); // true
  command.ExecuteTry(); // false
  ```

`Command` has two abstract inheritors of `CommandPrimary` and `CommandComposite`. Usually you will inherit `CommandComposite`.

It only requires the implementation of the method `protected override List <ICommand> GetCommandsConsidered()`, which should return a list of commands that you plan to use your new command.

Use object `CausesCheckAndAdd` in this method to verify the feasibility and completion of the causes for failure.

This list:

- It is not required to contain only commands for which `IsCanExecute == true`
  
  - *Note:* Commands in the list for which `IsCanExecute == false` are needed to analyze the causes of failure. Now this is only for use in `FilterReaction`.
- All commands for which `IsCanExecute == true` in this list will be executed upon call `command.ExecuteTry()` in the order in which they are located in the list.
  - *Example:* The command to move a given amount of Entity (coins) between inventories will contain valid Primary commands to move from which stack to which stack or empty space.
- It is not required to contain all the commands that have been tried to achieve the action. That is, if your command can be executed successfully, further filling out the list does not make sense.

#### How to work with it

Use an instance of your implemented class as follows:

```csharp
InventoryManager.ContainerDi.Commands.Create<RemoveCommand>().EnterData(new RemoveInputData(entityApple, 1))
```

### Filter

Filters allow you to prohibit certain actions for a given condition.

*Example: You can't take the Sword in the hand if the Hero does not have enough Strength.*

To create a filter, implement one or more of the following interfaces:

- IFilterCreate
- IFilterMoveInEmptySlots
- IFilterStack
- IFilterSwap
- IFilterRemove

#### How to work with it
Add the created filter to the needed collection of the object FiltersManager:

- FiltersForAll
  - Filters will work for all actions.
- FiltersOnlyHand
  - Filters will work only for actions performed using `Hand`.

```csharp
InventoryManager.Get().FiltersManager.FiltersForAll.Add(new FilterDummyBodyPart(Dummy, _heroComponent));
```

### Listener

Subscriptions allow you to react to simple events in Inventories.

*Example: Changing the characteristics of the Hero when putting on a thing.*

Implement class `InventoryListener`

```csharp
public class HotBarListener : InventoryListener
```

#### How to work with it

Create an instance of the class and activate it

```csharp
buffDebafListener = new BuffDebafListener(_heroComponent, _Dummy);
_buffDebafListener.On();
```

### FilterReaction

Allows you to set a reaction in case of denial of action using `Hand` only because of the filters.

*Example: Conclusion of the message how much Strength is not enough to take this Sword in hand.*

#### How to work with it

1. To create a FilterReaction, implement one or more of the following interfaces:
`IFilterResponseReactConcrete`.

    ```csharp
    public class NoFilterValidReactBodyPart :
        IFilterResponseReactConcrete<FilterDummyBodyPart, MoveInputData>,
        IFilterResponseReactConcrete<FilterDummyBodyPart, SwapPrimaryInputData>
    ```

2. Then add their instances to the collection: `FilterResponseReactCollection`.

    ```csharp
    var responseReactCollection = new FilterResponseReactCollection {new FilterValidReactBodyPart(), new NoFilterValidReactStats(_heroComponent)};
    ```

3. Subscribe this collection to the event `Hand`

    ```csharp
    InventoryManager.Get().Hand.NoValidFiltersEvent.AddListener(responseReactCollection.ProcessResponses);
    ```

### Samples

- [Hero](https://github.com/GoodCatGames/UnInventory/tree/master/Samples~/Sample.Hero)
- [Trade](https://github.com/GoodCatGames/UnInventory/tree/master/Samples~/Sample.Trade)

#### Hope, you have liked the UnInventory. Look forward for your comments/suggestions.