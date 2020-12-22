//------------------------------------------------------------------------------
// <copyright file="SBHelper.cs" company="Micro Support Center, Inc.">
//     Copyright (c) Micro Support Center, Inc..  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.Template.Helpers
{
    public class IndentingStringBuilder
    {
        private string _currentIndentString;
        private int _currentNumberOfIndents = 0;
        private StringBuilder _sb = new StringBuilder();

        public IndentingStringBuilder()
        {
            CurrentNumberOfIndents = 0;
        }

        public IndentingStringBuilder(int numberOfIndents)
        {
            CurrentNumberOfIndents = numberOfIndents;
        }

        public string CurrentIndentString
        {
            get
            {
                return _currentIndentString;
            }
        }

        public int CurrentNumberOfIndents
        {
            get
            {
                return _currentNumberOfIndents;
            }
            set
            {
                _currentNumberOfIndents = value;
                _currentIndentString = new string('\t', value);
            }
        }

        public void Append(string value)
        {
            _sb.Append(value); //don't futz with the indents on this one.
        }

        public void AppendLine(string value)
        {
            if (value.Equals(""))
            {
                _sb.AppendLine(""); //so we don't include unnecessary tabs that mess up winmerge
                return;
            }
            //figure out how much to increase or decrease our indent
            int addIndent = CountChars(value, '{');
            int subtractIndent = CountChars(value, '}');
            int original = CurrentNumberOfIndents;

            if (addIndent - subtractIndent < 0)
            {
                //recalc the indent if the indent is dropping. (more '}' than '{' )
                RecalcIndent(addIndent - subtractIndent);
                addIndent = 0;
                subtractIndent = 0;
            }

            _sb.Append(_currentIndentString);
            //SB.AppendLine(line + $"//{CurrentIndent} add:{addIndent} subtract: {subtractIndent}, original:{original}");
            _sb.AppendLine(value);

            //only change the indent string if the indent changed.
            if (addIndent - subtractIndent != 0)
            {
                RecalcIndent(addIndent - subtractIndent);
            }
        }

        public int CountChars(string line, char ch)
        {
            int cnt = 0;
            foreach (char c in line)
            {
                if (c == ch) cnt++;
            }
            return cnt;
        }

        public void RecalcIndent(int indentChange)
        {
            CurrentNumberOfIndents += indentChange;
            if (CurrentNumberOfIndents < 0)
            {
                CurrentNumberOfIndents = 0;
            }
            _currentIndentString = new string('\t', CurrentNumberOfIndents);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}