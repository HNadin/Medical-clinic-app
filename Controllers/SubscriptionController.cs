using Medical_clinic.Observer;
using Medical_clinic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Medical_clinic.Data;

namespace Medical_clinic.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly MedicalFacility _medicalFacility;
        private readonly ApplicationContext _context;

        public SubscriptionController(MedicalFacility medicalFacility, ApplicationContext context)
        {
            _medicalFacility = medicalFacility;
            _context = context;
        }


        public IActionResult SubscribeToUpdates()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                var existingSubscription = _context.Subscriptions.FirstOrDefault(s => s.UserId == userId);
                if (existingSubscription == null)
                {
                    var newSubscription = new Subscription { UserId = userId, IsSubscribed = true };
                    _context.Subscriptions.Add(newSubscription);
                }
                else
                {
                    existingSubscription.IsSubscribed = true;
                }

                _context.SaveChanges();
                _medicalFacility.Attach(new Patient { UserId = userId }, _context);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult UnsubscribeFromUpdates()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                var existingSubscription = _context.Subscriptions.FirstOrDefault(s => s.UserId == userId);
                if (existingSubscription != null)
                {
                    existingSubscription.IsSubscribed = false;
                    _context.SaveChanges();
                }

                _medicalFacility.Detach(new Patient { UserId = userId }, _context);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
