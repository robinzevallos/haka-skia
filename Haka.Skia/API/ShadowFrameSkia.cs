using Haka.Core;
using Kasay.BindableProperty;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

namespace Haka.Skia
{
    public class ShadowFrameSkia : ContentView
    {
        readonly SKCanvasView canvasView = new SKCanvasView();
        Color backgroundColorShadow;
        View contentOriginal;
        Boolean isShadowWithBackgroundColor;

        [Bind] public Single CornerRadius { get; set; }

        [Bind] public Color ShadowColor { get; set; }

        [Bind] public Single ShadowThickness { get; set; }

        public ShadowFrameSkia()
        {
            this
                .OnChanged(_ => _.BackgroundColor, () => OnBackgroundColorChanged())
                .OnChanged(_ => _.CornerRadius, () => OnCornerRadiusChanged())
                .OnChanged(_ => _.ShadowColor, () => OnShadowColorChanged())
                .OnChanged(_ => _.ShadowThickness, () => OnShadowThicknessChanged())
                ;

            Initialize();
        }

        void Initialize()
        {
            backgroundColorShadow = Color.LightGray;
            CornerRadius = 20;
            ShadowThickness = 5;

            canvasView.PaintSurface += (s, e) => OnPaintSurface(e);
        }

        void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;

            canvas.Clear();

            var shadowThickness = Device.RuntimePlatform == Device.iOS ?
                1.5f * ShadowThickness :
                ShadowThickness;

            var blurfilter = SKMaskFilter.CreateBlur(SKBlurStyle.Solid, shadowThickness);

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                MaskFilter = blurfilter,
                Color = isShadowWithBackgroundColor ?
                    ShadowColor.ToSKColor() :
                    backgroundColorShadow.ToSKColor(),
                BlendMode = SKBlendMode.ColorBurn,
            };

            var space = Device.RuntimePlatform == Device.iOS ?
                3f * ShadowThickness :
                2f * ShadowThickness;

            var rect = new SKRect(
                left: space,
                top: space,
                right: info.Width - space,
                bottom: info.Height - space);

            canvas.DrawRoundRect(
                rect: rect,
                rx: CornerRadius,
                ry: CornerRadius,
                paint: paint);

            if (isShadowWithBackgroundColor)
            {
                var paintBackgroundColor = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    MaskFilter = blurfilter,
                    Color = backgroundColorShadow.ToSKColor(),
                };

                canvas.DrawRoundRect(
                    rect: rect,
                    rx: CornerRadius,
                    ry: CornerRadius,
                    paint: paintBackgroundColor);
            }
        }    

        protected override void OnParentSet()
        {
            base.OnParentSet();

            contentOriginal = Content;

            OnContentChanged();
        }

        void OnContentChanged()
        {
            if (contentOriginal != null)
            {
                contentOriginal.Margin = 1.5 * ShadowThickness;

                var grid = new Grid();
                grid.Children.Add(canvasView);
                grid.Children.Add(contentOriginal);

                Content = grid;
            }
        }

        void OnBackgroundColorChanged()
        {
            if (BackgroundColor != Color.Transparent)
            {
                backgroundColorShadow = BackgroundColor;
                BackgroundColor = Color.Transparent;
            }

            canvasView.InvalidateSurface();
        }

        void OnCornerRadiusChanged()
        {
            canvasView.InvalidateSurface();
        }

        void OnShadowColorChanged()
        {
            isShadowWithBackgroundColor = ShadowColor != Color.Transparent;

            canvasView.InvalidateSurface();
        }

        void OnShadowThicknessChanged()
        {
            OnContentChanged();
        }
    }
}
