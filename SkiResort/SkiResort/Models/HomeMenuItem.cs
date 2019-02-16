using System;
using System.Collections.Generic;
using System.Text;

namespace SkiResort.Models
{
    public enum MenuItemType
    {
        SkiTest,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
