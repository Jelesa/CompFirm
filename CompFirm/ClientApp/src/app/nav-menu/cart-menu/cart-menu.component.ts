import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RequestsService } from '../../shared/requests.service';

@Component({
  selector: 'app-cart-menu',
  templateUrl: './cart-menu.component.html',
  styleUrls: ['./cart-menu.component.scss']
})
export class CartMenuComponent implements OnInit, OnDestroy {
  countCartItems: number;
  cartRefresh: any;

  constructor(
    private requestsService: RequestsService,
    private router: Router) { }

  ngOnInit(): void {
    this.setCartItems();

    this.cartRefresh = setInterval(_ => {
      this.setCartItems();
    }, 5000);
  }

  ngOnDestroy(): void {
    clearInterval(this.cartRefresh);
  }

  private setCartItems() {
    this.requestsService.getCartItems().subscribe(i => {
      this.countCartItems = i.length;
    },
      error => {
        console.error(error);
      });
  }

  goToCart() {
    this.router.navigate(['/cart']);
  }
}
