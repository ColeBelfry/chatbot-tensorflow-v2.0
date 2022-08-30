using PythonInterpreter;

namespace ChatBotTesting
{
    public class Tests
    {
        ToPython interpreter;

        [SetUp]
        public void Setup()
        {
            interpreter = new ToPython();
            interpreter.ExecuteCreateModelFunction("TestingModel", 500, 50, 0.001, new string[] { "dense", "dense" }, new int[] { 8, 8 });
        }

        [Test]
        public void SingleWordChat()
        {
            string result = interpreter.ExecuteChatFunction("TestingModel", "Hello");
            string[] validResponses = new string[] { "Hi!", "Hi there!", "Hello!", "Hello there!", "Hya!", "Hya there!", "Hey!", "Howdy!", "Sup!", "Yo." };
            if (string.IsNullOrEmpty(result))
            {
                Assert.Fail("Empty or null");
            }
            else if (validResponses.Where(res => result.Contains(res)).Count() == 0)
            {
                Assert.Fail("Not a valid response");
            }
            else
            {
                Assert.Pass();
            }
        }

        [Test]
        public void MultipleWordChat()
        {
            string result = interpreter.ExecuteChatFunction("bob", "What is your name?");
            string[] validResponses = new string[] { "You can call me ChatBot", "Call me ChatBot", "My name is ChatBot" };
            if (string.IsNullOrEmpty(result))
            {
                Assert.Fail("Empty or null");
            }
            else if (validResponses.Where(res => result.Contains(res)).Count() == 0)
            {
                Assert.Fail("Not a valid response");
            }
            else
            {
                Assert.Pass();
            }
        }

        [Test]
        public void EmptyChat()
        {
            string result = interpreter.ExecuteChatFunction("bob", " ");
            if (string.IsNullOrEmpty(result))
            {
                Assert.Fail("Empty or null");
            }
            else
            {
                Assert.Pass();
            }
        }
    }
}