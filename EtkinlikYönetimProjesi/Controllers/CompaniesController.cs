using AutoMapper;
using EtkinlikYönetimProjesi.Dtos;
using EtkinlikYönetimProjesi.IRepository.Repository;
using EtkinlikYönetimProjesi.Model;
using EtkinlikYönetimProjesi.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using System.Security.Cryptography;
using System.Text;

namespace EtkinlikYönetimProjesi.Controllers
{
    [Authorize( Roles = "Company")]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
       
        private readonly IUnitOfWork _unitOfwork;
        public CompaniesController( IUnitOfWork unitOfwork, IOptions<JwtSettings> options)

        {
           
            _unitOfwork = unitOfwork;           
        }

        [HttpPost]
        [Route("RegisterCompany")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] CompanyRegisterViewModel companyRegisterViewModel)
        {
            Company company = new Company();
            company.Name = companyRegisterViewModel.CompanyName;
            company.WebsiteUrl = companyRegisterViewModel.WebsiteUrl;
            company.EMail = companyRegisterViewModel.Email;
            company.Password = _unitOfwork.Company.HashPassword(companyRegisterViewModel.Password);
            var existingCompany = _unitOfwork.Company.GetFirstOrDefault(x => x.EMail == companyRegisterViewModel.Email);
            if (existingCompany != null)
            {
                return BadRequest("Email is used by other Company");
            }
            _unitOfwork.Company.Add(company);
            _unitOfwork.Save();
            string hashedPassword = _unitOfwork.Company.HashPassword(companyRegisterViewModel.Password);
                
            var companyRegister= _unitOfwork.Company.GetFirstOrDefault(x=> x.EMail == companyRegisterViewModel.Email &&  x.Password == hashedPassword);
            User user = new User()
            {
                Email = companyRegister.EMail,
                Password = companyRegister.Password,
                UserRole = "Company"
            };
            _unitOfwork.User.Add(user);
            _unitOfwork.Save();
            return Created("Company Register is succesfull", company);
        }
       

        [HttpGet]
        [Route("GetEventInformationForCompany")]
        [Produces("application/json", "application/xml")] // Hem JSON hem de XML formatlarını destekler
        public IActionResult GetEventsForCompany()
        {
            var getEvents = _unitOfwork.Event.GetEvents();
            if (getEvents == null)
            {
                return BadRequest("Events is not found");
            }
            return Ok(getEvents);
        }
    }
}
