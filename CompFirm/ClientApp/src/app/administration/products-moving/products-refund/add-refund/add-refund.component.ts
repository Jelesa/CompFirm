import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductShortInfo } from "../../../../shared/models/product-short-info.model";
import { ProductsService } from "../../../../shared/products.service";
import { ProductsMoving } from "../../products-moving.model";
import { REFUND_MOVING_TYPE } from "../products-refund.component";
import { ProductsMovingService } from "../../products-moving.service";

@Component({
  selector: 'app-add-refund',
  templateUrl: './add-refund.component.html',
  styleUrls: ['./add-refund.component.scss']
})
export class AddRefundComponent implements OnInit {

  products: ProductShortInfo[];
  refunds: ProductsMoving[] = [];
  addingRefund: boolean = false;
  edited: boolean = false;
  error: boolean = false;
  actionDate: string;

  constructor(
    private router: Router,
    private productsService: ProductsService,
    private productsMovingService: ProductsMovingService) { }

  ngOnInit() {
    this.actionDate = this.getUtcDateTime(new Date());
    this.setProducts();
  }

  getRefunds(): ProductsMoving[] {
    if (this.addingRefund && this.refunds.length > 0) {
      return this.refunds.slice(0, this.refunds.length - 1);
    }

    return this.refunds;
  }

  productChanged(event: any) {
    console.log(event);

    this.refunds[this.refunds.length - 1].productName = this.products.filter(x => x.id == event).pop().productName;
  }

  addRefund() {

    if (this.addingRefund) {
      return;
    }

    this.addingRefund = true;
    this.refunds.push({
      actionDate: this.actionDate,
      movingType: REFUND_MOVING_TYPE
    });
  }

  delete(idx: number) {
    this.refunds.splice(idx, 1);
  }

  acceptRefund() {

    const lastRefund = this.refunds[this.refunds.length - 1];

    if (!lastRefund.productName || !lastRefund.productId || !lastRefund.count) {
      this.refunds.pop();
    }

    console.log(new Date(this.actionDate));

    lastRefund.count = +lastRefund.count;

    this.addingRefund = false;
  }

  undoRefund() {
    this.addingRefund = false;
    this.refunds.pop();
  }

  confirm() {
    console.log(this.refunds);

    this.productsMovingService.create(this.refunds).subscribe(res => {
      this.edited = true;

      setTimeout(_ => {
        this.router.navigate(['/administration/products-moving/refunds']);
      },
        1500);
    }, error => {
      this.error = true;
    })
  }

  cancel() {
    this.router.navigate(['/administration/products-moving/refunds']);
  }

  getUtcDateTime(date: Date) {
    return `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}T${date.getHours()}:${date.getMinutes()}`;
  }

  private setProducts() {
    const filter = {
      limit: 20000000,
      offset: 0
    }

    this.productsService.getProductsShortInfo(filter).subscribe(res => {
      this.products = res;
    });
  }

}
