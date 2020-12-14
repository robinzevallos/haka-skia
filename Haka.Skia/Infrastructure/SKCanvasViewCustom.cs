using SkiaSharp.Views.Forms;
using System.ComponentModel;

namespace Haka.Skia.Infrastructure
{
#if __ANDROID__ || __IOS__
    public class SKCanvasViewCustom : SKCanvasView
#else
    public class SKCanvasViewCustom : SKCanvasViewMock
#endif
    {
    }
}
