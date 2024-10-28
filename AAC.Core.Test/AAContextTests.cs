using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAC.Core;
using User = AAC.Data.Entities.User;
using Aircraft = AAC.Data.Entities.AircraftType;
using Registration = AAC.Data.Entities.Registration;
namespace AAC.Core.Tests
{
    public class TestUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public bool Active { get; set; }
    }

    [TestClass()]
    public class AAContextTests
    {

        [TestMethod()]
        public void GenerateEntity()
        {
            AAContext context = new AAContext();
            string notExpected = string.Empty;
            var user = new TestUser();
            string a = "";
            var users = context.GetList<TestUser>( "select u.Active,u.Username,u.Password,RoleName=r.Name from [User]  u inner join [Role] r on r.ID=u.RoleID" );
            //ctx.
            //context.GetColumn( "select u.Username,u.Password,r.Name from [User] u inner join [Role] r on r.ID=u.RoleID" );
            foreach ( var u in users)
            {
                a += u.Username + ":" + u.RoleName + ":" + u.Active + ",";                
            }

            Assert.AreNotEqual( notExpected, a );
        }


        [TestMethod()]
        public void GetTableColumnTest()
        {
            AAContext context = new AAContext();
            string notExpected = string.Empty;
            string actual = string.Empty;

            actual = context.GetColumn<string>( "select username from [user]" );
            Assert.AreNotEqual( notExpected, actual );
        }

        [TestMethod()]
        public void GetTableColumnTestInt()
        {
            AAContext context = new AAContext();
            string notExpected = string.Empty;
            string actual = string.Empty;

            actual = context.GetColumn<string>( "select CAST(ID AS VARCHAR(10)) from [user]" );
            Assert.AreNotEqual( notExpected, actual );
        }

        [TestMethod()]
        public void ExecuteSp()
        {
            var sp = "sp_CreateRole";
            AAContext context = new AAContext();
            context.ExecuteTSQL( "exec sp_CreateRole 'test','testonly'" );
        }

        [TestMethod()]
        public void InsertUserTest()
        {
            User user = new User()
            {
                Username = "banbanboi5",
                Password = "pasok1234",
                FirstName = "banbanboi",
                LastName = "banbanboi",
                MiddleName = "A",
                Address = "adress 1",
                Phone = "1234324",
                Email = string.Format( "{0}@gmail.com", "mcnielv" ),
                SSS = "3242332432",
                TIN = "23432432",
                RoleID = 1,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            AAContext context = new AAContext();
            context.Add<User>( user );
            context.SaveChanges();
        }
        [TestMethod()]
        public void AttachTest()
        {
            string username = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 );
            User user = new User()
            {
                Username = username,
                Password = "pasok1234",
                FirstName = username,
                LastName = username,
                MiddleName = "A",
                Address = "adress 1",
                Phone = "1234324",
                Email = string.Format( "{0}@email.com", username ),
                SSS = "3242332432",
                TIN = "23432432",
                RoleID=1,
                DateCreated=DateTime.Now,
                DateModified=DateTime.Now
            };
            AAContext context = new AAContext();
            context.Add<User>( user );
            context.SaveChanges();

            user = new User()
            {
                Username = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                Password = "pasok1234",
                FirstName = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                LastName = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                MiddleName = "A",
                Address = "adress 1",
                Phone = "1234324",
                Email = string.Format( "{0}@email.com", username ),
                SSS = "3242332432",
                TIN = "23432432",
                RoleID = 6,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            context = new AAContext();
            context.Add<User>( user );
            context.SaveChanges();

            user = new User()
            {
                Username = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                Password = "pasok1234",
                FirstName = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                LastName = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                MiddleName = "A",
                Address = "adress 1",
                Phone = "1234324",
                Email = string.Format( "{0}@email.com", username ),
                SSS = "3242332432",
                TIN = "23432432",
                RoleID = 6,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            context = new AAContext();
            context.Add<User>( user );
            context.SaveChanges();

            user = new User()
            {
                Username = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                Password = "pasok1234",
                FirstName = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                LastName = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                MiddleName = "A",
                Address = "adress 1",
                Phone = "1234324",
                Email = string.Format( "{0}@email.com", username ),
                SSS = "3242332432",
                TIN = "23432432",
                RoleID = 5,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            context = new AAContext();
            context.Add<User>( user );
            context.SaveChanges();

            user = new User()
            {
                Username = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                Password = "pasok1234",
                FirstName = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                LastName = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 5 ),
                MiddleName = "A",
                Address = "adress 1",
                Phone = "1234324",
                Email = string.Format( "{0}@email.com", username ),
                SSS = "3242332432",
                TIN = "23432432",
                RoleID = 4,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            context = new AAContext();
            context.Add<User>( user );
            context.SaveChanges();
        }

        [TestMethod()]
        public void InsertAircraft()
        {
            Aircraft aircraft = new Aircraft()
            {
                Name = "AC1" + Guid.NewGuid().ToString().Replace("-","").Substring( 0, 4 ),
                Description = "AC1" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            AAContext context = new AAContext();
            context.Add<Aircraft>( aircraft );
            context.SaveChanges();

            aircraft = new Aircraft()
            {
                Name = "AC2" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                Description = "AC2" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            context = new AAContext();
            context.Add<Aircraft>( aircraft );
            context.SaveChanges();

            aircraft = new Aircraft()
            {
                Name = "AC3" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                Description = "AC3" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            context = new AAContext();
            context.Add<Aircraft>( aircraft );
            context.SaveChanges();
            
        }

        [TestMethod]
        public void InsertACRegistration()
        {
            DateTime datenow = DateTime.Now;
            Registration reg = new Registration()
            {
                Name = "Reg-" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                AircraftID = 1,
                Description = "Test Registration Only",
                DateCreated = datenow,
                DateModified = datenow
            };
            AAContext context = new AAContext();
            context.Add<Registration>( reg );
            context.SaveChanges();

            reg = new Registration()
            {
                Name = "Reg-" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                AircraftID = 3,
                Description = "Test Registration Only",
                DateCreated = datenow,
                DateModified = datenow
            };
            context = new AAContext();
            context.Add<Registration>( reg );
            context.SaveChanges();

            reg = new Registration()
            {
                Name = "Reg-" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                AircraftID = 2,
                Description = "Test Registration Only",
                DateCreated = datenow,
                DateModified = datenow
            };
            context = new AAContext();
            context.Add<Registration>( reg );
            context.SaveChanges();

            reg = new Registration()
            {
                Name = "Reg-" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                AircraftID = 3,
                Description = "Test Registration Only",
                DateCreated = datenow,
                DateModified = datenow
            };
            context = new AAContext();
            context.Add<Registration>( reg );
            context.SaveChanges();

            reg = new Registration()
            {
                Name = "Reg-" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                AircraftID = 3,
                Description = "Test Registration Only",
                DateCreated = datenow,
                DateModified = datenow
            };
            context = new AAContext();
            context.Add<Registration>( reg );
            context.SaveChanges();

            reg = new Registration()
            {
                Name = "Reg-" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                AircraftID = 7,
                Description = "Test Registration Only",
                DateCreated = datenow,
                DateModified = datenow
            };
            context = new AAContext();
            context.Add<Registration>( reg );
            context.SaveChanges();

            reg = new Registration()
            {
                Name = "Reg-" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                AircraftID = 7,
                Description = "Test Registration Only",
                DateCreated = datenow,
                DateModified = datenow
            };
            context = new AAContext();
            context.Add<Registration>( reg );
            context.SaveChanges();

            reg = new Registration()
            {
                Name = "Reg-" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                AircraftID = 8,
                Description = "Test Registration Only",
                DateCreated = datenow,
                DateModified = datenow
            };
            context = new AAContext();
            context.Add<Registration>( reg );
            context.SaveChanges();

            reg = new Registration()
            {
                Name = "Reg-" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                AircraftID = 9,
                Description = "Test Registration Only",
                DateCreated = datenow,
                DateModified = datenow
            };
            context = new AAContext();
            context.Add<Registration>( reg );
            context.SaveChanges();

            reg = new Registration()
            {
                Name = "Reg-" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 4 ),
                AircraftID = 9,
                Description = "Test Registration Only",
                DateCreated = datenow,
                DateModified = datenow
            };
            context = new AAContext();
            context.Add<Registration>( reg );
            context.SaveChanges();
        }
    }
}