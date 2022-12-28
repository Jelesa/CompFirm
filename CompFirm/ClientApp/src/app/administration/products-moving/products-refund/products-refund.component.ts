import { Component, OnInit } from '@angular/core';
import { ProductsMovingService } from "../products-moving.service";
import { ProductsMoving } from "../products-moving.model";
import { Router } from '@angular/router';

export const REFUND_MOVING_TYPE = 'Возврат товара';

@Component({
  selector: 'app-products-refund',
  templateUrl: './products-refund.component.html',
  styleUrls: ['./products-refund.component.scss']
})
export class ProductsRefundComponent implements OnInit {

  productsMoving: ProductsMoving[];
  pageCount: number;
  pageNumber: number = 0;

  private limit: number = 30;
  private offset: number = 0;
  private dateFrom: string;
  private dateTo: string;

  constructor(
    private productMovingService: ProductsMovingService,
    private router: Router) { }

  ngOnInit() {
    this.search();
  }

  search() {
    let filter = {
      limit: this.limit,
      offset: this.offset,
      dateFrom: this.dateFrom,
      dateTo: this.dateTo,
      movingType: REFUND_MOVING_TYPE
    }

    this.productMovingService.getProductMoving(filter).subscribe(res => {
      this.pageCount = Math.ceil(res.found / this.limit);
      this.productsMoving = res.result;
    });
  }

  add() {
    this.router.navigate(['/administration/products-moving/refunds/add']);
  }

  prevPage() {
    if (this.pageNumber > 0) {
      this.pageNumber--;

      this.offset = this.pageNumber * this.limit;
      this.search();
    }
  }

  nextPage() {
    if (this.pageNumber < this.pageCount - 1) {
      this.pageNumber++;

      this.offset = this.pageNumber * this.limit;
      this.search();
    }
  }

  delete(id: number) {
    if (confirm('Вы действительно хотите удалить?')) {

      this.productMovingService.delete(id)
        .subscribe(_ => {

          alert('Успешно');
          this.offset = 0;
          this.pageNumber = 0;
          this.search();

        }, error => {
          console.error(error);
          alert('Произошла ошибка при удалении. Повторите попытку позже');
        });
    }
  }

}
