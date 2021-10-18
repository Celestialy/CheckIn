using MatBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Wrappers
{
    public class MatRefWrapper<T> where T : class
    {
        public T item;
        /// <summary>
        /// Reference to the button for the menu
        /// </summary>
        public MatButton MenuButtonRef { get; set; }

        /// <summary>
        /// Reference to the menu
        /// </summary>
        public MatMenu MenuRef { get; set; }
        public MatRefWrapper(T _item)
        {
            item = _item;
        }
    }
}
