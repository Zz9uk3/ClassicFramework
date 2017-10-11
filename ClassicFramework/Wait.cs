using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicFramework
{
    internal static class Wait
    {
        /// <summary>
        /// internal class item to save the date when we first asked
        /// if the item is ready
        /// </summary>
        private class Item
        {
            // bind this construct to a state
            internal string BindToState;
            // Should we auto reset after enough time elapsed?
            internal bool AutoReset;
            // Name ís the identifier
            internal string Name;
            // the date we asked for the item with name the first time
            internal DateTime Added;
            // constructor
            internal Item(string parName, bool parAutoReset = true,
                string parBindToState = "")
            {
                Name = parName;
                Added = DateTime.Now;
                AutoReset = parAutoReset;
                BindToState = parBindToState;
            }
        }

        // internal list which stores all items
        private static List<Item> Items = new List<Item>();
        /// <summary>
        /// Did X ms pass after starting the stopwatch for name
        /// </summary>
        internal static bool For(string parName, int parMs, bool parAutoReset = true, string parBindToState = "")
        {
            // get the item with name XX
            Item tmpItem = Items.OfType<Item>()
                .Where(i => i.Name == parName).FirstOrDefault();
            // did we find a item?
            if (tmpItem == null)
            {
                // we didnt found one! lets create it
                tmpItem = new Item(parName, parAutoReset, parBindToState);
                // and add it to the list
                Items.Add(tmpItem);
                // the time supplied in parMs didnt elapsed since
                // item creation
                return false;
            }
            else
            {
                // if inbetween the state another state got run
                // we wont count the timer as ready
                // but we remove the item and return false
                if (tmpItem.BindToState != "")
                {
                    if (tmpItem.BindToState != "")
                    {
                        Items.Remove(tmpItem);
                        return false;
                    }
                }
                // the item exists! lets check when it got created
                bool Elapsed = ((DateTime.Now - tmpItem.Added).TotalMilliseconds >= parMs);
                // the time passed in parMs elapsed since the item creation
                // remove the item and return true
                if (Elapsed && tmpItem.AutoReset) Items.Remove(tmpItem);
                return Elapsed;
            }
        }

        internal static void Remove(string parName)
        {
            Item tmpItem = Items.OfType<Item>()
                .Where(i => i.Name == parName).FirstOrDefault();
            if (tmpItem == null) return;
            Items.Remove(tmpItem);
        }
    }
}
