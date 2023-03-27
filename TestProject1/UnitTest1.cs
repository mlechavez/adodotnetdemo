using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebApplication2;
using WebApplication2.Data;
using Xunit;

namespace TestProject1 {
    public class UnitTest1 {
        readonly AccountService service;
        readonly Mock<IDbConnection> moqConnection;

        public UnitTest1() {
            this.moqConnection = new Mock<IDbConnection>(MockBehavior.Strict);
            moqConnection.Setup(x => x.Open());
            moqConnection.Setup(x => x.Dispose());
            this.service = new AccountService(() => moqConnection.Object);
        }

        [Trait("DataReader", "2")]
        [Fact]
        public async Task ShouldReturnTwoAccountsAsync() {
            // Define the data reader, that returns one record.
            var moqDataReader = new Mock<IDataReader>();
            moqDataReader.SetupSequence(x => x.Read())
                .Returns(true) // First call return a record: true
                .Returns(true)
                .Returns(false); // Third call finish

            // Record to be returned
            moqDataReader.SetupGet<object>(x => x["Id"]).Returns(1);
            moqDataReader.SetupGet<object>(x => x["AccountNo"]).Returns("12345");
            moqDataReader.SetupGet<object>(x => x["Name"]).Returns("Matthew");

            // Define the command to be mock and use the data reader
            var commandMock = new Mock<IDbCommand>();

            // use this if you have a parameter we need to mock the parameter
            //commandMock.Setup(m => m.Parameters.Add
            //                 (It.IsAny<IDbDataParameter>())).Verifiable();
            commandMock.Setup(m => m.ExecuteReader())
            .Returns(moqDataReader.Object);

            // Now the mock if IDbConnection configure the command to be used
            this.moqConnection.Setup(m => m.CreateCommand()).Returns(commandMock.Object);

            // And we are ready to do the call.
            List<Account> result =
                 await this.service.GetAccountsAsync();
            Assert.Equal(2,result.Count);
   
        }
    }
}
