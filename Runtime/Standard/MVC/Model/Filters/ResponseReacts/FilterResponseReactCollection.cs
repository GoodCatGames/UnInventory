using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Filters;
using UnInventory.Core.MVC.Model.Filters.Response;
using UnInventory.Core.MVC.Model.Filters.ResponseReacts;

namespace UnInventory.Standard.MVC.Model.Filters.ResponseReacts
{
    public class FilterResponseReactCollection : Collection<IFilterResponseReact>
    {
        public void ProcessResponses(IReadOnlyFilterNoValidCollection filterNoValidCollection)
        {
            foreach (var filterNoValid in filterNoValidCollection)
            {
                foreach (var responseReact in this)
                {
                    ProcessIfNecessary(responseReact, filterNoValid);
                }
            }
        }

        /// <summary>
        /// For Interface
        /// </summary>
        /// <param name="responseReact"></param>
        /// <param name="filterNoValid"></param>
        private void ProcessIfNecessary(IFilterResponseReact responseReact, IFilterNoValid filterNoValid)
        {
            var typeResponseOpenGeneric = typeof(IFilterResponseReactConcrete<,>);
            
            var typeReactImplementation = responseReact.GetType();
            var interfacesReact = typeReactImplementation.GetInterfaces().Where(type =>
                type.IsGenericType && type.GetGenericTypeDefinition() == typeResponseOpenGeneric);

            foreach (var typeReact in interfacesReact)
            {
                // React
                var typeReactGenericTypeArguments = typeReact.GenericTypeArguments;
                var typeReactData = typeReactGenericTypeArguments.Single(typeGeneric => typeof(ICommandInputData).IsAssignableFrom(typeGeneric));
                var typeIFilterConcrete = typeof(IFilterConcrete<>).MakeGenericType(typeReactData);

                var typeReactFilter = typeReactGenericTypeArguments.Single(typeGeneric => typeIFilterConcrete.IsAssignableFrom(typeGeneric));

                // IFilterNoValid
                var typeFilter = filterNoValid.Filter.GetType();
                var typeData = filterNoValid.FilterData.GetType();

                if (typeFilter == typeReactFilter && typeReactData == typeData)
                {
                    var method = typeReactImplementation.GetMethod("Process", new[] {typeFilter, typeData} );
                    Debug.Assert(method != null, nameof(method) + " != null");
                    method.Invoke(responseReact, new object[] {filterNoValid.Filter, filterNoValid.FilterData});
                }
            }
        }
    }

   
}
