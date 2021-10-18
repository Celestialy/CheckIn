using MatBlazor;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Components
{
    public class CheckInPaginator : BaseMatPaginator
    {
        [Parameter]
        public bool Disabled { get; set; } = false;
    }
}
