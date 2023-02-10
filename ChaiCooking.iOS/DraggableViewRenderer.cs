using Foundation;
using UIKit;
using Xamarin.Forms;
using ChaiCooking.Layouts;
using ChaiCooking.iOS;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using CoreGraphics;
using System;

[assembly: ExportRenderer(typeof(DraggableView), typeof(DraggableViewRenderer))]
namespace ChaiCooking.iOS
{

    public class DraggableViewRenderer : VisualElementRenderer<View>
    {
        bool longPress = false;
        bool firstTime = true;
        double lastTimeStamp = 0f;
        UIPanGestureRecognizer panGesture;
        CGPoint lastLocation;
        CGPoint originalPosition;
        UIGestureRecognizer.Token panGestureToken;

        void DetectPan()
        {
            var dragView = Element as DraggableView;
            if (longPress || dragView.DragMode == DragMode.Touch)
            {
                if (panGesture.State == UIGestureRecognizerState.Began)
                {
                    dragView.DragStarted();
                    if (firstTime)
                    {
                        originalPosition = Center;
                        firstTime = false;
                    }
                }

                CGPoint translation = panGesture.TranslationInView(Superview);
                var currentCenterX = Center.X;
                var currentCenterY = Center.Y;
                if (dragView.DragDirection == DragDirectionType.All || dragView.DragDirection == DragDirectionType.Horizontal)
                {
                    currentCenterX = lastLocation.X + translation.X;
                }

                if (dragView.DragDirection == DragDirectionType.All || dragView.DragDirection == DragDirectionType.Vertical)
                {
                    currentCenterY = lastLocation.Y + translation.Y;
                }
                Console.WriteLine("IOS X: " + (int)currentCenterX);
                dragView.Drag((int)currentCenterX, (int)currentCenterY);
                Center = new CGPoint(currentCenterX, currentCenterY);

                if (panGesture.State == UIGestureRecognizerState.Ended)
                {
                    dragView.DragEnded();

                    if (dragView.IsSnappable)
                    {
                        // snap to nearest point here
                        dragView.Drag(dragView.SnapX, dragView.SnapY);
                        Center = new CGPoint(dragView.SnapX, dragView.SnapY);
                        
                    }
                    longPress = false;
                }
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            
            if (e.OldElement != null)
            {
                RemoveGestureRecognizer(panGesture);
                panGesture.RemoveTarget(panGestureToken);
            }

            if (e.NewElement != null)
            {
                var dragView = Element as DraggableView;
                panGesture = new UIPanGestureRecognizer();
                panGestureToken = panGesture.AddTarget(DetectPan);
                AddGestureRecognizer(panGesture);


                dragView.RestorePositionCommand = new Command(() =>
                {
                    if (!firstTime)
                    {
                        Center = originalPosition;
                        dragView.Drag((int)originalPosition.X, (int)originalPosition.Y);
                    }
                });

                dragView.SetPositionCommand = new Command(() =>
                {
                    /*
                    if (firstTime)
                    {
                        try
                        {
                            dragView.DragEnded();

                            if (dragView.IsSnappable)
                            {
                                // snap to nearest point here
                                dragView.Drag(dragView.SnapX, dragView.SnapY);
                                Center = new CGPoint(dragView.SnapX, dragView.SnapY);

                            }
                        }
                        catch (Exception me) { }
                        //originalPosition = new CGPoint(0, (int)originalPosition.Y);
                        //Center = new CGPoint(-40, (int)originalPosition.Y);
                        //dragView.Drag((int)originalPosition.X, (int)originalPosition.Y);


                    }*/
                });

                dragView.SetPositionCommand.Execute(null);

            }

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var dragView = Element as DraggableView;
            base.OnElementPropertyChanged(sender, e);

        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            lastTimeStamp = evt.Timestamp;
            Superview.BringSubviewToFront(this);
            lastLocation = Center; 
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            if (evt.Timestamp - lastTimeStamp >= 0.5)
            {
                longPress = true;
            }
            base.TouchesMoved(touches, evt);
        }

    }
}