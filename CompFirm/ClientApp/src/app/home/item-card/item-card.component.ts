import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth.service';
import { RequestsService } from '../../shared/requests.service';
import { ProductInfo } from '../models/productInfo.model';

const DefaultProductImg: string = '../../../assets/NoPhoto.jpg';

@Component({
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrls: ['./item-card.component.scss']
})
export class ItemCardComponent implements OnInit {

  @Input() product: ProductInfo;
  expandedBlock: boolean = false;
  itemInCart: boolean = false;
  cartSending: boolean = false;
  cartCount: number = 1;
  cartTimer: any;

  constructor(
    private authService: AuthService,
    private requestService: RequestsService) { }

  ngOnInit() {
  }

  isLoggedIn(): boolean {
    return this.authService.isUserLoggedIn();
  }

  changeExpanded() {
    this.expandedBlock = !this.expandedBlock;
  }

  getImageUrl() {
    if (!!this.product.imageUrl) {
      return this.product.imageUrl;
    }

    return DefaultProductImg;
  }

  addItemToCart() {
    this.cartCount = 1;
    this.itemInCart = true;

    this.sendCart();
  }

  sendCart() {
    this.cartSending = true;

    this.requestService.addItemToCart({ productId: this.product.productId, count: this.cartCount })
      .subscribe(res => {
        this.cartSending = false;
      })

  }

  decreaseCartCount() {
    if (!!this.cartTimer) {
      clearTimeout(this.cartTimer);
    }

    if (this.cartCount > 0) {
      this.cartCount--;
    }

    if (this.cartCount === 0) {
      this.itemInCart = false;
    }

    this.cartTimer = setTimeout(_ => this.sendCart(), 500);
  }

  increaseCartCount() {
    if (!!this.cartTimer) {
      clearTimeout(this.cartTimer);
    }

    this.cartCount++;

    console.log('increaseCartCount');
    this.cartTimer = setTimeout(_ => this.sendCart(), 500);
  }

}
