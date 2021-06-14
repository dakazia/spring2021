using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.Logger;
using BrainstormSessions.ViewModels;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace BrainstormSessions.Controllers
{
    public class SessionController : Controller
    {
        private readonly IBrainstormSessionRepository _sessionRepository;
        
        public SessionController(IBrainstormSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
           }

        public async Task<IActionResult> Index(int? id)
        {
            if (!id.HasValue)
            {
                LoggerManager.Log.Debug("Id hasn't value, return default");

                return RedirectToAction(actionName: nameof(Index),
                    controllerName: "Home");
            }

            var session = await _sessionRepository.GetByIdAsync(id.Value);

            if (session == null)
            {
                LoggerManager.Log.Error("Session not found by id.");
                return Content("Session not found.");
            }

            LoggerManager.Log.Debug("Session id isn't null");

            var viewModel = new StormSessionViewModel()
            {
                DateCreated = session.DateCreated,
                Name = session.Name,
                Id = session.Id
            };

            LoggerManager.Log.Debug("Return view model");

            return View(viewModel);
        }
    }
}
