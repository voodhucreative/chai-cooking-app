using System;
using System.Threading.Tasks;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Tools
{
    public class Gestures : ContentView
    {
        private View mainContent;
        private Color animationColor = Color.Transparent;
        private BoxView animateFlashBox;

        private static readonly object TapLocker = new object();

        private bool isTaped;

        private double xLoc;
        private double yLoc;

        private EventHandler<PositionEventArgs> tap;
        private EventHandler<DragEventArgs> drag;
        private EventHandler longTap;
        private EventHandler swipeLeft;
        private EventHandler swipeRight;
        private EventHandler swipeUp;
        private EventHandler swipeDown;

        public bool DragEnabled;
        public bool LongTapEnabled;
        public bool DoubleTapEnabled;
        public bool SwipeEnabled;
        public bool FlingEnabled;
        public bool IsBusy;

        public int GestureWidth { get; set; }
        public int GestureHeight { get; set; }

        public View ViewToAffect { get; set; }

        public Gestures()
        {
            DragEnabled = false;
            LongTapEnabled = false;
            DoubleTapEnabled = false;
            SwipeEnabled = false;
            FlingEnabled = false;
            IsBusy = false;

            TouchBegan += PressBeganEffect;
            TouchBegan += PressEndedEffectWithDelay;
            TouchEnded += PressEndedEffect;

            BackgroundColor = Color.Transparent;
        }

        public Gestures(View content) : this()
        {
            DragEnabled = false;
            LongTapEnabled = false;
            DoubleTapEnabled = false;
            SwipeEnabled = false;
            FlingEnabled = false;
            IsBusy = false;
            Content = content;
            //ViewToAffect = content;
            //Content.WidthRequest = Units.ScreenWidth25Percent;
            //Content.HeightRequest = Units.ScreenHeight25Percent;
        }

        [Flags]
        public enum GestureType
        {
            gtNone = 0,
            gtTap = 1,
            gtLongTap = 2,
            gtDrag = 4,
            gtSwipeLeft = 8,
            gtSwipeRight = 16,
            gtSwipeUp = 32,
            gtSwipeDown = 64,

            gtSwipeHorizontal = gtSwipeLeft | gtSwipeRight,
            gtSwipeVertical = gtSwipeUp | gtSwipeDown,
            gtSwipe = gtSwipeHorizontal | gtSwipeVertical
        };

        public GestureType SupportGestures;

        public new View Content
        {
            get { return base.Content; }
            set
            {
                mainContent = value;
                var contentLayout = new AbsoluteLayout();


                AbsoluteLayout.SetLayoutFlags(value, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(value, new Rectangle(0f, 0f, 1f, 1f));


                contentLayout.Children.Add(value);

                animateFlashBox = new BoxView
                {
                    BackgroundColor = animationColor,
                    InputTransparent = true
                };

                AbsoluteLayout.SetLayoutFlags(animateFlashBox, AbsoluteLayoutFlags.HeightProportional | AbsoluteLayoutFlags.XProportional);

                animateFlashBox.WidthRequest = 1;

                //AbsoluteLayout.SetLayoutBounds(animateFlashBox, new Rectangle(0.5f, 0f, AbsoluteLayout.AutoSize, 1f));
                AbsoluteLayout.SetLayoutBounds(animateFlashBox, new Rectangle(0.5f, 0f, this.GestureWidth, 1f));

                contentLayout.Children.Add(animateFlashBox);
                animateFlashBox.IsVisible = false;

                var boxAbsorbent = new BoxView
                {
                    BackgroundColor = Color.Blue,
                    Opacity = 0.2,

                    InputTransparent = true
                };

                AbsoluteLayout.SetLayoutFlags(boxAbsorbent, AbsoluteLayoutFlags.All);
                //AbsoluteLayout.SetLayoutBounds(boxAbsorbent, new Rectangle(0f, 0f, 1f, 1f));



                contentLayout.Children.Add(boxAbsorbent);

                base.Content = contentLayout;
            }
        }

        #region Animation

        public enum AnimationType
        {
            atNone = 0,
            atScaling = 1,
            atFlashing = 2,
            atFlashingTap = 3,
            atFading = 4
        }

        public AnimationType AnimationEffect = AnimationType.atNone;

        public Color AnimationColor
        {
            get { return animationColor; }
            set
            {
                animationColor = value;
                if (animateFlashBox != null)
                    animateFlashBox.BackgroundColor = animationColor;
            }
        }

        public double AnimationScale { get; set; } = 0.0;

        public uint AnimationSpeed { get; set; } = 100;

        #endregion

        #region TagProperty

        public static readonly BindableProperty TagProperty = BindableProperty.Create("Tag", typeof(string), typeof(Gestures));

        public string Tag
        {
            get { return (string)GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }

        public static string GetTagByChild(object sender)
        {
            if (!(sender is Element))
            {
                return null;
            }

            var parentElement = ((Element)sender).Parent;

            while (parentElement != null && !(parentElement is Gestures))
            {
                parentElement = parentElement.Parent;
            }

            return (parentElement as Gestures)?.Tag;
        }

        #endregion

        #region Main gesture

        public event EventHandler<PositionEventArgs> TouchBegan;

        public void OnTouchBegan(double positionX, double positionY)
        {
            Console.WriteLine("TOUCH BEGAN");

            if (ViewToAffect != null)
            {
                Console.WriteLine("SPECIFIC VIEW TOUCH BEGAN");
                ViewToAffect.Opacity = 0.5; 
            }
            xLoc = positionX;
            yLoc = positionY;
            TouchBegan?.Invoke(Content, new PositionEventArgs(positionX, positionY));
        }

        public event EventHandler Touch;

        public void OnTouch(GestureType gestureType)
        {
            Console.WriteLine("TOUCH");
            if (ViewToAffect != null)
            {
                Console.WriteLine("SPECIFIC VIEW TOUCH");
                ViewToAffect.Opacity = 0.5;
            }
            Touch?.Invoke(Content, EventArgs.Empty);
        }

        public event EventHandler<PositionEventArgs> TouchEnded;

        public void OnTouchEnded(double positionX, double positionY)
        {
            Console.WriteLine("Position X " + positionX);
            Console.WriteLine("Position Y " + positionY);

            if (ViewToAffect != null)
            {
                Console.WriteLine("SPECIFIC VIEW TOUCH ENDED");
                //ViewToAffect.BackgroundColor = Color.Black;
                ViewToAffect.Opacity = 1.0;
            }

            TouchEnded?.Invoke(Content, new PositionEventArgs(positionX, positionY));
        }

        #endregion

        #region Tap gesture

        public event EventHandler<PositionEventArgs> Tap
        {
            add
            {
                tap += value;
                SupportGestures |= GestureType.gtTap;
            }
            remove
            {
                tap -= value;
                if (tap == null)
                    SupportGestures ^= GestureType.gtTap;
            }
        }

        public void OnTap(double positionX, double positionY)
        {
            lock (TapLocker)
                if (!isTaped)
                {
                    isTaped = true;

                    tap?.Invoke(Content, new PositionEventArgs(positionX, positionY));
                    OnTouch(GestureType.gtTap);
                }
        }

        #endregion

        #region LongTap gesture

        public event EventHandler LongTap
        {
            add
            {
                longTap += value;
                SupportGestures |= GestureType.gtLongTap;
            }
            remove
            {
                longTap -= value;
                if (longTap == null)
                    SupportGestures ^= GestureType.gtLongTap;
            }
        }

        public void OnLongTap()
        {
            if (LongTapEnabled)
            {
                longTap?.Invoke(Content, EventArgs.Empty);
                OnTouch(GestureType.gtLongTap);
            }
        }

        #endregion

        #region Swipe gesture

        public event EventHandler SwipeLeft
        {
            add
            {
                swipeLeft += value;
                SupportGestures |= GestureType.gtSwipeLeft;
            }
            remove
            {
                swipeLeft -= value;
                if (swipeLeft == null)
                    SupportGestures ^= GestureType.gtSwipeLeft;
            }
        }

        public bool OnSwipeLeft()
        {
            if (SwipeEnabled)
            {
                if (swipeLeft != null)
                {
                    swipeLeft(Content, EventArgs.Empty);
                    OnTouch(GestureType.gtSwipeLeft);
                    return true;
                }
                OnTouch(GestureType.gtSwipe);
            }
            return false;
        }

        public event EventHandler SwipeRight
        {
            add
            {
                swipeRight += value;
                SupportGestures |= GestureType.gtSwipeRight;
            }
            remove
            {
                swipeRight -= value;
                if (swipeRight == null)
                    SupportGestures ^= GestureType.gtSwipeRight;
            }
        }

        public bool OnSwipeRight()
        {
            if (SwipeEnabled)
            {
                if (swipeRight != null)
                {
                    swipeRight(Content, EventArgs.Empty);
                    OnTouch(GestureType.gtSwipeRight);
                    return true;
                }

                OnTouch(GestureType.gtSwipe);
            }
            return false;
        }

        public event EventHandler SwipeUp
        {
            add
            {
                swipeUp += value;
                SupportGestures |= GestureType.gtSwipeUp;
            }
            remove
            {
                swipeUp -= value;
                if (swipeUp == null)
                    SupportGestures ^= GestureType.gtSwipeUp;
            }
        }

        public bool OnSwipeUp()
        {
            if (SwipeEnabled)
            {
                if (swipeUp != null)
                {
                    swipeUp(Content, EventArgs.Empty);
                    OnTouch(GestureType.gtSwipeUp);
                    return true;
                }

                OnTouch(GestureType.gtSwipe);
            }
            return false;
        }

        public event EventHandler SwipeDown
        {
            add
            {
                swipeDown += value;
                SupportGestures |= GestureType.gtSwipeDown;
            }
            remove
            {
                swipeDown -= value;
                if (swipeDown == null)
                    SupportGestures ^= GestureType.gtSwipeDown;
            }
        }

        public bool OnSwipeDown()
        {
            if (SwipeEnabled)
            {
                if (swipeDown != null)
                {
                    swipeDown(Content, EventArgs.Empty);
                    OnTouch(GestureType.gtSwipeDown);
                    return true;
                }

                OnTouch(GestureType.gtSwipe);
            }
            return false;
        }

        #endregion

        #region Drag

        public event EventHandler<DragEventArgs> Drag
        {
            add
            {
                drag += value;
                SupportGestures |= GestureType.gtDrag;
            }
            remove
            {
                drag -= value;
                if (drag == null)
                    SupportGestures ^= GestureType.gtDrag;
            }
        }

        public void OnDrag(double distanceX, double distanceY)
        {
            if (DragEnabled)
            {
                drag?.Invoke(Content, new DragEventArgs(distanceX, distanceY));
                OnTouch(GestureType.gtDrag);
            }
        }

        #endregion

        #region Animations
        private async void PressBeganEffect(object sender, PositionEventArgs e)
        {
            lock (TapLocker)
                isTaped = false;

            if (AnimationEffect == AnimationType.atScaling)
            {
                await this.ScaleTo(1 + (AnimationScale / 100), AnimationSpeed, Easing.CubicOut);
            }
            else if (AnimationEffect == AnimationType.atFlashingTap && !animateFlashBox.IsVisible)
            {
                animateFlashBox.TranslationX = e.PositionX - mainContent.Width / 2;
                animateFlashBox.IsVisible = true;
                animateFlashBox.Animate(
                    "AnimateFlashBox",
                    t => t * mainContent.Width,
                    x =>
                    {
                        animateFlashBox.WidthRequest = x;

                        var delta = mainContent.Width / 2 - Math.Abs(animateFlashBox.TranslationX) - x / 2;
                        if (delta < 0)
                        {
                            if (animateFlashBox.TranslationX < 0)
                                animateFlashBox.TranslationX -= delta;
                            else
                                animateFlashBox.TranslationX += delta;
                        }
                    },
                    16, AnimationSpeed, Easing.SinOut,
                    (x, y) =>
                    {
                        animateFlashBox.WidthRequest = 1;
                        animateFlashBox.IsVisible = false;
                    });
            }
            else if (AnimationEffect == AnimationType.atFlashing && !animateFlashBox.IsVisible)
            {
                animateFlashBox.TranslationX = 0;
                animateFlashBox.IsVisible = true;
                animateFlashBox.Animate(
                    "AnimateFlashBox",
                    t => t * mainContent.Width,
                    x => animateFlashBox.WidthRequest = x,
                    16, AnimationSpeed, Easing.SinOut,
                    (x, y) =>
                    {
                        animateFlashBox.WidthRequest = 1;
                        animateFlashBox.IsVisible = false;
                    });
            }
        }

        private async void PressEndedEffect(object sender, EventArgs e)
        {
            if (AnimationEffect == AnimationType.atScaling)
                await this.ScaleTo(1, AnimationSpeed, Easing.CubicOut);
        }

        private async void PressEndedEffectWithDelay(object sender, EventArgs e)
        {
            if (drag != null)
                return;
            Console.WriteLine("Tits");
            //OnTouchEnded isn't called if view is included in scroll panel

            //await Task.Delay(200000);
            //OnTouchEnded(-1, -1);
        }
    }
    #endregion
    public class DragEventArgs : EventArgs
    {
        public readonly double DistanceX;
        public readonly double DistanceY;

        public DragEventArgs(double distanceX, double distanceY)
        {
            DistanceX = distanceX;
            DistanceY = distanceY;
        }
    }

    public class PositionEventArgs : EventArgs
    {
        public readonly double PositionX;
        public readonly double PositionY;

        public PositionEventArgs(double positionX, double positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }
    }
}
