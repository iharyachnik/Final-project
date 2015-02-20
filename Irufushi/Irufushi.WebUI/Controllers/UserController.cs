using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abstract.Abstract;
using Abstract.Entities;
using Irufushi.Domain.Concrete;
using Irufushi.WebUI.Models;
using WebMatrix.WebData;

namespace Irufushi.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserPagesRepository _repository;

        public UserController(IUserPagesRepository userRepository)
        {
            _repository = userRepository;
        }

        [Authorize]
        public ActionResult Index(int? id)
        {
            if(id == null) id = WebSecurity.CurrentUserId;

            UserProfile viewModel = _repository.GetUser((int)id);

            return View(viewModel);
        }

        public ActionResult Buttons(int id)
        {
            if (id == WebSecurity.CurrentUserId) return null;
            ButtonModel viewModel = new ButtonModel();
            if (_repository.IsFriend(id, WebSecurity.CurrentUserId))
            {
                viewModel.Label = "Delete from friends";
                viewModel.Action = "DeleteFriend";
            }
            else
            {
                viewModel.Label = "Add to friends";
                viewModel.Action = "AddFriend";
            }
                
            return PartialView(viewModel);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null) id = WebSecurity.CurrentUserId;
            UserProfile user = _repository.GetUser((int)id);

            return View(user);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(UserProfile model, int? id)
        {
            if (id == null) id = WebSecurity.CurrentUserId;
            if(model.UserId == 0) model.UserId = (int)id;
            if(model.Contacts.Id == 0) model.Contacts.Id = (int)id;
            if (model.Location.Id == 0) model.Location.Id = (int)id;

            _repository.SaveProfile(model);

            return RedirectToAction("Index", "User", new { id = (int)id });
        }

        [Authorize]
        public ActionResult AddFriend(int id)
        {
            FriendShip friendship = new FriendShip
            {
                UserId = WebSecurity.CurrentUserId,
                FriendId = id
            };

            _repository.AddFriend(friendship);

            return RedirectToAction("Index", "User", new { id = id });
        }

        [Authorize]
        public ActionResult DeleteFriend(int id)
        {
            FriendShip friendship = new FriendShip
            {
                UserId = WebSecurity.CurrentUserId,
                FriendId = id
            };

            _repository.DeleteFriend(friendship);

            return RedirectToAction("Index", "User", new { id = id });
        }

        [Authorize]
        public ActionResult ShowFriends(int? id)
        {
            if(id == null) id = WebSecurity.CurrentUserId;

            UserModel viewModel = new UserModel
            {
                Users = _repository.GetFriends((int)id)
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult ShowAllUsers()
        {
            List<UserProfile> userWM = _repository.UserProfiles.ToList();

            userWM.Remove(_repository.GetUser(WebSecurity.CurrentUserId));

            UserModel viewModel = new UserModel
            {
                Users = userWM
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Search(SearchModel model)
        {
            model.Users = _repository.SearchUsers(model.FirstName, model.LastName, 
                model.Country, model.City);

            model.Users = model.Users.Where(x => x.UserId != WebSecurity.CurrentUserId);

            return View(model);
        }

        [Authorize]
        public ActionResult ShowDialogs()
        {
            MessageModel viewModel = new MessageModel
            {
                Dialogs = _repository.GetDialogs(WebSecurity.CurrentUserId)
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult ShowMessages(int? id)
        {
            MessageContentModel viewModel = new MessageContentModel
            {
                Messages = _repository.GetMessages(WebSecurity.CurrentUserId, (int)id)
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ShowMessages(MessageContentModel model, int id)
        {
            Message message = new Message
            {
                SendDateTime = DateTime.Now,
                SenderId = WebSecurity.CurrentUserId,
                ReceiverId = id,
                Content = model.Content
            };
            _repository.AddMessage(message);

            if (Request.IsAjaxRequest())
            {
                MessageContentModel viewModel = new MessageContentModel
                {
                    NewMessage = _repository.GetMessages(message.SenderId,message.ReceiverId).First()
                };
                viewModel.NewMessage.Sender = _repository.GetUser(WebSecurity.CurrentUserId);
                return ShowNewMessage(viewModel);
            }

            return RedirectToAction("ShowMessages", "User", new { id = id });
        }

        public ActionResult ShowNewMessage(MessageContentModel message)
        {
            if (message.NewMessage == null) return null;
            return PartialView("ShowNewMessage", message);
        }
    }
}
