using MailKit.Net.Smtp;
using AutoMapper;
using Etkinlik.Dtos;
using EtkinlikYönetimProjesi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Text;
using System.Security.Cryptography;
using EtkinlikYönetimProjesi.IRepository.Repository;
using System.Net.Mail;
using EtkinlikYönetimProjesi.Dtos;
using MimeKit;

namespace EtkinlikYönetimProjesi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize( Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        
        private readonly IUnitOfWork _unitOfwork;

        public AdminController( IUnitOfWork unitOfwork)
        {
           
            _unitOfwork = unitOfwork;
        }
   
       
        [HttpPost]
        [Route("CategoryAdd")]
        public IActionResult CategoryAdd([FromBody] CategoryDto
            categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest();
            }
            Category category = new Category();
            category.Name = categoryDto.Name;
            _unitOfwork.Category.Add(category);
            _unitOfwork.Save();
            return Created("Categories added Succefully", category);

        }
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categories = _unitOfwork.Category.Getlist();    
            return Ok(categories);
        }
        [HttpPut]
        [Route("updateCategory/{categoryId}")]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto? categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest();
            }
            Category category = new Category();
            var update = _unitOfwork.Category.GetCategoryById(categoryId);
            if (update == null)
            {
                return NotFound("User is not Found");
            }
            update.Name = categoryDto.Name;
            _unitOfwork.Save();
            return Ok("User is updated");
        }
        [HttpDelete]
        [Route("DeleteCategory/{deleteCategoryId}")]
        public IActionResult DeleteCategory(int deleteCategoryId)
        {

            var deleteCategory = _unitOfwork.Category.GetCategoryById(deleteCategoryId);
            if (deleteCategory == null)
            {
                return NotFound("User is not Found");
            }
            _unitOfwork.Category.Delete(deleteCategory);
            _unitOfwork.Save();
            return Ok("User deleted Succesfully");
        }

        [HttpPost]
        [Route("AddCity")]
        public IActionResult AddCity([FromBody] CityDto cityDto)
        {
            if (cityDto == null)
            {
                return BadRequest();
            }
            var cityIsExist = _unitOfwork.City.GetFirstOrDefault(x=> x.Name == cityDto.Name);
            if(cityIsExist == null)
            {
                City city = new City();
                city.Name = cityDto.Name;
                _unitOfwork.City.Add(city);
                _unitOfwork.Save();
                return Created("City added Succefully", city);
            }
            return BadRequest("City exist");
        }
        [HttpGet]
        [Route("GetEvents")]
        public IActionResult GetEvents()
        {
            var eventList = _unitOfwork.Event.Getlist();
            return Ok(eventList);
            //Bu sayfada kullanıcıların oluştudugu etkinlikleri görür ve hakiyede etkinliklerin yanlarında
            // onay ver ya da reddet gibi buton oldugunu düşünerek böyle yazdım.
        }


        [HttpPost]
        [Route("ApproveEvent/{eventId}")]
        public IActionResult ApproveEvent(int eventId)
        {
            // Admin, etkinliği onaylar
            var eventToApprove = _unitOfwork.Event.GetEventById(eventId);
            if (eventToApprove != null)
            {
                eventToApprove.IsApproved = true;
                _unitOfwork.Save();
                return Ok("Event is approved successfully");
            }
            return BadRequest("Event is not approved");
        }

       
        [HttpPost]
        [Route("RejectEvent/{eventId}")]
        public IActionResult EventReject(int eventId)
        {
            var rejectEvent = _unitOfwork.Event.GetEventById(eventId);   
            if (rejectEvent == null)
            {
                return BadRequest("Event is not found");
            }
            _unitOfwork.Event.Delete(rejectEvent);
            _unitOfwork.Save();
            var responseModel = new { Message = "Event rejected by Admin" };
            var findUserForMail = _unitOfwork.User.GetUserById(rejectEvent.UserId);
            SendEmail(findUserForMail.Email); // sistem dinamik olarak eventi oluşturan email hesabına bilgilendirme maili atmaktadır.
            return Ok(responseModel);
        }

        private void SendEmail(string email)
        {
            MimeMessage mimemessage = new MimeMessage();
            MailboxAddress mailboxAddressFrom = new MailboxAddress("Etkinlik Yönetim ", "omerguden@gmail.com");
            MailboxAddress mailboxAddressTo = new MailboxAddress("User", email);
            mimemessage.From.Add(mailboxAddressFrom);
            mimemessage.To.Add(mailboxAddressTo);
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Sayın" + " " + email + "Bu bir bilgilendirme mailidir. " + "Event admin tarafından reddedilmiştir.";
            mimemessage.Body = bodyBuilder.ToMessageBody();
            mimemessage.Subject = "Etkinlik Hakkında";
            MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("omerguden@gmail.com", "lqokxprdiaupewep");
            smtpClient.Send(mimemessage);
            smtpClient.Disconnect(true);
        }

    }
}
