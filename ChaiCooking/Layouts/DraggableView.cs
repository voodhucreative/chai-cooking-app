using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChaiCooking.Layouts
{
    public class DraggableView : ContentView
    {
        public event EventHandler DragStart = delegate { };
        public event EventHandler DragEnd = delegate { };
        public event EventHandler Dragging = delegate { };
        public event EventHandler Tapped = delegate { };
        
        public int InitialX { get; set; }
        public int InitialY { get; set; }
        public int MovedX { get; set; }
        public int MovedY { get; set; }
        public int SnapX { get; set; }
        public int SnapY { get; set; }
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }

        public bool IsSnappable { get; set; }

        public static readonly BindableProperty DragDirectionProperty = BindableProperty.Create(
            propertyName: "DragDirection",
            returnType: typeof(DragDirectionType),
            declaringType: typeof(DraggableView),
            defaultValue: DragDirectionType.All,
            defaultBindingMode: BindingMode.TwoWay);

        public DragDirectionType DragDirection
        {
            get { return (DragDirectionType)GetValue(DragDirectionProperty); }
            set { SetValue(DragDirectionProperty, value); }
        }


        public static readonly BindableProperty DragModeProperty = BindableProperty.Create(
           propertyName: "DragMode",
           returnType: typeof(DragMode),
           declaringType: typeof(DraggableView),
           defaultValue: DragMode.LongPress,
           defaultBindingMode: BindingMode.TwoWay);

        public DragMode DragMode
        {
            get { return (DragMode)GetValue(DragModeProperty); }
            set { SetValue(DragModeProperty, value); }
        }

        public static readonly BindableProperty IsDraggingProperty = BindableProperty.Create(
          propertyName: "IsDragging",
          returnType: typeof(bool),
          declaringType: typeof(DraggableView),
          defaultValue: false,
          defaultBindingMode: BindingMode.TwoWay);

        public bool IsDragging
        {
            get { return (bool)GetValue(IsDraggingProperty); }
            set { SetValue(IsDraggingProperty, value); }
        }

        public static readonly BindableProperty RestorePositionCommandProperty = BindableProperty.Create(nameof(RestorePositionCommand), typeof(ICommand), typeof(DraggableView), default(ICommand), BindingMode.TwoWay, null, OnRestorePositionCommandPropertyChanged);

        static void OnRestorePositionCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var source = bindable as DraggableView;
            if (source == null)
            {
                return;
            }
            source.OnRestorePositionCommandChanged();
        }

        private void OnRestorePositionCommandChanged()
        {
            OnPropertyChanged("RestorePositionCommand");
        }


        public ICommand RestorePositionCommand
        {
            get
            {
                return (ICommand)GetValue(RestorePositionCommandProperty);
            }
            set
            {
                SetValue(RestorePositionCommandProperty, value);
            }
        }

        public static readonly BindableProperty SetPositionCommandProperty = BindableProperty.Create(nameof(SetPositionCommand), typeof(ICommand), typeof(DraggableView), default(ICommand), BindingMode.TwoWay, null, OnSetPositionCommandPropertyChanged);

        static void OnSetPositionCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var source = bindable as DraggableView;
            if (source == null)
            {
                return;
            }
            source.OnSetPositionCommandChanged();
        }

        private void OnSetPositionCommandChanged()
        {
            OnPropertyChanged("SetPositionCommand");
        }


        public ICommand SetPositionCommand
        {
            get
            {
                return (ICommand)GetValue(SetPositionCommandProperty);
            }
            set
            {
                SetValue(SetPositionCommandProperty, value);
            }
        }










        public void DragStarted()
        {
            DragStart(this, default(EventArgs));
            IsDragging = true;
        }

        public void DragEnded()
        {
            IsDragging = false;
            DragEnd(this, default(EventArgs));
        }

        public void Drag(int x, int y)
        {
            MovedX = x;
            MovedY = y;
            IsDragging = true;
            Dragging(this, default(EventArgs));
        }

        public void Tap(int x, int y)
        {
            InitialX = x;
            InitialY = y;
            MovedX = x;
            MovedY = y;
        }

        public void Init()
        {
            Console.WriteLine("Initialise");
            
        }


        public void SnapTo(int x, int y)
        {

        }

        public void SetLimits(int minX, int maxX, int minY, int maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

    }
}