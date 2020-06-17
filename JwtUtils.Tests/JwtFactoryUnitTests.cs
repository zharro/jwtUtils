using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using JwtUtils.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace JwtUtils.Tests
{
    public class JwtFactoryUnitTests
    {
        [Fact]
        public async void GenerateEncodedToken_GivenValidInputs_ReturnsExpectedTokenData()
        {
            // arrange
            var token = Guid.NewGuid().ToString();
            var id = Guid.NewGuid().ToString();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("secret_key"));
            var jwtIssuerOptions = new JwtIssuerOptions
            {
                Issuer = "",
                Audience = "",
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            mockJwtTokenHandler.Setup(handler => handler.WriteToken(It.IsAny<JwtSecurityToken>())).Returns(token);

            var jwtFactory = new JwtFactory(mockJwtTokenHandler.Object, Options.Create(jwtIssuerOptions));

            // act
            var result = await jwtFactory.GenerateEncodedToken(id, "userName");

            // assert
            Assert.Equal(token, result.Token);
        }
    }
}
