using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace UnInventory.Plugins.Standard.Commands.MoveBetweenInventoriesCommand
{
    public static class MoveBetweenInventoriesInputDataExtension
    {
        /// <summary>
        /// Reduce the number of necessary movements
        /// </summary>
        /// <param name="inputDates"></param>
        /// <returns></returns>
        public static IEnumerable<MoveBetweenInventoriesInputData> Optimize(this IEnumerable<MoveBetweenInventoriesInputData> inputDates)
        {
            var inputDatesArray = inputDates.Where(data => data != null).ToArray();
            if (!inputDatesArray.Any())
            {
                return inputDatesArray;
            }

            var result = new List<MoveBetweenInventoriesInputData>();
            var excluding = new List<MoveBetweenInventoriesInputData>();
            foreach (var inputData in inputDatesArray)
            {
                if (excluding.Contains(inputData)) { continue; }
                var inputDatesCanSum = inputDatesArray.Where(data => inputData.InputDatesCanSum(data)).ToArray();
                excluding.AddRange(inputDatesCanSum);
                var inputDataResult = inputDatesCanSum.Aggregate(Optimize);
                result.Add(inputDataResult);
            }
            return result.Where(data => data != null);
        }

        [CanBeNull]
        public static MoveBetweenInventoriesInputData Optimize(MoveBetweenInventoriesInputData first, MoveBetweenInventoriesInputData second)
        {
            // check
            if (first.IdEntity != second.IdEntity) { throw new Exception(); }

            if (!(first.InputDatesCanSum(second)))
            {
                throw new NotSupportedException("Different Inventories!");
            }

            if (first.FromInventory == second.FromInventory)
            {
                var sum = first.Amount + second.Amount;
                var inputData = new MoveBetweenInventoriesInputData(first.FromInventory, first.ToInventory, first.IdEntity, sum);
                return inputData;
            }
            else
            {
                var sum = first.Amount - second.Amount;
                if (sum == 0)
                {
                    return null;
                }

                if (sum > 0)
                {
                    var inputData = new MoveBetweenInventoriesInputData(first.FromInventory, first.ToInventory, first.IdEntity, sum);
                    return inputData;
                }
                else
                {
                    var inputData = new MoveBetweenInventoriesInputData(second.FromInventory, second.ToInventory, first.IdEntity, -sum);
                    return inputData;
                }
            }
        }

        public static bool InputDatesCanSum(this MoveBetweenInventoriesInputData first, MoveBetweenInventoriesInputData second)
        {
            return !(first.Inventories.Concat(second.Inventories).Distinct().Count() > 2);
        }
    }
}
