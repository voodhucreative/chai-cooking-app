using System;
using System.Collections.Generic;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using Xamarin.Forms;
using XFShapeView;

namespace ChaiCooking.Components.Composites
{
    public class ValueAdjuster : ActiveComponent
    {
        public ActiveImage PlusIcon;
        public ActiveImage MinusIcon;
        public StaticLabel Title;
        public StaticLabel ValueLabel;
        public ShapeView BackgroundShape;
        public Grid ContentContainer;
        public StackLayout ControlContainer;

        List<KeyValuePair<string, float>> FloatValues;
        int CurrentFloatValueIndex;
        float CurrentFloatValue;
        float MinFloatValue;
        float MaxFloatValue;
        float StepFloatValue;
        int FormatDp;

        List<KeyValuePair<string, int>> IntValues;
        int CurrentIntValueIndex;
        int CurrentIntValue;
        int MinIntValue;
        int MaxIntValue;
        int StepIntValue;

        List<KeyValuePair<string, string>> StringValues;
        int CurrentStringValueIndex;
        string CurrentStringValue;

        const int TYPE_FLOAT = 0;
        const int TYPE_INT = 1;
        const int TYPE_STRING = 2;

        int ValueType;
        int CurrentValueIndex;

        // does this handle a range, based on min and max?
        // if so, it's a continuous value adjuster (true)
        // other wise, it's a value list adjuster (false)
        bool Continuous;

