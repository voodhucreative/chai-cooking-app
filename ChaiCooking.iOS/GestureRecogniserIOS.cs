using Foundation;
using ChaiCooking.iOS;
using ChaiCooking.Tools;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Gestures), typeof(GesturesRenderer))]

namespace ChaiCooking.iOS
{
    public class GesturesRenderer : ViewRenderer
    {
        public new static void Init()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            var cGesture = (Gestures)e.NewElement;

            var tapGestureRecognizer = new TapGestureRecognizer
            {
                OnTouchesBegan = (x, y) => cGesture.OnTouchBegan(x, y),
                OnTouchesEnded = (x, y) => cGesture.OnTouchEnded(x, y),
                OnTap = (x, y) => cGesture.OnTap(x, y)
            };

            var longPressGestureRecognizer = new LongPressGestureRecognizer(() => cGesture.OnLongTap())
            {
                OnTouchesBegan = (x, y) => cGesture.OnTouchBegan(x, y),
                OnTouchesEnded = (x, y) => cGesture.OnTouchEnded(x, y)
            };

            #region SwipeGestureRecognizer

            var swipeLeftGestureRecognizer = new SwipeGestureRecognizer(() => cGesture.OnSwipeLeft())
            {
                Direction = UISwipeGestureRecognizerDirection.Left,
                OnTouchesBegan = (x, y) => cGesture.OnTouchBegan(x, y),
                OnTouchesEnded = (x, y) => cGesture.OnTouchEnded(x, y)
            };

            var swipeRightGestureRecognizer = new SwipeGestureRecognizer(() => cGesture.OnSwipeRight())
            {
                Direction = UISwipeGestureRecognizerDirection.Right,
                OnTouchesBegan = (x, y) => cGesture.OnTouchBegan(x, y),
                OnTouchesEnded = (x, y) => cGesture.OnTouchEnded(x, y)
            };

            var swipeUpGestureRecognizer = new SwipeGestureRecognizer(() => cGesture.OnSwipeUp())
            {
                Direction = UISwipeGestureRecognizerDirection.Up,
                OnTouchesBegan = (x, y) => cGesture.OnTouchBegan(x, y),
                OnTouchesEnded = (x, y) => cGesture.OnTouchEnded(x, y)
            };

            var swipeDownGestureRecognizer = new SwipeGestureRecognizer(() => cGesture.OnSwipeDown())
            {
                Direction = UISwipeGestureRecognizerDirection.Down,
                OnTouchesBegan = (x, y) => cGesture.OnTouchBegan(x, y),
                OnTouchesEnded = (x, y) => cGesture.OnTouchEnded(x, y)
            };

            #endregion

            #region DragGestureRecognizer

            var dragGestureRecognizer = new DragGestureRecognizer
            {
                OnTouchesBegan = (x, y) => cGesture.OnTouchBegan(x, y),
                OnTouchesEnded = (x, y) => cGesture.OnTouchEnded(x, y),
                OnDrag = (x, y) => cGesture.OnDrag(x, y)
            };

            if (cGesture != null)
            {

                if ((cGesture.SupportGestures & Gestures.GestureType.gtSwipeLeft) != 0)
                    dragGestureRecognizer.RequireGestureRecognizerToFail(swipeLeftGestureRecognizer);
                if ((cGesture.SupportGestures & Gestures.GestureType.gtSwipeRight) != 0)
                    dragGestureRecognizer.RequireGestureRecognizerToFail(swipeRightGestureRecognizer);
                if ((cGesture.SupportGestures & Gestures.GestureType.gtSwipeUp) != 0)
                    dragGestureRecognizer.RequireGestureRecognizerToFail(swipeUpGestureRecognizer);
                if ((cGesture.SupportGestures & Gestures.GestureType.gtSwipeDown) != 0)
                    dragGestureRecognizer.RequireGestureRecognizerToFail(swipeDownGestureRecognizer);
            }

            #endregion

            if (e.NewElement == null)
            {
                RemoveGestureRecognizer(tapGestureRecognizer);
                RemoveGestureRecognizer(longPressGestureRecognizer);
                RemoveGestureRecognizer(swipeLeftGestureRecognizer);
                RemoveGestureRecognizer(swipeRightGestureRecognizer);
                RemoveGestureRecognizer(swipeUpGestureRecognizer);
                RemoveGestureRecognizer(swipeDownGestureRecognizer);
                RemoveGestureRecognizer(dragGestureRecognizer);
            }

