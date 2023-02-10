using System;
using System.Collections.Generic;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Models.Custom;
using FFImageLoading.Transformations;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class FolderTile : StandardLayout
    {
        StaticImage FolderActive;
        //StaticImage FolderInactive;
        StaticImage TintImage;
        public Label Title;
        public bool IsActive { get; set; }
        public bool IsDraggable { get; set; }

        public FolderTile(string title, bool isActive, bool isDraggable)
        {

            IsActive = isActive;

            Title = new Label
            {
                Text = title,
                BackgroundColor = Color.Transparent,
                FontSize = 12,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 100,
                HeightRequest = 64,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(4, 12, 4, 0)
            };

            Content.HeightRequest = 100;
            Content.BackgroundColor = Color.Transparent;
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Content.VerticalOptions = LayoutOptions.CenterAndExpand;

            //FolderInactive = new StaticImage("tintfolder.png", 100, null);
            //FolderActive = new StaticImage("folderfg.png", 120, null);
            FolderActive = new StaticImage("folderbg.png", 104, null);
            TintImage = new StaticImage("tintfolder.png", 100, null);
            TintImage.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            TintImage.Content.Transformations.Add(new FFImageLoading.Transformations.TintTransformation { HexColor = "#ffffffff" });



            FolderActive.Content.Aspect = Aspect.AspectFit;

            TintImage.Content.Aspect = Aspect.AspectFit;


            //Container.Children.Add(FolderInactive.Content, 0, 0);
            Container.Children.Add(FolderActive.Content, 0, 0);
            Container.Children.Add(TintImage.Content, 0, 0);
            Container.Children.Add(Title, 0, 0);

            if (IsActive)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }

            if (IsDraggable)
            {

                DraggableView = new DraggableView
                {
                    BackgroundColor = Color.Transparent,
                    WidthRequest = Content.Width,
                    HeightRequest = Content.Height,
                    DragMode = DragMode.Touch,
                    DragDirection = DragDirectionType.All
                };

                DraggableView.DragStart += delegate (object sender, EventArgs e) { DragStarted(sender, e); };
                DraggableView.DragEnd += delegate (object sender, EventArgs e) { DragEnded(sender, e); };

                //DraggableView.GestureRecognizers.Add(leftSwipeGesture);
                //DraggableView.GestureRecognizers.Add(rightSwipeGesture);
                //DraggableView.GestureRecognizers.Add(upSwipeGesture);
                //DraggableView.GestureRecognizers.Add(downSwipeGesture);

                DraggableView.Content = Container;

                Content.Children.Add(DraggableView, 0, 0);
            }
            else
            {
                Content.Children.Add(Container);
            }

            //Content.Children.Add(Container);
        }

        private void DragEnded(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Deactivate();
            DraggableView.RestorePositionCommand.Execute(null);
        }

        public void Toggle()
        {
            IsActive = !IsActive;

            if (IsActive)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        private void DragStarted(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Activate();
        }

        public void Activate()
        {
            //FolderInactive.Content.IsVisible = false;
            FolderActive.Content.IsVisible = true;

        }

        public void Deactivate()
        {
            //FolderInactive.Content.IsVisible = false;
            FolderActive.Content.IsVisible = false;
        }

        public void SetActiveColor(Color color)
        {
            //Title.Content.BackgroundColor = color;

            int albumId = 0;
            foreach (Album album in AppSession.CurrentUser.Albums)
            {
                if (album.Name == Title.Text)
                {
                    AppSession.CurrentUser.Albums[albumId].FolderColor = color.ToHex();
                }
                albumId++;
            }

            Tint(color);
        }

        public void Tint(string color)
        {
            string colorVal = "#" + color;

            Container.Children.Remove(TintImage.Content);
            Container.Children.Remove(Title);

            TintImage = new StaticImage("tintfolder.png", 100, null);
            TintImage.Content.Opacity = 1;
            TintImage.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            TintImage.Content.Transformations.Add(new TintTransformation { HexColor = colorVal });

            Container.Children.Add(TintImage.Content, 0, 0);
            Container.Children.Add(Title, 0, 0);

        }

        public void Tint(Color color)
        {
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)color.ToHex(),
                EnableSolidColor = true

            };

            //colorTint = new TintTransformation("#388459");

            //Container.Children.Clear();
            Container.Children.Remove(TintImage.Content);
            Container.Children.Remove(Title);

            TintImage = new StaticImage("tintfolder.png", 100, null);
            TintImage.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            TintImage.Content.Transformations.Add(colorTint);
            TintImage.Content.Opacity = 1;

            Container.Children.Add(TintImage.Content, 0, 0);
            Container.Children.Add(Title, 0, 0);
        }

        public void SetScale(double size)
        {
            TintImage.Content.Scale = size;
        }
    }
}
