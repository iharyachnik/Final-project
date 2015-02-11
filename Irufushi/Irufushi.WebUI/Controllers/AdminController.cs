using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Irufushi.WebUI.Models;
using Abstract.Abstract;
using Abstract.Entities;

namespace Irufushi.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserPagesRepository _repository;

        public AdminController(IUserPagesRepository userRepository)
        {
            _repository = userRepository;
        }
        //
        // GET: /Admin/

        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            UserModel viewModel = new UserModel
            {
                Users = _repository.UserProfiles
            };

            return View(viewModel);
        }

        [Authorize(Roles="Admin")]
        public ActionResult ShowUser(int? id)
        {
            UserProfile viewModel = new UserProfile();
            if(id != null) viewModel = _repository.GetUser((int)id);

            return View(viewModel);
        }
    }
}
