using System;
using Code.Helpers;
using Xunit;

namespace Tests
{
    public class ArgumentParserTests
    {
        [Fact]
        public void Returns_Error_When_No_Arguments_Are_Passed()
        {
            var args = Array.Empty<string>();
            var argumentParser = new ArgumentParser(args);
            
            Assert.Single(argumentParser.ErrorMessages);
            Assert.Equal(
                "No arguments have been passed in",
                argumentParser.ErrorMessages[0]
            );
            Assert.Null(argumentParser.FilePath);
        }
        
        [Fact]
        public void Returns_Error_When_Wrong_Arguments_Are_Passed()
        {
            var args = new []{"string", "-d", "13312", "---f"};
            var argumentParser = new ArgumentParser(args);
            
            Assert.Equal(1, argumentParser.ErrorMessages.Count);
            Assert.Equal(
                "You need to specify a file path",
                argumentParser.ErrorMessages[0]
            );
            Assert.Null(argumentParser.FilePath);
        }
        
        [Fact]
        public void Returns_Error_When_incomplete_Arguments_Are_Passed()
        {
            var args = new []{"13312","--f"};
            var argumentParser = new ArgumentParser(args);
            
            Assert.Single(argumentParser.ErrorMessages);
            Assert.Equal(
                "You need to specify a file path",
                argumentParser.ErrorMessages[0]
            );
            Assert.Null(argumentParser.FilePath);
        }

        [Theory]
        [InlineData("not/a/real/path")]
        [InlineData("just a string")]
        public void Returns_Error_When_File_Path_Doesnt_Exist(string filePath)
        {
            var args = new[]
            {
                "--f", filePath
            };
            var argumentParser = new ArgumentParser(args);

            Assert.Null(argumentParser.FilePath);
            Assert.Single(argumentParser.ErrorMessages);
            Assert.Equal(
                "You need to provide an existing file path",
                argumentParser.ErrorMessages[0]
            );
        }

        [Fact]
        public void Returns_Error_When_File_Is_Bigger_Than_100K()
        {
            var args = new[]
            {
                "--f", "../../../big.file"
            };
            var argumentParser = new ArgumentParser(args);

            Assert.Null(argumentParser.FilePath);
            Assert.Single(argumentParser.ErrorMessages);
            Assert.Equal(
                "The file exceeds the allowed limit of 100kB",
                argumentParser.ErrorMessages[0]
            );
        }

        [Theory]
        [InlineData("--f,../../../ArgumentParserTests.cs,--d,23-04-2021", "../../../ArgumentParserTests.cs")]
        [InlineData("--f,../../../ArgumentParserTests.cs,not supposed to be here,--d,04-2021", "../../../ArgumentParserTests.cs")]
        [InlineData("--d,04-2021,--f,../../../ArgumentParserTests.cs", "../../../ArgumentParserTests.cs")]
        public void Returns_Arguments_Correctly(string inputArgs, string pathToFile)
        {
            var args = inputArgs.Split(',');
            var argumentParser = new ArgumentParser(args);
            
            Assert.Empty(argumentParser.ErrorMessages);
            Assert.Equal(pathToFile, argumentParser.FilePath);
        }
    }
}