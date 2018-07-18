using System;
using Xunit;

namespace FtpConnection.Tests
{
    public class FtpConnectionTests
    {
        [Fact]
        public void Validate_TrueIfEmpty()
        {
	    FtpConnectionManager _FtpConnection = new FtpConnectionManager("", "", "");
	    var result = _FtpConnection.Validate();
	    Assert.True(result, "should be true for no input");
        }
    }
}
