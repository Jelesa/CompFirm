import { Component, OnInit } from '@angular/core';
import { ProductsMovingService } from "../products-moving.service";
import { ProductsMoving } from "../products-moving.model";
import { Router } from '@angular/router';

export const GIVING_MOVING_TYPE = 'Выдача товара';

@Component({
  selector: 'app-products-giving',
  templateUrl: './products-giving.component.html',
  styleUrls: ['./products-giving.component.scss']
})
export class ProductsGivingComponent implements OnInit {

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
      movingType: GIVING_MOVING_TYPE
    }

    this.productMovingService.getProductMoving(filter).subscribe(res => {
      this.pageCount = Math.ceil(res.found / this.limit);
      this.productsMoving = res.result;
    });
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

}