        public ValueAdjuster(string title, int width, int height)
        {
            int controlWidth = height*5;
            int controlHeight = height;
            int cornerRadius = height / 2;

            CurrentFloatValue = 0.0f;
            CurrentIntValue = 0;
            CurrentStringValue = "";

            CurrentFloatValueIndex = 0;
            CurrentIntValueIndex = 0;
            CurrentStringValueIndex = 0;
            
            Continuous = true;

            BackgroundShape = new ShapeView
            {
                ShapeType = ShapeType.Box,
                WidthRequest = controlWidth,
                HeightRequest = controlHeight,
                Color = Color.Orange,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                CornerRadius = cornerRadius
            };

            ContentContainer = new Grid
            {
                BackgroundColor = Color.Transparent,
                //WidthRequest = controlWidth,
                //HeightRequest = controlHeight,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ContentContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            ContentContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            

            ControlContainer = new StackLayout {
                WidthRequest = controlWidth,
                HeightRequest = controlHeight,
                //Spacing = 8,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Title = new StaticLabel(title);
            Title.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.HorizontalTextAlignment = TextAlignment.Center;
            Title.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.VerticalTextAlignment = TextAlignment.Center;

            ValueLabel = new StaticLabel(CurrentStringValue.ToString());
            ValueLabel.Content.BackgroundColor = Color.White;
            ValueLabel.Content.WidthRequest = controlWidth - (controlHeight * 2);
            ValueLabel.Content.HeightRequest = controlHeight;
            ValueLabel.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            ValueLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            ValueLabel.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            ValueLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            // pre-create these in asset manager
            PlusIcon = new ActiveImage("plus.png", 24, 24, null, null);
            MinusIcon = new ActiveImage("minus.png", 24, 24, null, null);

            PlusIcon.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            PlusIcon.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            PlusIcon.Content.Margin = new Thickness(0, 4, 8, 4);

            MinusIcon.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            MinusIcon.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            MinusIcon.Content.Margin = new Thickness(8, 4, 0, 4);


            PlusIcon.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                Increase();
                            });
                        })
                    }
                );

            MinusIcon.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                Decrease();
                            });
                        })
                    }
                );

            ControlContainer.Children.Add(MinusIcon.Content);

            ControlContainer.Children.Add(ValueLabel.Content);

            ControlContainer.Children.Add(PlusIcon.Content);



            ContentContainer.Children.Add(BackgroundShape, 0, 0);
            ContentContainer.Children.Add(ControlContainer, 0, 0);
            ContentContainer.Children.Add(Title.Content, 0, 1);



            Container.Children.Add(ContentContainer);
        }


        public void SetTitleTop()
        {
            ContentContainer.Children.Clear();
            ContentContainer.Children.Add(Title.Content, 0, 0);
            ContentContainer.Children.Add(BackgroundShape, 0, 1);
            ContentContainer.Children.Add(ControlContainer, 0, 1);
        }

        public void SetTitleBottom()
        {
            ContentContainer.Children.Clear();
            ContentContainer.Children.Add(BackgroundShape, 0, 0);
            ContentContainer.Children.Add(ControlContainer, 0, 0);
            ContentContainer.Children.Add(Title.Content, 0, 1);
        }

        public void LeftAlign()
        {
            Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            ContentContainer.HorizontalOptions = LayoutOptions.StartAndExpand;
            ControlContainer.HorizontalOptions = LayoutOptions.StartAndExpand;
        }

        public void RightAlign()
        {
            Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            ContentContainer.HorizontalOptions = LayoutOptions.EndAndExpand;
            ControlContainer.HorizontalOptions = LayoutOptions.EndAndExpand;
        }

        public void CenterAlign()
        {
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            ContentContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;
            ControlContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;
        }

        public void SetIntValues(List<KeyValuePair<string, int>> keyValuePairs)
        {
            Continuous = false;
            ValueType = TYPE_INT;
            IntValues = new List<KeyValuePair<string, int>>();
            IntValues = keyValuePairs;

            UpdateIntValue();
        }

        public void SetFloatValues(List<KeyValuePair<string, float>> keyValuePairs)
        {
            Continuous = false;
            ValueType = TYPE_FLOAT;
            FloatValues = new List<KeyValuePair<string, float>>();
            FloatValues = keyValuePairs;

            UpdateFloatValue();
        }

        public void SetStringValues(List<KeyValuePair<string, string>> keyValuePairs)
        {
            Continuous = false;
            ValueType = TYPE_STRING;
            StringValues = new List<KeyValuePair<string, string>>();
            StringValues = keyValuePairs;

            UpdateStringValue();
        }

        public void SetFloatValueRange(float min, float max, float current, float step)
        {
            Continuous = true;
            ValueType = TYPE_FLOAT;
            CurrentFloatValue = current;
            MinFloatValue = min;
            MaxFloatValue = max;
            StepFloatValue = step;

            FormatDp = 0;

            if (StepFloatValue < 1)
            {
                FormatDp = 1;
            }
            if (StepFloatValue < 0.1)
            {
                FormatDp = 2;
            }

            UpdateFloatValue();
        }

        public void SetIntValueRange(int min, int max, int current, int step)
        {
            Continuous = true;
            ValueType = TYPE_INT;
            CurrentFloatValue = current;
            MinIntValue = min;
            MaxIntValue = max;
            StepIntValue = step;

            UpdateIntValue();
        }

        public void IncreaseFloatValue()
        {
            if (Continuous)
            {
                if (CurrentFloatValue + StepFloatValue <= MaxFloatValue)
                {
                    CurrentFloatValue += StepFloatValue;
                }
            }
            else
            {
                if(CurrentFloatValueIndex < FloatValues.Count-1)
                {
                    CurrentFloatValueIndex++;
                }
            }
            UpdateFloatValue();
        }

        public void DecreaseFloatValue()
        {
            if (Continuous)
            {
                if (CurrentFloatValue - StepFloatValue >= MinFloatValue)
                {
                    CurrentFloatValue -= StepFloatValue;
                }
            }
            else
            {
                if (CurrentFloatValueIndex > 0)
                {
                    CurrentFloatValueIndex--;
                }
            }
            UpdateFloatValue();
        }

        private void IncreaseIntValue()
        {
            if (Continuous)
            {
                if (CurrentIntValue + StepIntValue <= MaxIntValue)
                {
                    CurrentIntValue += StepIntValue;
                }
            }
            else
            {
                if (CurrentIntValueIndex < IntValues.Count-1)
                {
                    CurrentIntValueIndex++;
                }
            }
            UpdateIntValue();
        }

        private void DecreaseIntValue()
        {
            if (Continuous)
            {
                if (CurrentIntValue - StepIntValue >= MinIntValue)
                {
                    CurrentIntValue -= StepIntValue;
                }
            }
            else
            {
                if(CurrentIntValueIndex > 0)
                {
                    CurrentIntValueIndex--;
                }
            }
            UpdateIntValue();
        }

        private void IncreaseStringValue()
        {
            if(CurrentStringValueIndex < StringValues.Count-1)
            {
                CurrentStringValueIndex++;
            }
            UpdateStringValue();
        }

        private void DecreaseStringValue()
        {
            if(CurrentStringValueIndex > 0)
            {
                CurrentStringValueIndex--;
            }
            UpdateStringValue();
        }

        private void UpdateFloatValue()
        {
            if(!Continuous)
            {
                CurrentFloatValue = FloatValues[CurrentFloatValueIndex].Value;
            }

            ValueLabel.Content.Text = CurrentFloatValue.ToString();

            if (FormatDp == 1)
            {
                ValueLabel.Content.Text = CurrentFloatValue.ToString("0.0");
            }

            if (FormatDp == 2)
            {
                ValueLabel.Content.Text = CurrentFloatValue.ToString("0.00");
            }
        }

        private void UpdateIntValue()
        {
            if(!Continuous)
            {
                CurrentIntValue = IntValues[CurrentStringValueIndex].Value;
            }
            ValueLabel.Content.Text = IntValues[CurrentIntValueIndex].Key;// CurrentIntValue.ToString();
        }

        private void UpdateStringValue()
        {
            if(!Continuous)
            {
                CurrentStringValue = StringValues[CurrentStringValueIndex].Value;
            }
            ValueLabel.Content.Text = CurrentStringValue;
        }

        public string GetValue()
        {
            switch(ValueType)
            {
                case TYPE_FLOAT:
                    UpdateFloatValue();
                    return ""+CurrentFloatValue;
                case TYPE_INT:
                    UpdateIntValue();
                    return ""+CurrentIntValue;
                case TYPE_STRING:
                    return CurrentStringValue;
            }
            throw new FrameworkException("Unknown value type in ValueAdjuster : " + this.ToString());
        }

        public void Increase()
        {
            switch (ValueType)
            {
                case TYPE_FLOAT:
                    IncreaseFloatValue();
                    break;
                case TYPE_INT:
                    IncreaseIntValue();
                    break;
                case TYPE_STRING:
                    IncreaseStringValue();
                    break;
            }
            //throw new FrameworkException("Unknown value type in ValueAdjuster : " + this.ToString());
        }

        public void Decrease()
        {

            switch (ValueType)
            {
                case TYPE_FLOAT:
                    DecreaseFloatValue();
                    break;
                case TYPE_INT:
                    DecreaseIntValue();
                    break;
                case TYPE_STRING:
                    DecreaseStringValue();
                    break;
            }
            //throw new FrameworkException("Unknown value type in ValueAdjuster : " + this.ToString());
        }


        public void Set()
        {

        }

        public void Reset()
        {

        }

        public void UpdateValue()
        {
            switch (ValueType)
            {
                case TYPE_FLOAT:
                    UpdateFloatValue();
                    break;
                case TYPE_INT:
                    UpdateIntValue();
                    break;
                case TYPE_STRING:
                    UpdateStringValue();
                    break;
            }
        }

    }
}
