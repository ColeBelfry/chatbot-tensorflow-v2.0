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
            string result = interpreter.ExecuteChatFunction("TestingModel", "What is your name?");
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
            string result = interpreter.ExecuteChatFunction("TestingModel", " ");
            if (string.IsNullOrEmpty(result))
            {
                Assert.Fail("Empty or null");
            }
            else
            {
                Assert.Pass();
            }
        }

        [Test]
        public void CanCreateModel()
        {
            string result = interpreter.ExecuteCreateModelFunction("TestingCreateModel", 1000, 50, 0.01, new string[] { "dense", "flatten", "dropout" }, new int[] { 8, 4, 6 });
            if (result.Contains("trained and saved successfully") || result.Contains("already exists"))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail("Unable to create a new model");
            }
        }

        [Test]
        public void CanTrainModel()
        {
            interpreter.ExecuteCreateModelFunction("TestingTrainModel", 500, 50, 0.001, new string[] { "dense", "dense" }, new int[] { 8, 8 });
            string result = interpreter.ExecuteTrainModelFunction("TestingTrainModel", 1000, 75, 0.001);
            if (result.Contains("trained successfully"))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail("Unable to train a model");
            }
        }
    }
}