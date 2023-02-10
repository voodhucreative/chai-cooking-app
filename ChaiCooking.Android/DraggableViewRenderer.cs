using AView = Android.Views;
using Android.Runtime;
using Android.Views;
using ChaiCooking.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ChaiCooking.Layouts;
using System.ComponentModel;
using Android.Content;
using System;

[assembly: ExportRenderer(typeof(DraggableView), typeof(DraggableViewRenderer))]
namespace ChaiCooking.Droid
{
    public class DraggableViewRenderer : VisualElementRenderer<Xamarin.Forms.View>
	{
        public DraggableViewRenderer(Context context) : base(context) { }

        float originalX;
		float originalY;
		float dX;
		float dY;
		int pixelX;
		int pixelY;
		bool firstTime = true;
		bool touchedDown = false;

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
		{
			base.OnElementChanged(e);


			if (e.OldElement != null)
			{
				LongClick -= HandleLongClick;
			}
			if (e.NewElement != null)
			{
				LongClick += HandleLongClick;
				var dragView = Element as DraggableView;
				dragView.RestorePositionCommand = new Command(() =>
				{
					if (!firstTime)
					{
						SetX(originalX);
						SetY(originalY);
						dragView.Drag(pixelX, pixelY);
					}

				});
			}

		}

		private void HandleLongClick(object sender, LongClickEventArgs e)
		{
			var dragView = Element as DraggableView;
			if (firstTime)
			{
				originalX = GetX();
				originalY = GetY();
				firstTime = false;
			}
			dragView.DragStarted();
			touchedDown = true;
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var dragView = Element as DraggableView;
			base.OnElementPropertyChanged(sender, e);

		}
		protected override void OnVisibilityChanged(AView.View changedView, [GeneratedEnum] ViewStates visibility)
		{
			base.OnVisibilityChanged(changedView, visibility);
			if (visibility == ViewStates.Visible)
			{



			}
		}
		public override bool OnTouchEvent(MotionEvent e)
		{
			float x = e.RawX;
			float y = e.RawY;
			var scale = Resources.DisplayMetrics.Density;
			pixelX = (int)((e.RawX - 0.5f) / scale);
			pixelY = (int)((e.RawY - 0.5f) / scale);


			

			//x = e.GetX();
			//y = e.GetY();
			var dragView = Element as DraggableView;
			switch (e.Action)
			{
				case MotionEventActions.Down:
					if (dragView.DragMode == DragMode.Touch)
					{
						if (!touchedDown)
						{
							if (firstTime)
							{
								originalX = GetX();
								originalY = GetY();
								firstTime = false;
							}
							dragView.DragStarted();
							dragView.Drag(pixelX, pixelY);
						}

						touchedDown = true;
					}
					dX = x - this.GetX();
					dY = y - this.GetY();
					
					break;
				case MotionEventActions.Move:
					//if (touchedDown)
					//{
						if (dragView.DragDirection == DragDirectionType.All || dragView.DragDirection == DragDirectionType.Horizontal)
						{
							SetX(x - dX);
						}

						if (dragView.DragDirection == DragDirectionType.All || dragView.DragDirection == DragDirectionType.Vertical)
						{
							SetY(y - dY);
						}

					//}
					Console.WriteLine("ANDROID PIXEL X: " + pixelX);
					Console.WriteLine("ANDROID RAW X: " + (int)e.RawX);
					Console.WriteLine("ANDROID X: " + GetX());



					dragView.Drag(pixelX, pixelY);
					break;
				case MotionEventActions.Up:
					touchedDown = false;
					dragView.DragEnded();
					if (dragView.IsSnappable)
					{
						// snap to nearest point here
						//dragView.Drag(dragView.SnapX, dragView.SnapY);
						//SetX(dragView.SnapX);
                        //SetY(dragView.SnapY);
					}
					break;
				case MotionEventActions.Cancel:
					touchedDown = false;
					break;
			}
			//return base.OnTouchEvent(e);
            return true;// base.OnTouchEvent(e);
        }

		public override bool OnInterceptTouchEvent(MotionEvent e)
		{

			BringToFront();
			return true;
		}

	}


}