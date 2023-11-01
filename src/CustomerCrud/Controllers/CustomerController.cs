using Microsoft.AspNetCore.Mvc;
using CustomerCrud.Core;
using CustomerCrud.Requests;
using CustomerCrud.Repositories;

namespace CustomerCrud.Controllers;

[ApiController]
[Route("customers")]

public class CustomerController : ControllerBase
{

  private readonly ICustomerRepository _customerRespository;

  public CustomerController(ICustomerRepository customerRepository)
  {
    _customerRespository = customerRepository;
  }

  [HttpGet]
  public ActionResult GetAll()
  {
    var response = _customerRespository.GetAll();
    return Ok(response);
  }
}
