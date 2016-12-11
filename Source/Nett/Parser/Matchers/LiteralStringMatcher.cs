﻿using System.Text;

namespace Nett.Parser.Matchers
{
    internal sealed class LiteralStringMatcher
    {
        private const char StringTag = '\'';

        internal static Token? TryMatch(CharBuffer cs)
        {
            StringBuilder sb = new StringBuilder(256);
            if (!cs.TryExpect(StringTag))
            {
                return null;
            }

            var errPos = cs.FilePosition;

            if (cs.TryExpectAt(1, StringTag) && cs.TryExpectAt(2, StringTag))
            {
                return MultilineLiteralStringMatcher.TryMatch(cs);
            }

            sb.Append(cs.Consume());

            while (!cs.End && !cs.TryExpect(StringTag))
            {
                sb.Append(cs.Consume());
            }

            if (!cs.TryExpect(StringTag))
            {
                throw Parser.CreateParseError(errPos, Constants.ParseErrorStringNotClosed);
            }
            else
            {
                sb.Append(cs.Consume());
                return new Token(TokenType.LiteralString, sb.ToString());
            }
        }
    }
}
