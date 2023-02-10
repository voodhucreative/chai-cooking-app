using System;
using ChaiCooking.Components.Images;

namespace ChaiCooking.Models
{
    public class MenuLink
    {
        public string Name { get; set; }
        public string Target { get; set; }
        public Action LinkAction { get; set; }
        public string ImageSource { get; set; }
        public bool IsActive { get; set; }

        public MenuLink(string name, string target, Action linkAction, string imgSource, bool isActive)
        {
            Name = name;
            Target = target;
            LinkAction = linkAction;
            ImageSource = imgSource;
            IsActive = isActive;
        }
    }
}
