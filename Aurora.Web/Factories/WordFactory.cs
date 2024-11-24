namespace Aurora.Web.Factories
{

    /// <summary>
    /// A factory that provides a collection of words to be used in the game.
    /// Use this by newing up an instance of the factory and calling GetWords().
    /// </summary>
    public class HardcodedWordFactory : IWordFactory
    {
        public Task<IEnumerable<string>> GetWords(string? listType)
        {
            var random = new Random();
            var allWords = new List<string>();

            //Culture
            var Culture = new List<string> {
                "together", "opportunity", "positivity", "partnerships", "Luke Fribbens", "Gold", "Tech", "Bournemouth", "Dubai", "Cold",
                "Banana", "digital", "quality", "teams", "agency", "home", "office", "awards", "great", "culture", "understand",
                "support", "personal", "success", "challenges", "impact", "upgrade", "solution", "placement", "modern", "team", "can do",
                "teamwork", "communication", "creativity"
            };

            //Office
            var Office = new List<string> {
                "Ahmed", "Ben", "Colgan", "Buiscuit", "Donut", "Mehdi", "Placements", "Hackathon", "Amber", "Carly",
                "Carl", "Spray Tan", "Pub", "Ping Pong", "Darts", "Freya", "Gen Z Marketing", "Polar Bear", "Owen", "Ollie", "Jess",
                "Coffee", "Diet Coke", "Tea", "Millie", "Heater", "Pokemon", "Minecraft", "Pint", "Beer", "Fridge", "Water",
                "Slack", "XMas", "Birthday"
            };

            //Dev Hard
            var DevHard = new List<string> {
                "algorithm", "backend", "frontend", "refactor", "recursion", "repository", "inheritance", "constructor", "iteration", "namespace",
                "dependency", "exception", "interface", "runtime", "Deployment", "Framework", "Full Stack", "Umbraco", "Azure", "Website", "development",
                "Kentico", "Cyber", "Automation", "testing", "features", "content", "development", "upgrades", "support", "build", "cloud",
                "Transformation", "products", "services"
            };

            //Dev Easy
            var DevEasy = new List<string> {
                "array", "async", "await", "binary", "bug", "class", "closure", "compile", "database", "debug",
                "design", "framework", "function", "javascript", "typescript", "json", "lambda", "library", "linq", "method", "null",
                "object", "syntax", "oop", "variable", "CMS", "API", "Browser", "Code", "Git", "mvc", "Text", "push", "pull", "commit"
            };

            allWords.AddRange(Culture);
            allWords.AddRange(DevHard);
            allWords.AddRange(DevEasy);

            return listType switch
            {
                "Culture" => Task.FromResult((IEnumerable<string>)Culture),
                "Office" => Task.FromResult((IEnumerable<string>)Office),
                "DevHard" => Task.FromResult((IEnumerable<string>)DevHard),
                "DevEasy" => Task.FromResult((IEnumerable<string>)DevEasy),
                _ => Task.FromResult((IEnumerable<string>)allWords.OrderBy(x => random.Next()).Take(35).ToList())
            };
        }
    }

    interface IWordFactory
    {
        public Task<IEnumerable<string>> GetWords(string? listType);
    }
}
