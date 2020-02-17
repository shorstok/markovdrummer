﻿namespace markov_drummer.Markov.Chiscore.Models
{
    public class NgramChainMatch<T>
    {
        internal NgramChainMatch(T ngram, bool matches)
        {
            Ngram = ngram;
            MatchesChain = matches;
        }

        public T Ngram { get; }
        public bool MatchesChain { get; }
    }
}