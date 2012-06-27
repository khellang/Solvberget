﻿using System;
using System.Collections.Generic;

namespace Solvberget.Domain.Abstract
{
    public interface ISpellingDictionary
    {
        string Lookup(string value);
        string[] SuggestionList();
    }

}