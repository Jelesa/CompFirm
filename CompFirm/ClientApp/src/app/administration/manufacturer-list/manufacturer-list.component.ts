import { Component, OnInit } from '@angular/core';
import { Manufacturer } from '../../home/models/manufacturer.model';
import { ManufacturersService } from "../../shared/manufacturers.service";

@Component({
  selector: 'app-manufacturer-list',
  templateUrl: './manufacturer-list.component.html',
  styleUrls: ['./manufacturer-list.component.scss']
})
export class ManufacturerListComponent implements OnInit {
  isSearch: boolean = false;
  adding: boolean = false;
  addingName: string = '';

  manufacturers: Manufacturer[];

  constructor(private manufacturersService: ManufacturersService) { }

  ngOnInit() {
    this.search();
  }

  search() {
    this.manufacturersService.getManufacturers().subscribe(x => {
      this.manufacturers = x;
    });
  }

  add() {
    this.adding = true;
  }

  undoAdding() {
    this.adding = false;
  }

  acceptAdding() {
    this.manufacturersService.create(this.addingName).subscribe(res => {

      alert('Успешно добавлено');
      this.adding = false;
      this.addingName = '';
      this.search();

    }, error => {
    });
  }

  delete(id: number) {
    if (confirm('Вы действительно хотите удалить?')) {

      this.manufacturersService.delete(id)
        .subscribe(_ => {

          alert('Успешно');
          this.search();

        }, error => {
        });
    }
  }
}
