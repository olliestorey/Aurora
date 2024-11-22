namespace Aurora.Web.Factories
{

    /// <summary>
    /// A factory that provides a collection of words to be used in the game.
    /// Use this by newing up an instance of the factory and calling GetWords().
    /// </summary>
    public class HardcodedWordFactory : IWordFactory
    {
        public Task<ICollection<string>> GetWords()
        {
            return Task.FromResult((ICollection<string>)new List<string> {
                "algorithm", "array", "async", "await", "backend", "binary", "bug", "class", "closure", "compile",
                "constructor", "database", "debug", "dependency", "design", "exception", "framework", "frontend",
                "function", "inheritance", "interface", "iteration", "javascript", "json", "lambda", "library",
                "linq", "method", "namespace", "null", "object", "oop", "polymorphism", "recursion", "refactor",
                "repository", "runtime", "syntax", "variable"
            });
        }
    }

    interface IWordFactory
    {
        public Task<ICollection<string>> GetWords();
    }
}
