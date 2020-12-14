using Haka.Skia.Sample;
using Xunit;

namespace Haka.Skia.Test
{
    public class API_Test : BaseTest
    {    
        [Fact]
        public void ImageSkia_Render()
        {
            Assert.NotNull(new ImageSkia());
        }

        [Fact]
        public void ShadowFrameSkia_Render()
        {
            Assert.NotNull(new ShadowFrameSkia());

        }

        [Fact]
        public void SvgSkia_Render()
        {
            Assert.NotNull(new SvgSkia());
        }

        [Fact]
        public void Client_test()
        {
            Assert.NotNull(new MainPage());
        }
    }
}
