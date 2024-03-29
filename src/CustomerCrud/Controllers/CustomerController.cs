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

  [HttpGet("{id}")]
  public ActionResult GetById(int id)
  {
    var response = _customerRespository.GetById(id);
    if (response == null)
    {
      return NotFound("Customer not found");
    }
    return Ok(response);
  }

  [HttpPost]
  public ActionResult Create(CustomerRequest request)
  {
    var id = _customerRespository.GetNextIdValue();
    var customer = new Customer(id, request);
    _customerRespository.Create(customer);
    return CreatedAtAction("GetById", new { id = customer.Id }, customer);
  }

  [HttpPut("{id}")]
  public ActionResult Update(int id, CustomerRequest request) 
  { 
    var didUpdate = _customerRespository.Update(id,
      new 
      { 
        Name = request.Name,
        CPF = request.CPF,
        Transactions = request.Transactions,
        UpdatedAt = DateTime.Now 
      }
    );
    if (!didUpdate)
    {
      return NotFound("Customer not found");
    }
    return Ok($"Customer {id} updated");
  }

  [HttpDelete("{id}")]
  public ActionResult Delete(int id)
  {
    var didDelete = _customerRespository.Delete(id);
    
    if (!didDelete)
    {
      return NotFound("Customer not found");
    }
    return NoContent(); 
  }
}
