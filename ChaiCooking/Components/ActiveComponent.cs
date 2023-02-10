using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Helpers;
using ChaiCooking.Layouts;
using ChaiCooking.Tools;
using Xamarin.Forms;

namespace ChaiCooking.Components
{
    public class ActiveComponent
    {
        public Grid Container;
        public Grid Content { get; set; }
        public DraggableView DraggableView;
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public List<Models.Action> Actions { get; set; }

        public Models.Action DefaultAction { get; set; }
        public Models.Action DownAction { get; set; }
        public Models.Action UpAction { get; set; }


        public Gestures Gestures { get; set; }

        public bool IsDraggable { get; set; }
        public bool IsActive { get; set; }

        public ActiveComponent()
        {
            Content = new Grid();
            Container = new Grid();
            
        }

        public virtual async Task<bool> Update()
        {
            await Task.Delay(50);
            return true;
        }

        public virtual async Task<bool> ShowClicked()
        {
            await Task.Delay(10);
            return true;
        }

        public virtual async Task<bool> ShowUnclicked()
        {
            await Task.Delay(10);
            return true;
        }

        public virtual async Task<bool> ShowFocussed()
        {
            await Task.Delay(10);
            return true;
        }

        public virtual async Task<bool> ShowUnfocussed()
        {
            await Task.Delay(10);
            return true;
        }

        public void AddAction(Models.Action action)
        {
            if (Actions == null)
            {
                Actions = new List<Models.Action>();
            }
            Actions.Add(action);
        }

        public void AddGestures(Gestures.GestureType gestureType)
        {
            this.Gestures = new Gestures(this.Content);
        }

        public void AddGestures(Gestures gestures)
        {
            gestures.Content = this.Content;
            this.Gestures = gestures;
        }

        public void AddGestures(Gestures gestures, Models.Action action)
        {
            this.Gestures = gestures;
            this.Gestures.Content = this.Content;
            this.DefaultAction = action;
        }

        public void SetAsDraggable(DragDirectionType direction)
        {
            IsDraggable = true;
            DraggableView = new DraggableView
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = Content.Width,
                HeightRequest = Content.Height,
                DragMode = DragMode.Touch,
                DragDirection = direction
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

        public virtual void DragStarted(object sender, EventArgs e)
        {
    
        }
          
        public virtual void DragEnded(object sender, EventArgs e)
        {
            
        }

        public virtual void Dragged(object sender, EventArgs e)
        {

        }

        public Grid GetContent()
        {
            if (IsDraggable)
            { 
                return Content;
            }
            else
            {
                if (Container == null)
                {
                    Container = new Grid { };
                    Container.Margin = Content.Margin;
                }
                Container.Children.Add(Content, 0, 0);
                if (this.Gestures != null)
                {
                    Gestures.Content = this.Content;
                    Container.Children.Add(Gestures, 0, 0);
                }
                return Container;
            }
        }
    }
}
