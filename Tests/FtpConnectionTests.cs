using System;
using Xunit;

namespace FtpConnection.Tests
{
    public class FtpConnectionTests
    {
        [Fact]
        public void Validate_FalseIfEmpty()
        {
	    FtpConnectionManager _FtpConnection = new FtpConnectionManager("", "", "");
	    var result = _FtpConnection.Validate();
	    Assert.False(result, "should be false for invalid input");
        }
    }
}
