using System;
using System.Collections.Generic;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Fonts;

namespace ChaiCooking.Components.Composites
{
    public class AltDragSlider : ActiveComponent
    {
        // the drag slider is made up of several separate elements
        StackLayout SectionContiner;

        // title
        public StaticLabel Title;

        // slide bar point value labels
        List<StaticLabel> BarPointValueLabels;

        // single slide bar point value label
        public StaticLabel BarPointValueLabel;

        // slideable object (button, shape, view, etc)

        // a list of points and their associated values
        List<List<string>> BarPointValues;
        //List<int> SnapToPoints;

        // horizontal / vertical
        public int BarOrientation { get; set; }

        public int SelectedIndex { get; set; }
        public int MaxValues { get; set; }

        public string CurrentValue { get; set; }

        Grid SliderContainer { get; set; }
        public ColourButton SliderObject { get; set; }
        Grid BackgroundSliderLine { get; set; }
        Grid ForegroundSliderLine { get; set; }
        public int SliderValue { get; set; }
        public int SliderWidth { get; set; }
        public int SliderHeight { get; set; }
        public int SliderButtonSize { get; set; }
        public int SliderHorizontalMargin { get; set; }
        public int SliderVerticalMargin { get; set; }
        public int ContentMargin { get; set; }


        public AltDragSlider(string title, List<List<string>> barPointValues, int width, int height)
        {
            Content = new Xamarin.Forms.Grid();
            ContentMargin = (int)(Units.ScreenWidth - width) / 2;
            Content.Margin = new Thickness(ContentMargin, 0);
            StackLayout ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, Margin = new Thickness(0, Units.ScreenUnitXS)};
            
            SectionContiner = new StackLayout { Orientation = StackOrientation.Vertical, BackgroundColor = Color.Transparent };
            
            SelectedIndex = 0;
            MaxValues = barPointValues.Count;
            CurrentValue = "";
            SliderValue = 25;
            SliderHorizontalMargin = Units.ScreenWidth10Percent;
            SliderWidth = Units.ScreenWidth;// -(SliderHorizontalMargin * 2);
            SliderHeight = height;
            if (SliderHeight < Units.TapSizeS)
            {
                SliderHeight = Units.TapSizeS;
            }
            SliderButtonSize = 24;
            SliderVerticalMargin = SliderHeight;

            Console.WriteLine("Screen Width: " + Units.ScreenWidth);
            Console.WriteLine("Margin H: " + SliderHorizontalMargin);
            Console.WriteLine("Slider Width: " + SliderWidth);


            SliderContainer = new Grid { HeightRequest = SliderHeight, BackgroundColor=Color.Transparent, Margin=new Thickness(0, 0) };

            //BarPointValues = barPointValues;
            Title = new StaticLabel(title);
            Title.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            Title.Content.HorizontalTextAlignment = TextAlignment.Start;
            Title.Content.FontSize = Units.FontSizeXL;
            Title.Content.FontFamily = Fonts.GetFont(FontName.MontserratBold);
            Title.Content.TextColor = Color.White;

            BarPointValueLabels = new List<StaticLabel>();
            

            BarPointValueLabel = new StaticLabel(title);
            BarPointValueLabel.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            BarPointValueLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            BarPointValueLabel.Content.FontSize = Units.FontSizeL;
            BarPointValueLabel.Content.FontFamily = Fonts.GetFont(FontName.MontserratRegular);
            BarPointValueLabel.Content.TextColor = Color.White;

            // test
            BarPointValues = new List<List<string>>();
            foreach(List<string> barPointValue in barPointValues)
            {
                BarPointValues.Add(barPointValue);
            }

            SliderObject = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.Wheat, "", null);
            SliderObject.SetAsDraggable(Layouts.DragDirectionType.Horizontal);
            SliderObject.SetSize(Units.TapSizeS, Units.TapSizeS);
            SliderObject.DraggableView.InitialX = 50;
            SliderObject.DraggableView.InitialY = 0;



            SliderObject.DraggableView.DragStart += delegate (object sender, EventArgs e) { DragStarted(sender, e); };
            SliderObject.DraggableView.DragEnd += delegate (object sender, EventArgs e) { DragEnded(sender, e); };
            SliderObject.DraggableView.Dragging += delegate (object sender, EventArgs e) { Dragged(sender, e); };

            


