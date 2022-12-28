import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductShortInfo } from "../../../../shared/models/product-short-info.model";
import { ProductsService } from "../../../../shared/products.service";
import { ProductsMoving } from "../../products-moving.model";
import { RECEIPT_MOVING_TYPE } from "../products-receipt.component";
import { ProductsMovingService } from "../../products-moving.service";

@Component({
  selector: 'app-add-receipt',
  templateUrl: './add-receipt.component.html',
  styleUrls: ['./add-receipt.component.scss']
})
export class AddReceiptComponent implements OnInit {

  products: ProductShortInfo[];
  receipts: ProductsMoving[] = [];
  addingReceipt: boolean = false;
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

  getReceipts(): ProductsMoving[] {
    if (this.addingReceipt && this.receipts.length > 0) {
      return this.receipts.slice(0, this.receipts.length - 1);
    }

    return this.receipts;
  }

  productChanged(event: any) {
    console.log(event);

    this.receipts[this.receipts.length - 1].productName = this.products.filter(x => x.id == event).pop().productName;
  }

  addReceipt() {

    if (this.addingReceipt) {
      return;
    }

    this.addingReceipt = true;
    this.receipts.push({
      actionDate: this.actionDate,
      movingType: RECEIPT_MOVING_TYPE
    });
  }

  delete(idx: number) {
    this.receipts.splice(idx, 1);
  }

  acceptReceipt() {

    const lastReceipt = this.receipts[this.receipts.length - 1];

    if (!lastReceipt.productName || !lastReceipt.productId || !lastReceipt.count) {
      this.receipts.pop();
    }

    console.log(new Date(this.actionDate));

    lastReceipt.count = +lastReceipt.count;

    this.addingReceipt = false;
  }

  undoReceipt() {
    this.addingReceipt = false;
    this.receipts.pop();
  }

  confirm() {
    console.log(this.receipts);

    this.productsMovingService.create(this.receipts).subscribe(res => {
      this.edited = true;

      setTimeout(_ => {
        this.router.navigate(['/administration/products-moving/receipts']);
      },
        1500);
    }, error => {
      this.error = true;
    })
  }

  cancel() {
    this.router.navigate(['/administration/products-moving/receipts']);
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
