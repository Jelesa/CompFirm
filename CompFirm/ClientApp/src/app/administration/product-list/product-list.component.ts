import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductShortInfo } from '../../shared/models/product-short-info.model';
import { ProductsService } from '../../shared/products.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  productsShortInfo: ProductShortInfo[];
  isSearch: boolean = false;
  pageCount: number = 0;
  pageNumber: number = 0;
  limit: number = 20;
  offset: number = 0;

  constructor(
    private router: Router,
    private productsServices: ProductsService) { }

  ngOnInit() {
    this.search();
  }

  search() {
    this.isSearch = true;

    this.productsShortInfo = [];

    const filter = {
      limit: this.limit,
      offset: this.offset
    }

    this.productsServices.getProductsShortInfo(filter).subscribe(res => {
      this.productsShortInfo = res;
      this.pageCount = Math.ceil(res.length / this.limit);

      this.isSearch = false;
    });
  }

  editProduct(id: number) {
    this.router.navigate([`administration/edit-product/${id}`]);
  }

  deleteProduct(id: number) {
    if (confirm('Вы действительно хотите удалить данный товар или услугу?')) {

      this.productsServices.deleteProduct(id)
        .subscribe(_ => {

          alert('Товар или услуга удалена');
          location.reload();

        }, error => {
          console.error(error);
          alert('Произошла ошибка при удалении товара или услуги. Повторите попытку позже');
        });
    }
  }

  createProduct() {
    this.router.navigate(['/administration/create-product']);
  }

  prevPage() {
    if (this.pageNumber > 1) {
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
