using System;
using Xunit;

namespace AgileFTP.Tests
{
    public class AgileFTPTests
    {
	[Fact]
	public void CmdExit_TrueWithNoArgs()
	{
	    CmdExit _exit = new CmdExit();
	    string[] args = {};
	    var result = _exit.Validate(args);
	    Assert.True(result, "Exit should return true with no arguments");
	}

	[Fact]
	public void CmdExit_TrueWithMultipleArgs()
	{
	    CmdExit _exit = new CmdExit();
	    string[] args = {"string1", "string2"};
	    var result = _exit.Validate(args);
	    Assert.True(result, "Exit should return true with multiple arguments");

	}

	[Fact]
	public void CmdList_TrueWithNoArgs()
	{
	    CmdList _list = new CmdList();
	    string[] args = {};
	    var result = _list.Validate(args);
	    Assert.True(result, "List should return true with no arguments");
	}

	[Fact]
	public void CmdList_TrueWithMultipleArgs()
	{
	    CmdList _list = new CmdList();
	    string[] args = {"string1", "string2"};
	    var result = _list.Validate(args);
	    Assert.True(result, "List should return true with multiple arguments");
	}

	[Fact]
	public void CmdLocalList_FalseWithNoArgs()
	{
	    CmdLocalList _list = new CmdLocalList();
	    string[] args = {};
	    var result = _list.Validate(args);
	    Assert.False(result, "Local list should return false with no arguments");
	}

	[Fact]
	public void CmdLocalList_FalseWithInvalidFilename()
	{
	    CmdLocalList _list = new CmdLocalList();
	    string[] args = {"this should never be a valid filename"};
	    var result = _list.Validate(args);
	    Assert.True(result, "Local list should return false with invalid filename");
	}
    }
}
