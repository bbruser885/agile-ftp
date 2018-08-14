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

	[Fact]
	public void CmdMoveLocalFile_FalseWithNoArguments()
	{
	    CmdMoveLocalFile _move = new CmdMoveLocalFile();
	    string[] args = {};
	    var result = _move.Validate(args);
	    Assert.False(result, "Local move should return false with no arguments");
	}

	[Fact]
	public void CmdMoveLocalFile_FalseWithLessThan3()
	{
	    CmdMoveLocalFile _move = new CmdMoveLocalFile();
	    string[] args = {"string1", "string2"};
	    var result = _move.Validate(args);
	    Assert.False(result, "Local move should return false with less than 3 arguments");
	}

	[Fact]
	public void CmdMoveLocalFile_FalseWithInvalidFilename()
	{
	    CmdMoveLocalFile _move = new CmdMoveLocalFile();
	    string[] args = {"string1", "this should never be a valid filename", "string2"};
	    var result = _move.Validate(args);
	    Assert.False(result, "Local move should return false with less than 3 arguments");
	}

	[Fact]
	public void CmdUpload_FalseWithNoArguments()
	{
	    CmdUpload _upload = new CmdUpload();
	    string[] args = {};
	    var result = _upload.Validate(args);
	    Assert.False(result, "Upload should return false with no arguments");
	}

	[Fact]
	public void CmdUpload_TrueWithMoreThan2Args()
	{
	    CmdUpload _upload = new CmdUpload();
	    string[] args = {"string1", "string2", "string3"};
	    var result = _upload.Validate(args);
	    Assert.True(result, "Upload should return true with more than 2 arguments");
	}

	[Fact]
	public void CmdDownload_FalseWithNoArguments()
	{
	    CmdDownload _download = new CmdDownload();
	    string[] args = {};
	    var result = _download.Validate(args);
	    Assert.False(result, "Download should return false with no arguments");
	}

	[Fact]
	public void CmdDownload_TrueWithMoreThan2Args()
	{
	    CmdDownload _download = new CmdDownload();
	    string[] args = {"string1", "string2", "string3"};
	    var result = _download.Validate(args);
	    Assert.True(result, "Download should return true with more than 2 arguments");
	}

	[Fact]
	public void CmdRename_FalseWithNoArguments()
	{
	    CmdRename _rename = new CmdRename();
	    string[] args = {};
	    var result = _rename.Validate(args);
	    Assert.False(result, "Rename should return false with no arguments");
	}

	[Fact]
	public void CmdRename_TrueWithMoreThan2Args()
	{
	    CmdRename _rename = new CmdRename();
	    string[] args = {"string1", "string2", "string3"};
	    var result = _rename.Validate(args);
	    Assert.True(result, "Rename should return true with more than 2 arguments");
	}

	[Fact]
	public void CmdDelete_FalseWithNoArguments()
	{
	    CmdDelete _delete = new CmdDelete();
	    string[] args = {};
	    var result = _delete.Validate(args);
	    Assert.False(result, "Delete should return false with no arguments");
	}

	[Fact]
	public void CmdDelete_TrueWithMoreThan2Args()
	{
	    CmdDelete _delete = new CmdDelete();
	    string[] args = {"string1", "string2", "string3"};
	    var result = _delete.Validate(args);
	    Assert.True(result, "Delete should return true with more than 2 arguments");
	}

	[Fact]
	public void CmdDeleteDir_FalseWithNoArguments()
	{
	    CmdDeleteDir _delete = new CmdDeleteDir();
	    string[] args = {};
	    var result = _delete.Validate(args);
	    Assert.False(result, "Delete directory should return false with no arguments");
	}

	[Fact]
	public void CmdDeleteDir_TrueWithMoreThan2Args()
	{
	    CmdDeleteDir _delete = new CmdDeleteDir();
	    string[] args = {"string1", "string2", "string3"};
	    var result = _delete.Validate(args);
	    Assert.True(result, "Delete directory should return true with more than 2 arguments");
	}

	[Fact]
	public void CmdHelp_TrueWithNoArguments()
	{
	    CmdHelp _help = new CmdHelp();
	    string[] args = {};
	    var result = _help.Validate(args);
	    Assert.True(result, "Help should return true with no arguments");
	}

	[Fact]
	public void CmdHelp_FalseWithMore1Args()
	{
	    CmdHelp _help = new CmdHelp();
	    string[] args = {"string1", "string2"};
	    var result = _help.Validate(args);
	    Assert.False(result, "Help should return false with more than one arguments");
	}
    }
}
