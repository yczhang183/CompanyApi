﻿using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();
        private static Dictionary<string, Employee> employees = new Dictionary<string, Employee>();

        [HttpPost]
        public ActionResult<Company> Create(CreateCompanyRequest request)
        {
            if (HasCompanyName(request.Name))
            {
                return BadRequest();
            }
            Company companyCreated = new Company(request.Name);
            companies.Add(companyCreated);
            return StatusCode(StatusCodes.Status201Created, companyCreated);
        }

        [HttpDelete]
        public void ClearData()
        {
            companies.Clear();
        }

        [HttpGet("{id}")]
        public ActionResult<Company> Get(string id)
        {
            Company? company = GetCompanyById(id);
            if (company is null)
            {
                return NotFound();
            }
            return StatusCode(StatusCodes.Status200OK, companies.First(company => company.Id.Equals(id)));
        }

        [HttpGet]
        public ActionResult<List<Company>> GetCompanyByPage([FromQuery] int pageSize = 0, int pageIndex = 0)
        {
            var returnCompanies = pageSize > 0 ? companies.Skip(pageSize * (pageIndex - 1)).Take(pageSize) : companies;
            return StatusCode(StatusCodes.Status200OK, returnCompanies);
        }

        [HttpPut("{id}")]
        public ActionResult<Company> UpdateCompany(string id, [FromBody] CreateCompanyRequest request)
        {
            Company? company = GetCompanyById(id);
            if (company is null)
            {
                return NotFound();
            }

            company.Name = request.Name;

            return StatusCode(StatusCodes.Status200OK, company);
        }

        private bool HasCompanyName(string name)
        {
            return companies.Exists(company => company.Name.Equals(name));
        }

        private Company? GetCompanyById(string id)
        {
            return companies.Find(company => company.Id.Equals(id));
        }


    }
}
