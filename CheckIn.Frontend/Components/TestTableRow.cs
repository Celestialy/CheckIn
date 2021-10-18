using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Components
{
    public class TestTableRow<TItem> : BaseMatDomComponent
    {
        [CascadingParameter]
        public TableWithVirtualscroll<TItem> Table { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Selected { get; set; }

        [Parameter]
        public bool AllowSelection { get; set; }

        [Parameter]
        public TItem RowItem { get; set; }

        [Parameter]
        public EventCallback<bool> SelectedChanged { get; set; }

        public async Task ToggleSelectedAsync()
        {
            Selected = !Selected;
            await SelectedChanged.InvokeAsync(Selected);
            await Table.ToggleSelectedAsync(this);
            StateHasChanged();
        }

        public TestTableRow()
        {
            ClassMapper
                .Add("mdc-table-row")
                .If("mdc-table-row-hover", () => AllowSelection)
                .If("mdc-table-row-selected", () => Selected);
        }

        protected async Task OnClickHandler(MouseEventArgs _)
        {
            if (AllowSelection)
            {
                await ToggleSelectedAsync();
            }
        }
    }
}
