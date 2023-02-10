using System;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class InfluencerTile : ActiveComponent
    {
        Influencer.Datum Influencer;
        Grid NameBar;

        public StaticLabel NameLabel { get; set; }
        public StaticImage MainImage { get; set; }

        public InfluencerTile(Influencer.Datum influencer)
        {
            Influencer = influencer;
            if (Influencer.image_url == null || Influencer.image_url == "")
            {
                Influencer.image_url = "charactericon.png";
            }
            else
            {
                Influencer.image_url = influencer.image_url;
            }

            Influencer.display_name = influencer.display_name;
                
            Container = new Grid { VerticalOptions = LayoutOptions.EndAndExpand };

            NameBar = new Grid { Margin = new Thickness(0, Units.ThirdScreenWidth-24, 0, 0), BackgroundColor = Color.Black,Opacity = 0.5, HeightRequest = 24};

            NameLabel = new StaticLabel(influencer.display_name);
            NameLabel.CenterAlign();
            NameLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            NameLabel.Content.TextColor = Color.White;
            NameLabel.Content.Margin = new Thickness(0, Units.ThirdScreenWidth - 24, 0, 0);
            MainImage = new StaticImage(influencer.image_url, Units.ThirdScreenWidth, null);
            MainImage.Content.Aspect = Aspect.AspectFill;

            Container.Children.Add(MainImage.Content, 0, 0);
            Container.Children.Add(NameBar, 0, 0);
            Container.Children.Add(NameLabel.Content, 0, 0);

            Content.Children.Add(Container);
        }
    }
}
