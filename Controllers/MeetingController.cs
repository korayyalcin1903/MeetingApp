using System.Security.Claims;
using MeetingApp.Data.Abstract;
using MeetingApp.Data.Concrete;
using MeetingApp.Data.Entity;
using MeetingApp.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingApp.Controllers
{
    public class MeetingController:Controller
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly MeetingContext _context;

        public MeetingController(IMeetingRepository meeting, MeetingContext context)
        {
            _meetingRepository = meeting;
            _context = context;
        }

        public IActionResult Index()
        {
            var meetings = _meetingRepository.Meetings.ToList();
            return View(meetings);
        }


        public async Task<IActionResult> Details(int id)
        {
            if(id.ToString() != null){
                var meeting = await _meetingRepository.Meetings.Include(m => m.MeetingUsers).ThenInclude(x => x.User).FirstOrDefaultAsync(m => m.MeetingId == id);
                return View(meeting);
            } else {
                return NotFound();
            }
        }

        [Authorize]
        public async Task<IActionResult> Register()
        {
            return View();           
        }

        [HttpPost]
        public async Task<IActionResult> Register(int id)
        {
            var meeting = await _context.Meetings.FirstOrDefaultAsync(x => x.MeetingId == id);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if(meeting != null){
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);

                var isExist = await _context.MeetingUsers.FirstOrDefaultAsync(x => x.MeetingId == id && x.UserId == user.Id); 

                if(isExist == null){
                    _context.Add( new MeetingUser {
                        MeetingId = meeting.MeetingId,
                        UserId = user.Id
                    });
                } else {
                    return RedirectToAction("RegisterError","Error");
                }
                
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Meeting", new { id = id});
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

    }

}