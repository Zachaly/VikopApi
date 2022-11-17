using VikopApi.Application.Command;
using VikopApi.Application.Models.Command;

namespace VikopApi.Tests.Unit.FactoryTests
{
    [TestFixture]
    public class CommandResponseFactoryTests
    {
        [Test]
        public void CreateSuccess_CommandResponseModel()
        {
            var factory = new CommandResponseFactory();

            var res = factory.CreateSuccess();

            Assert.That(res.Code, Is.EqualTo(CommandResponseCode.Success));
        }

        [Test]
        public void CreateFailure_CommandResponseModel()
        {
            var factory = new CommandResponseFactory();

            var errors = new Dictionary<string, IEnumerable<string>>();
            errors.Add("username", new List<string> { "username is too short", "username cointains not allowed characters" });

            var res = factory.CreateFailure(errors);

            Assert.Multiple(() =>
            {
                Assert.That(res.Code, Is.EqualTo(CommandResponseCode.Fail));
                Assert.That(res.Errors.Keys, Is.EquivalentTo(errors.Keys));
                Assert.That(res.Errors.Values, Is.EquivalentTo(errors.Values));
            });
        }

        [Test]
        public void CreateSuccess_DataCommandResponseModel()
        {
            var factory = new CommandResponseFactory();
            int data = 1;

            var res = factory.CreateSuccess(data);

            Assert.Multiple(() =>
            {
                Assert.That(res.Code, Is.EqualTo(CommandResponseCode.Success));
                Assert.That(res.Data, Is.EqualTo(data));
                Assert.That(res, Is.InstanceOf<DataCommandResponseModel<int>>());
            });
        }

        [Test]
        public void CreateFailure_DataCommandResponseModel()
        {
            var factory = new CommandResponseFactory();

            var errors = new Dictionary<string, IEnumerable<string>>();
            errors.Add("username", new List<string> { "username is too short", "username cointains not allowed characters" });

            var res = factory.CreateFailure<int>(errors);

            Assert.Multiple(() =>
            {
                Assert.That(res.Code, Is.EqualTo(CommandResponseCode.Fail));
                Assert.That(res.Errors.Keys, Is.EquivalentTo(errors.Keys));
                Assert.That(res.Errors.Values, Is.EquivalentTo(errors.Values));
                Assert.That(res, Is.InstanceOf<DataCommandResponseModel<int>>());
            });
        }
    }
}
