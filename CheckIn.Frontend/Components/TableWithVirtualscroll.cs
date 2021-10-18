using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Components
{
    public class TableWithVirtualscroll<TItem> : BaseMatDomComponent, IMatVirtualScroll
    {
        public TestTableRow<TItem> Current { get; private set; }
        [Parameter]
        public int ItemHeight { get; set; } = 50;

        [Parameter]
        public RenderFragment<TItem> MatTableRow { get; set; }

        [Parameter]
        public RenderFragment MatTableHeader { get; set; }
        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        
        [Parameter]
        public bool AllowSelection { get; set; }

        [Parameter]
        public Action<object> SelectionChanged { get; set; }
        [Parameter]
        public IEnumerable<TItem> Items { get; set; }
        
        public MatDotNetObjectReference<MatVirtualScrollJsHelper> JsHelperReference { get; set; }

        public TableWithVirtualscroll()
        {
            ClassMapper.Add("mdc-table");

            CallAfterRender(async () =>
            {
                if (!Disabled)
                {
                    JsHelperReference =
                        new MatDotNetObjectReference<MatVirtualScrollJsHelper>(new MatVirtualScrollJsHelper(this));
                    var scrollView = await Js.InvokeAsync<MatVirtualScrollView>("matBlazor.matVirtualScroll.init", Ref,
                        JsHelperReference.Reference);
                    this.SetScrollView(scrollView);
                }
            });
        }

        public void VirtualScrollingSetView(MatVirtualScrollView scrollView)
        {
            this.SetScrollView(scrollView);
        }


        public override void Dispose()
        {
            base.Dispose();
            // todo call js to dispose events (window resize)
            JsHelperReference?.Dispose();
        }


        protected string GetContentStyle()
        {
            if (Disabled)
            {
                return $"height: {Items.Count() * ItemHeight}px;";
            }

            return
                $"height: {(ScrollViewResult.Height - ScrollViewResult.SkipItems * ItemHeight)}px; padding-top: {(ScrollViewResult.SkipItems * ItemHeight)}px;";
        }

        protected IEnumerable<TItem> GetContentItems()
        {
            if (Disabled)
            {
                return Items;
            }

            return
                Items.Skip(ScrollViewResult.SkipItems).Take(ScrollViewResult.TakeItems);
        }

        private void SetScrollView(MatVirtualScrollView scrollView)
        {
            this.ScrollView = scrollView;
            this.ScrollViewResult = new MatVirtualScrollViewResult
            {
                Height = Items.Count() * ItemHeight,
                SkipItems = scrollView.ScrollTop / this.ItemHeight
            };
            this.ScrollViewResult.TakeItems =
                (int)Math.Ceiling((double)(scrollView.ScrollTop + scrollView.ClientHeight) / (double)ItemHeight) -
                this.ScrollViewResult.SkipItems;
            //            Console.WriteLine(ScrollViewResult.SkipItems + " " + ScrollViewResult.TakeItems);
            this.StateHasChanged();
        }

        public MatVirtualScrollViewResult ScrollViewResult { get; set; }

        public MatVirtualScrollView ScrollView { get; set; }

        public async Task ToggleSelectedAsync(TestTableRow<TItem> row)
        {
            if (row.Selected)
            {
                var current = Current;
                Current = row;

                if (current != null && current != row && current.Selected)
                {
                    await current.ToggleSelectedAsync();
                }
                SelectionChanged?.Invoke(Current.RowItem);
            }
            else
            {
                SelectionChanged?.Invoke(null);
            }
        }
    }
}
