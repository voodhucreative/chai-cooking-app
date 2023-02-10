using System;
namespace ChaiCooking.Models.Custom
{
    public class SocialReward
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public SocialReward()
        {
            Name = "";
            ImageUrl = "";
        }
    }
}
