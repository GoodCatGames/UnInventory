using UnInventory.Standard;

namespace UnInventory.Samples.Sample.Trade
{
    public class UiView
    {
        private readonly ApplicationTrade _applicationTrade;
        private readonly InventoryOpenCloseObject _traderTable;
        private readonly InventoryOpenCloseObject _heroTable;
        
        public UiView(ApplicationTrade applicationTrade, InventoryOpenCloseObject traderTable, InventoryOpenCloseObject heroTable)
        {
            _applicationTrade = applicationTrade;
            _traderTable = traderTable;
            _heroTable = heroTable;
        }

        public void UpdateView()
        {
            UpdateDealSumForHero();
        }

        private void UpdateDealSumForHero()
        {
            var sumHero = ApplicationTrade.GetSumPrice(_heroTable);
            var sumTrade = ApplicationTrade.GetSumPrice(_traderTable);
            _applicationTrade.DealSumForHero = sumHero - sumTrade;
            UpdateDealText();
        }

        private void UpdateDealText()
        {
            if (!_traderTable.IsOpen || !_heroTable.IsOpen)
            {
                SetDealText(string.Empty);
                return;
            }
            var text = _applicationTrade.DealSumForHero == 0 ? "" : _applicationTrade.DealSumForHero.ToString();
            SetDealText(text);
        }

        private void SetDealText(string text)
        {
            _applicationTrade.DealText.text = text;
        }
    }
}