using System;
using System.Collections.Generic;
using System.Text;

namespace NinepinKiosk.App.Model
{
    public class Item
    {
        public Item(string title, string subtitle, string description)
        {
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
        }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
    }
}