            if (e.OldElement == null)
            {
                AddGestureRecognizer(tapGestureRecognizer);
                AddGestureRecognizer(longPressGestureRecognizer);
                AddGestureRecognizer(swipeLeftGestureRecognizer);
                AddGestureRecognizer(swipeRightGestureRecognizer);
                AddGestureRecognizer(swipeUpGestureRecognizer);
                AddGestureRecognizer(swipeDownGestureRecognizer);
                AddGestureRecognizer(dragGestureRecognizer);
            }
        }

        private class TapGestureRecognizer : UITapGestureRecognizer
        {
            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                double positionX = -1;
                double positionY = -1;

                if (touches.AnyObject is UITouch touch)
                {
                    positionX = touch.LocationInView(View).X;
                    positionY = touch.LocationInView(View).Y;
                }

                OnTouchesBegan?.Invoke(positionX, positionY);
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);

                double positionX = -1;
                double positionY = -1;

                if (OnTap != null && touches.AnyObject is UITouch touch)
                {
                    positionX = touch.LocationInView(View).X;
                    positionY = touch.LocationInView(View).Y;
                    OnTap(positionX, positionY);
                }

                OnTouchesEnded?.Invoke(positionX, positionY);
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                OnTouchesEnded?.Invoke(-1, -1);
            }

            public Action<double, double> OnTap;
            public Action<double, double> OnTouchesBegan;
            public Action<double, double> OnTouchesEnded;
        }

        private class LongPressGestureRecognizer : UILongPressGestureRecognizer
        {
            public LongPressGestureRecognizer(Action action)
                : base(action)
            {
            }

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                double positionX = -1;
                double positionY = -1;

                if (touches.AnyObject is UITouch touch)
                {
                    positionX = touch.LocationInView(View).X;
                    positionY = touch.LocationInView(View).Y;
                }

                OnTouchesBegan?.Invoke(positionX, positionY);
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);

                double positionX = -1;
                double positionY = -1;

                if (touches.AnyObject is UITouch touch)
                {
                    positionX = touch.LocationInView(View).X;
                    positionY = touch.LocationInView(View).Y;
                }

                OnTouchesEnded?.Invoke(positionX, positionY);
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                OnTouchesEnded?.Invoke(-1, -1);
            }

            public Action<double, double> OnTouchesBegan;
            public Action<double, double> OnTouchesEnded;
        }

        private class SwipeGestureRecognizer : UISwipeGestureRecognizer
        {
            public SwipeGestureRecognizer(Action action)
                : base(action)
            {
            }

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                double positionX = -1;
                double positionY = -1;

                if (touches.AnyObject is UITouch touch)
                {
                    positionX = touch.LocationInView(View).X;
                    positionY = touch.LocationInView(View).Y;
                }

                OnTouchesBegan?.Invoke(positionX, positionY);
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);

                double positionX = -1;
                double positionY = -1;

                if (touches.AnyObject is UITouch touch)
                {
                    positionX = touch.LocationInView(View).X;
                    positionY = touch.LocationInView(View).Y;
                }

                OnTouchesEnded?.Invoke(positionX, positionY);
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                OnTouchesEnded?.Invoke(-1, -1);
            }

            public Action<double, double> OnTouchesBegan;
            public Action<double, double> OnTouchesEnded;
        }

        private class DragGestureRecognizer : UIPanGestureRecognizer
        {
            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                double positionX = -1;
                double positionY = -1;

                if (touches.AnyObject is UITouch touch)
                {
                    positionX = touch.LocationInView(View).X;
                    positionY = touch.LocationInView(View).Y;
                }

                OnTouchesBegan?.Invoke(positionX, positionY);
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);

                double positionX = -1;
                double positionY = -1;

                if (touches.AnyObject is UITouch touch)
                {
                    positionX = touch.LocationInView(View).X;
                    positionY = touch.LocationInView(View).Y;
                }

                OnTouchesEnded?.Invoke(positionX, positionY);
            }

            public override void TouchesMoved(NSSet touches, UIEvent evt)
            {
                base.TouchesMoved(touches, evt);

                if (OnDrag != null && touches.AnyObject is UITouch touch)
                {
                    var offsetX = touch.PreviousLocationInView(View).X - (double)touch.LocationInView(View).X;
                    var offsetY = touch.PreviousLocationInView(View).Y - (double)touch.LocationInView(View).Y;
                    OnDrag(-offsetX, -offsetY);
                }
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                OnTouchesEnded?.Invoke(-1, -1);
            }

            public Action<double, double> OnTouchesBegan;
            public Action<double, double> OnTouchesEnded;
            public Action<double, double> OnDrag;
        }
    }
}