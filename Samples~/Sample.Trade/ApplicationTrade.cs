using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Plugins.Standard.Commands.MoveBetweenInventoriesCommand;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Standard;
using UnityEngine;
using UnityEngine.UI;
using UnInventory.Samples.Sample.Trade.Commands.Balance;
using UnInventory.Samples.Sample.Trade.Data;
using UnInventory.Samples.Sample.Trade.Filters;
using UnInventory.Samples.Sample.Trade.Listeners;
using UnInventory.SamplesEnvironment;
using Button = UnityEngine.UI.Button;

namespace UnInventory.Samples.Sample.Trade
{
    public class ApplicationTrade : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Vector2 _positionHeroBagInventory;

        [SerializeField] private Vector2 _positionTraderBagInventory;
        [SerializeField] private Vector2 _positionHeroTableInventory;
        [SerializeField] private Vector2 _positionTraderTableInventory;

        [SerializeField] private Button _buttonTrade;
        [SerializeField] private Button _buttonBalance;
        [SerializeField] private Button _buttonDoTrade;

        [SerializeField] private GameObject _dataEntityPrice;

        [SerializeField] private GameObject _prefabBagInventory;
        [SerializeField] private GameObject _prefabTableInventory;
#pragma warning restore 649

        [SerializeField] public Text DealText;

        private InventoryOpenCloseObject _heroBag;
        private InventoryOpenCloseObject _traderBag;
        private InventoryOpenCloseObject _heroTable;
        private InventoryOpenCloseObject _traderTable;

        public int DealSumForHero;
        private const string IdCoins = "coins";
        private const string NameInventoryHeroBag = "HeroBag";
        private const string NameInventoryTraderBag = "TraderBag";
        private const string NameInventoryHeroTable = "HeroTable";
        private const string NameInventoryTraderTable = "TraderTable";

        private const string PathToEntitiesHeroBag = "HeroBag";
        private const string PathToEntitiesTraderBag = "TraderBag";

        private IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly;

        private UiView _uiView;
        private UpdateUiViewListener _updateUiViewListener;
        private IFiltersManager _filtersManager;

        private ICommandsFactory Commands => InventoryManager.ContainerDi.Commands;

        [UsedImplicitly]
        private void Start()
        {
            Init();
            BindUi();

            // Subscribes
            _updateUiViewListener.On();
            
            _filtersManager.FiltersForHandOnly.Add(new FilterTrade(new List<IDataInventoryContainer>() {_heroBag, _heroTable}));
            _filtersManager.FiltersForHandOnly.Add(new FilterTrade(new List<IDataInventoryContainer>() {_traderBag, _traderTable}));

            // Open
            ButtonTradeOnClick();
        }

        [UsedImplicitly]
        public void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void Init()
        {
            // Bind
            var heroEntities = Core.Extensions.ResourcesExt.LoadDataEntities(PathToEntitiesHeroBag);
            var traderEntities = Core.Extensions.ResourcesExt.LoadDataEntities(PathToEntitiesTraderBag);

            _heroBag = new InventoryOpenCloseObject(_prefabBagInventory, heroEntities, NameInventoryHeroBag);
            _traderBag = new InventoryOpenCloseObject(_prefabBagInventory, traderEntities, NameInventoryTraderBag);
            _heroTable = new InventoryOpenCloseObject(_prefabTableInventory, null, NameInventoryHeroTable);
            _traderTable = new InventoryOpenCloseObject(_prefabTableInventory, null, NameInventoryTraderTable);

            _uiView = new UiView(this, _traderTable, _heroTable);
            _updateUiViewListener = new UpdateUiViewListener(_uiView);

            _filtersManager = InventoryManager.Get().FiltersManager;
        }

        private void BindUi()
        {
            // Bind to button
            _buttonTrade.onClick.AddListener(ButtonTradeOnClick);
            _buttonBalance.onClick.AddListener(BalanceTables);
            _buttonDoTrade.onClick.AddListener(DoTrade);
        }

        private void ButtonTradeOnClick()
        {
            _heroBag.OpenClose(_positionHeroBagInventory);
            _traderBag.OpenClose(_positionTraderBagInventory);
            _heroTable.OpenClose(_positionHeroTableInventory);
            _traderTable.OpenClose(_positionTraderTableInventory);

            _uiView.UpdateView();
        }

        private void BalanceTables()
        {
            var hero = new BagTableStruct(_heroBag, _heroTable);
            var trader = new BagTableStruct(_traderBag, _traderTable);
            var command = (BalanceTables) Commands.Create<BalanceTables>()
                .EnterData(new BalanceTablesInputData(hero, trader, IdCoins));
            command.ExecuteTry();

            var displayCauses = new DisplayCauses(hero.Bag.DataInventory, trader.Bag.DataInventory);
            displayCauses.Display(command);
        }

        /// <summary>
        /// Here it is implemented with the help of the function, but in a real project it is better to implement the CommandComposite
        /// </summary>
        private void DoTrade()
        {
            var sumHero = GetSumPrice(_heroTable);
            var sumTrader = GetSumPrice(_traderTable);
            if (sumHero == 0 && sumTrader == 0)
            {
                return;
            }

            if (DealSumForHero != 0)
            {
                Label.Show("Need Balance!");
                return;
            }

            MoveAllEntities(_heroTable.DataInventory, _traderBag.DataInventory);
            MoveAllEntities(_traderTable.DataInventory, _heroBag.DataInventory);
        }

        /// <summary>
        /// Here it is implemented with the help of the function, but in a real project it is better to implement the CommandComposite
        /// </summary>
        /// <param name="fromInventory"></param>
        /// <param name="toInventory"></param>
        private void MoveAllEntities(DataInventory fromInventory, DataInventory toInventory)
        {
            var command = Commands.Create<MoveBetweenInventoriesCommand>();

            var idAmountDictionary = DatabaseReadOnly.GetDataEntitiesInventory(fromInventory)
                .ToDictionary(entity => entity, entity => entity.Amount)
                .GroupBy(pair => pair.Key.Id)
                .ToDictionary(pairs => pairs.Key, pairs => pairs.Sum(pair => pair.Value));

            foreach (var pair in idAmountDictionary)
            {
                command.EnterData(new MoveBetweenInventoriesInputData(fromInventory, toInventory, pair.Key,
                            pair.Value))
                        .ExecuteTry();
            }
        }

        public static int GetSumPrice(IDataInventoryContainer dataInventoryContainer)
        {
            var databaseReadOnly = InventoryManager.ContainerDi.DatabaseReadOnly;
            var entities = databaseReadOnly.GetDataEntitiesInventory(dataInventoryContainer.DataInventory);
            var sum = entities.OfType<DataEntityPrice>().Sum(data => data.Price * data.Amount);
            return sum;
        }
    }
}
