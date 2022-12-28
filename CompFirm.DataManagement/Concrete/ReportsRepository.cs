using CompFirm.DataManagement.Abstract;
using CompFirm.Dto.Products;
using Dapper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompFirm.DataManagement.Constants;
using CompFirm.Domain.Models;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace CompFirm.DataManagement.Concrete
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly IDataAccess dataAccess;

        public ReportsRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<byte[]> GetPriceListCsv()
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                var products = await connection.QueryAsync<ProductShortInfoDto>(QueryTexts.Products.GetProductShortInfo);

                var csvContent = new StringBuilder();

                csvContent.AppendLine($"Артикул,Группа,Название,Цена");

                products.AsList().ForEach(x => csvContent.AppendLine($"{x.Id},{x.GroupName},{x.ProductName},{x.Price}"));

                return Encoding.UTF8.GetBytes(csvContent.ToString());
            }
        }

        public async Task<byte[]> GetPriceListPdf()
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                var products = await connection.QueryAsync<ProductShortInfoDto>(QueryTexts.Products.GetProductShortInfo);

                var html = await this.GetPriceListHtml(products.AsList());

                return this.GenerateBytesForHtml(html);
            }
        }

        public async Task<byte[]> GetOstatkiPdf()
        {
            var ostatki = await this.GetOstatki();

            var html = await this.GetOstatkiHtml(ostatki);

            return this.GenerateBytesForHtml(html);
        }

        public async Task<byte[]> GetOstatkiCsv()
        {
            var ostatki = await this.GetOstatki();

            var csvContent = new StringBuilder();

            csvContent.AppendLine($"Артикул,Название,Остаток");

            ostatki.ToList().ForEach(x => csvContent.AppendLine($"{x.Art},{x.ProductName},{x.Count}"));

            return Encoding.UTF8.GetBytes(csvContent.ToString());
        }

        private async Task<string> GetPriceListHtml(IReadOnlyCollection<ProductShortInfoDto> products)
        {
            using (var sr = new StreamReader("./HtmlTemplates/pricelist.html"))
            {
                var fileStr = await sr.ReadToEndAsync();

                var tableContent = string.Join('\n', products.Select(x =>
                    $"<tr><td>{x.Id}</td><td>{x.GroupName}</td><td>{x.ProductName}</td><td>{x.Price}</td></tr>")
                    .ToArray());

                return string.Format(fileStr, tableContent);
            }
        }

        private async Task<IReadOnlyCollection<Ostatki>> GetOstatki()
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                return (await connection.QueryAsync<Ostatki>(@"select
p.id Art,
p.`name` ProductName,
IFNULL(SUM(pm.count), 0) `Count`
from products p
join `groups` g on p.id_group = g.id
left join `groups` parentGroup on g.parent_group_id = parentGroup.id
left join products_moving pm on p.id = pm.id_product
WHERE g.`name` != 'Услуги' and parentGroup.`name` != 'Услуги'
group by p.id, p.`name`")).AsList();
            }
        }

        private async Task<string> GetOstatkiHtml(IReadOnlyCollection<Ostatki> ostatki)
        {
            using (var sr = new StreamReader("./HtmlTemplates/ostatki.html"))
            {
                var fileStr = await sr.ReadToEndAsync();

                var tableContent = string.Join('\n', ostatki.Select(x =>
                        $"<tr><td>{x.Art}</td><td>{x.ProductName}</td><td>{x.Count}</td></tr>")
                    .ToArray());

                return string.Format(fileStr, tableContent);
            }
        }

        private byte[] GenerateBytesForHtml(string html)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
