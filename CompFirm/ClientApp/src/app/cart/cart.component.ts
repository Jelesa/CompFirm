import { Component, OnInit } from '@angular/core';
import { CartItem } from '../shared/models/cart-item.model';
import { RequestsService } from '../shared/requests.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  cartItems: CartItem[];
  cartTimer: any;
  itemInCart: boolean = false;
  cartSending: boolean = false;
  orderConfirmed: boolean = false;

  constructor(
    private requestsService: RequestsService) { }

  ngOnInit(): void {
    this.requestsService.getCartItems().subscribe(i => {
      this.cartItems = i;
    },
      error => {
        console.error(error);
      });
  }

  getTotalSum(): number {

    if (!this.cartItems) {
      return 0;
    }

    var sum = 0;

    for (let i = 0; i < this.cartItems.length; i += 1) {
      sum += this.cartItems[i].count * this.cartItems[i].productPrice;
    }
    return sum;
  }

  sendCart(i: number) {
    this.cartSending = true;

    this.requestsService.addItemToCart({ productId: this.cartItems[i].productId, count: this.cartItems[i].count })
      .subscribe(res => {
        this.cartSending = false;
      });
  }

  decreaseCartCount(i: number) {
    if (!!this.cartTimer) {
      clearTimeout(this.cartTimer);
    }

    if (this.cartItems[i].count > 0) {
      this.cartItems[i].count--;
    }

    if (this.cartItems[i].count === 0) {
      this.itemInCart = false;
    }

    this.cartTimer = setTimeout(_ => this.sendCart(i), 500);
  }

  increaseCartCount(i: number) {
    if (!!this.cartTimer) {
      clearTimeout(this.cartTimer);
    }

    this.cartItems[i].count++;

    console.log('increaseCartCount');
    this.cartTimer = setTimeout(_ => this.sendCart(i), 500);
  }

  deleteCartItem(id: number) {
    this.requestsService.addItemToCart({ productId: id, count: 0 })
      .subscribe(_ => {
      });

    this.requestsService.getCartItems().subscribe(i => {
      this.cartItems = i;
    },
      error => {
        console.error(error);
      });
  }

  sendRequest() {
    this.requestsService.confirmRequest()
      .subscribe(_ => {
        this.orderConfirmed = true;
      });

    this.cartItems = [];
  }
}
