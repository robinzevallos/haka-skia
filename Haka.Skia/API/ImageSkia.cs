using Haka.Core;
using Kasay.BindableProperty;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Xamarin.Forms;

namespace Haka.Skia
{
    public class ImageSkia : SKCanvasView
    {
        static readonly HttpClient httpClient = httpClient ?? new HttpClient();
        static readonly Dictionary<String, SKBitmap> bitmapsStore = bitmapsStore ?? new Dictionary<String, SKBitmap>();

        SKBitmap bitmap;

        [Bind] public String Source { get; set; }

        [Bind] public Aspect Aspect { get; set; }

        public ImageSkia()
        {
            this
                .OnChanged(_ => _.Aspect, () => InvalidateSurface())
                .OnChanged(_ => _.Source, () => OnSourceChanged());
        }

        async void OnSourceChanged()
        {
            try
            {
                if (Source.Contains("http"))
                {
                    bitmap = GetCoverBitmap();
                    InvalidateSurface();

                    if (!bitmapsStore.ContainsKey(Source))
                    {
                        using (Stream stream = await httpClient.GetStreamAsync(Source))
                        {
                            using (MemoryStream memStream = new MemoryStream())
                            {
                                await stream.CopyToAsync(memStream);
                                memStream.Seek(0, SeekOrigin.Begin);

                                bitmap = SKBitmap.Decode(memStream);

                                bitmapsStore.Add(Source, bitmap);
                            }
                        }
                    }
                    else
                    {
                        bitmap = bitmapsStore[Source];
                    }
                }
                else
                {
                    using (Stream stream = LocalResource.GetStream(Source))
                    {
                        bitmap = SKBitmap.Decode(stream);
                    };
                }

                InvalidateSurface();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            SKImageInfo info = e.Info;
            SKCanvas canvas = e.Surface.Canvas;

            canvas.Clear();

            if (bitmap != null)
            {
                if (Aspect == Aspect.AspectFit)
                {
                    var ratio = (float)bitmap.Height / bitmap.Width;
                    var newHeigh = (int)(info.Width * ratio);

                    var newInfo = new SKImageInfo(info.Width, newHeigh);
                    var bitmapRisized = bitmap.Resize(newInfo, SKFilterQuality.Medium);

                    var x = (info.Width - bitmapRisized.Width) / 2;
                    var y = (info.Height - bitmapRisized.Height) / 2;

                    canvas.DrawBitmap(
                       bitmap: bitmapRisized,
                       x: x,
                       y: y);
                }
                else if (Aspect == Aspect.Fill)
                {
                    var newInfo = new SKImageInfo(info.Width, info.Height);
                    var bitmapRisized = bitmap.Resize(newInfo, SKFilterQuality.Medium);

                    canvas.DrawBitmap(
                       bitmap: bitmapRisized,
                       x: 0,
                       y: 0);
                }
                else if (Aspect == Aspect.AspectFill)
                {
                    canvas.DrawBitmap(
                        bitmap,
                        0,
                        0);
                }
            }
        }

        SKBitmap GetCoverBitmap()
        {
            using (Stream stream = LocalResource.GetStream("cover.png", this))
            {
                return SKBitmap.Decode(stream);
            };
        }
    }
}
