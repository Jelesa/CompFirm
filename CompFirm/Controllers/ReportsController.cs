using CompFirm.DataManagement.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompFirm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsRepository reportsRepository;

        public ReportsController(
            IReportsRepository reportsRepository)
        {
            this.reportsRepository = reportsRepository;
        }

        [HttpGet("prices/pdf")]
        public async Task<IActionResult> GetPriceListPdf()
        {
            var file = await this.reportsRepository.GetPriceListPdf();

            return File(file, "application/octet-stream; charset=utf-8", "pricelist.pdf");
        }

        [HttpGet("prices/csv")]
        public async Task<IActionResult> GetPriceListCsv()
        {
            var file = await this.reportsRepository.GetPriceListCsv();

            return File(file, "application/octet-stream; charset=utf-8", "pricelist.csv");
        }

        [HttpGet("ostatki/pdf")]
        public async Task<IActionResult> GetOstatkiPdf()
        {
            var file = await this.reportsRepository.GetOstatkiPdf();

            return File(file, "application/octet-stream; charset=utf-8", "ostatki.pdf");
        }

        [HttpGet("ostatki/csv")]
        public async Task<IActionResult> GetOstatkiCsv()
        {
            var file = await this.reportsRepository.GetOstatkiCsv();

            return File(file, "application/octet-stream; charset=utf-8", "ostatki.csv");
        }
    }
}