            SliderObject.DraggableView.Content = SliderObject.ButtonShape;
            SliderObject.DraggableView.IsSnappable = true;
            


            BackgroundSliderLine = new Grid { Margin = new Thickness(0, (SliderHeight/2)-3), HeightRequest = 3, WidthRequest = Units.ScreenWidth, BackgroundColor = Color.LightGray };
            ForegroundSliderLine = new Grid { Margin = new Thickness(0, (SliderHeight / 2) - 3, (SliderWidth - SliderValue), (SliderHeight / 2) - 3), HeightRequest = 3, WidthRequest = Units.ScreenWidth, BackgroundColor = Color.FromHex(Colors.CC_ORANGE) };

            

            SliderContainer.Children.Add(BackgroundSliderLine, 0, 0);
            SliderContainer.Children.Add(ForegroundSliderLine, 0, 0);
            SliderContainer.Children.Add(SliderObject.GetContent(), 0, 0);
            SliderObject.DraggableView.SetLimits((int)(SliderContainer.X - SliderContainer.Width / 2), (int)(SliderContainer.Y - SliderContainer.Width / 2), (int)(SliderContainer.Y - SliderContainer.Height / 2), (int)(SliderContainer.Y - SliderContainer.Height / 2));

            SelectedIndex = 1;
            
            UpdateSliderObjectPosition(true);

            // multiple labels?
            /*
            int index = 0;
            foreach (List<string> barPointValue in BarPointValues)
            {
                BarPointValueLabels.Add(new StaticLabel(barPointValue[0]));
                Content.Children.Add(BarPointValueLabels[index].Content, index, 0);
                index++;
            }
            */

            // title


            // single label
            BarPointValueLabel.Content.Text = BarPointValues[SelectedIndex][0];
            SectionContiner.Children.Add(BarPointValueLabel.Content);
            SectionContiner.Children.Add(SliderContainer);

            ContentContainer.Children.Add(Title.Content);
            ContentContainer.Children.Add(SectionContiner);
            Content.Children.Add(ContentContainer);


            //SnapToPoints = CreateSnapPoints(BarPointValues.Count);
            try
            {
                //SliderObject.DraggableView.SetPositionCommand.Execute(null);
            }
            catch (Exception e) { }

