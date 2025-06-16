using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mind_Your_Drink_Models.Models;
using FluentAssertions;

namespace ModelTest
{
    public class UserTests
    {
        [Fact]
        public void User_CanBe_Created()
        {
            var user = User.CreateUser("Name", "Password");

            user.Name.Should().Be("L");
            user.HashPassword.Should().Be("e7cf3ef4f17c3999a94f2c6f612e8a888e5b1026878e4e19398b23bd38ec221a");
            user.Email.Should().BeNull();
            user.StateName.Should().Be("Active");
        }

        [Fact]
        public void User_CanBe_Ban()
        {
            var user = User.CreateUser("Name", "Password");

            var admin = Admin.CreateAdmin("Name2", "Password2");

            admin.Ban(user);

            user.StateName.Should().Be("Banned");
        }

        [Fact]
        public void User_CanBe_UnBan()
        {
            var user = User.CreateUser("Name", "Password");

            var admin = Admin.CreateAdmin("Name2", "Password2");

            admin.Ban(user);

            user.StateName.Should().Be("Banned");

            admin.UnBan(user);

            user.StateName.Should().Be("Active");
        }
    }
}
