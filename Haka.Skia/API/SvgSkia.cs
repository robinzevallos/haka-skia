using Haka.Core;
using Kasay.BindableProperty;
using SkiaSharp.Extended.Svg;
using SkiaSharp.Views.Forms;
using System;
using System.IO;
using System.Net.Http;

namespace Haka.Skia
{
    public class SvgSkia : SKCanvasView
    {
        private readonly SKSvg skSvg = new SKSvg();
        static readonly HttpClient httpClient = httpClient ?? new HttpClient();

        [Bind] public Double Size { get; set; }

        [Bind] public String Source { get; set; }

        public SvgSkia()
        {
            this
                .OnChanged(_ => _.Size, () => OnSizeChanged())
                .OnChanged(_ => _.Source, () => OnSourceChanged());
        }

        void OnSizeChanged()
        {
            HeightRequest = Size;
            WidthRequest = Size;
        }

        async void OnSourceChanged()
        {
            if (Source.Contains("http"))
            {
                using (Stream stream = await httpClient.GetStreamAsync(Source))
                {
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memStream);
                        memStream.Seek(0, SeekOrigin.Begin);
                        skSvg.Load(memStream);
                    }
                }
            }
            else
            {
                using (var memStream = LocalResource.GetStream(Source))
                {
                    memStream.Seek(0, SeekOrigin.Begin);
                    skSvg.Load(memStream);
                }
            }

            InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var surface = e.Surface;
            var canvas = surface.Canvas;
            var width = e.Info.Width;
            var height = e.Info.Height;

            canvas.Clear();

            if (skSvg.Picture != null)
            {
                var canvasMin = Math.Min(width, height);
                var svgMax = Math.Max(
                    skSvg.Picture.CullRect.Width,
                    skSvg.Picture.CullRect.Height);
                var scale = canvasMin / svgMax;
                var matrix = SkiaSharp.SKMatrix.CreateScale(scale, scale);

                canvas.DrawPicture(skSvg.Picture, ref matrix);
            }
        }
    }
}
