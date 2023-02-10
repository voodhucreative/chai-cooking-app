using System;
using System.Threading.Tasks;
using ChaiCooking.Tools;
using Xamarin.Forms;

namespace ChaiCooking.Layouts
{
    public class StandardLayout : ICloneable
    {
        public Grid Content;
        public Grid Container;
        public DraggableView DraggableView;

        public int Height { get; set; }
        public int Width { get; set; }
        public uint TransitionTime { get; set; }
        public int TransitionType { get; set; }

        public string Id { get; set; }

        public StandardLayout()
        {
            Content = new Grid { };
            Container = new Grid { };
        }

        public Grid GetContent()
        {
            return this.Content;
        }

        public void SetWidth(int width)
        {
            Width = width;
            Content.WidthRequest = width;
        }

        public void SetHeight(int height)
        {
            Height = height;
            Content.HeightRequest = height;
        }

        public async Task<bool> Show()
        {
            Content.IsVisible = true;
            switch (TransitionType)
            {
                case (int)AppSettings.TransitionTypes.SlideOutTop:
                case (int)AppSettings.TransitionTypes.SlideOutBottom:
                case (int)AppSettings.TransitionTypes.SlideOutLeft:
                case (int)AppSettings.TransitionTypes.SlideOutRight:
                    await Task.WhenAll(
                        Content.TranslateTo(0, 0, TransitionTime, Easing.Linear),
                        Content.FadeTo(1, TransitionTime, Easing.Linear)
                        );
                    break;
                case (int)AppSettings.TransitionTypes.FadeOut:
                    await Task.WhenAll(
                        Content.FadeTo(1, TransitionTime, Easing.Linear)
                        );
                    break;
            }

            if (DraggableView != null)
            {
                DraggableView.RestorePositionCommand.Execute(null);
            }
            return true;
        }

        public async Task<bool> Hide()
        {
            switch (TransitionType)
            {
                case (int)AppSettings.TransitionTypes.SlideOutTop:
                    await Task.WhenAll(
                        Content.TranslateTo(0, -Height, TransitionTime, Easing.Linear)
                        );
                    break;
                case (int)AppSettings.TransitionTypes.SlideOutBottom:
                    await Task.WhenAll(
                        Content.TranslateTo(0, -Height, TransitionTime, Easing.Linear)
                        );
                    break;
                case (int)AppSettings.TransitionTypes.SlideOutLeft:
                    await Task.WhenAll(
                        Content.TranslateTo(-Width, 0, TransitionTime, Easing.Linear),
                        Content.FadeTo(0, TransitionTime, Easing.Linear)
                        );
                    break;
                case (int)AppSettings.TransitionTypes.SlideOutRight:
                    await Task.WhenAll(
                        Content.TranslateTo(Width, 0, TransitionTime, Easing.Linear),
                        Content.FadeTo(0, TransitionTime, Easing.Linear)
                        );
                    break;
                case (int)AppSettings.TransitionTypes.FadeOut:
                    await Task.WhenAll(
                        Content.FadeTo(0, TransitionTime, Easing.Linear)
                        );
                    break;
            }
            Content.IsVisible = false;
            return true;
        }

        public virtual void Update()
        {
            if (this.Id != null)
            {
                Console.WriteLine("Updating StandardLayout: " + Id);
            }
        }

        public object Clone()
        {
            return (StandardLayout)this.MemberwiseClone();
        }
    }
}
