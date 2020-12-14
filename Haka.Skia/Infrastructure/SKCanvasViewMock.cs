using SkiaSharp.Views.Forms;
using System.ComponentModel;
using Xamarin.Forms;

namespace Haka.Skia.Infrastructure
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SKCanvasViewMock : View
    {
        protected virtual void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
        }

        public void InvalidateSurface()
        {

        }
    }
}