            //SliderObject.DraggableView.TranslateTo(-width / 2, 0, 0, null);
            

        }

       
        /*
        public List<int> CreateSnapPoints(int numberOfPoints)
        {
            List<int> snapPoints = new List<int>();

            int intervalWidth = (int)(SliderWidth / (numberOfPoints-1));

            snapPoints.Add(0);
            for (int i = 1; i < numberOfPoints-1; i++)
            {
                snapPoints.Add(intervalWidth * i-SliderHorizontalMargin);
                Console.WriteLine((i + 1) + ": " + snapPoints[i]);
            }
            
            snapPoints.Add(SliderWidth);

            return snapPoints;
        }*/

        public override void DragStarted(object sender, EventArgs e)
        {
            //SliderObject.Button.BackgroundColor = Color.Black;
            //base.DragStarted(sender, e);
            //UpdateSliderObjectPosition();
        }

        public override void DragEnded(object sender, EventArgs e)
        {
            //base.DragEnded(sender, e);
            //SliderObject.Button.BackgroundColor = Color.Red;
            //SliderObject.DraggableView.RestorePositionCommand.Execute(null);

            //UpdateSliderObjectPosition();
            //SnapToPoint(SelectedIndex);
            //SliderObject.DraggableView.SetPositionCommand.Execute(null);
        }

        public override void Dragged(object sender, EventArgs e)
        {
            AllowDrag = true;

            Console.WriteLine("Dragged to " + SliderObject.DraggableView.MovedX);
            Console.WriteLine("Drag limits: MinX: " + SliderObject.DraggableView.MinX);
            Console.WriteLine("Drag limits: MaxX: " + SliderObject.DraggableView.MaxX);
            Console.WriteLine("Drag limits: MinY: " + SliderObject.DraggableView.MinY);
            Console.WriteLine("Drag limits: MaxY: " + SliderObject.DraggableView.MaxY);

            UpdateSliderObjectPosition(false);
            
        }

        bool AllowDrag;

        public void UpdateSliderObjectPosition(bool init)
        {
            if (!AllowDrag) { return; }

            if (init)
            {
                SliderObject.DraggableView.MovedX = (int)(SelectedIndex * (SliderWidth / BarPointValues.Count));
                SliderValue = SliderObject.DraggableView.MovedX;

                BarPointValueLabel.Content.Text = BarPointValues[SelectedIndex][0];


                int marginOffSet = (int)(SliderWidth - SliderValue) - SliderButtonSize;

                
                ForegroundSliderLine.Margin = new Thickness(0, (SliderHeight / 2) - 3, marginOffSet + (SliderButtonSize / 2) - (ContentMargin * 2), (SliderHeight / 2) - 3);



                int intervalWidth = (int)SliderContainer.Width / (BarPointValues.Count - 1);

                SliderObject.DraggableView.SnapX = SelectedIndex * intervalWidth;

                if (SelectedIndex == 0) { SliderObject.DraggableView.SnapX = 0; }
                if (SelectedIndex == (BarPointValues.Count - 1)) { SliderObject.DraggableView.SnapX = (int)SliderContainer.Width; };

                SliderObject.DraggableView.SnapY = SliderObject.DraggableView.MovedY;

            }
            else
            {
                SliderValue = SliderObject.DraggableView.MovedX;


                Console.WriteLine("Slider value: " + SliderValue);

                int percent = (int)(100 / SliderContainer.Width * SliderValue);

                if (percent > 100) { percent = 100; }
                if (percent < 0) { percent = 0; }

                Console.WriteLine(percent + "% , " + "Slider width: " + SliderContainer.Width);


                if (percent <=0 || percent >= 100)
                {
                    AllowDrag = false;
                    
                    return;
                }

                SelectedIndex = 0;


                int numberOfIntermediatePoints = BarPointValues.Count - 2;
                int valueTolerance = 100 / BarPointValues.Count;

                for (int i = valueTolerance; i < 100; i += valueTolerance)
                {
                    if (percent >= i)
                    {
                        SelectedIndex++; //next point
                    }
                }
                if (SelectedIndex > BarPointValues.Count - 1) { SelectedIndex = BarPointValues.Count - 1; }

                //BarPointValueLabel.Content.Text = percent+"%\n"+BarPointValues[SelectedIndex][0];
                BarPointValueLabel.Content.Text = BarPointValues[SelectedIndex][0];


                int marginOffSet = (int)(SliderWidth - SliderValue) - SliderButtonSize;

                //BarPointValueLabel.Content.TranslateTo(SliderObject.DraggableView.MovedX + 3, SliderObject.DraggableView.MovedY + 2, 0, null);

                SliderObject.DropShadow.TranslateTo(SliderObject.DraggableView.MovedX + 3, SliderObject.DraggableView.MovedY + 2, 0, null);




                ForegroundSliderLine.Margin = new Thickness(0, (SliderHeight / 2) - 3, marginOffSet + (SliderButtonSize / 2) - (ContentMargin * 2), (SliderHeight / 2) - 3);



                int intervalWidth = (int)SliderContainer.Width / (BarPointValues.Count - 1);

                SliderObject.DraggableView.SnapX = SelectedIndex * intervalWidth;

                if (SelectedIndex == 0) { SliderObject.DraggableView.SnapX = 0; }
                if (SelectedIndex == (BarPointValues.Count - 1)) { SliderObject.DraggableView.SnapX = (int)SliderContainer.Width; };

                SliderObject.DraggableView.SnapY = SliderObject.DraggableView.MovedY;
            }

        }

        public void SnapToPoint(int selectedIndex)
        {
            // snap to nearest point
            //SliderObject.DraggableView.SnapX = SnapToPoints[selectedIndex];
            //SliderObject.DraggableView.SnapY = SliderObject.DraggableView.MovedY;

        }
        
        public void UpdateSelected(int selectedValue)
        {

        }

        public string GetSelectedValue()
        {
            return BarPointValues[SelectedIndex][1];
        }
    }
}
