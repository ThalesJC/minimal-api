
using MinimalApi.Domain.Entities;

namespace test.Domain.Entities
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void GetSetProperties()
        {
            var admin = new Admin
            {
                Id = 1,
                Email = "teste@email.com.br",
                Password = "senha teste",
                Role = "admin"
            };

            Assert.AreEqual(1, admin.Id);
            Assert.AreEqual("teste@email.com.br", admin.Email);
            Assert.AreEqual("senha teste", admin.Password);
            Assert.AreEqual("admin", admin.Role);
        }
    }
}