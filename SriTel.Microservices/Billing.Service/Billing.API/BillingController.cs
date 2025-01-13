using Billing.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Service.Billing.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
    }
}
[ApiController]
[Route("api/[controller]")]
public class BillingController : ControllerBase
{
    private readonly BillingService _billingService;

    public BillingController(BillingService billingService)
    {
        _billingService = billingService;
    }

    //[HttpPost("create")]
    //public async Task<IActionResult> CreateBill(Guid userId, decimal amount, DateTime dueDate)
    //{
    //    var bill = await _billingService.CreateBillAsync(userId, amount, dueDate);
    //    return Ok(bill);
    //}

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetBillsByUser(Guid userId)
    {
        var bills = await _billingService.GetBillByIdAsync(userId);
        return Ok(bills);
    }

    //[HttpPost("{billId}/pay")]
    //public async Task<IActionResult> RecordPayment(Guid billId, decimal amount)
    //{
    //    await _billingService.re(billId, amount);
    //    return NoContent();
    //}
}

