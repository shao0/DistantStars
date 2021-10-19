using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DistantStars.Client.Resource.Controls.Tips
{
    public class TipsAdorner : Adorner
    {
        public TipsAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }
        private UIElement _Child;

        public UIElement Child
        {
            get => _Child;
            set
            {
                if (value == null)
                {
                    RemoveVisualChild(_Child);
                    // ReSharper disable once ExpressionIsAlwaysNull
                    _Child = value;
                    return;
                }
                AddVisualChild(value);
                _Child = value;
            }
        }
        protected override int VisualChildrenCount => _Child != null ? 1 : 0;
        protected override Size ArrangeOverride(Size finalSize)
        {
            Child.Arrange(new Rect(finalSize));
            return finalSize;
        }
        protected override Visual GetVisualChild(int index)
        {
            if (index == 0 && _Child != null) return _Child;
            return base.GetVisualChild(index);
        }
    }
}
