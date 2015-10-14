using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace AiJianShu.Common
{
    public class IncrementalConvertObservableCollection<T, TSource> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        private bool isBusy = false;

        private int? qureyCount;

        private Func<string, List<TSource>> qureyMoreData;

        private Func<string> getLastIndex;

        private Func<TSource, T> convert;

        public IncrementalConvertObservableCollection(Func<string, List<TSource>> moreDataFunc, Func<string> getLastIndexFunc, Func<TSource, T> convertFunc)
        {
            qureyMoreData = moreDataFunc;
            getLastIndex = getLastIndexFunc;
            convert = convertFunc;
        }

        public bool HasMoreItems
        {
            get
            {
                if (qureyCount != null && qureyCount > 0)
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

            return AsyncInfo.Run
            (
                token =>
                    Task.Run<LoadMoreItemsResult>
                    (
                        async () =>
                        {
                            await GalaSoft.MvvmLight.Threading.DispatcherHelper.RunAsync
                            (
                                () =>
                                {
                                    try
                                    {
                                        List<TSource> items = qureyMoreData(getLastIndex());
                                        this.qureyCount = items.Count;
                                        items.ForEach(x => this.Items.Add(convert(x)));
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

