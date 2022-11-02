using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Application;
using VikopApi.Application.User;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.FactoryTests
{
    [TestFixture]
    public class UserFactoryTests
    {
        [Test]
        public void CreateListItem()
        {
            var factory = new UserFactory();
            var user = new ApplicationUser
            {
                UserName = "name",
                Created = new DateTime(1, 1, 1),
                Rank = Rank.Green,
                Id = "id"
            };

            var item = factory.CreateListItem(user);

            Assert.Multiple(() =>
            {
                Assert.That(item.Id, Is.EqualTo(user.Id));
                Assert.That(item.Username, Is.EqualTo(user.UserName));
                Assert.That(item.Rank, Is.EqualTo(user.Rank));
            });
        }

        [Test]
        public void CreateModel()
        {
            var factory = new UserFactory();
            var user = new ApplicationUser
            {
                UserName = "name",
                Created = new DateTime(1, 1, 1),
                Rank = Rank.Green,
                Id = "id"
            };

            var model = factory.CreateModel(user);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(user.Id));
                Assert.That(model.UserName, Is.EqualTo(user.UserName));
                Assert.That(model.Rank, Is.EqualTo(user.Rank));
                Assert.That(model.Created, Is.EqualTo(user.Created.GetDate()));
            });
        }
    }
}
