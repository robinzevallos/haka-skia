using Haka.Core.Test;
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
            XamarinMock.Init(this);
        }

        public void Dispose()
        {
        }
    }
}
