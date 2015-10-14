using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AiJianShu.Common
{
    public class IncrementalObservableCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        protected bool isBusy = false;

        protected int? qureyCount;

        public delegate Task<List<T>> QueryData();

        private QueryData queryData;

        public IncrementalObservableCollection(QueryData queryData)
        {
            this.queryData = queryData;
        }
           
        public bool HasMoreItems
        {
            get
            {
                if(qureyCount == null || qureyCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (isBusy)
            {
                return AsyncInfo.Run(token => Task.Run<LoadMoreItemsResult>(() => new LoadMoreItemsResult { Count = (uint)this.Count }, token));
            }

            isBusy = true;

            var dispatcher = Window.Current.Dispatcher;
            return AsyncInfo.Run
            (
                token =>
                    Task.Run<LoadMoreItemsResult>
                    (
                        async () =>
                        {
                            await dispatcher.RunAsync(
                                    CoreDispatcherPriority.Normal, async () =>
                                    {
                                        try
                                    {
                                        List<T> items = await queryData();
                                        this.qureyCount = items.Count;
                                        items.ForEach(x => this.Items.Add(x));
                                    }
                                    finally
                                    {
                                        isBusy = false;
                                    }
                                }
                              );
                            return new LoadMoreItemsResult { Count = (uint)this.Count };
                        },
                        token
                   )
             );
        }
    }
}
