import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CharacteristicValue } from '../../../home/models/characteristicValue.model';
import { Group } from '../../../home/models/group.model';
import { Manufacturer } from '../../../home/models/manufacturer.model';
import { ProductInfo } from '../../../home/models/productInfo.model';
import { CharacteristicsService } from '../../../shared/characteristics.service';
import { GroupsService } from '../../../shared/groups.service';
import { ManufacturersService } from '../../../shared/manufacturers.service';
import { Characteristic } from '../../../shared/models/characteristics.model';
import { ProductsService } from '../../../shared/products.service';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.scss']
})
export class CreateProductComponent implements OnInit {
  defaultGroupItem: Group = {
    id: -1,
    name: '---'
  };

  addingCharacteristic: boolean = false;
  edited: boolean = false;
  error: boolean = false;
  productId: number;
  product: ProductInfo = {
    name: '',
    group: {
      id: -1,
      name: ''
    },
    manufacturer: {
      id: -1,
      name: ''
    },
    characteristics: []
  };
  groups: Group[];
  manufacturers: Manufacturer[];
  characteristics: Characteristic[];
  notExistingCharacteristics: Characteristic[];

  constructor(
    private router: Router,
    private groupsService: GroupsService,
    private manufacturersService: ManufacturersService,
    private characteristicsService: CharacteristicsService,
    private productsService: ProductsService) { }

  ngOnInit() {
    this.groupsService.getGroups(2147483647, 0).subscribe(g => {
      this.groups = [this.defaultGroupItem, ...g.result];
    });

    this.manufacturersService.getManufacturers().subscribe(m => {
      this.manufacturers = [this.defaultGroupItem, ...m];
    });

    this.characteristicsService.getAll().subscribe(ch => {
      this.characteristics = ch;
      this.setNotExistingCharacteristics();
    });
  }

  addCharacteristic() {
    this.product.characteristics.push({});
    this.addingCharacteristic = true;
  }

  addedChChanged(item: Characteristic) {

    const chIndex = this.product.characteristics.length - 1;

    this.product.characteristics[chIndex].characteristicId = item.id;
    this.product.characteristics[chIndex].characteristicName = item.name;
    this.product.characteristics[chIndex].unit = item.unit;

    this.setNotExistingCharacteristics();
  }

  acceptCharacteristics() {
    const chIndex = this.product.characteristics.length - 1;

    if (this.product.characteristics[chIndex].characteristicId <= 0
      || !this.product.characteristics[chIndex].value) {
      this.product.characteristics.pop();
    }

    this.addingCharacteristic = false;
    this.setNotExistingCharacteristics();
  }

  undoCharacteristics() {
    this.characteristics.pop();

    this.addingCharacteristic = false;
    this.setNotExistingCharacteristics();
  }

  setNotExistingCharacteristics(): Characteristic[] {
    if (!this.characteristics || this.characteristics.length === 0) {
      this.notExistingCharacteristics = [];

      return;
    }

    if (!this.product || !this.product.characteristics || this.product.characteristics.length === 0) {
      this.notExistingCharacteristics = this.characteristics;

      return;
    }

    this.notExistingCharacteristics = this.characteristics.filter(
      x => this.product.characteristics.map(pch => pch.characteristicId).indexOf(x.id) < 0);
  }

  getProductCharacteristics(): CharacteristicValue[] {
    if (this.addingCharacteristic && this.product.characteristics.length > 0) {
      return this.product.characteristics.slice(0, this.product.characteristics.length - 1);
    }

    return this.product.characteristics;
  }

  deleteCharacteristic(idx: number) {
    this.product.characteristics.splice(idx, 1);

    this.setNotExistingCharacteristics();
  }

  confirm() {
    this.productsService.createProduct(this.product)
      .subscribe(res => {
        this.edited = true;

        setTimeout(_ => {
            this.router.navigate(['/administration/product-list']);
          },
          1500);

      }, error => {
        this.error = true;
      });
  }

  cancel() {
    this.router.navigate(['/administration/product-list']);
  }

}
