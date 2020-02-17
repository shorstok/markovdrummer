using System;

namespace markov_drummer.Markov.Chiscore.Components
{
    public abstract class UnigramSelectorBase
    {
        public abstract Guid Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
    }
}