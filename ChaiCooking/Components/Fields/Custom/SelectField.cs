using System;
using System.Collections.Generic;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Tools;
using Xamarin.Forms;

namespace ChaiCooking.Components.Fields.Custom
{
	public class SelectField
	{
		public StackLayout Content { get; set; }
		//public StaticImage Icon { get; set; }
		public Label Label;
		public CustomPicker Picker;
		public List<string> Items { get; set; }
		protected string SelectedItem;

		public SelectField(string title, List<string> items)
		{
			this.Items = items;

			if (this.Items.Count == 0)
			{
				this.Items.Add("");
			}

			this.SelectedItem = this.Items[0];

			Content = new StackLayout
			{
				Orientation = StackOrientation.Vertical
			};

			Label = new Label
			{
				Text = title,
				FontSize = 18,
				BackgroundColor = Color.Transparent,
				TextColor = Color.FromHex(Colors.DARK_GREY),
				FontFamily = Fonts.GetBoldAppFont(),
				HorizontalTextAlignment = TextAlignment.Center
			};

			Label.HorizontalOptions = LayoutOptions.Start;

			//Icon = ImageTools.Tint(new StaticImage(AppImages.down_arrow_in_circle, 24, null), Color.FromHex(Colors.GD_BUTTON_PINK));
			//Icon.Content.HorizontalOptions = LayoutOptions.End;
			//Icon.Content.Margin = new Thickness(Units.ScreenUnitM, 0);

			Picker = new CustomPicker
			{
				Title = title,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				WidthRequest = Units.LargeButtonWidth,
				FontFamily = Fonts.GetRegularAppFont(),
				FontSize = 16,
				BackgroundColor = Color.White,
				TextColor = Color.Black,
				Margin = new Thickness(Units.ScreenUnitM, 0),
				HorizontalTextAlignment = TextAlignment.Center,
			};

			foreach (string item in this.Items)
			{
				Picker.Items.Add(item);
			}

			Picker.SelectedIndex = 0;
			Picker.SelectedIndexChanged += this.Picker_SelectedIndexChanged;


			Content.Margin = new Thickness(Units.ScreenUnitM, Units.ScreenUnitM);

			Grid optionContainer = new Grid { HeightRequest = Units.TapSizeM, ColumnSpacing = 0, RowSpacing = 0, Padding = new Thickness(Units.ScreenUnitXS, 0) };
			optionContainer.Children.Add(new Frame { VerticalOptions = LayoutOptions.CenterAndExpand, BackgroundColor = Color.White, WidthRequest = Units.LargeButtonWidth, HeightRequest = Units.TapSizeM, BorderColor = Color.Transparent, CornerRadius = (int)(Units.TapSizeM / 2), HasShadow = false, }, 0, 0);
			optionContainer.Children.Add(Picker, 0, 0);
			//optionContainer.Children.Add(Icon.Content, 0, 0);

			Content.Children.Add(Label);
			Content.Children.Add(optionContainer);
		}

		private void Picker_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SelectedItem = Picker.Items[Picker.SelectedIndex];
		}

		public void HideLabel()
		{
			Picker.Margin = 0;
			Content.Margin = 0;
			Content.Children.Remove(Label);
		}

		public string GetSelectedItem()
		{
			return this.SelectedItem;
		}
	}
}

