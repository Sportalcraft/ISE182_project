using Microsoft.VisualStudio.TestTools.UnitTesting;
using ISE182_project;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISE182_project.Layers.CommunicationLayer;
using System.Collections.Generic;

namespace ISE182_project.Layers.BusinessLogic
{
    [TestClass]
    public class ChatRoomTests
    {
        string ExistingUserRegistration = "client tried to register with an already existing user";
        string UserEmptyNullString = "The client tried to use illegal nickname";
        string LogoutNotLogin = "A user tried to logout without being logedin to a user";
        string PartialLogInWhenLoggedin = "The user tried to login while loggedin to: ";
        string LogInNotRegistered = "A user tried to login to a not register account";

        [TestInitialize]
        public void Init()
        {
            ChatRoom.start(ChatRoom.Place.Home);
        }

        #region isLoggedIn

        [TestMethod]
        public void isLoggedInTest_Positive()
        {
            string UserName = RandomString(8);
            int GroupID = RandomInt();

            // if already logged in - logout
            try
            {
                ChatRoom.logout();
            }
            catch { }

            // Make sure there is no user currenly connected
            try
            {
                Assert.IsFalse(ChatRoom.isLoggedIn());
            }
            catch
            {
                Assert.Fail("falid to check if a user is loggedIn");
            }

            LogIn(UserName, GroupID);

            // check if a user is now connected
            try
            {
                Assert.IsTrue(ChatRoom.isLoggedIn());
            }
            catch
            {
                Assert.Fail("falid to check if a user is loggedIn");
            }
        }

        [TestMethod()]
        public void isLoggedInTest_Negative()
        {
            // if already logged in - logout
            try
            {
                ChatRoom.logout();
            }
            catch { }

            try
            {
                Assert.IsFalse(ChatRoom.isLoggedIn());
            }
            catch
            {
                Assert.Fail("falid to check if a user is loggedIn");
            }
        }

        #endregion

        #region register

        [TestMethod()]
        public void registerTest_Positive()
        {
            string UserName = RandomString(8);
            int GroupID = RandomInt();

            // if already logged in - logout
            try
            {
                ChatRoom.logout();
            }
            catch { }

            // try to register
            try
            {
                ChatRoom.register(UserName, GroupID);
            }
            catch
            {
                Assert.Fail("falid to check if a user is loggedIn");
            }
        }

        [TestMethod()]
        public void registerTest_Negative()
        {
            int GroupID = RandomInt();

            // "" registration
            try
            {
                ChatRoom.register("", GroupID);
                Assert.Fail("registered with empty string");
            }
            catch (Exception e)
            {
                Assert.AreEqual(UserEmptyNullString, e.Message);
            }

            //Null registration
            try
            {
                ChatRoom.register(null, GroupID); ;
                Assert.Fail("registered with null");
            }
            catch (Exception e)
            {
                Assert.AreEqual(UserEmptyNullString, e.Message);
            }

            // try to register with an existing user

            try
            {
                ChatRoom.register("Elephant", GroupID);
            }
            catch { }

            try
            {
                ChatRoom.register("Elephant", GroupID);
                Assert.Fail("registered with existing user");
            }
            catch (Exception e)
            {
                Assert.AreEqual(ExistingUserRegistration, e.Message);
            }
        }

        #endregion

        #region login

        [TestMethod()]
        public void loginTest_Positive()
        {
            string UserName = RandomString(5);
            int GroupID = RandomInt();

            LogIn(UserName, GroupID);

            // logout
            try
            {
                ChatRoom.logout();
            }
            catch { }
        }

        [TestMethod()]
        public void loginTest_Negative()
        {
            string UserName = RandomString(5);
            int GroupID = RandomInt();

            LogIn(UserName, GroupID);

            //Login when logged in
            try
            {
                ChatRoom.login(UserName, GroupID);
                Assert.Fail("login when loggedin");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(PartialLogInWhenLoggedin));
            }

            // "" LogIn
            try
            {
                ChatRoom.login("", GroupID);
            }
            catch (Exception e)
            {
                Assert.AreEqual(UserEmptyNullString, e.Message);
            }

            // Null LogIn
            try
            {
                ChatRoom.login(null, GroupID);
            }
            catch (Exception e)
            {
                Assert.AreEqual(UserEmptyNullString, e.Message);
            }

            // logIn whithout register
            try
            {
                ChatRoom.login(RandomString(15), GroupID);
            }
            catch (Exception e)
            {
                Assert.AreEqual(LogInNotRegistered, e.Message);
            }
        }

        #endregion

        #region logout

        [TestMethod()]
        public void logoutTest_Positive()
        {
            string UserName = "Yossi";
            int GroupID = RandomInt();

            LogIn(UserName, GroupID);

            //logout
            try
            {
                ChatRoom.logout();
                Assert.IsFalse(ChatRoom.isLoggedIn());
            }
            catch
            {
                Assert.Fail("Faild to logout");
            }
        }

        [TestMethod()]
        public void logoutTest_Negative()
        {
            //logout if logged in
            try
            {
                ChatRoom.logout();
            }
            catch { }

            //logout when loggedout
            try
            {
                ChatRoom.logout();
            }
            catch (Exception e)
            {
                Assert.AreEqual(LogoutNotLogin, e.Message);
            }
        }

        #endregion

        #region send

        [TestMethod()]
        public void sendTest_Positive()
        {
            string UserName = RandomString(5);
            int GroupID = RandomInt();

            LogIn(UserName, GroupID);

            try
            {
                string message = "";

                for (int i = 1; i <= 30; i++)
                {
                    message = "Message #" + i;
                    ChatRoom.send(message);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Faild to send a message!");
            }
        }

        [TestMethod()]
        public void sendTest_Negative()
        {
            string UserName = RandomString(5);
            int GroupID = RandomInt();

            LogIn(UserName, GroupID);

            // "" sending
            try
            {
                ChatRoom.send("");
                Assert.Fail("Empty message was sent!");
            }
            catch
            {
                //Good
            }

            //null Sending
            try
            {
                ChatRoom.send(null);
                Assert.Fail("null message was sent!");
            }
            catch
            {
                //Good
            }

            // more tham 150 sending
            try
            {
                string morethan150 = "";

                for (int i = 1; i <= 200; i++)
                    morethan150 += "A";

                ChatRoom.send(morethan150);

                Assert.Fail("too long message was sent!");
            }
            catch
            {
                //Good
            }
        }

        #endregion

        

        #region Healpers

        private static string RandomString(int length)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static int RandomInt()
        {
            Random random = new Random();
            return random.Next(1, 35);
        }

        private static void LogIn(string UserName, int GroupID)
        {
            //logout if logged in
            try
            {
                ChatRoom.logout();
            }
            catch { }

            //rgister if not registered
            try
            {
                ChatRoom.register(UserName, GroupID);
            }
            catch { }

            //logIn
            try
            {
                ChatRoom.login(UserName, GroupID);
            }
            catch
            {
                Assert.Fail("Failed to logged in");
            }

            //Make sure user is connected
            try
            {
                Assert.IsTrue(ChatRoom.isLoggedIn());
            }
            catch
            {
                Assert.Fail("Didn't logged in");
            }
        }

        #endregion
    }
}