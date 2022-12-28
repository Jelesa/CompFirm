import { Component } from '@angular/core';
import { GroupsService } from '../shared/groups.service';
import { ProductsService } from '../shared/products.service';
import { Group } from './models/group.model';
import { PriceInterval } from './models/price-interval.model';
import { ProductInfo } from './models/productInfo.model';
import { ProductsFilter } from './models/productsFilter.model';

const AllGroupId: number = -999;
const PageSize: number = 15;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  groups: Group[] = [];
  subGroups: Group[] = [];
  priceInterval: PriceInterval;
  productTypes: string[] = [];
  pageCount: number = 0;
  pageNumber: number = 0;
  isSearch: boolean = false;

  maxPrice: number = 1000000;
  minPrice: number = 0;

  products: ProductInfo[];

  filter: ProductsFilter = {
    groupId: AllGroupId,
    name: '',
    productType: '',
    minPrice: this.minPrice,
    maxPrice: this.maxPrice,
    limit: PageSize,
    offset: 0
  }

  params: string[] = [];

  selectedGroup: number;
  selectedSubGroup: number;
  selectedProductType: string;

  constructor(private groupsService: GroupsService,
    private productsService: ProductsService) {
  }

  ngOnInit() {
    this.groupsService.getMainGroups().subscribe(g => {
      this.groups = g;
    },
      error => {
        console.error(error);
      });

    this.productsService.getPrices().subscribe(p => {
      this.priceInterval = p;
      this.minPrice = p.minPrice;
      this.maxPrice = p.maxPrice;
    })

    this.search();
  }

  changeSubCategories(groupId: number) {

    this.filter.groupId = groupId;
    this.filter.productType = '';

    console.log(groupId);

    this.subGroups = [];
    this.productTypes = [];

    if (groupId === AllGroupId) {
      return;
    }

    this.groupsService.getChildGroups(groupId).subscribe(g => {
      if (!!g && g.length > 0) {
        this.subGroups = g;
      }
    },
      error => {
        console.error(error);
      });
  }

  setSubGroup(subGroupId: number) {
    this.filter.productType = '';

    this.filter.groupId = subGroupId;

    this.productTypes = [];

    this.groupsService.getProductTypes(subGroupId).subscribe(pt => {
      if (pt && pt.length > 0) {
        this.productTypes = pt;
      }
    })
  }

  setProductType(productType: string) {
    this.filter.productType = productType;
  }

  startSearch() {
    this.pageNumber = 0;
    this.filter.offset = this.pageNumber * PageSize;

    this.search();
  }

  search() {

    this.filter.minPrice = this.minPrice;
    this.filter.maxPrice = this.maxPrice;

    this.isSearch = true;

    this.products = [];

    this.productsService.getProducts(this.filter).subscribe(p => {
      this.products = p.result;
      this.pageCount = Math.ceil(p.found / PageSize);

      this.isSearch = false;
    });
  }

  prevPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;

      this.filter.offset = this.pageNumber * PageSize;
      this.search();
    }
  }

  nextPage() {
    if (this.pageNumber < this.pageCount - 1) {
      this.pageNumber++;

      this.filter.offset = this.pageNumber * PageSize;
      this.search();
    }
  }

  checkMinPrice(newValue: any) {
    if (this.priceInterval && this.minPrice < this.priceInterval.minPrice) {
      this.minPrice = this.priceInterval.minPrice
    }

    if (this.maxPrice < this.minPrice) {
      this.maxPrice = this.minPrice;
    }
  }

  checkMaxPrice(newValue: any) {
    if (this.priceInterval && this.maxPrice > this.priceInterval.maxPrice) {
      this.maxPrice = this.priceInterval.maxPrice
    }

    if (this.minPrice > this.maxPrice) {
      this.minPrice = this.maxPrice;
    }
  }

}
