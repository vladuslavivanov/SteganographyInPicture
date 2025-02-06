using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace SteganographyInPicture.UI.Helpers;

internal static class ScrollViewerCursorHelper
{
    private static Point _lastPointerPosition;
    private static bool _isDragging = false;

    internal static void ScrollViewer_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        var scrollViewer = sender as ScrollViewer;
        if (scrollViewer == null) return;

        var pointer = e.GetCurrentPoint(scrollViewer);
        if (pointer.Properties.IsLeftButtonPressed)
        {
            _isDragging = true;
            _lastPointerPosition = pointer.Position;
            scrollViewer.CapturePointer(e.Pointer);
        }
    }

    internal static void ScrollViewer_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        var scrollViewer = sender as ScrollViewer;
        if (scrollViewer == null) return;

        if (_isDragging)
        {
            var pointer = e.GetCurrentPoint(scrollViewer);
            var currentPosition = pointer.Position;

            var deltaX = (currentPosition.X - _lastPointerPosition.X) * 5;
            var deltaY = (currentPosition.Y - _lastPointerPosition.Y) * 5;

            scrollViewer.ChangeView(
                scrollViewer.HorizontalOffset - deltaX,
                scrollViewer.VerticalOffset - deltaY, 
                null);                                  

            _lastPointerPosition = currentPosition;
        }
    }

    internal static void ScrollViewer_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        var scrollViewer = sender as ScrollViewer;
        if (scrollViewer == null) return;

        if (_isDragging)
        {
            _isDragging = false;
            scrollViewer.ReleasePointerCaptures();
        }
    }
}
