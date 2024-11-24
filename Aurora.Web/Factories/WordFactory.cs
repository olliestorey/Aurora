namespace Aurora.Web.Factories
{

    /// <summary>
    /// A factory that provides a collection of words to be used in the game.
    /// Use this by newing up an instance of the factory and calling GetWords().
    /// </summary>
    public class HardcodedWordFactory : IWordFactory
    {
        public static List<string> CultureWords = new List<string> {
                    "together", "opportunity", "positivity", "partnerships", "Luke Fribbens", "Gold", "Tech", "Bournemouth", "Dubai", "Cold",
                    "Banana", "digital", "quality", "teams", "agency", "home", "office", "awards", "great", "culture", "understand",
                    "support", "personal", "success", "challenges", "impact", "upgrade", "solution", "placement", "modern", "team", "can do",
                    "teamwork", "communication", "creativity"
            }.Select(x => x.ToLower()).ToList();

        public static List<string> OfficeWords = new List<string> {
                    "Niz", "Ben", "Colgan", "Biscuit", "Donut", "Mehdi", "Placements", "Hackathon", "Amber", "Carly",
                    "Carl", "Spray Tan", "Pub", "Ping Pong", "Darts", "Freya", "Gen Z", "Bear", "Owen", "Ollie", "Jess",
                    "Coffee", "Diet Coke", "Tea", "Millie", "Heater", "Pokemon", "Minecraft", "Pint", "Beer", "Fridge", "Water",
                    "Slack", "XMas", "Birthday", "Fribbens"
            }.Select(x => x.ToLower()).ToList();

        public static List<string> DevHard = new List<string> {
                        "algorithm", "backend", "frontend", "refactor", "recursion", "repository", "inheritance", "constructor", "iteration", "namespace",
                    "dependency", "exception", "interface", "runtime", "Deployment", "Framework", "Full Stack", "Umbraco", "Azure", "Website", "development",
                    "Kentico", "Cyber", "Automation", "testing", "features", "content", "development", "upgrades", "support", "build", "cloud",
                    "Transformation", "products", "services"
            }.Select(x => x.ToLower()).ToList();

        public static List<string> DevEasy = new List<string> {
                    "array", "async", "await", "binary", "bug", "class", "closure", "compile", "database", "debug",
                    "design", "framework", "function", "javascript", "typescript", "json", "lambda", "library", "linq", "method", "null",
                    "object", "syntax", "oop", "variable", "CMS", "API", "Browser", "Code", "Git", "mvc", "Text", "push", "pull", "commit"
            }.Select(x => x.ToLower()).ToList();

        public Task<IEnumerable<string>> GetWords(string? listType)
        {
            var allWords = new List<string>();

            allWords.AddRange(CultureWords);
            allWords.AddRange(OfficeWords);
            allWords.AddRange(DevHard);
            allWords.AddRange(DevEasy);

            return listType switch
            {
                "Culture" => Task.FromResult((IEnumerable<string>)CultureWords),
                "Office" => Task.FromResult((IEnumerable<string>)OfficeWords),
                "DevHard" => Task.FromResult((IEnumerable<string>)DevHard),
                "DevEasy" => Task.FromResult((IEnumerable<string>)DevEasy),
                _ => Task.FromResult((IEnumerable<string>)allWords.OrderBy(x => new Random().Next()).Take(35).ToList())
            };
        }

    }
}
interface IWordFactory
{
    public Task<IEnumerable<string>> GetWords(string? listType);
}
