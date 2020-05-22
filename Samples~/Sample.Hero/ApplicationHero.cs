using System;
using JetBrains.Annotations;
using UnInventory.Plugins.Standard.Listeners;
using UnInventory.Core.Configuration;
using UnInventory.Core.Extensions;
using UnInventory.Core.InventoryBindings;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Core.MVC.View.Components;
using UnInventory.Standard;
using UnInventory.Standard.MVC.Model.Commands.Primary.Remove;
using UnInventory.Standard.MVC.Model.Filters.ResponseReacts;
using UnityEngine;
using UnityEngine.UI;
using UnInventory.Samples.Sample_Hero.Commands;
using UnInventory.Samples.Sample_Hero.Components;
using UnInventory.Samples.Sample_Hero.Filters;
using UnInventory.Samples.Sample_Hero.Listeners;
using UnInventory.Samples.Sample_Hero.NoFilterValidReact;
using UnInventory.SamplesEnvironment;

namespace UnInventory.Samples.Sample_Hero
{
    public class ApplicationHero : MonoBehaviour
    {
        [SerializeField] private GameObject _label;

        private const string NameBag = "Bag";
        private const string NameDummy = "Dummy";
        private const string NameHotBar = "QuickSkillsPanel";

#pragma warning disable 649
        [SerializeField] private Button _buttonOpenClose;
        [SerializeField] private GameObject _prefabDummyInventory;
        [SerializeField] private GameObject _prefabBagInventory;

        [SerializeField] private InventoryComponent _inventoryComponentHotBar;
#pragma warning restore 64

        private InventoryOpenCloseObject _dummy;
        private InventoryOpenCloseObject _bag;
        private IInventoryBinding _inventoryDataBindHotBar;

        private HeroComponent _heroComponent;

        private IDatabaseReadOnly DatabaseReadOnly => _containerDi.DatabaseReadOnly;


        private BuffDebuffListener _buffDebafListener;
        private HotBarListener _hotBarListener;
        private ChangeAmountEntityInInventoryListener _changeAmountEntityInInventoryListener;

        private ICommandsFactory Commands => _containerDi.Commands;
        private IContainerDi _containerDi => InventoryManager.ContainerDi;
        private IInventoryManager _inventoryManager => InventoryManager.Get();

        [UsedImplicitly]
        private void Start()
        {
            Init();
            BindUi();

            // Subscribes
            _buffDebafListener.On();
            _hotBarListener.On();
            _changeAmountEntityInInventoryListener.On();

            // filters
            _inventoryManager.FiltersManager.FiltersForAll.Add(new FilterDummyBodyPart(_dummy));
            _inventoryManager.FiltersManager.FiltersForAll.Add(new FilterDummyStats(_dummy, _heroComponent));
            _inventoryManager.FiltersManager.FiltersForAll.Add(new FilterHotBar(_inventoryDataBindHotBar));
            _inventoryManager.FiltersManager.FiltersForAll.Add(new FilterDummyTwoHanded(_dummy));
            _inventoryManager.FiltersManager.FiltersForAll.Add(new FilterDummyDamnedItem(_dummy));
            _inventoryManager.FiltersManager.FiltersForAll.Add(new FilterDummyStackDenied(_dummy));

            // response filters
            var responseReactCollection = new FilterResponseReactCollection
            {
                new NoFilterValidReactBodyPart(),
                new NoFilterValidReactStats(_heroComponent),
                new NoFilterValidReactTwoHanded(),
                new NoFilterValidReactDamnedItem()
            };

            InventoryManager.Hand.NoValidFiltersEvent.AddListener(responseReactCollection.ProcessResponses);

            _buttonOpenClose.onClick.Invoke();
        }

        private void Init()
        {
            // DataEntities
            _heroComponent = FindObjectOfType<HeroComponent>();
            var bagEntities = ResourcesExt.LoadDataEntities(NameBag);
            var dummyEntities = ResourcesExt.LoadDataEntities(NameDummy);

            // Inventories
            _dummy = new InventoryOpenCloseObject(_prefabDummyInventory, dummyEntities, NameDummy);
            _bag = new InventoryOpenCloseObject(_prefabBagInventory, bagEntities, NameBag);

            _inventoryDataBindHotBar = _containerDi.InventoryBindingFactory.Create(_inventoryComponentHotBar)
                .Bind(_inventoryComponentHotBar);

            // Listeners
            _buffDebafListener = new BuffDebuffListener(_heroComponent, _dummy);
            _hotBarListener = new HotBarListener(_inventoryDataBindHotBar, _bag);
            _changeAmountEntityInInventoryListener = new ChangeAmountEntityInInventoryListener(_bag);
        }

        private void BindUi()
        {
            // Bind to button
            _buttonOpenClose.onClick.AddListener(() => _dummy.OpenClose());
            _buttonOpenClose.onClick.AddListener(() => _bag.OpenClose());
        }

        [UsedImplicitly]
        public void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                UseHotBar(1);
            }

            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                UseHotBar(2);
            }
        }

        /// HotBar
        private void UseHotBar(int numberSlotColumn)
        {
            var slot = DatabaseReadOnly.GetSlotOrNull(_inventoryDataBindHotBar.DataInventory,
                new Vector2Int(numberSlotColumn, 1));
            var entity = DatabaseReadOnly.GetEntityOrNull(slot);
            if (entity == null)
            {
                return;
            }

            Label.Show($"Used {entity.name} !");

            // remove entity from panel
            Commands.Create<RemoveCommand>().EnterData(new RemoveInputData(entity, 1)).ExecuteTry();

            // remove entity from bag
            Commands.Create<RemoveById>().EnterData(new RemoveByIdInputData(_bag.DataInventory, entity.Id, 1))
                .ExecuteTry();
        }
    }
}