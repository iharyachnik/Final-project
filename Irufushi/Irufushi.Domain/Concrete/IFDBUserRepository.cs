using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract.Abstract;
using Abstract.Entities;
using System.Data.Entity;

namespace Irufushi.Domain.Concrete
{
    public class IFDBUserRepository : IUserPagesRepository
    {
        EFDBContext context = new EFDBContext();

        public IQueryable<UserProfile> UserProfiles
        {
            get { return context.UserProfiles; }
        }

        public UserProfile GetUser(int id)
        {
            return context.UserProfiles.First(m => m.UserId == id);
        }

        public void SaveProfile(UserProfile user)
        {
            if (user == null) return;

            UserProfile dbUser = context.UserProfiles.Find(user.UserId);
            if (dbUser == null) return;

            Contacts contacts = context.Contacts.Find(user.UserId);
            if (contacts == null) user.Contacts.Id = user.UserId;
            Location location = context.Locations.Find(user.UserId);
            if (location == null) user.Location.Id = user.UserId;

            SaveAboutUser(user);
            SaveContacts(user.Contacts);
            SaveLocation(user.Location);

            context.SaveChanges();
        }

        public void SaveAboutUser(UserProfile about)
        {
            UserProfile dbAbout = context.UserProfiles.Find(about.UserId);

            dbAbout.FirstName = about.FirstName;
            dbAbout.LastName = about.LastName;
            dbAbout.BirthDate = about.BirthDate;
            dbAbout.Gender = about.Gender;

            context.SaveChanges();
        }

        public void SaveContacts(Contacts contacts)
        {
            Contacts dbContacts = context.Contacts.Find(contacts.Id);

            if (dbContacts == null)
            {
                context.Contacts.Add(contacts);
            }
            else
            {
                dbContacts.PhoneNum = contacts.PhoneNum;
                dbContacts.MobPhoneNum = contacts.MobPhoneNum;
                dbContacts.Skype = contacts.Skype;
                dbContacts.ContEmail = contacts.ContEmail;
                dbContacts.ICQ = contacts.ICQ;
            }
            context.SaveChanges();
        }

        public void SaveLocation(Location location)
        {
            Location dbLocation = context.Locations.Find(location.Id);

            if (dbLocation == null)
            {
                context.Locations.Add(location);
            }
            else
            {
                dbLocation.Country = location.Country;
                dbLocation.City = location.City;
                dbLocation.Address = location.Address;
            }
            context.SaveChanges();
        }

        public void AddFriend(FriendShip friendship)
        {
            if (friendship.UserId == 0 || friendship.FriendId == 0 
                || friendship.UserId == friendship.FriendId) return;

            var dbFriendshipsUserId = context.FriendShips
                .Where(x => x.UserId == friendship.UserId);
            foreach (var item in dbFriendshipsUserId)
            {
                if (item.FriendId == friendship.FriendId) return;
            }
            var dbFriendshipsFriendId = context.FriendShips
                .Where(x => x.FriendId == friendship.FriendId);
            foreach (var item in dbFriendshipsFriendId)
            {
                if (item.UserId == friendship.UserId) return;
            }

            context.FriendShips.Add(friendship);
            context.SaveChanges();
        }

        public void DeleteFriend(FriendShip friendship)
        {
            if (friendship.UserId == 0 || friendship.FriendId == 0) return;

            var friendshipQueryUID = context.FriendShips
                .Where(x => x.UserId == friendship.UserId)
                .Where(x => x.FriendId == friendship.FriendId).FirstOrDefault();

            var friendshipQueryFID = context.FriendShips
                .Where(x => x.FriendId == friendship.UserId)
                .Where(x => x.UserId == friendship.FriendId).FirstOrDefault();

            if(friendshipQueryUID != null) context.FriendShips.Remove(friendshipQueryUID);
            if(friendshipQueryFID != null) context.FriendShips.Remove(friendshipQueryFID);

            context.SaveChanges();
        }

        public List<UserProfile> GetFriends(int id)
        {
            var userIds = context.FriendShips.Where(x => x.UserId == id);
            var friendIds = context.FriendShips.Where(x=> x.FriendId == id);

            List<int> friendshipList = new List<int>();
            foreach (var item in userIds)
            {
                friendshipList.Add(item.FriendId);
            }
            foreach (var item in friendIds)
            {
                friendshipList.Add(item.UserId);
            }

            List<UserProfile> users = new List<UserProfile>();
            foreach (var item in friendshipList)
            {
                users.Add(GetUser(item));
            }

            return users;
        }

        public bool IsFriend(int idUser, int idFriend)
        {
            if (idUser == 0 || idFriend == 0) return false;

            var friendshipUID = context.FriendShips.Where(x => x.UserId == idUser)
                .Where(x => x.FriendId == idFriend);

            foreach (var item in friendshipUID)
            {
                if (item.UserId == idUser && item.FriendId == idFriend)
                    return true;
            }

            var friendshipFID = context.FriendShips.Where(x => x.FriendId == idUser)
                .Where(x => x.UserId == idFriend);

            foreach (var item in friendshipFID)
            {
                if (item.UserId == idFriend && item.FriendId == idUser)
                    return true;
            }

            return false;
        }

        public List<UserProfile> SearchUsers(string firstName, string lastName,
            string country, string city)
        {

            var users = context.UserProfiles
                .Where(x => firstName == null || x.FirstName.Contains(firstName))
                .Where(x => lastName == null || x.LastName.Contains(lastName))
                .Where(x => country == null || x.Location.Country.Contains(country))
                .Where(x => city == null || x.Location.City.Contains(city))
                .ToList();

            return users;
        }

        public List<Message> GetMessages(int uId, int fId)
        {
            List<Message> messages = context.Messages
                .Where(x => (x.ReceiverId == uId && x.SenderId == fId || x.SenderId == uId && x.ReceiverId == fId))
                .OrderByDescending(x => x.SendDateTime).ToList();

            return messages;
        }

        public List<UserProfile> GetDialogs(int id)
        {
            var dialogsIn = context.Messages.Where(x => x.ReceiverId == id)
                .OrderByDescending(x => x.SendDateTime);
            var dialogsOut = context.Messages.Where(x => x.SenderId == id)
                .OrderByDescending(x => x.SendDateTime);

            List<int> users = new List<int>();

            foreach (var item in dialogsIn)
            {
                users.Add(item.SenderId);
            }
            foreach (var item in dialogsOut)
            {
                users.Add(item.ReceiverId);
            }

            List<UserProfile> up = new List<UserProfile>();

            foreach (var item in users)
            {
                up.Add(GetUser(item));
            }
            up = up.Distinct().ToList();

            return up;           
        }

        public void AddMessage(Message message)
        {
            if (message.Content == null || message.SenderId == 0
                || message.ReceiverId == 0)
                return;

            context.Messages.Add(message);
            context.SaveChanges();
        }

        public void AddRoles()
        {
            webpages_Roles roleAdmin = new webpages_Roles();
            webpages_Roles roleUser = new webpages_Roles();

            roleAdmin.RoleName = "Admin";
            roleUser.RoleName = "User";

            context.webpages_Roles.Add(roleAdmin);
            context.webpages_Roles.Add(roleUser);

            context.SaveChanges();
        }

        public void SetRole(int id)
        {
            webpages_UsersInRoles userRole = new webpages_UsersInRoles();

            userRole.Role = context.webpages_Roles.Where(x => x.RoleName == "User").FirstOrDefault();
            userRole.UserId = id;

            context.webpages_UsersInRoles.Add(userRole);
            context.SaveChanges();
        }
    }
}
