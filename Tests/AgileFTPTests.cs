using System;
using Xunit;
using Moq;

namespace AgileFTP.Tests
{
    public class AgileFTPTests
    {
	[Fact]
	public void CmdExit_AlwaysTrueNoArgs()
	{
	    CmdExit _exit = new CmdExit();
	    string[] args = {""};
	    var result = _exit.Validate(args);
	    Assert.True(result, "Exit should always return true");
	}

	[Fact]
	public void CmdExit_AlwaysTrueWithArgs()
	{
	    CmdExit _exit = new CmdExit();
	    string[] args = {"string1", "string2"};
	    var result = _exit.Validate(args);
	    Assert.True(result, "Exit should always return true");

	}
    }
}
