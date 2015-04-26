﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nett.UnitTests
{
    /// <summary>
    /// Test for the basic test examples from https://github.com/BurntSushi/toml-test/tree/master/tests
    /// These cases handle some special cases and some document structure cases.
    /// Everything will be deserialized into a generic TomlTable data structure. Extracting the data has to be done by hand.
    /// </summary>  
    public class ReadValidTomlUntypedTests
    {
        [Fact]
        public void ReadValidTomlUntyped_EmptyArray()
        {
            // Arrange
            var toml = TomlStrings.Valid.ArrayEmpty;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.NotNull(read);
            Assert.Equal(1, read.Rows.Count);
            Assert.Equal(typeof(TomlArray), read.Rows["thevoid"].GetType());
            var rootArray = read.Rows["thevoid"].Get<TomlArray>();
            Assert.Equal(1, rootArray.Count);
            var subArray = rootArray.Get<TomlArray>(0);
            Assert.Equal(0, subArray.Count);
        }

        [Fact]
        public void ReadValidToml_ArrayNoSpaces()
        {
            // Arrange
            var toml = TomlStrings.Valid.ArrayNoSpaces;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(1, read.Get<TomlArray>("ints").Get<int>(0));
            Assert.Equal(2, read.Get<TomlArray>("ints").Get<int>(1));
            Assert.Equal(3, read.Get<TomlArray>("ints").Get<int>(2));
        }

        [Fact]
        public void ReadValidToml_HetArray()
        {
            // Arrange
            var toml = TomlStrings.Valid.ArrayHeterogenous;

            // Act
            var read = Toml.Read(toml);

            // Assert
            var a = read.Get<TomlArray>("mixed");
            Assert.NotNull(a);
            Assert.Equal(3, a.Count);

            var intArray = a.Get<TomlArray>(0);
            Assert.Equal(1, intArray.Get<int>(0));
            Assert.Equal(2, intArray.Get<int>(1));

            var stringArray = a.Get<TomlArray>(1);
            Assert.Equal("a", stringArray.Get<string>(0));
            Assert.Equal("b", stringArray.Get<string>(1));

            var doubleArray = a.Get<TomlArray>(2);
            Assert.Equal(1.1, doubleArray.Get<double>(0));
            Assert.Equal(2.1, doubleArray.Get<double>(1));
        }

        [Fact]
        public void RealValidToml_NestedArrays()
        {
            // Arrange
            var toml = TomlStrings.Valid.ArraysNested;

            // Act
            var read = Toml.Read(toml);

            // Assert
            var a = read.Get<TomlArray>("nest");
            Assert.Equal("a", a.Get<TomlArray>(0).Get<string>(0));
            Assert.Equal("b", a.Get<TomlArray>(1).Get<string>(0));
        }

        [Fact]
        public void ReadValidToml_Arrays()
        {
            // Arrange
            var toml = TomlStrings.Valid.Arrays;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(3, read.Get<TomlArray>("ints").Get<TomlArray>().Count);
            Assert.Equal(3, read.Get<TomlArray>("floats").Get<TomlArray>().Count);
            Assert.Equal(3, read.Get<TomlArray>("strings").Get<TomlArray>().Count);
            Assert.Equal(3, read.Get<TomlArray>("dates").Get<TomlArray>().Count);
        }

        [Fact]
        public void ReadValidToml_TableArrayNested()
        {
            // Arrange
            var toml = TomlStrings.Valid.TableArrayNested;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(1, read.Rows.Count);
            Assert.Equal(typeof(TomlTableArray), read.Rows["albums"].GetType());
            var arr = read.Get<TomlTableArray>("albums");
            Assert.Equal(2, arr.Count);

            var t0 = arr[0];
            Assert.Equal("Born to Run", t0.Get<string>("name"));
            var t0s = t0.Get<TomlTableArray>("songs");
            Assert.Equal(2, t0s.Count);
            var s0 = t0s[0];
            var s1 = t0s[1];
            Assert.Equal("Jungleland", s0.Get<string>("name"));
            Assert.Equal("Meeting Across the River", s1.Get<string>("name"));
        }

        [Fact]
        public void ReadValidTomlUntyped_Boolean()
        {
            // Arrange
            var toml = TomlStrings.Valid.Boolean;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(true, read.Get<bool>("t"));
            Assert.Equal(false, read.Get<bool>("f"));
        }

        [Fact]
        public void ReadValidTomlUntyped_CommentsEverywere()
        {
            // Arrange
            var toml = TomlStrings.Valid.CommentsEverywhere;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(42, read.Get<TomlTable>("group").Get<int>("answer"));
            Assert.Equal(2, ((TomlTable)read["group"]).Get<int[]>("more").Length);
            Assert.Equal(42, ((TomlTable)read["group"]).Get<List<int>>("more")[0]);
            Assert.Equal(42, ((TomlTable)read["group"]).Get<int[]>("more")[0]);
        }

        [Fact]
        public void ReadValidTomlUntyped_DateTime()
        {
            // Arrange
            var toml = TomlStrings.Valid.DateTime;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(DateTime.Parse("1987-07-05T17:45:00Z"), read.Get<DateTime>("bestdayever"));
        }

        [Fact]
        public void ReadValidTomlUntyped_Empty()
        {
            // Arrange
            var toml = TomlStrings.Valid.Empty;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(0, read.Rows.Count);
        }

        [Fact]
        public void ReadValidToml_Example()
        {
            // Arrange
            var toml = TomlStrings.Valid.Example;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(2, read.Rows.Count);
            Assert.Equal(DateTime.Parse("1987-07-05T17:45:00Z"), read.Get<DateTime>("best-day-ever"));
            var tt = (TomlTable)read["numtheory"];
            Assert.Equal(false, tt.Get<bool>("boring"));
            var ta = (TomlArray)tt["perfection"];
            Assert.Equal(3, ta.Count);
            Assert.Equal(6, ta.Get<int>(0));
            Assert.Equal(28, ta.Get<char>(1));
            Assert.Equal(496, ta.Get<ushort>(2));
        }

        [Fact]
        public void ReadValidTomlUntyped_Floats()
        {
            // Arrange
            var toml = TomlStrings.Valid.Floats;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(3.14, read.Get<float>("pi"), 2);
            Assert.Equal(-3.14, read.Get<double>("negpi"), 2);
        }

        [Fact]
        public void ReadValidTomlUntyped_ImplicitAndExplicitAfter()
        {
            // Arrange
            var toml = TomlStrings.Valid.ImplicitAndExplicitAfter;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(42, read.Get<TomlTable>("a").Get<TomlTable>("b").Get<TomlTable>("c").Get<int>("answer"));
            Assert.Equal(43, read.Get<TomlTable>("a").Get<long>("better"));
        }

        [Fact]
        public void ReadValidTomlUntyped_ImplicitAndExplicitBefore()
        {
            // Arrange
            var toml = TomlStrings.Valid.ImplicitAndExplicitBefore;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(42, read.Get<TomlTable>("a").Get<TomlTable>("b").Get<TomlTable>("c").Get<int>("answer"));
            Assert.Equal(43, read.Get<TomlTable>("a").Get<long>("better"));
        }

        [Fact]
        public void ReadValidToml_ImplicitGroups()
        {
            // Arrange
            var toml = TomlStrings.Valid.ImplicitGroups;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(1, read.Rows.Count);
            Assert.Equal(42, read.Get<TomlTable>("a").Get<TomlTable>("b").Get<TomlTable>("c").Get<char>("answer"));
        }

        [Fact]
        public void ReadValidTomlUntyped_Integer()
        {
            // Arrange
            var toml = TomlStrings.Valid.Integer;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(2, read.Rows.Count);
            Assert.Equal((uint)42, read.Get<uint>("answer"));
            Assert.Equal(-42, read.Get<short>("neganswer"));
        }

        [Fact]
        public void ReadValidTomlUntyped_KeyEqualsNoSpace()
        {
            // Arrange
            var toml = TomlStrings.Valid.KeyEqualsNoSpace;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(1, read.Rows.Count);
            Assert.Equal(42, read.Get<int>("answer"));
        }

        [Fact]
        public void ReadValidTomlUntyped_KeyEqualsSpace()
        {
            // Arrange
            var toml = TomlStrings.Valid.KeyEqualsSpace;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(1, read.Rows.Count);
            Assert.Equal(42, read.Get<int>("a b"));
        }

        [Fact]
        public void ReadValidTomlUntyped_KeySpecialChars()
        {
            // Arrange
            var toml = TomlStrings.Valid.KeySpecialChars;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(1, read.Rows.Count);
            Assert.Equal(42, read.Get<int>("~!@$^&*()_+-`1234567890[]|/?><.,;:'"));
        }


        [Fact]
        public void ReadValidTomlUntyped_LongFloats()
        {
            // Arrange
            var toml = TomlStrings.Valid.LongFloats;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(2, read.Rows.Count);
            Assert.Equal(double.Parse("3.141592653589793", CultureInfo.InvariantCulture), read.Get<double>("longpi"));
            Assert.Equal(double.Parse("-3.141592653589793", CultureInfo.InvariantCulture), read.Get<double>("neglongpi"));
        }

        [Fact]
        public void ReadValidTomlUntyped_LongInts()
        {
            // Arrange
            var toml = TomlStrings.Valid.LongInts;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(2, read.Rows.Count);
            Assert.Equal(9223372036854775806, read.Get<long>("answer"));
            Assert.Equal(-9223372036854775807, read.Get<long>("neganswer"));
        }


        [Fact]
        public void ReadValidTomlUntyped_MultiLineStrings()
        {
            // Arrange
            var toml = TomlStrings.Valid.MultiLineStrings;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(7, read.Rows.Count);
            Assert.Equal("", read.Get<string>("multiline_empty_one"));
            Assert.Equal("", read.Get<string>("multiline_empty_two"));
            Assert.Equal("", read.Get<string>("multiline_empty_three"));
            Assert.Equal("" , read.Get<string>("multiline_empty_four"));
            Assert.Equal("The quick brown fox jumps over the lazy dog.", read.Get<string>("equivalent_one"));
            Assert.Equal("The quick brown fox jumps over the lazy dog.", read.Get<string>("equivalent_two"));
            Assert.Equal("The quick brown fox jumps over the lazy dog.", read.Get<string>("equivalent_three"));
        }

        [Fact(Skip = "Error in tokenizer when there is a ' within the ''' tags. Not important rightnow.")]
        public void ReadValidTomlUntyped_RawMultineStrings()
        {
            // Arrange
            var toml = TomlStrings.Valid.RawMultilineStrings;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(3, read.Rows.Count);
            Assert.Equal("This string has a ' quote character.", read.Get<string>("oneline"));
            Assert.Equal("\r\nThis string has a ' quote character.", read.Get<string>("firstnl"));
            Assert.Equal("\r\n\r\nThis string\r\n has a ' quote character\r\nand more than\r\none newline\r\nin it.", read.Get<string>("multiline"));
        }

        [Fact]
        public void ReadValidStringsUntyped_RawStrings()
        {
            // Arrange
            var toml = TomlStrings.Valid.RawStrings;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(7, read.Rows.Count);
            Assert.Equal("This string has a \\b backspace character.", read.Get<string>("backspace"));
            Assert.Equal("This string has a \\t tab character.", read.Get<string>("tab"));
            Assert.Equal("This string has a \\n new line character.", read.Get<string>("newline"));
            Assert.Equal("This string has a \\f form feed character.", read.Get<string>("formfeed"));
            Assert.Equal("This string has a \\r carriage return character.", read.Get<string>("carriage"));
            Assert.Equal("This string has a \\/ slash character.", read.Get<string>("slash"));
            Assert.Equal("This string has a \\\\ backslash character.", read.Get<string>("backslash"));
        }

        [Fact]
        public void ReadValidStringsUntyped_StringEmpty()
        {
            // Arrange
            var toml = TomlStrings.Valid.StringEmpty;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(1, read.Rows.Count);
            Assert.Equal("", read.Get<string>("answer"));
        }

        [Fact(Skip = "Case 3 fails, because newline in string is not allowed, parsing will fail, Not important for now.")]
        public void ReadValidStringsUntyped_StringEsapces()
        {
            // Arrange
            var toml = TomlStrings.Valid.StringEscapes;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(11, read.Rows.Count);
            Assert.Equal("This string has a \b backspace character.", read.Get<string>("backspace"));
            Assert.Equal("This string has a \t tab character.", read.Get<string>("tab"));
            Assert.Equal("This string has a \n new line character.", read.Get<string>("newline"));
            Assert.Equal("This string has a \f form feed character.", read.Get<string>("formfeed"));
            Assert.Equal("This string has a \r carriage return character.", read.Get<string>("carriage"));
            Assert.Equal("This string has a \" quote character.", read.Get<string>("quote"));
            Assert.Equal("This string has a \\ backslash character.", read.Get<string>("backslash"));
            Assert.Equal("This string does not have a unicode \\u escape.", read.Get<string>("notunicode1"));
            Assert.Equal("This string does not have a unicode \u005Cu escape.", read.Get<string>("notunicode2"));
            Assert.Equal("This string does not have a unicode \\u0075 escape.", read.Get<string>("notunicode3"));
            Assert.Equal("This string does not have a unicode \\\u0075 escape.", read.Get<string>("notunicode4"));
        }

        [Fact]
        public void ReadValidStringsUntyped_StringWihtPound()
        {
            // Arrange
            var toml = TomlStrings.Valid.StringWithPound;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(2, read.Rows.Count);
            Assert.Equal("We see no # comments here.", read.Get<string>("pound"));
            Assert.Equal("But there are # some comments here.", read.Get<string>("poundcomment"));
        }

        [Fact(Skip = "Corner case more important stuff to get to work first")]
        public void ReadValidStringsUntyped_TableArrayImplicit()
        {
            // Arrange
            var toml = TomlStrings.Valid.TableArrayImplicit;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(2, read.Rows.Count);
            Assert.Equal("Glory Days", ((read.Get<TomlTableArray>("albums")[0]).Get<TomlTableArray>("songs")[0]).Get<string>("name"));
            Assert.Equal("We see no # comments here.", read.Get<string>("pound"));
            Assert.Equal("But there are # some comments here.", read.Get<string>("poundcomment"));
        }

        [Fact]
        public void ReadValidStringsUntyped_TableArrayMany()
        {
            // Arrange
            var toml = TomlStrings.Valid.TableArrayMany;

            // Act
            var read = Toml.Read(toml);

            // Assert
            Assert.Equal(1, read.Rows.Count);
            Assert.Equal(3, read.Get<TomlTableArray>("people").Count);
            var tt = read.Get<TomlTableArray>("people")[0];
            Assert.Equal("Bruce", tt.Get<string>("first_name"));
            Assert.Equal("Springsteen", tt.Get<string>("last_name"));
            tt = read.Get<TomlTableArray>("people")[1];
            Assert.Equal("Eric", tt.Get<string>("first_name"));
            Assert.Equal("Clapton", tt.Get<string>("last_name"));
            tt = read.Get<TomlTableArray>("people")[2];
            Assert.Equal("Bob", tt.Get<string>("first_name"));
            Assert.Equal("Seger", tt.Get<string>("last_name"));
        }
    }
}