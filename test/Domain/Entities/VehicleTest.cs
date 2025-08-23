
using MinimalApi.Domain.Entities;

namespace test.Domain.Entities
{
    [TestClass]
    public class VehicleTest
    {
        [TestMethod]
        public void GetSetProperties()
        {
            var vehicle = new Vehicle
            {
                Id = 1,
                Brand = "Porshe",
                Name = "911",
                Year = "2020"
            };

            Assert.AreEqual(1, vehicle.Id);
            Assert.AreEqual("Porshe", vehicle.Brand);
            Assert.AreEqual("911", vehicle.Name);
            Assert.AreEqual("2020", vehicle.Year);
        }
    }
}