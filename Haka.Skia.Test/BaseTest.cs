using Haka.Core.Test;
using Haka.Skia.Sample;
using System;
using Xunit;

namespace Haka.Skia.Test
{
    public class BaseTest : IClassFixture<MainFixture>
    {
    }

    public class MainFixture : IDisposable
    {
        public MainFixture()
        {
            XamarinMock.Init(new AppMock());
        }

        public void Dispose()
        {
        }
    }
}
